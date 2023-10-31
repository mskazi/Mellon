using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;

namespace Mellon.Services.Infrastracture.Repositotiries
{
    public interface ILookupRepository : IRepository
    {
        Task<IEnumerable<Dim>> GetDepartments(CancellationToken cancellationToken);

        Task<IEnumerable<Dim>> GetCompanies(string country, CancellationToken cancellationToken);


    }

    public class LookupRepository : ILookupRepository
    {
        private readonly MellonContext context;
        public LookupRepository(MellonContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork UnitOfWork => context;

        public async Task<IEnumerable<Dim>> GetCompanies(string country, CancellationToken cancellationToken)
        {
            return await this.context.Dims
                 .Where(p => p.Name == "sys_company" && p.SysCountry == country)
                 .OrderBy(o => o.ValueChar).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Dim>> GetDepartments(CancellationToken cancellationToken)
        {
            return await this.context.Dims
                .Where(p => p.Name == "sys_department")
                .OrderBy(o => o.ValueChar).ToListAsync(cancellationToken);
        }
    }
}
