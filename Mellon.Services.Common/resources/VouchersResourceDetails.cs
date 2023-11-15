namespace Mellon.Services.Common.resources
{

    public class VoucherSummary
    {
        public int? CDelivered { get; set; }
        public int? PDelivered { get; set; }
        public int? CIntransit { get; set; }
        public int? PIntransit { get; set; }
        public int? CForPickup { get; set; }
        public int? CForVoucher { get; set; }
        public int? PForVoucher { get; set; }
        public int? CCancelledMellon { get; set; }
        public int? CCancelledCarrier { get; set; }


    }



    public class VoucherDetails
    {
        public int? Id { get; set; }
        public int? SystemStatus { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherContact { get; set; }
        public string? VoucherAddress { get; set; }
        public string? VoucherCity { get; set; }
        public string? VoucherPostCode { get; set; }
        public string? VoucherPhoneNo { get; set; }

        public string? VoucherMobileNo { get; set; }
        public string? VoucherDescription { get; set; }
        public string? SerialNo { get; set; }
        public string? SystemCompany { get; set; }
        public string? NavisionServiceOrderNo { get; set; }
        public string? NavisionSalesOrderNo { get; set; }
        public string? ActionTypeDescription { get; set; }

        public string? CarrierVoucherNo { get; set; }
        public string? StatusDescription { get; set; }
        public string? OrderedBy { get; set; }

        public string? MellonProject { get; set; }
        public string? CarrierName { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual IEnumerable<VoucherDataInability>? DataInabilities { get; set; }

        public virtual IEnumerable<VoucherDataLine>? DataLines { get; set; }

        public DateTime? CarrierPickupDate { get; set; }
        public DateTime? CarrierDeliveryDate { get; set; }
        public string? CarrierDeliveredTo { get; set; }
        public string? ConditionCode { get; set; }
        public bool? DeliverSaturday { get; set; }
        public string? DeliveryDescription { get; set; }
        public decimal? CODAmount { get; set; }
        public string? SystemDepertment { get; set; }
        public string? CarrierCode { get; set; }
        public string? CarrierDelivereyStatus { get; set; }
        public int CarrierId { get; set; }

        public int ElectraProjectId { get; set; }
        public int CarrierActionType { get; set; }

        public string? CarrierJobId { get; set; }


    }

    public partial class VoucherDataInability
    {
        public int Id { get; set; }

        public int? DataId { get; set; }

        public DateTime? TrnDate { get; set; }

        public string? Reason { get; set; }

    }

    public partial class VoucherDataLine
    {
        public int Id { get; set; }

        public int DataId { get; set; }

        public string Name { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public string UpdatedBy { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }


}
