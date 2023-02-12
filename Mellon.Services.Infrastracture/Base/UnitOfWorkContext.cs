using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public  async Task<int> ExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default)
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
