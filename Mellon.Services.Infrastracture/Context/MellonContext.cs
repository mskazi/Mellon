using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mellon.Services.Infrastracture.Context
{
    public partial class MellonContextTemp : UnitOfWorkContext<MellonContext>
    {
        private readonly ClaimsPrincipal Principal;
        public MellonContextTemp(DbContextOptions<MellonContext> options) : base(options)
        {
        }

        public MellonContextTemp(DbContextOptions<MellonContext> options, ClaimsPrincipal principal)
            : base(options)
        {
            Principal = principal;
        }

        public virtual DbSet<ApprovalOrder> ApprovalOrders { get; set; }
        public virtual DbSet<ApprovalLine> ApprovalLines { get; set; }
        public virtual DbSet<Approval> Approvals { get; set; }
        public virtual DbSet<ApprovalNotification> ApprovalNotifications { get; set; }
    }
}
