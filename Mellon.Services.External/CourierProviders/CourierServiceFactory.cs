namespace Mellon.Services.External.CourierProviders
{
    public class CourierServiceFactory
    {
        private readonly IEnumerable<ICourierService> courierService;
        public CourierServiceFactory(IEnumerable<ICourierService> _courierService)
        {
            courierService = _courierService;
        }

        public ICourierService GetCourierService(CourierMode courierMode)
        {
            return courierService.FirstOrDefault(e => e.CourierMode == courierMode)
                ?? throw new NotSupportedException();
        }

    }
}
