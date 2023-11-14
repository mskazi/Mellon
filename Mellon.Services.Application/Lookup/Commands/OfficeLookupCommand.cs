using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Lookup.Commands
{
    public class GetVoucherTypeLookupCommand : IRequest<ListResult<TypeLookupResourse>>
    {
        public GetVoucherTypeLookupCommand()
        {
        }
    }

    public class GetVocuherConditionLookupCommand : IRequest<ListResult<ConditionLookupResourse>>
    {
        public GetVocuherConditionLookupCommand()
        {
        }
    }

    public class GetVoucherDepartmentLookupCommand : IRequest<ListResult<DepartmentLookupResourse>>
    {
        public GetVoucherDepartmentLookupCommand(VoucherCreateRoleType voucherCreateRoleType)
        {
            this.VoucherCreateRoleType = voucherCreateRoleType;
        }
        public VoucherCreateRoleType VoucherCreateRoleType { get; set; }
    }



    public class GetVoucherDeliveryTimeLookupCommand : IRequest<ListResult<DeliveryTimeLookupResourse>>
    {
        public GetVoucherDeliveryTimeLookupCommand()
        {
        }
    }

    public class GetElectraProjectOfficesLookupCommand : IRequest<ListResult<string>>
    {
        public GetElectraProjectOfficesLookupCommand()
        {
        }
    }
}
