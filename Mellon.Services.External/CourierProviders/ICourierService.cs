using Mellon.Services.Common.resources;
using Mellon.Services.Common.resources.Couriers;
using Mellon.Services.Infrastracture.Models;

namespace Mellon.Services.External.CourierProviders
{
    public interface ICourierService
    {
        public ElectraProjectSetup ProjectSetup { get; set; }
        public CourierMode CourierMode
        {
            get;
        }
        public Task<Stream> Print(IEnumerable<string> vouchers, CancellationToken cancellation);
        public Task<CourierTrackResource> Track(VoucherDetails voucherDetails, CancellationToken cancellation);

        public Task<CourierCreateResource> Create(Datum voucherData, CancellationToken cancellation);

        public Task<Boolean> Cancel(VoucherDetails voucherDetails, CancellationToken cancellation);


    }

    public enum CourierMode
    {
        GenikiTaxydromiki = 1
    }


}
