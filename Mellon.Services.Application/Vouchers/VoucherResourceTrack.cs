using Mellon.Services.Common.resources;
using Mellon.Services.Common.resources.Couriers;

namespace Mellon.Services.Application.Vouchers
{
    public class VoucherTrack
    {
        public VoucherTrack(VoucherDetails voucherDetails, CourierTrackResource courierTrackResource)
        {
            this.VoucherDetails = voucherDetails;
            this.CourierTrackResource = courierTrackResource;
        }
        public VoucherDetails VoucherDetails { get; }
        public CourierTrackResource CourierTrackResource { get; }



    }
}
