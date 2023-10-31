using Mellon.Services.Infrastracture.Models;

namespace Mellon.Services.Application.Lookup
{
    public class DepartmentLookupResourse : LookupStringResource
    {
        public DepartmentLookupResourse(Dim dim) : base(dim)
        {

        }
    }

    public class CompanyLookupResourse : LookupStringResource
    {
        public CompanyLookupResourse(Dim dim) : base(dim)
        {

        }
    }



    public class LookupStringResource
    {
        public LookupStringResource(Dim dim)
        {
            Code = dim?.ValueChar;
            Description = dim?.Description;
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }


}
