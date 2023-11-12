using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class Datum
{
    public int Id { get; set; }

    public string VoucherName { get; set; } = null!;

    public string? VoucherContact { get; set; }

    public string VoucherAddress { get; set; } = null!;

    public string VoucherCity { get; set; } = null!;

    public string VoucherPostCode { get; set; } = null!;

    public string VoucherCountry { get; set; } = null!;

    public string? VoucherPhoneNo { get; set; }

    public string? VoucherMobileNo { get; set; }

    public string? VoucherDescription { get; set; }

    public string SysSource { get; set; } = null!;

    public string SysType { get; set; } = null!;

    public string SysCompany { get; set; } = null!;

    public string SysDepartment { get; set; } = null!;

    public int SysStatus { get; set; }

    public bool SysCheck { get; set; }

    public string? NavisionSalesOrderNo { get; set; }

    public string? NavisionServiceOrderNo { get; set; }

    public DateTime? NavisionServiceOrderDate { get; set; }

    public string? NavisionSellToCustomerNo { get; set; }

    public string? NavisionLinkedDocumentNo { get; set; }

    public DateTime? NavisionLinkedDocumentDate { get; set; }

    public int CarrierId { get; set; }

    public int CarrierActionType { get; set; }

    public string? CarrierVoucherNo { get; set; }

    public string? CarrierDeliveryStatus { get; set; }

    public DateTime? CarrierPickupDate { get; set; }

    public DateTime? CarrierDeliveryDate { get; set; }

    public string? CarrierDeliveredTo { get; set; }

    public decimal CarrierPackageItems { get; set; }

    public decimal CarrierPackageWeight { get; set; }

    public int ElectraProjectId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int OldId { get; set; }

    public string? SysCountry { get; set; }

    public int? VoucherSpecialTreatment { get; set; }

    public int? VoucherScheduledDelivery { get; set; }

    public string? ConditionCode { get; set; }

    public bool? DeliverSaturday { get; set; }

    public int? DeliverTime { get; set; }

    public decimal? CodAmount { get; set; }

    public int? OrderedBy { get; set; }

    public string? CarrierJobid { get; set; }

    public string? Barcode { get; set; }

    public int? ErrorCounter { get; set; }

    public int? VoucherPrintType { get; set; }

    public virtual Carrier Carrier { get; set; } = null!;

    public virtual ICollection<DataInability> DataInabilities { get; set; } = new List<DataInability>();

    public virtual ICollection<DataLine> DataLines { get; set; } = new List<DataLine>();
}
