using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Infrastracture.Base
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> ExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default);
    }
}
