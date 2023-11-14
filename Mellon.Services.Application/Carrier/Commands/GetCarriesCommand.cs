using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Carrier.Commands
{
    public class GetCarriersLookupCommand : IRequest<ListResult<CarrierLookupResourse>>
    {
        public GetCarriersLookupCommand(string postalCode, VoucherCreateRoleType voucherCreateRoleType)
        {
            PostalCode = postalCode;
            voucherCreateRoleType = voucherCreateRoleType;

        }

        public string PostalCode { get; }
        public VoucherCreateRoleType VoucherCreateRoleType { get; }


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
