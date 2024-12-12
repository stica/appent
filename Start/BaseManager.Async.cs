using Start.Infrastructure;
using Start.Infrastructure.Entites;
using System;
using System.Threading.Tasks;

namespace Start
{
    /// <summary>
    /// Provides base methods for executing database operations with and without transactions.
    /// </summary>
    public partial class BaseManager
    {
        /// <summary>
        /// Executes a database operation asynchronously without transaction management.
        /// </summary>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected virtual async Task ExecuteAsync(Func<DapperContext, Task> action, MethodInfo info)
        {
            await DoExecuteAsync(action, info);
        }

        /// <summary>
        /// Executes a database operation asynchronously within a transaction.
        /// </summary>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected virtual async Task ExecuteInTransactionAsync(Func<DapperContext, Task> action, MethodInfo info)
        {
            await DoExecuteInTransactionAsync(action, info);
        }

        /// <summary>
        /// Executes a database operation asynchronously and returns the result without transaction management.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the operation.</typeparam>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation with the result.</returns>
        protected virtual async Task<T> ExecuteWithResultAsync<T>(Func<DapperContext, Task<T>> action, MethodInfo info)
        {
            return await DoExecuteWithResultAsync(action, info);
        }

        /// <summary>
        /// Executes a database operation asynchronously within a transaction and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the operation.</typeparam>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation with the result.</returns>
        protected virtual async Task<T> ExecuteInTransactionWithResultAsync<T>(Func<DapperContext, Task<T>> action, MethodInfo info)
        {
            return await DoExecuteWithResultInTransactionAsync(action, info);
        }

        /// <summary>
        /// Executes a database operation asynchronously without transaction management.
        /// </summary>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DoExecuteAsync(Func<DapperContext, Task> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                await UnitOfWork.Create(_connectionString).ExecuteAsync(action);
            }
            catch (Exception ex)
            {
                ErrorExecutingMethod(ex, info);
                throw;
            }
            finally
            {
                FinishExecuteMethod(info);
            }
        }

        /// <summary>
        /// Executes a database operation asynchronously within a transaction.
        /// </summary>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DoExecuteInTransactionAsync(Func<DapperContext, Task> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                await UnitOfWork.Create(_connectionString).ExecuteInTransactionAsync(action);
            }
            catch (Exception ex)
            {
                ErrorExecutingMethod(ex, info);
                throw;
            }
            finally
            {
                FinishExecuteMethod(info);
            }
        }

        /// <summary>
        /// Executes a database operation asynchronously and returns the result without transaction management.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the operation.</typeparam>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation with the result.</returns>
        private async Task<T> DoExecuteWithResultAsync<T>(Func<DapperContext, Task<T>> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                return await UnitOfWork.Create(_connectionString).ExecuteWithResultAsync(action);
            }
            catch (Exception ex)
            {
                ErrorExecutingMethod(ex, info);
                throw;
            }
            finally
            {
                FinishExecuteMethod(info);
            }
        }

        /// <summary>
        /// Executes a database operation asynchronously within a transaction and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the operation.</typeparam>
        /// <param name="action">The asynchronous function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>A task representing the asynchronous operation with the result.</returns>
        private async Task<T> DoExecuteWithResultInTransactionAsync<T>(Func<DapperContext, Task<T>> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                return await UnitOfWork.Create(_connectionString).ExecuteWithResultInTransactionAsync(action);
            }
            catch (Exception ex)
            {
                ErrorExecutingMethod(ex, info);
                throw;
            }
            finally
            {
                FinishExecuteMethod(info);
            }
        }
    }
}
