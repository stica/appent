using Dapper;
using Dapper.Contrib.Extensions;
using Start.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Start.Infrastructure
{
    /// <summary>
    /// Provides a context for working with the database using Dapper.
    /// </summary>
    public partial class DapperContext : IDisposable
    {
        private readonly IDbConnection _dbConnection;
        private static readonly ConcurrentDictionary<Type, TableMetadata> _tableMetaData = new ConcurrentDictionary<Type, TableMetadata>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContext"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public DapperContext(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
        }

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity matching the provided ID, or <c>null</c> if not found.</returns>
        public T Get<T>(dynamic id) where T : class
        {
            var metaData = GetTableMetadata<T>();

            T entity = _dbConnection.QuerySingleOrDefault<T>($"SELECT * FROM {metaData.TableName} WHERE {metaData.Key.Name} = @Id", new { Id = id });

            return entity;
        }

        /// <summary>
        /// Retrieves all entities of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of entities to retrieve.</typeparam>
        /// <returns>An enumerable collection of entities.</returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            var metaData = GetTableMetadata<T>();

            return _dbConnection.Query<T>($"SELECT * FROM {metaData.TableName}");
        }

        /// <summary>
        /// Inserts a new entity into the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity to insert.</typeparam>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The ID of the newly inserted entity.</returns>
        public long Insert<T>(T entity) where T : class
        {
            CheckAuditInfoForInsert(entity);
            return _dbConnection.Insert(entity);
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity to update.</typeparam>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public bool Update<T>(T entity) where T : class
        {
            CheckAuditInfoForUpdate(entity);
            return _dbConnection.Update(entity);
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>Numbers of rows affected</returns>
        public int Execute(string query, object parameters = null)
        {
            return _dbConnection.Execute(query, parameters);
        }

        /// <summary>
        /// Executes a query and returns the results as an enumerable collection of entities.
        /// </summary>
        /// <typeparam name="T">The type of entities to return.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>An enumerable collection of entities.</returns>
        public IEnumerable<T> Query<T>(string query, object parameters = null) where T : class
        {
            return _dbConnection.Query<T>(query, parameters);
        }

        /// <summary>
        /// Executes a query and returns the first result or the default value if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>The first entity that matches the query, or <c>null</c> if no result is found.</returns>
        public T QueryFirstOrDefault<T>(string query, object parameters = null) where T : class
        {
            return _dbConnection.QueryFirstOrDefault<T>(query, parameters);
        }

        /// <summary>
        /// Retrieves metadata for a specific table based on the entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The table metadata.</returns>
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

            metadata.IsAuditable = typeof(IAuditInfo).IsAssignableFrom(type);

            return metadata;
        }

        /// <summary>
        /// Check if entity is a type of IAuditInfo, if it is set CreatedAt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        private void CheckAuditInfoForInsert<T>(T entity)
            where T : class
        {
            var metadata = GetTableMetadata<T>();

            if (metadata.IsAuditable)
            {
                var auditable = entity as IAuditInfo;
                auditable.CreatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Check if entity is a type of IAuditInfo, if it is set UpdatedAt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        private void CheckAuditInfoForUpdate<T>(T entity)
            where T : class
        {
            var metadata = GetTableMetadata<T>();

            if (metadata.IsAuditable)
            {
                var auditable = entity as IAuditInfo;
                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Dispose()
        {

        }
    }
}
