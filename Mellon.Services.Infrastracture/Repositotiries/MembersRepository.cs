using Mellon.Common.Services;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface IMembersRepository : IRepository
    {
        Member GetMember(string name);
        Task<PaginatedListResult<Member>> GetMembers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);
        Task<Member> GetMemberById(int id, CancellationToken cancellationToken);
        void AddMember(Member member);
    }

    public class MembersRepository : IMembersRepository
    {
        private readonly MellonContext context;
        public MembersRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public void AddMember(Member member)
        {
            this.context.Members.Add(member);
        }

        public Member GetMember(string name)
        {
            return context.Members.FirstOrDefault(x => x.MemberName == name);
        }

        public async Task<Member> GetMemberById(int id, CancellationToken cancellationToken)
        {
            return await context.Members.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public async Task<PaginatedListResult<Member>> GetMembers(string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {
            paging ??= new ListPaging();
            int? total = null;

            var query = context.Members.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s => s.MemberName.Contains(term) ||
                    s.Company.Contains(term) ||
                   s.Department.Contains(term) ||
                   s.SysCountry.Contains(term)
                );
            }
            total = await query.CountAsync(cancellationToken);

            //apply ordering
            if (order?.Properties?.Any() ?? false)
            {
                //var listOrder = new ListOrder();
                //foreach (var prop in order.Properties)
                //{
                //    switch (prop.Name)
                //    {
                //        case "id": prop.Name = "data.Id"; break;
                //        case "memberName": prop.Name = "MemberName"; break;
                //    }
                //}

                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
               .Take(paging.Length)
               .ToListAsync(cancellationToken);

            return new PaginatedListResult<Member>(
               paging.Start,
               paging.Length,
               total,
               results);

        }
    }
}
