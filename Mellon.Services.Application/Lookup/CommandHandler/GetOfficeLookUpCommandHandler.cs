using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Lookup.Commands;
using Mellon.Services.Infrastracture.Repositotiries;

namespace Mellon.Services.Application.Lookup.CommandHandler
{


    public class GetOfficeLookUpCommandHandler : IRequestHandler<GetOfficeTypeLookupCommand, ListResult<TypeLookupResourse>>,
                                                 IRequestHandler<GetOfficeConditionLookupCommand, ListResult<ConditionLookupResourse>>,
                                                 IRequestHandler<GetOfficeDepartmentLookupCommand, ListResult<DepartmentLookupResourse>>,
                                                 IRequestHandler<GetOfficeDeliveryTimeLookupCommand, ListResult<DeliveryTimeLookupResourse>>
    {
        private readonly ILookupRepository repository;

        public GetOfficeLookUpCommandHandler(ILookupRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ListResult<TypeLookupResourse>> Handle(GetOfficeTypeLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherTypes(cancellationToken);
            var items = entities.Select(entity => new TypeLookupResourse(entity));
            return new ListResult<TypeLookupResourse>(items);
        }

        public async Task<ListResult<ConditionLookupResourse>> Handle(GetOfficeConditionLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherConditions(cancellationToken);
            var items = entities.Select(entity => new ConditionLookupResourse(entity));
            return new ListResult<ConditionLookupResourse>(items);
        }

        public async Task<ListResult<DepartmentLookupResourse>> Handle(GetOfficeDepartmentLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherOfficeDepartments(cancellationToken);
            var items = entities.Select(entity => new DepartmentLookupResourse(entity));
            return new ListResult<DepartmentLookupResourse>(items);
        }


        public async Task<ListResult<DeliveryTimeLookupResourse>> Handle(GetOfficeDeliveryTimeLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherOfficeDeliveryTimes(cancellationToken);
            var items = entities.Select(entity => new DeliveryTimeLookupResourse(entity));
            return new ListResult<DeliveryTimeLookupResourse>(items);
        }
    }
}
