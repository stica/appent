using System;
using System.Threading;
using System.Transactions;

namespace Start.Infrastructure
{
    public class UnitOfWork
    {
        private static AsyncLocal<DapperContext> _context = new AsyncLocal<DapperContext>();
        private readonly TransactionOptions _transactionOptions;

        public UnitOfWork(string connectionString)
        {
            _transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            if (_context.Value == null)
            {
                _context.Value = new DapperContext(connectionString);
            }
        }

        public static UnitOfWork Create(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }

        public void Execute(Action<DapperContext> action)
        {
            action(_context.Value);
        }

        public T ExecuteWithResult<T>(Func<DapperContext, T> execute)
        {
            T result = execute(_context.Value);
            return result;
        }

        public void ExecuteInTransaction(Action<DapperContext> action)
        {
            using(var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                action(_context.Value);
            }
        }

        public T ExecuteWithResultInTransaction<T>(Func<DapperContext, T> execute)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                T result = execute(_context.Value);
                return result;
            }
        }
    }
}
