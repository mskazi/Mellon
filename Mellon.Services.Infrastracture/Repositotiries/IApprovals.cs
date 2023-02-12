using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Infrastracture.Repositotiries
{

    public interface IApprovalsRepository : IRepository
    {
        Task<Approval> GetApproval(string documentType, CancellationToken cancellationToken);
        Task<ApprovalOrder> GetApprovalOrder(int id, CancellationToken cancellationToken);

        Task<ApprovalOrder> GetApprovalOrderByDocument(string document, CancellationToken cancellationToken);

        void Decision(ApprovalOrder approval);

        void AddApprovalOrder(ApprovalOrder approval);

        void AddApprovalNotification(ApprovalNotification approvalNotification);

        Task<ApprovalNotification> getApprovalNotification(string documentToken, CancellationToken cancellationToken);

        Task<List<ApprovalNotification>> GetDelayerdApprovals(int daysToCalcualte, CancellationToken cancellationToken);
        void ResetContext();
        
    }
}
