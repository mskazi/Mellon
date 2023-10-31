using MediatR;
using Mellon.Common.Services;
using Mellon.Services.Application.Services;

namespace Mellon.Services.Application.Member
{
    public class GetCurrentMemberCommand : IRequest<ElectraUser>
    {
    }

    public class GetMembersServiceCommand : IRequest<PaginatedListResult<MemberResource>>
    {
        public GetMembersServiceCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class GetMemberCommand : IRequest<MemberResource>
    {
        public GetMemberCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class MemberCreateCommand : MemberResourceData, IRequest<MemberResource>
    {
        public MemberCreateCommand(string memberName, string department, string company, string country, bool isActive)
        {
            MemberName = memberName;
            Department = department;
            SysCountry = country;
            Company = company;
            IsActive = isActive;
            Validate();
        }
    }

    public class MemberUpdateCommand : MemberResource, IRequest<MemberResource>
    {
        public MemberUpdateCommand(int id, string memberName, string department, string company, string country, bool isActive)
        {
            Id = id;
            MemberName = memberName;
            Department = department;
            SysCountry = country;
            Company = company;
            IsActive = isActive;
            Validate();
        }
    }
}