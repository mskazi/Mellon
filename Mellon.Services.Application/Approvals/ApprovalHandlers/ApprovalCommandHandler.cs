using External.ERP.Service;
using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Services;
using Mellon.Services.Infrastracture.ModelExtensions;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mellon.Services.Application.Approvals.ApprovalHandlers
{
    public class ApprovalCommandHandler : IRequestHandler<GetApprovalCommand, ApprovalOrderResource>
    {
        private readonly ILogger logger;
        private readonly IApprovalsRepository repository;
        public ApprovalCommandHandler(IApprovalsRepository repository, ILogger<ApprovalCommandHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger;
        }
        public async Task<ApprovalOrderResource> Handle(GetApprovalCommand request, CancellationToken cancellationToken)
        {
            var approval = await repository.GetApproval(request.documentToken, cancellationToken);
            if (approval is null)
            {
                throw new NotFoundException(nameof(approval), request.documentToken);
            }

            if (!(approval.Status == ApprovalStatusEnum.Open || approval.Status == ApprovalStatusEnum.Requested))
            {
                throw new ApprovalStatusException(nameof(approval), request.documentToken);
            }

            var approvalOrder = await repository.GetApprovalOrder(approval.ApprovalOrderId, cancellationToken);
            return new ApprovalOrderResource(approval, approvalOrder);
        }
    }

    public class ApprovalDecitionCommandHandler : IRequestHandler<ApprovalDecisionCommand, Boolean>
    {
        private readonly string serviceUrl;
        private readonly IApprovalsRepository repository;
        private readonly IEmailService emailService;

        public ApprovalDecitionCommandHandler(IApprovalsRepository repository, IConfiguration configuration, IEmailService emailService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.emailService = emailService;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            serviceUrl = configuration["Endpoints:ERP"] ?? throw new Exception("ERP Service endpoint not found in configuration.");

        }

        public async Task<bool> Handle(ApprovalDecisionCommand request, CancellationToken cancellationToken)
        {

            //await processLock.IsWaiting();
            var approval = await repository.GetApproval(request.documentToken, cancellationToken);
            if (approval is null)
            {
                throw new NotFoundException(nameof(approval), request.documentToken);
            }

            if (!(approval.Status == ApprovalStatusEnum.Open || approval.Status == ApprovalStatusEnum.Requested))
            {
                throw new ApprovalStatusException(nameof(approval), request.documentToken);
            }

            var approvalOrder = await repository.GetApprovalOrder(approval.ApprovalOrderId, cancellationToken);
            var max = approvalOrder.Approvals.Where((a) => a.DocumentNo == approval.DocumentNo && a.ERPCompany == approval.ERPCompany).Max(x => x.ApprovalSequence);
            var endpoint = DataAccessSoapClient.EndpointConfiguration.DataAccessSoap;
            using var client = new DataAccessSoapClient(endpoint, serviceUrl);
            var action = new Document_Appover_NAS_ActionRequest();
            var status = request.decision.ToString();
            var owner = approval.DocumentOwner;
            var approvalResponsible = approval.ApprovalResponsible;
            action = new Document_Appover_NAS_ActionRequest(
                 approval.ERPCountry, approval.ERPCompany, approval.DocumentType,
                approval.DocumentNo,
                owner,
                approvalResponsible, status,
                request.comment);
            var responseApproverOrder = await client.Document_Appover_NAS_ActionAsync(action);
            if (responseApproverOrder.Document_Appover_NAS_ActionResult.Contains("Error"))
            {
                throw new ERPDecisionException(responseApproverOrder.Document_Appover_NAS_ActionResult);
            }

            if (request.decision == ApprovalStatusEnum.Approved)
            {
                if (approval.ApprovalSequence == max)
                {
                    this.emailService.sendApprovalDecision(approval, approval.DocumentOwnerEmail, request.decision);

                    if (approvalOrder.NotificationMail?.Length > 5 && request.decision == ApprovalStatusEnum.Approved)
                    {
                        this.emailService.sendApprovalDecision(approval, approvalOrder.NotificationMail, request.decision);
                    }
                }
            }
            else if (request.decision == ApprovalStatusEnum.Rejected)
            {
                this.emailService.sendApprovalDecision(approval, approval.DocumentOwnerEmail, request.decision);
            }

            //await processLock.IsWaiting();
            var approvalRefresh = await repository.GetApproval(request.documentToken, cancellationToken);
            approvalRefresh.Status = request.decision;
            await repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
