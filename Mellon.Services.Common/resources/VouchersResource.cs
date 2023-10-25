namespace Mellon.Services.Common.resources
{
    public class VoucherServiceItem
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

    }
}
