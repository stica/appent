using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Start.Infrastructure
{
    public class DapperContext
    {
        private readonly IDbConnection _dbConnection;
        private static readonly ConcurrentDictionary<Type, TableMetadata> _tableMetaData = new ConcurrentDictionary<Type, TableMetadata>();

        public DapperContext(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        public T Get<T>(dynamic id)
            where T : class
        {
            var metaData = GetTableMetadata<T>();

            T entity = _dbConnection.QuerySingleOrDefault<T>($"SELECT * FROM {metaData.TableName} WHERE {metaData.Key.Name} = @Id", new { Id = id });

            return entity;
        }

        public IEnumerable<T> GetAll<T>()
            where T : class
        {
            var metaData = GetTableMetadata<T>();

            return _dbConnection.Query<T>($"SELECT * FROM {metaData.TableName}");
        }

        public long Insert<T>(T entity)
            where T : class
        {
            return _dbConnection.Insert(entity);
        }

        public bool Update<T>(T entity)
            where T : class
        {
            return _dbConnection.Update(entity);
        }

        public IEnumerable<T> Query<T>(string query, object parameters = null)
            where T : class
        {
            return _dbConnection.Query<T>(query, parameters);
        }

        public T QueryFirstOrDefault<T>(string query, object parameters = null)
            where T : class
        {
            return _dbConnection.QueryFirstOrDefault<T>(query, parameters);
        }

        private TableMetadata GetTableMetadata<T>()
        {
            var type = typeof(T);
            if (_tableMetaData.TryGetValue(type, out TableMetadata metadata))
            {
                return metadata;
            }

            metadata = new TableMetadata();

            if (type.GetCustomAttributes(false).SingleOrDefault(x => x is TableAttribute) is TableAttribute ta)
            {
                metadata.TableName = ta.Name;
            }

            var key = type.GetProperties().SingleOrDefault(x => x.GetCustomAttributes(true).Any(x => x is KeyAttribute || x is ExplicitKeyAttribute));
            if (key != null)
            {
                metadata.Key = key;
            }

            return metadata;
        }
    }
}
