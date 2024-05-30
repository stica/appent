using Start.Infrastructure;
using Start.Infrastructure.Entites;
using System;

namespace Start
{
    public class BaseManager
    {
        protected string _connectionString;
        protected readonly Logger _logger;

        public BaseManager(AppSettings appSettings)
        {
            _connectionString = appSettings.ConnectionString;
            _logger = Logger.Create(GetType().FullName);
        }

        protected virtual void Execute(Action<DapperContext> action, MethodInfo info)
        {
            DoExecute(action, info);
        }

        protected virtual void ExecuteInTransaction(Action<DapperContext> action, MethodInfo info)
        {
            DoExecuteInTransaction(action, info);
        }

        protected virtual T ExecuteWithResult<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            return DoExecuteWithResult(action, info);
        }

        protected virtual T ExecuteInTransactionWithResult<T>(Func<DapperContext, T> action, MethodInfo info)
        {
            return DoExecuteWithResultInTransaction(action, info);
        }

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

        private void ExecuteMethod(MethodInfo info)
        {
            _logger.Info($"Executing : {info.Name}");
        }

        private void ErrorExecutingMethod(Exception ex, MethodInfo info) {
            _logger.Info($"Executing : {info.Name}");
            _logger.Error(ex);
        }

        private void FinishExecuteMethod(MethodInfo info)
        {
            _logger.Info($"Executing finished : {info.Name}");
        }
    }
}
