using Mellon.Common.Services;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface IContactsRepository : IRepository
    {
        Task<PaginatedListResult<OfficeContact>> GetContacts(int role, string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken);
        Task<OfficeContact> GetContact(int id, CancellationToken cancellationToken);
        void AddContact(OfficeContact member);
    }

    public class ContactsRepository : IContactsRepository
    {
        private readonly MellonContext context;
        public ContactsRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public void AddContact(OfficeContact contact)
        {
            this.context.OfficeContacts.Add(contact);
        }

        public async Task<OfficeContact> GetContact(int id, CancellationToken cancellationToken)
        {

            return await context.OfficeContacts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public async Task<PaginatedListResult<OfficeContact>> GetContacts(int role, string term, ListPaging paging, ListOrder order, CancellationToken cancellationToken)
        {

            paging ??= new ListPaging();
            int? total = null;

            var query = context.OfficeContacts.AsQueryable();
            query = query.Where(p => p.Flag0 == role);
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s => s.VoucherFrom.Contains(term) ||
                    s.VoucherName.Contains(term) ||
                   s.VoucherContact.Contains(term) ||
                   s.VoucherAddress.Contains(term) ||
                   s.VoucherCity.Contains(term) ||
                   s.VoucherPostCode.Contains(term) ||
                   s.VoucherCountry.Contains(term) ||
                   s.VoucherPhoneNo.Contains(term) ||
                   s.VoucherMobileNo.Contains(term) ||
                   s.SysCountry.Contains(term)
                );
            }
            total = await query.CountAsync(cancellationToken);

            //apply ordering
            if (order?.Properties?.Any() ?? false)
            {
                query = query.OrderBy(order?.Properties);
            }

            var results = await query.Skip(paging.Start)
               .Take(paging.Length)
               .ToListAsync(cancellationToken);

            return new PaginatedListResult<OfficeContact>(
               paging.Start,
               paging.Length,
               total,
               results);

        }
    }
}
