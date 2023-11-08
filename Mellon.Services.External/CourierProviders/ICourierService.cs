using Mellon.Services.Common.resources;
using Mellon.Services.Common.resources.Couriers;

namespace Mellon.Services.External.CourierProviders
{
    public interface ICourierService
    {
        CourierMode CourierMode
        {
            get;
        }
        void Print();
        Task<CourierTrackResource> Track(VoucherDetails voucherDetails);

    }

    public enum CourierMode
    {
        GenikiTaxydromiki = 1
    }
}
