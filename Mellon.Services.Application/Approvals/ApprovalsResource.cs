using Mellon.Services.Infrastracture.ModelExtensions;
using Mellon.Services.Infrastracture.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Application.Approvals
{
    public class ApprovalOrderResource
    {
        public ApprovalOrderResource()
        {

        }

        public ApprovalOrderResource(Approval mainApproval,ApprovalOrder approval)
        {
            this.DocumentToken = mainApproval.DocumentToken;
            this.DocumentOwner = mainApproval.DocumentOwner.Replace("MELLONGROUP\\","");
            this.ApprovalProcessComment = mainApproval.ApprovalProcessComment;
            this.ERPCountry = approval.ERPCountry;
            this.ERPCompany = approval.ERPCompany;
            this.DocumentNo = approval.DocumentNo;
            this.DocumentType = approval.DocumentType;
            this.SourceName = approval.SourceName;
            this.OrderDate = approval.OrderDate;
            this.Bu = approval.Bu;
            this.BUName = approval.BUName;
            this.BL = approval.BL;
            this.BLName = approval.BLName;
            this.PLLines = approval.PLLines;
            this.PLLinesName = approval.PLLinesName;
            this.Ergo = approval.Ergo;
            this.Approvals = approval.Approvals;
            this.ApprovalLines = approval.ApprovalLines;
        }
        public string DocumentToken { get; set; }
        public string DocumentOwner { get; set; }
        public string ApprovalProcessComment { get; set; }

        public string? ERPCountry { get; set; }

        public string? ERPCompany { get; set; }

        public string? DocumentNo { get; set; }

        public string? DocumentType { get; set; }

        public string? SourceName { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Bu { get; set; }

        public string? BUName { get; set; }

        public string? BL { get; set; }

        public string? BLName { get; set; }

        public string? PLLines { get; set; }

        public string? PLLinesName { get; set; }

        public string? Ergo { get; set; }

        public string? ErgoName { get; set; }
        public string? Currency { get; set; }
        public virtual ICollection<Approval> Approvals { get; set; }

        public virtual ICollection<ApprovalLine> ApprovalLines { get; set; }
    }
}
