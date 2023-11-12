using System;
using System.Collections.Generic;

namespace Mellon.Services.Api.Models;

public partial class DataSubvoucher
{
    public int Id { get; set; }

    public int? DataId { get; set; }

    public string? PrimaryVoucherNo { get; set; }

    public string? CarrierVoucherNo { get; set; }

    public DateTime? CarrierDeliveryDate { get; set; }

    public DateTime? CarrierPickupDate { get; set; }

    public string? CarrierDeliveredTo { get; set; }

    public string? CarrierDeliveryStatus { get; set; }
}
