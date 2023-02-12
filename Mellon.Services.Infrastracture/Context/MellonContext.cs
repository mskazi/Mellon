using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Infrastracture.Context
{
    public partial class MellonContext : UnitOfWorkContext<MellonContext>
    {
        private readonly ClaimsPrincipal Principal;
        public MellonContext(DbContextOptions<MellonContext> options) : base(options)
        {
        }

        public MellonContext(DbContextOptions<MellonContext> options, ClaimsPrincipal principal)
            : base(options)
        {
            Principal = principal;
        }

        public virtual DbSet<ApprovalOrder> ApprovalOrders { get; set; }
        public virtual DbSet<ApprovalLine> ApprovalLines { get; set; }
        public virtual DbSet<Approval> Approvals{ get; set; }
        public virtual DbSet<ApprovalNotification> ApprovalNotifications { get; set; }
    }
}
