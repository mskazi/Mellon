using MediatR;
using Mellon.Common.Services;

namespace Mellon.Services.Application.Lookup.Commands
{
    public class GetOfficeTypeLookupCommand : IRequest<ListResult<TypeLookupResourse>>
    {
        public GetOfficeTypeLookupCommand()
        {
        }
    }

    public class GetOfficeConditionLookupCommand : IRequest<ListResult<ConditionLookupResourse>>
    {
        public GetOfficeConditionLookupCommand()
        {
        }
    }

    public class GetOfficeDepartmentLookupCommand : IRequest<ListResult<DepartmentLookupResourse>>
    {
        public GetOfficeDepartmentLookupCommand()
        {
        }
    }



    public class GetOfficeDeliveryTimeLookupCommand : IRequest<ListResult<DeliveryTimeLookupResourse>>
    {
        public GetOfficeDeliveryTimeLookupCommand()
        {
        }
    }
}
