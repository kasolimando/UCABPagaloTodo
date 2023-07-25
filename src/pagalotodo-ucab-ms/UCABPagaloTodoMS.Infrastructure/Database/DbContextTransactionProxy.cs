using UCABPagaloTodoMS.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class DbContextTransactionProxy : IDbContextTransactionProxy
    {
        /// <summary>
        ///     Real Class which we want to control.
        ///     We can't mock it's because it does not have public constructors.
        /// </summary>
        private readonly IDbContextTransaction _transaction;

        private bool _disposed;

        public DbContextTransactionProxy(DbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
