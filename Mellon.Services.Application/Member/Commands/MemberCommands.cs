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
        public GetMembersServiceCommand(string term, ListPaging paging, ListOrder ordering, bool? isActive = false)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
            IsActive = isActive;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }

        public bool? IsActive { get; set; }


    }

    public class GetAllActiveMembersServiceCommand : IRequest<PaginatedListResult<MemberResource>>
    {
        public GetAllActiveMembersServiceCommand(string term, ListPaging paging, ListOrder ordering)
        {
            Term = term;
            Paging = paging;
            Ordering = ordering;
        }
        public string? Term { get; set; }
        public ListPaging Paging { get; }
        public ListOrder Ordering { get; }
    }

    public class GetAllActiveMembersByDepartmentCommand : IRequest<ListResult<MemberResource>>
    {
        public GetAllActiveMembersByDepartmentCommand(string company, string department)
        {
            Company = company;
            Department = department;
        }
        public string Company { get; set; }
        public string Department { get; set; }

    }

    public class GetAllActiveMembersCommand : IRequest<ListResult<MemberResource>>
    {
        public GetAllActiveMembersCommand()
        {
        }
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