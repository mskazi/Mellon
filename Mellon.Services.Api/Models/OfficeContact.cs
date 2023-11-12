using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class OfficeContact
{
    public int Id { get; set; }

    public string? VoucherFrom { get; set; }

    public string? VoucherName { get; set; }

    public string? VoucherContact { get; set; }

    public string VoucherAddress { get; set; } = null!;

    public string VoucherCity { get; set; } = null!;

    public string VoucherPostCode { get; set; } = null!;

    public string VoucherCountry { get; set; } = null!;

    public string? VoucherPhoneNo { get; set; }

    public string? VoucherMobileNo { get; set; }

    public string? ContactNotes { get; set; }

    public string SysCompany { get; set; } = null!;

    public bool Active { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? SysCountry { get; set; }

    public string? TerminalId { get; set; }

    public string? SerialNo { get; set; }

    public string? BankMerchantId { get; set; }

    public string? MerchantName { get; set; }

    public string? MerchantAddress { get; set; }

    public string? MerchantCity { get; set; }

    public string? MerchantPostcode { get; set; }

    public string? MerchantPhone { get; set; }

    public string? MerchantFax { get; set; }

    public int? Flag0 { get; set; }

    public string? CustomerNo { get; set; }

    public int? Flag1 { get; set; }

    public string? MerchantComment { get; set; }

    public string? ItemNo { get; set; }

    public string? SrvOrderNo { get; set; }

    public DateTime? UploadedAt { get; set; }

    public string? SalesOrderNo { get; set; }

    public bool? EmailNotifyWhSent { get; set; }
}
