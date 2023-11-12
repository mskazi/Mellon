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

    public class ConditionLookupResourse : LookupStringResource
    {
        public ConditionLookupResourse(Dim dim) : base(dim)
        {

        }
    }

    public class CountryLookupResourse : LookupStringResource
    {
        public CountryLookupResourse(Country country) : base()
        {
            Id = country.Iso;
            Description = country.Country1;
        }
    }

    public class LookupStringResource
    {
        public LookupStringResource()
        {

        }
        public LookupStringResource(Dim dim)
        {
            Id = dim?.ValueChar;
            Description = dim?.Description;
        }

        public string Id { get; set; }

        public string Description { get; set; }
    }

    public class LookupIntResource
    {
        public LookupIntResource()
        {

        }
        public LookupIntResource(Dim dim)
        {
            Id = dim.ValueInt;
            Description = dim.Description;
        }

        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class TypeLookupResourse : LookupIntResource
    {
        public TypeLookupResourse(Dim dim) : base(dim)
        {

        }
    }

    public class DeliveryTimeLookupResourse : LookupIntResource
    {
        public DeliveryTimeLookupResourse(Dim dim) : base(dim)
        {

        }
    }






}
