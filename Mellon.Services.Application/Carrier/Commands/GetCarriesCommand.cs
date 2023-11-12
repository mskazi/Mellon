using MediatR;
using Mellon.Common.Services;

namespace Mellon.Services.Application.Carrier.Commands
{
    public class GetCarriersLookupCommand : IRequest<ListResult<CarrierLookupResourse>>
    {
        public GetCarriersLookupCommand(string postalCode)
        {
            PostalCode = postalCode;
        }

        public string PostalCode { get; }

    }

    public class GetCarrierPostalCodeCommand : IRequest<CarrierLookupResourse>
    {
        public GetCarrierPostalCodeCommand(string postalCode)
        {
            PostalCode = postalCode;
        }

        public string PostalCode { get; }

    }
}
