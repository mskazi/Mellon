namespace Mellon.Services.External.CourierProviders
{
    public interface ICourierService
    {
        CourierMode CourierMode
        {
            get;
        }
        void Print();
    }

    public enum CourierMode
    {
        GenikiTaxydromiki = 4
    }
}
