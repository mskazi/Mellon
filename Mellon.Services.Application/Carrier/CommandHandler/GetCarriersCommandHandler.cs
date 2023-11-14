using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Carrier.Commands;
using Mellon.Services.Infrastracture.Repositotiries;

namespace Mellon.Services.Application.Carrier.CommandHandler
{
    public class GetCarrriersCommmandHandler :
                                               IRequestHandler<GetCarriersLookupCommand, ListResult<CarrierLookupResourse>>,
           IRequestHandler<GetCarrierPostalCodeCommand, CarrierLookupResourse>
    {
        private readonly ICarriersRepository repository;

        public GetCarrriersCommmandHandler(ICarriersRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public async Task<ListResult<CarrierLookupResourse>> Handle(GetCarriersLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherCarriers(request.PostalCode, request.VoucherCreateRoleType, cancellationToken);
            var items = entities.Select(entity => new CarrierLookupResourse(entity));
            return new ListResult<CarrierLookupResourse>(items);
        }

        public async Task<CarrierLookupResourse> Handle(GetCarrierPostalCodeCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetVoucherCarrierByPostalCode(request.PostalCode, cancellationToken);
            return new CarrierLookupResourse(entity);
        }
    }
}
