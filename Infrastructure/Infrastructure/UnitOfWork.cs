using NLog.Fluent;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Start.Infrastructure
{
    /// <summary>
    /// Represents a unit of work that manages database operations using Dapper and provides support for transactions.
    /// </summary>
    public class UnitOfWork
    {
        private static AsyncLocal<DapperContext> _context = new AsyncLocal<DapperContext>();
        private readonly TransactionOptions _transactionOptions;
        private readonly bool _isParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public UnitOfWork(string connectionString)
        {
            _isParent = false;
            _transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout,
            };

            if (_context.Value == null)
            {
                _context.Value = new DapperContext(connectionString);
                _isParent = true;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <returns>A new instance of <see cref="UnitOfWork"/>.</returns>
        public static UnitOfWork Create(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }

        /// <summary>
        /// Executes an action within the unit of work context.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void Execute(Action<DapperContext> action)
        {
            try
            {
                action(_context.Value);

                if (_isParent)
                {
                    Dispose();
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes a function within the unit of work context and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="execute">The function to execute.</param>
        /// <returns>The result of the function.</returns>
        public T ExecuteWithResult<T>(Func<DapperContext, T> execute)
        {
            try
            {
                T result = execute(_context.Value);

                if (_isParent)
                {
                    Dispose();
                }

                return result;
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes an action within a transaction..
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void ExecuteInTransaction(Action<DapperContext> action)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    action(_context.Value);

                    if (_isParent)
                    {
                        Dispose();
                    }

                    scope.Complete();
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }


        /// <summary>
        /// Executes a function within a transaction and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="execute">The function to execute.</param>
        /// <returns>The result of the function.</returns>
        public T ExecuteWithResultInTransaction<T>(Func<DapperContext, T> execute)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    T result = execute(_context.Value);
                    scope.Complete();

                    if (_isParent)
                    {
                        Dispose();
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes a function within a transaction asynchronously and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="execute">The function to execute.</param>
        /// <returns>A task representing the asynchronous operation that returns the result.</returns>
        public async Task<T> ExecuteWithResultInTransactionAsync<T>(Func<DapperContext, Task<T>> execute)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    T result = await execute(_context.Value);
                    scope.Complete();

                    if (_isParent)
                    {
                        Dispose();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transaction failed", ex);

                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes an asynchronous function within the unit of work context.
        /// </summary>
        /// <param name="action">The asynchronous function to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(Func<DapperContext, Task> action)
        {
            try
            {
                await action(_context.Value);

                if (_isParent)
                {
                    Dispose();
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes an asynchronous function within the unit of work context and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="execute">The asynchronous function to execute.</param>
        /// <returns>A task representing the asynchronous operation that returns the result.</returns>
        public async Task<T> ExecuteWithResultAsync<T>(Func<DapperContext, Task<T>> execute)
        {
            try
            {
                T result = await execute(_context.Value);

                if (_isParent)
                {
                    Dispose();
                }

                return result;
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Executes an asynchronous function within a transaction.
        /// </summary>
        /// <param name="action">The asynchronous function to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteInTransactionAsync(Func<DapperContext, Task> action)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await action(_context.Value);
                    scope.Complete();

                    if (_isParent)
                    {
                        Dispose();
                    }
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            if (_context.Value != null)
            {
                _context.Value.Dispose();
                _context.Value = null;
            }
        }
    }
}
