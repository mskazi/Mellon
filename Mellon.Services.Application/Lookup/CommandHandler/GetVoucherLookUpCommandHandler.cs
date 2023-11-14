using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Lookup.Commands;
using Mellon.Services.Infrastracture.Repositotiries;

namespace Mellon.Services.Application.Lookup.CommandHandler
{


    public class GetVoucherLookUpCommandHandler : IRequestHandler<GetVoucherTypeLookupCommand, ListResult<TypeLookupResourse>>,
                                                 IRequestHandler<GetVocuherConditionLookupCommand, ListResult<ConditionLookupResourse>>,
                                                 IRequestHandler<GetVoucherDepartmentLookupCommand, ListResult<DepartmentLookupResourse>>,
                                                 IRequestHandler<GetVoucherDeliveryTimeLookupCommand, ListResult<DeliveryTimeLookupResourse>>
    {
        private readonly ILookupRepository repository;

        public GetVoucherLookUpCommandHandler(ILookupRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ListResult<TypeLookupResourse>> Handle(GetVoucherTypeLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherTypes(cancellationToken);
            var items = entities.Select(entity => new TypeLookupResourse(entity));
            return new ListResult<TypeLookupResourse>(items);
        }

        public async Task<ListResult<ConditionLookupResourse>> Handle(GetVocuherConditionLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherConditions(cancellationToken);
            var items = entities.Select(entity => new ConditionLookupResourse(entity));
            return new ListResult<ConditionLookupResourse>(items);
        }

        public async Task<ListResult<DepartmentLookupResourse>> Handle(GetVoucherDepartmentLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherDepartments(request.VoucherCreateRoleType, cancellationToken);
            var items = entities.Select(entity => new DepartmentLookupResourse(entity));
            return new ListResult<DepartmentLookupResourse>(items);
        }


        public async Task<ListResult<DeliveryTimeLookupResourse>> Handle(GetVoucherDeliveryTimeLookupCommand request, CancellationToken cancellationToken)
        {
            var entities = await repository.GetVoucherDeliveryTimes(cancellationToken);
            var items = entities.Select(entity => new DeliveryTimeLookupResourse(entity));
            return new ListResult<DeliveryTimeLookupResourse>(items);
        }
    }
}
