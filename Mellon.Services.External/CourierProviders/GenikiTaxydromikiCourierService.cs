namespace Mellon.Services.External.CourierProviders
{
    public class GenikiTaxydromikiCourierService : ICourierService
    {
        //public GenikiTaxydromikiCourierService(IConfiguration configuration, ILogger<GenikiTaxydromikiCourierService> logger)
        //{
        //    if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        //}

        public CourierMode CourierMode => CourierMode.GenikiTaxydromiki;

        public void Print()
        {
            throw new NotImplementedException();
        }


        private void Connect()
        {

        }
    }
}
