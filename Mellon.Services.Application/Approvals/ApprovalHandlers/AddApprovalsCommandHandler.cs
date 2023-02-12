using External.ERP.Service;
using MediatR;
using Mellon.Services.Application.Services;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.ModelExtensions;
using Mellon.Services.Infrastracture.Models;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Nager.Date;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Mellon.Services.Application.Approvals.ApprovalHandlers
{
    public class AddApprovalCommandHandler : IRequestHandler<InsertERPApprovalsCommand, ApprovalOrderResource>
    {
        private readonly IApprovalsRepository repository;
        private readonly string serviceUrl;
        private readonly IEmailService emailService;
        private readonly int daysToResend;
        private readonly string holidaysAPIUrl;



        public AddApprovalCommandHandler(IApprovalsRepository repository, IConfiguration configuration, IEmailService emailService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.emailService = emailService;

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            serviceUrl = configuration["Endpoints:ERP"] ?? throw new Exception("ERP Service endpoint not found in configuration.");
            Int32.TryParse(configuration["EmailSettings:ResendEmail"] ?? "0", out daysToResend); ;
            holidaysAPIUrl = configuration["Endpoints:holidaysURL"] ?? throw new Exception("Holidays API Url");
        }
        public async Task<ApprovalOrderResource> Handle(InsertERPApprovalsCommand request, CancellationToken cancellationToken)
        {
            var endpoint = DataAccessSoapClient.EndpointConfiguration.DataAccessSoap;
            using var client = new DataAccessSoapClient(endpoint, serviceUrl);
            var responseApproverOrder = await client.Document_Approver_OrderAsync(new Document_Approver_OrderRequest());
            var responseApproverOrderList = JsonConvert.DeserializeObject<Document_Approver_Order_Response[]>(responseApproverOrder.Body.Document_Approver_OrderResult)?.ToList();

            var responseLine = await client.Document_Approver_LineAsync(new Document_Approver_LineRequest());
            var responseApprovalLineList = JsonConvert.DeserializeObject<Document_Approver_Line_Response[]>(responseLine.Body.Document_Approver_LineResult)?.ToList();

            var responseApprover = await client.Document_ApproverAsync(new Document_ApproverRequest());
            var responseApproverList = JsonConvert.DeserializeObject<Document_Approver_Response[]>(responseApprover.Body.Document_ApproverResult).ToList();
            var approvalsToSendEmail = new List<ApprovalNotification>();

            repository.ResetContext();
            FormattableString deleteApprovalLines = $"DELETE FROM dbo.ApprovalLines";
            FormattableString deleteApprovalOrders = $"DELETE FROM dbo.ApprovalOrders";
            FormattableString deleteApprovals = $"DELETE FROM dbo.Approvals";
            FormattableString resetIdApprovalLines = $"DBCC CHECKIDENT ('ApprovalLines', RESEED, 0)";
            FormattableString resetIdApprovalOrders = $"DBCC CHECKIDENT ('ApprovalOrders', RESEED, 0)";
            FormattableString resetIdApprovals = $"DBCC CHECKIDENT ('Approvals', RESEED, 0)";
            await repository.UnitOfWork.ExecuteSqlAsync(deleteApprovalLines);
            await repository.UnitOfWork.ExecuteSqlAsync(deleteApprovalOrders);
            await repository.UnitOfWork.ExecuteSqlAsync(deleteApprovals);
            await repository.UnitOfWork.ExecuteSqlAsync(resetIdApprovalLines);
            await repository.UnitOfWork.ExecuteSqlAsync(resetIdApprovalOrders);
            await repository.UnitOfWork.ExecuteSqlAsync(resetIdApprovals);
            await repository.UnitOfWork.SaveChangesAsync();

            HttpClient holidayClient = new HttpClient();
            HttpResponseMessage response = await holidayClient.GetAsync(String.Format("{0}/{1}", holidaysAPIUrl, CountryCode.GR));

            foreach (var responseApproval in responseApproverOrderList)
            {

                var newApproval = createApprovalOrder(responseApproval);
                var approvalLineList = responseApprovalLineList.Where(p => p.DocumentNo == responseApproval.DocumentNo).ToList();
                foreach (var responseApprovalLine in approvalLineList)
                {
                    newApproval.ApprovalLines.Add(createApprovalLine(responseApprovalLine));
                }
                repository.AddApprovalOrder(newApproval);

                var approvalList = responseApproverList.Where(p => p.DocumentNo == responseApproval.DocumentNo).ToList();
                foreach (var approval in approvalList)
                {
                    newApproval.Approvals.Add(createApproval(approval));
                    if (approval.Status == ApprovalStatusEnum.Open || approval.Status == ApprovalStatusEnum.Requested && approval.DocumentType== "Purchace")
                    {
                        approvalsToSendEmail.Add(createApprovalNotification(approval));
                    }
                }
                repository.AddApprovalOrder(newApproval);
            }

            await repository.UnitOfWork.SaveChangesAsync();
            foreach (var approvalToSendEmail in approvalsToSendEmail)
            {
                 var  approvalNotification = await repository.getApprovalNotification(approvalToSendEmail.DocumentToken, cancellationToken);
                if (approvalNotification==null)
                {
                    var approval = await repository.GetApproval(approvalToSendEmail.DocumentToken, cancellationToken);
                    if (approval!=null)
                    {
                        approvalToSendEmail.NotificationSend = DateTime.Now;
                        emailService.sendApprovalNotification(approvalToSendEmail, approval);
                        repository.AddApprovalNotification(approvalToSendEmail);
                        await repository.UnitOfWork.SaveChangesAsync();
                    }
                }
            }
          
            if ((DateTime.Now.DayOfWeek != DayOfWeek.Saturday || DateTime.Now.DayOfWeek != DayOfWeek.Sunday)  &&  this.daysToResend!=0 && response.StatusCode!= System.Net.HttpStatusCode.OK)
            {
                var delayedApprovalNotifications = await repository.GetDelayerdApprovals(daysToResend, cancellationToken);
                foreach (var delayedApprovalNotification in delayedApprovalNotifications)
                {
                    var approval = await repository.GetApproval(delayedApprovalNotification.DocumentToken, cancellationToken);
                    if (approval != null)
                    {
                        delayedApprovalNotification.NotificationSend = DateTime.Now;
                        emailService.reSendApprovalNotification(delayedApprovalNotification, approval);
                        await repository.UnitOfWork.SaveChangesAsync();
                    }

                }
            }
            return new ApprovalOrderResource();
        }

        private ApprovalOrder createApprovalOrder(Document_Approver_Order_Response response)
        {
            var approvalOrder = new ApprovalOrder();
            approvalOrder.ERPCountry = response.ERPCountry;
            approvalOrder.ERPCompany = response.ERPCompany;
            approvalOrder.DocumentNo = response.DocumentNo;
            approvalOrder.DocumentType = response.DocumentType;
            approvalOrder.SourceName = response.SourceName;
            approvalOrder.OrderDate = response.OrderDate;
            approvalOrder.Bu = response.BU;
            approvalOrder.BUName = response.BU_Name;
            approvalOrder.BL = response.BL;
            approvalOrder.BLName = response.BL_Name;
            approvalOrder.PLLines = response.PL_LINES;
            approvalOrder.PLLinesName = response.PL_LINES_Name;
            approvalOrder.Ergo = response.ERGO;
            approvalOrder.ErgoName = response.ERGO_Name;
            approvalOrder.ERPTimeStamp = response.ERPTimeStamp;
            approvalOrder.NotificationMail = response.Notification_Mail;
            approvalOrder.Currency = response.Currency;
            return approvalOrder;
        }

        private ApprovalLine createApprovalLine(Document_Approver_Line_Response response)
        {
            var approvalLine = new ApprovalLine();
            approvalLine.ERPCompany = response.ERPCompany;
            approvalLine.DocumentNo = response.DocumentNo;
            approvalLine.DocumentType = response.DocumentType;
            approvalLine.LineNo = response.LineNo;
            approvalLine.LineAmount = response.LineAmount;
            approvalLine.Description = response.Description;
            approvalLine.BU = response.BU;
            approvalLine.BUName = response.BU_Name;
            approvalLine.BL = response.BL;
            approvalLine.BLName = response.BL_Name;
            approvalLine.PLLines = response.PL_LINES;
            approvalLine.PLLinesName = response.PL_LINES_Name;
            approvalLine.Ergo = response.ERGO;
            approvalLine.ErgoName = response.ERGO_Name;
            approvalLine.ERPTimeStamp = response.ERPTimeStamp;
            approvalLine.UnitPrice = response.UnitPrice;
            approvalLine.Quantity = response.Quantity;
            return approvalLine;
        }

        private Approval createApproval(Document_Approver_Response response)
        {
            var approval = new Approval();
            approval.ERPCountry = response.ERPCountry;
            approval.ERPCompany = response.ERPCompany;
            approval.DocumentType = response.DocumentType;
            approval.DocumentNo = response.DocumentNo;
            approval.ApprovalSequence = response.ApprovalSequence;
            approval.DocumentOwner = response.DocumentOwner;
            approval.RequestNo = response.RequestNo;
            approval.ApprovalResponsible = response.ApprovalResponsible;
            approval.email = response.email;
            approval.DocumentOwnerEmail = response.DocumentOwnerEmail;
            approval.Status = response.Status;
            approval.DocumentToken = response.DocumentToken;
            approval.ERPTimeStamp = response.ERPTimeStamp;
            approval.ApprovalProcessComment = response.ApprovalProcessComment;
            approval.ApprovalRequestComment = response.ApprovalRequestComment;
            return approval;
        }
        private ApprovalNotification createApprovalNotification(Document_Approver_Response response)
        {
            var approvalNotification = new ApprovalNotification();
            approvalNotification.DocumentType = response.DocumentType;
            approvalNotification.DocumentNo = response.DocumentNo;
            approvalNotification.email = response.email;
            approvalNotification.DocumentToken = response.DocumentToken;
            approvalNotification.DocumentOwner = response.DocumentOwner.Replace("MELLONGROUP\\", "");
            return approvalNotification;
        }
        
    }

    internal class Document_Approver_Order_Response
    {
        public string ERPCountry { get; set; }
        public string ERPCompany { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string SourceName { get; set; }

        public DateTime OrderDate { get; set; }
        public string BU { get; set; }
        public string BU_Name { get; set; }
        public string BL { get; set; }
        public string BL_Name { get; set; }
        public string PL_LINES { get; set; }
        public string PL_LINES_Name { get; set; }
        public string ERGO { get; set; }
        public string ERGO_Name { get; set; }
        public string ERPTimeStamp { get; set; }
        public string Notification_Mail { get; set; }
        public string Currency { get; set; }

    }

    internal class Document_Approver_Line_Response
    {
        public string ERPCompany { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string Description { get; set; }
        public double LineAmount { get; set; }
        public string BU { get; set; }
        public string BU_Name { get; set; }
        public string BL { get; set; }
        public string BL_Name { get; set; }
        public string PL_LINES { get; set; }
        public string PL_LINES_Name { get; set; }
        public string ERGO { get; set; }
        public string ERGO_Name { get; set; }
        public double UnitPrice { get; set; }

        public double Quantity { get; set; }
        public string ERPTimeStamp { get; set; }
    }

    internal class Document_Approver_Response
    {
        public int Id { get; set; }
        public string ERPCountry { get; set; }
        public string ERPCompany { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int ApprovalSequence { get; set; }
        public string DocumentOwner { get; set; }
        public int RequestNo { get; set; }
        public string ApprovalResponsible { get; set; }
        public string email { get; set; }
        public string DocumentOwnerEmail { get; set; }
        public ApprovalStatusEnum Status { get; set; }
        public string DocumentToken { get; set; }
        public string ERPTimeStamp { get; set; }
        public string ApprovalProcessComment { get; set; }
        public string ApprovalRequestComment { get; set; }
    }
}