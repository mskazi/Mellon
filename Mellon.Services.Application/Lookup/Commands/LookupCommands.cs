using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;

namespace Mellon.Services.Application.Lookup
{
    public class GetDepartmentookupCommand : IRequest<ListResult<DepartmentLookupResourse>>
    {
        public GetDepartmentookupCommand()
        {
        }
    }

    public class GeCompanyLookupCommand : IRequest<ListResult<CompanyLookupResourse>>
    {
        public GeCompanyLookupCommand(VoucherCreateRoleType voucherCreateRoleType)
        {
            this.VouucherCreateRoleType = voucherCreateRoleType;
        }
        public VoucherCreateRoleType VouucherCreateRoleType { get; }
    }


    public class GeCountryLookupCommand : IRequest<ListResult<CountryLookupResourse>>
    {
        public GeCountryLookupCommand()
        {
        }
    }


}