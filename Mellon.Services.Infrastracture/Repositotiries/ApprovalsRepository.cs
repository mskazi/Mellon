using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Context;
using Mellon.Services.Infrastracture.ModelExtensions;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;


namespace Mellon.Services.Infrastracture.Repositotiries
{
    public class ApprovalsRepository : IApprovalsRepository
    {
        private readonly MellonContext context;
        public ApprovalsRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public void AddApprovalOrder(ApprovalOrder approval)
        {
            context.ApprovalOrders.Add(approval);
        }

        public void AddApprovalNotification(ApprovalNotification approvalNotification)
        {
            context.ApprovalNotifications.Add(approvalNotification);
        }

        public Task<ApprovalNotification> getApprovalNotification(string documentToken, CancellationToken cancellationToken)
        {
            return context.ApprovalNotifications.FirstOrDefaultAsync(x => x.DocumentToken == documentToken, cancellationToken);
        }

        public void Decision(ApprovalOrder approval)
        {
            throw new NotImplementedException();
        }

        public Task<ApprovalOrder> GetApprovalOrder(int id, CancellationToken cancellationToken)
        {
            return context.ApprovalOrders.Include(x => x.ApprovalLines).Include(x => x.Approvals).IgnoreAutoIncludes().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task<Approval> GetApproval(string documentToken, CancellationToken cancellationToken)
        {
            return context.Approvals.FirstOrDefaultAsync(x => x.DocumentToken == documentToken, cancellationToken);
        }

        public Task<ApprovalOrder> GetApprovalOrderByDocument(string documentNo, CancellationToken cancellationToken)
        {
            return context.ApprovalOrders?.FirstOrDefaultAsync(c => c.DocumentNo == documentNo, cancellationToken);
        }


        public Task<List<ApprovalNotification>> GetDelayerdApprovals(int daysToCalcualte, CancellationToken cancellationToken)
        {
            var date = DateTime.Now.Subtract(TimeSpan.FromDays(daysToCalcualte));
            var query = (from an in context.ApprovalNotifications
                         join a in context.Approvals
                                      on an.DocumentToken equals a.DocumentToken   
                                       where (a.Status == ApprovalStatusEnum.Open || a.Status== ApprovalStatusEnum.Canceled)  &&
                                       an.NotificationSend < date
                                      select  an
                                      ).ToListAsync(cancellationToken);

            return query;
        }

        public void ResetContext()
        {
            context.ApprovalOrders.Local.Reset();
            context.ApprovalLines.Local.Reset();
            context.Approvals.Local.Reset();
            //context.Approvals.AsNoTracking();
            //context.ApprovalLines.AsNoTracking();
            //context.ApprovalOrders.AsNoTracking();
            context.ChangeTracker.Clear();

            var changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;

        }

    }
}
