using MediatR;
using Mellon.Common.Services;

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
        public GeCompanyLookupCommand()
        {
        }
    }


    public class GeCountryLookupCommand : IRequest<ListResult<CountryLookupResourse>>
    {
        public GeCountryLookupCommand()
        {
        }
    }


}