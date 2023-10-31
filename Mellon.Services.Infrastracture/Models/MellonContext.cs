using Mellon.Services.Infrastracture.Base;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mellon.Services.Infrastracture.Models;

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

    public virtual DbSet<ElectraProjectSetup> ElectraProjectSetups { get; set; }

    public virtual DbSet<FromErp> FromErps { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Returned> Returneds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=erptestdbgr.mellongroup.com;Database=MELLON_INTERNAL_APPS; Integrated Security=false; User ID=erpportal;Password=1234ep;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.ToTable("carriers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DescrLong)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descr_long");
            entity.Property(e => e.DescrShort)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descr_short");
            entity.Property(e => e.OrFlag).HasColumnName("or_flag");
            entity.Property(e => e.SysCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sys_country");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<CarriersPostcodeRestriction>(entity =>
        {
            entity.ToTable("carriers_postcode_restrictions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.PostCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("post_code");

            entity.HasOne(d => d.Carrier).WithMany(p => p.CarriersPostcodeRestrictions)
                .HasForeignKey(d => d.CarrierId)
                .HasConstraintName("FK_carriers_postcode_restrictions_carriers");
        });

        modelBuilder.Entity<CarriersProjectRestriction>(entity =>
        {
            entity.ToTable("carriers_project_restrictions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.Project)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("project");

            entity.HasOne(d => d.Carrier).WithMany(p => p.CarriersProjectRestrictions)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_carriers_project_restrictions_carriers");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Iso);

            entity.ToTable("countries");

            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("iso");
            entity.Property(e => e.Country1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<DataCancellation>(entity =>
        {
            entity.ToTable("data_cancellations");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CancellationReason)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cancellation_reason");
            entity.Property(e => e.CancellationStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cancellation_status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ElectraId).HasColumnName("electra_id");
        });

        modelBuilder.Entity<DataInability>(entity =>
        {
            entity.ToTable("data_inabilities");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DataId).HasColumnName("data_id");
            entity.Property(e => e.Reason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("reason");
            entity.Property(e => e.TrnDate)
                .HasColumnType("date")
                .HasColumnName("trn_date");

            entity.HasOne(d => d.Data).WithMany(p => p.DataInabilities)
                .HasForeignKey(d => d.DataId)
                .HasConstraintName("FK_data_inabilities_data");
        });

        modelBuilder.Entity<DataLine>(entity =>
        {
            entity.ToTable("data_lines");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DataId).HasColumnName("data_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.Value)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("value");

            entity.HasOne(d => d.Data).WithMany(p => p.DataLines)
                .HasForeignKey(d => d.DataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_data_lines_data");
        });

        modelBuilder.Entity<DataSubvoucher>(entity =>
        {
            entity.ToTable("data_subvouchers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CarrierDeliveredTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("carrier_delivered_to");
            entity.Property(e => e.CarrierDeliveryDate)
                .HasColumnType("date")
                .HasColumnName("carrier_delivery_date");
            entity.Property(e => e.CarrierDeliveryStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_delivery_status");
            entity.Property(e => e.CarrierPickupDate)
                .HasColumnType("date")
                .HasColumnName("carrier_pickup_date");
            entity.Property(e => e.CarrierVoucherNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_voucher_no");
            entity.Property(e => e.DataId).HasColumnName("data_id");
            entity.Property(e => e.PrimaryVoucherNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("primary_voucher_no");
        });

        modelBuilder.Entity<DataUpload>(entity =>
        {
            entity.ToTable("data_upload");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CarrierActionType).HasColumnName("carrier_action_type");
            entity.Property(e => e.CarrierDeliveredTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("carrier_delivered_to");
            entity.Property(e => e.CarrierDeliveryDate)
                .HasColumnType("date")
                .HasColumnName("carrier_delivery_date");
            entity.Property(e => e.CarrierDeliveryStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_delivery_status");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.CarrierPackageItems)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("carrier_package_items");
            entity.Property(e => e.CarrierPackageWeight)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("carrier_package_weight");
            entity.Property(e => e.CarrierPickupDate)
                .HasColumnType("date")
                .HasColumnName("carrier_pickup_date");
            entity.Property(e => e.CarrierVoucherNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_voucher_no");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.ElectraProjectId).HasColumnName("electra_project_id");
            entity.Property(e => e.NavisionLinkedDocumentDate)
                .HasColumnType("date")
                .HasColumnName("navision_linked_document_date");
            entity.Property(e => e.NavisionLinkedDocumentNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_linked_document_no");
            entity.Property(e => e.NavisionSalesOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_sales_order_no");
            entity.Property(e => e.NavisionSellToCustomerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_sell_to_customer_no");
            entity.Property(e => e.NavisionServiceOrderDate)
                .HasColumnType("date")
                .HasColumnName("navision_service_order_date");
            entity.Property(e => e.NavisionServiceOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_service_order_no");
            entity.Property(e => e.SysCheck).HasColumnName("sys_check");
            entity.Property(e => e.SysCompany)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("sys_company");
            entity.Property(e => e.SysDepartment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sys_department");
            entity.Property(e => e.SysSource)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sys_source");
            entity.Property(e => e.SysStatus).HasColumnName("sys_status");
            entity.Property(e => e.SysType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sys_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.VoucherAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_address");
            entity.Property(e => e.VoucherCity)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_city");
            entity.Property(e => e.VoucherContact)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_contact");
            entity.Property(e => e.VoucherCountry)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("voucher_country");
            entity.Property(e => e.VoucherDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("voucher_description");
            entity.Property(e => e.VoucherMobileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_mobile_no");
            entity.Property(e => e.VoucherName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_name");
            entity.Property(e => e.VoucherPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_phone_no");
            entity.Property(e => e.VoucherPostCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("voucher_post_code");

            entity.HasOne(d => d.Carrier).WithMany(p => p.DataUploads)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_data_upload_carriers");
        });

        modelBuilder.Entity<Datum>(entity =>
        {
            entity.ToTable("data");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("barcode");
            entity.Property(e => e.CarrierActionType).HasColumnName("carrier_action_type");
            entity.Property(e => e.CarrierDeliveredTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("carrier_delivered_to");
            entity.Property(e => e.CarrierDeliveryDate)
                .HasColumnType("date")
                .HasColumnName("carrier_delivery_date");
            entity.Property(e => e.CarrierDeliveryStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_delivery_status");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.CarrierJobid)
                .HasColumnType("text")
                .HasColumnName("carrier_jobid");
            entity.Property(e => e.CarrierPackageItems)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("carrier_package_items");
            entity.Property(e => e.CarrierPackageWeight)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("carrier_package_weight");
            entity.Property(e => e.CarrierPickupDate)
                .HasColumnType("date")
                .HasColumnName("carrier_pickup_date");
            entity.Property(e => e.CarrierVoucherNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_voucher_no");
            entity.Property(e => e.CodAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cod_amount");
            entity.Property(e => e.ConditionCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("condition_code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeliverSaturday).HasColumnName("deliver_saturday");
            entity.Property(e => e.DeliverTime).HasColumnName("deliver_time");
            entity.Property(e => e.ElectraProjectId).HasColumnName("electra_project_id");
            entity.Property(e => e.ErrorCounter).HasColumnName("error_counter");
            entity.Property(e => e.NavisionLinkedDocumentDate)
                .HasColumnType("date")
                .HasColumnName("navision_linked_document_date");
            entity.Property(e => e.NavisionLinkedDocumentNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_linked_document_no");
            entity.Property(e => e.NavisionSalesOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_sales_order_no");
            entity.Property(e => e.NavisionSellToCustomerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_sell_to_customer_no");
            entity.Property(e => e.NavisionServiceOrderDate)
                .HasColumnType("date")
                .HasColumnName("navision_service_order_date");
            entity.Property(e => e.NavisionServiceOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navision_service_order_no");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OrderedBy).HasColumnName("ordered_by");
            entity.Property(e => e.SysCheck).HasColumnName("sys_check");
            entity.Property(e => e.SysCompany)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sys_company");
            entity.Property(e => e.SysCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sys_country");
            entity.Property(e => e.SysDepartment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sys_department");
            entity.Property(e => e.SysSource)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sys_source");
            entity.Property(e => e.SysStatus).HasColumnName("sys_status");
            entity.Property(e => e.SysType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sys_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.VoucherAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_address");
            entity.Property(e => e.VoucherCity)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_city");
            entity.Property(e => e.VoucherContact)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("voucher_contact");
            entity.Property(e => e.VoucherCountry)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("voucher_country");
            entity.Property(e => e.VoucherDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("voucher_description");
            entity.Property(e => e.VoucherMobileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_mobile_no");
            entity.Property(e => e.VoucherName)
                .HasMaxLength(600)
                .IsUnicode(false)
                .HasColumnName("voucher_name");
            entity.Property(e => e.VoucherPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_phone_no");
            entity.Property(e => e.VoucherPostCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("voucher_post_code");
            entity.Property(e => e.VoucherPrintType).HasColumnName("voucher_print_type");
            entity.Property(e => e.VoucherScheduledDelivery).HasColumnName("voucher_scheduled_delivery");
            entity.Property(e => e.VoucherSpecialTreatment).HasColumnName("voucher_special_treatment");

            entity.HasOne(d => d.Carrier).WithMany(p => p.Data)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_data_carriers");
        });

        modelBuilder.Entity<Dim>(entity =>
        {
            entity.ToTable("dims");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Description2)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("description_2");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SysCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sys_country");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.ValueChar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("value_char");
            entity.Property(e => e.ValueInt).HasColumnName("value_int");
        });

        modelBuilder.Entity<ElectraProjectSetup>(entity =>
        {
            entity.ToTable("electra_project_setup");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_code");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.CarrierKey)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("carrier_key");
            entity.Property(e => e.CarrierPassword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("carrier_password");
            entity.Property(e => e.CarrierUsername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("carrier_username");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsPallet).HasColumnName("is_pallet");
            entity.Property(e => e.MellonProject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mellon_project");
            entity.Property(e => e.MellonProjectMaster)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mellon_project_master");
            entity.Property(e => e.SysCompany)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sys_company");
            entity.Property(e => e.SysCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sys_country");
            entity.Property(e => e.SysDepartment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sys_department");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FromErp>(entity =>
        {
            entity.ToTable("from_erp");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BillToCustomerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bill_to_customer_no");
            entity.Property(e => e.CarrierFlag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("carrier_flag");
            entity.Property(e => e.CourierServiceType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("courier_service_type");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.DeliveryNoteNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("delivery_note_no");
            entity.Property(e => e.LinkedDocNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("linked_doc_no");
            entity.Property(e => e.LinkedPostDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("linked_post_date");
            entity.Property(e => e.MellonCourierProject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mellon_courier_project");
            entity.Property(e => e.No)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("no_");
            entity.Property(e => e.OrderDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("order_date");
            entity.Property(e => e.PostingDescription)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("posting_description");
            entity.Property(e => e.SellToCustomerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sell_to_customer_no");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("serial_no");
            entity.Property(e => e.ServiceOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("service_order_no");
            entity.Property(e => e.ServiceOrderType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("service_order_type");
            entity.Property(e => e.ShipToAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_address");
            entity.Property(e => e.ShipToAddress2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_address2");
            entity.Property(e => e.ShipToCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_city");
            entity.Property(e => e.ShipToContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_contact");
            entity.Property(e => e.ShipToCountryCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ship_to_country_code");
            entity.Property(e => e.ShipToCounty)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_county");
            entity.Property(e => e.ShipToMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_mobile");
            entity.Property(e => e.ShipToName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_name");
            entity.Property(e => e.ShipToName2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_name2");
            entity.Property(e => e.ShipToPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_phone");
            entity.Property(e => e.ShipToPostCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ship_to_post_code");
            entity.Property(e => e.ShipmentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("shipment_date");
            entity.Property(e => e.ShipmentMethodCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shipment_method_code");
            entity.Property(e => e.TerminalId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("terminal_id");
        });

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

        modelBuilder.Entity<Returned>(entity =>
        {
            entity.ToTable("returned");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BankMerchantId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank_merchant_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CustomerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer_no");
            entity.Property(e => e.EmailNotifyWhSent).HasColumnName("email_notify_wh_sent");
            entity.Property(e => e.Flag0).HasColumnName("flag_0");
            entity.Property(e => e.Flag1).HasColumnName("flag_1");
            entity.Property(e => e.ItemNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("item_no");
            entity.Property(e => e.MerchantAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_address");
            entity.Property(e => e.MerchantCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_city");
            entity.Property(e => e.MerchantComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_comment");
            entity.Property(e => e.MerchantFax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_fax");
            entity.Property(e => e.MerchantName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_name");
            entity.Property(e => e.MerchantPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_phone");
            entity.Property(e => e.MerchantPostcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("merchant_postcode");
            entity.Property(e => e.SalesOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sales_order_no");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("serial_no");
            entity.Property(e => e.SrvOrderNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("srv_order_no");
            entity.Property(e => e.TerminalId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("terminal_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.UploadedAt)
                .HasColumnType("datetime")
                .HasColumnName("uploaded_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
