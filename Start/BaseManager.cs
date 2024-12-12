using Start.Common.Classes;
using Start.Infrastructure;
using Start.Infrastructure.Entites;
using System;

namespace Start
{
    /// <summary>
    /// Provides base methods for executing database operations with and without transactions, and handling logging.
    /// </summary>
    public partial class BaseManager
    {
        /// <summary>
        /// The connection string for the database.
        /// </summary>
        protected string _connectionString;

        /// <summary>
        /// Logger instance for logging method executions and errors.
        /// </summary>
        protected readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseManager"/> class.
        /// </summary>
        /// <param name="appSettings">Application settings containing the connection string.</param>
        public BaseManager(CustomConfigurationProvider appSettings)
        {
            _connectionString = appSettings.ConnectionString;
            _logger = Logger.Create(GetType().FullName);
        }

        /// <summary>
        /// Executes an action asynchronously without transaction management.
        /// </summary>
        /// <param name="action">The action representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        protected virtual void Execute(Action<DapperContext> action, MethodInfo info)
        {
            DoExecute(action, info);
        }

        /// <summary>
        /// Executes an action asynchronously within a transaction.
        /// </summary>
        /// <param name="action">The action representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        protected virtual void ExecuteInTransaction(Action<DapperContext> action, MethodInfo info)
        {
            DoExecuteInTransaction(action, info);
        }

        /// <summary>
        /// Executes a function asynchronously and returns the result without transaction management.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the function.</typeparam>
        /// <param name="action">The function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>The result of the operation.</returns>
        protected virtual T ExecuteWithResult<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            return DoExecuteWithResult(action, info);
        }

        /// <summary>
        /// Executes a function asynchronously within a transaction and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the function.</typeparam>
        /// <param name="action">The function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>The result of the operation.</returns>
        protected virtual T ExecuteInTransactionWithResult<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            return DoExecuteWithResultInTransaction(action, info);
        }

        /// <summary>
        /// Executes an action asynchronously without transaction management.
        /// </summary>
        /// <param name="action">The action representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        private void DoExecute(Action<DapperContext> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                UnitOfWork.Create(_connectionString).Execute(action);
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
        /// Executes an action asynchronously within a transaction.
        /// </summary>
        /// <param name="action">The action representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        private void DoExecuteInTransaction(Action<DapperContext> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                UnitOfWork.Create(_connectionString).ExecuteInTransaction(action);
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
        /// Executes a function asynchronously and returns the result without transaction management.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the function.</typeparam>
        /// <param name="action">The function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>The result of the operation.</returns>
        private T DoExecuteWithResult<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                return UnitOfWork.Create(_connectionString).ExecuteWithResult(action);
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
        /// Executes a function asynchronously within a transaction and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the function.</typeparam>
        /// <param name="action">The function representing the operation to be executed.</param>
        /// <param name="info">Information about the method being executed.</param>
        /// <returns>The result of the operation.</returns>
        private T DoExecuteWithResultInTransaction<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            try
            {
                ExecuteMethod(info);
                return UnitOfWork.Create(_connectionString).ExecuteWithResultInTransaction(action);
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
        /// Logs the start of method execution.
        /// </summary>
        /// <param name="info">Information about the method being executed.</param>
        private void ExecuteMethod(MethodInfo info)
        {
            _logger.Info($"Executing : {info.Name}");
        }

        /// <summary>
        /// Logs any errors that occur during method execution.
        /// </summary>
        /// <param name="ex">The exception that was thrown.</param>
        /// <param name="info">Information about the method being executed.</param>
        private void ErrorExecutingMethod(Exception ex, MethodInfo info)
        {
            _logger.Info($"Executing : {info.Name}");
            _logger.Error(ex);
        }

        /// <summary>
        /// Logs the completion of method execution.
        /// </summary>
        /// <param name="info">Information about the method being executed.</param>
        private void FinishExecuteMethod(MethodInfo info)
        {
            _logger.Info($"Executing finished : {info.Name}");
        }
    }
}
