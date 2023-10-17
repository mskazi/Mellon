using System;
using System.Collections.Generic;

namespace Mellon.Services.Infrastracture.Models;

public partial class DataUpload
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

    public string NavisionSalesOrderNo { get; set; } = null!;

    public string NavisionServiceOrderNo { get; set; } = null!;

    public DateTime? NavisionServiceOrderDate { get; set; }

    public string NavisionSellToCustomerNo { get; set; } = null!;

    public string NavisionLinkedDocumentNo { get; set; } = null!;

    public DateTime? NavisionLinkedDocumentDate { get; set; }

    public int CarrierId { get; set; }

    public int CarrierActionType { get; set; }

    public string CarrierVoucherNo { get; set; } = null!;

    public string CarrierDeliveryStatus { get; set; } = null!;

    public DateTime? CarrierPickupDate { get; set; }

    public DateTime? CarrierDeliveryDate { get; set; }

    public string CarrierDeliveredTo { get; set; } = null!;

    public decimal CarrierPackageItems { get; set; }

    public decimal CarrierPackageWeight { get; set; }

    public int ElectraProjectId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Carrier Carrier { get; set; } = null!;
}
