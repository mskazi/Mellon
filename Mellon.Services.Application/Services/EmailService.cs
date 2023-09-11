using MailKit.Net.Smtp;
using MailKit.Security;
using Mellon.Services.Infrastracture.ModelExtensions;
using Mellon.Services.Infrastracture.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Application.Services
{
    public class EmailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
    public interface IEmailService
    {
        bool SendEmail(EmailData emailData);

        bool sendApprovalNotification(ApprovalNotification approvalNotification,Approval approval);

        bool reSendApprovalNotification(ApprovalNotification approvalNotification, Approval approval);
        

        bool sendApprovalDecision(Approval approval, String email, ApprovalStatusEnum decision);
    }
    public class EmailSettings
    {
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string? overrideEmailIdTo { get; set; }
        public string documentApprovalURL { get; set; }
     }


        public class EmailService : IEmailService
        {
        private readonly bool isTest = false;
        EmailSettings _emailSettings = null;
            public EmailService(IOptions<EmailSettings> options,IConfiguration configuration)
            {
                _emailSettings = options.Value;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (!string.IsNullOrEmpty(configuration["Enviroment"]))
            {
                this.isTest = configuration["Enviroment"].ToLower() == "test";
            }
        }
        public bool SendEmail(EmailData emailData)
            {
                try
                {
                    MimeMessage emailMessage = new MimeMessage();
                    MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                    emailMessage.From.Add(emailFrom);
                var overrideEmailName = string.IsNullOrEmpty(_emailSettings.overrideEmailIdTo) ? emailData.EmailToName : _emailSettings.overrideEmailIdTo;
                var overrideEmailId = string.IsNullOrEmpty(_emailSettings.overrideEmailIdTo) ? emailData.EmailToId : _emailSettings.overrideEmailIdTo;
                    MailboxAddress emailTo = new MailboxAddress(overrideEmailName, overrideEmailId);
                    emailMessage.To.Add(emailTo);
                   var subject = emailData.EmailSubject;
                    if (this.isTest)
                    {
                        emailMessage.Subject = "THIS IS TEST!!! " + subject;
                    }
                    else
                    {
                    emailMessage.Subject = subject;
                    }
                
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailData.EmailBody ;
                   
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                
                SmtpClient emailClient = new SmtpClient();
                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    emailClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
                   // emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                    emailClient.Send(emailMessage);
                    emailClient.Disconnect(true);
                    emailClient.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    //Log Exception Details
                    return false;
                }
            }

            public bool sendApprovalNotification(ApprovalNotification approvalNotification, Approval approval)
            {
                var emailData = new EmailData();
                emailData.EmailSubject = String.Format("{0}: {1} Pending Approval", approval.ERPCompany, approvalNotification.DocumentNo);
                emailData.EmailToId = approvalNotification.email;
                emailData.EmailBody = String.Format("A request for approval is required by you.Please click <a HREF=\"{0}{1}\">HERE</a> to Approve or Reject. <br><br> <strong> DO NOT REPLY TO THIS MESSAGE!!!</strong> <br><br> Token:{1} ", _emailSettings.documentApprovalURL, approvalNotification.DocumentToken);
                return SendEmail(emailData);
            }
            public bool reSendApprovalNotification(ApprovalNotification approvalNotification, Approval approval)
            {
                var emailData = new EmailData();
                emailData.EmailSubject = String.Format("{0}: {1} Pending Approval - Kind Remind", approval.ERPCompany, approvalNotification.DocumentNo);
                emailData.EmailToId = approvalNotification.email;
                emailData.EmailBody = String.Format("A request for approval is required by you.Please click <a HREF=\"{0}{1}\">HERE</a> to Approve or Reject. <br><br><strong>DO NOT REPLY TO THIS MESSAGE!!!</strong>", _emailSettings.documentApprovalURL, approvalNotification.DocumentToken);
                return SendEmail(emailData);
            }
        
            public bool sendApprovalDecision(Approval approval, String email, ApprovalStatusEnum decision)
            {
                var emailData = new EmailData();
                emailData.EmailSubject = String.Format("Navision {0} {1} {2}", approval.DocumentType, approval.DocumentNo, decision.ToString());
                emailData.EmailToId = email;
                emailData.EmailBody = String.Format("ApplicationNo -> {0} Outcome -> {1}", approval.DocumentNo, decision.ToString());
                return SendEmail(emailData);
            }
        }

    }
