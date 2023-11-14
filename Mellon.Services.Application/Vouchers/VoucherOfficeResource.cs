using Mellon.Services.Infrastracture.Models;

namespace Mellon.Services.Application.Vouchers
{
    public class VoucherOfficePrintResource
    {
        public VoucherOfficePrintResource(Datum voucher)
        {
            this.VoucherName = voucher.VoucherName;
            this.VoucherContact = voucher.VoucherContact;
            this.VoucherCity = voucher.VoucherCity;
            this.VoucherNo = voucher.CarrierVoucherNo;
        }

        public string VoucherName { get; }

        public string VoucherContact { get; }

        public string VoucherCity { get; }

        public string VoucherNo { get; }


    }
}
