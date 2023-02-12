using MediatR;
using Mellon.Services.Infrastracture.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Application.Approvals
{
    public class GetApprovalCommand :  IRequest<ApprovalOrderResource>
    {
        public GetApprovalCommand(string documentToken)
        {
            this.documentToken = documentToken;
        }
        public string? documentToken { get; set; }

    }


    public class InsertERPApprovalsCommand :  IRequest<ApprovalOrderResource>
    {
        public InsertERPApprovalsCommand()
        {
        }
    }


    public class ApprovalDecisionCommand : IRequest<Boolean>
    {
        public ApprovalDecisionCommand(string documentToken, string comment, ApprovalStatusEnum decision)
        {
            this.documentToken = documentToken;
            this.decision = decision;
            this.comment = comment;
        }
        public string documentToken { get; set; }
        public ApprovalStatusEnum decision { get; set; }

        public string comment { get; set; }


    }
}
