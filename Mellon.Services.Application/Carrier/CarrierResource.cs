namespace Mellon.Services.Application.Carrier
{
    public class CarrierLookupResourse
    {
        public CarrierLookupResourse(Mellon.Services.Infrastracture.Models.Carrier carrier)
        {
            Id = carrier.Id;
            Description = carrier.DescrLong;
            DescriptionShort = carrier.DescrShort;
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string DescriptionShort { get; set; }

    }
}
