using Mellon.Services.Infrastracture.Models;

namespace Mellon.Services.External.CourierProviders
{
    public class CourierServiceFactory
    {
        private readonly IEnumerable<ICourierService> courierServices;
        public CourierServiceFactory(IEnumerable<ICourierService> _courierService)
        {
            courierServices = _courierService;
        }

        public ICourierService GetCourierService(CourierMode courierMode, ElectraProjectSetup elactraProjectSetup)
        {
            ICourierService courierService = courierServices.FirstOrDefault(e => e.CourierMode == courierMode)
               ?? throw new NotSupportedException();
            courierService.ProjectSetup = elactraProjectSetup;
            return courierService;
        }

    }
}
