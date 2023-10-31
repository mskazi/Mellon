
using Mellon.Common.Services;

namespace Mellon.Services.Application
{
    public class MemberResource : MemberResourceData
    {
        public MemberResource() { }

        public MemberResource(Mellon.Services.Infrastracture.Models.Member member)
            : base(member)
        {
            Id = member.Id;
        }

        public int Id { get; set; }

        protected override void Validate()
        {
            base.Validate();
            Guards.ValidIdentifier(Id);
        }
    }

    public class MemberResourceData
    {
        public MemberResourceData() { }

        public MemberResourceData(Mellon.Services.Infrastracture.Models.Member member)
        {
            Company = member?.Company;
            MemberName = member?.MemberName;
            Department = member?.Department;
            SysCountry = member?.SysCountry;
            IsActive = member?.IsActive ?? true;
        }


        public string Company { get; set; }

        public string Department { get; set; }

        public string MemberName { get; set; }

        public bool IsActive { get; set; }

        public string SysCountry { get; set; }

        protected virtual void Validate()
        {
            Guards.StringNotNullOrEmpty(Company, nameof(Company));
            Guards.StringMaximumLength(Company, nameof(Company), 50);
            Guards.StringNotNullOrEmpty(Department, nameof(Department));
            Guards.StringMaximumLength(Department, nameof(Department), 50);
            Guards.StringNotNullOrEmpty(MemberName, nameof(MemberName));
            Guards.StringMaximumLength(MemberName, nameof(MemberName), 50);
            Guards.StringNotNullOrEmpty(SysCountry, nameof(SysCountry));
            Guards.StringMaximumLength(SysCountry, nameof(SysCountry), 2);
        }
    }
}
