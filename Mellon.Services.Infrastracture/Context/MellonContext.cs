using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public virtual DbSet<Approval> Approvals { get; set; }
        public virtual DbSet<ApprovalNotification> ApprovalNotifications { get; set; }

        public virtual DbSet<Carrier> Carriers { get; set; }

        public virtual DbSet<CarriersPostcodeRestriction> CarriersPostcodeRestrictions { get; set; }

        public virtual DbSet<CarriersProjectRestriction> CarriersProjectRestrictions { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<DataCancellation> DataCancellations { get; set; }

        public virtual DbSet<DataInability> DataInabilities { get; set; }

        public virtual DbSet<DataLine> DataLines { get; set; }

        public virtual DbSet<DataSubvoucher> DataSubvouchers { get; set; }

        public virtual DbSet<DataUpload> DataUploads { get; set; }

        public virtual DbSet<Datum> Data { get; set; }

        public virtual DbSet<Dim> Dims { get; set; }

        public virtual DbSet<DimDate> DimDates { get; set; }

        public virtual DbSet<ElectraProjectSetup> ElectraProjectSetups { get; set; }

        public virtual DbSet<FromErp> FromErps { get; set; }

        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<OfficeContact> OfficeContacts { get; set; }

        public virtual DbSet<Returned> Returneds { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("members");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("company");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("created_by");
                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("department");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.MemberName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("member_name");
                entity.Property(e => e.SysCountry)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("sys_country");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("updated_by");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
