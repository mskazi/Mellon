using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mellon.Services.Infrastracture.Base
{
    public abstract class UnitOfWorkContext<T> : DbContext, IUnitOfWork where T : DbContext
    {
        public UnitOfWorkContext()
        {
        }

        public UnitOfWorkContext(DbContextOptions<T> options)
            : base(options)
        {
        }

        public IDbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }

        public void Commit()
        {
            base.Database.CommitTransaction();
        }

        public void Rollback()
        {
            base.Database.RollbackTransaction();
        }

        public async Task<int> ExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.Database.ExecuteSqlAsync(sql, cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                var number = 0;
                if (ex.GetBaseException() is SqlException sqlException)
                    number = sqlException.Number; // 547
                throw new RepositoryException(number, ex);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                var number = 0;
                if (ex.GetBaseException() is SqlException sqlException)
                    number = sqlException.Number; // 547
                throw new RepositoryException(number, ex);
            }
        }
    }
}
