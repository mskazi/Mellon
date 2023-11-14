using Mellon.Common.Services;

namespace Mellon.Services.Application.Vouchers
{
    public class CreateVoucherRequestData
    {
        public int? VoucherAction { get; set; }
        public int VoucherCarrier { get; set; }
        public string? VoucherCompany { get; set; }
        public string? VoucherDepartment { get; set; }
        public int? VoucherMember { get; set; }
        public string? VoucherType { get; set; }
        public string? VoucherDeliverTo { get; set; }
        public decimal? VoucherWeight { get; set; }
        public int VoucherQuantities { get; set; }
        public string? Comments { get; set; }
        public string? VoucherReference { get; set; }
        public string? VoucherCondition { get; set; }
        public decimal? VoucherCodAmount { get; set; }
        public int? VoucherSaturdayDelivery { get; set; }
        public int? VoucherDeliveryTime { get; set; }

        protected virtual void Validate()

        {
            Guards.StringNotNullOrEmpty(VoucherCompany, nameof(VoucherCompany));
            Guards.StringNotNullOrEmpty(VoucherDepartment, nameof(VoucherDepartment));
            Guards.ValidIdentifier(VoucherMember, nameof(VoucherMember));
        }
    }

    public class CreateVoucherServiceScanData
    {
        public string? ScanSerial { get; set; }
        public int? VoucherSaturdayDelivery { get; set; }
        public int? VoucherDeliveryTime { get; set; }

        protected virtual void Validate()

        {
            Guards.StringNotNullOrEmpty(ScanSerial, nameof(ScanSerial));
        }
    }
}

