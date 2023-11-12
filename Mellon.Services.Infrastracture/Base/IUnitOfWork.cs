using Microsoft.EntityFrameworkCore.Storage;

namespace Mellon.Services.Infrastracture.Base
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> ExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();

        void Commit();
        void Rollback();
    }
}
