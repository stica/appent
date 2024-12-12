using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Infrastructure
{
    /// <summary>
    /// Provides a async context for working with the database using Dapper.
    /// </summary>
    public partial class DapperContext
    {
        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity matching the provided ID, or <c>null</c> if not found.</returns>
        public async Task<T> GetAsync<T>(dynamic id) where T : class
        {
            var metaData = GetTableMetadata<T>();

            T entity = await _dbConnection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {metaData.TableName} WHERE {metaData.Key.Name} = @Id", new { Id = id });

            return entity;
        }

        /// <summary>
        /// Retrieves all entities of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of entities to retrieve.</typeparam>
        /// <returns>An enumerable collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            var metaData = GetTableMetadata<T>();

            return await _dbConnection.QueryAsync<T>($"SELECT * FROM {metaData.TableName}");
        }

        /// <summary>
        /// Inserts a new entity into the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity to insert.</typeparam>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The ID of the newly inserted entity.</returns>
        public async Task<long> InsertAsync<T>(T entity) where T : class
        {
            CheckAuditInfoForInsert(entity);
            var result = await _dbConnection.InsertAsync(entity);
            return result;
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity to update.</typeparam>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            CheckAuditInfoForUpdate(entity);
            return await _dbConnection.UpdateAsync(entity);
        }

        /// <summary>
        /// Executes a query and returns the results as an enumerable collection of entities.
        /// </summary>
        /// <typeparam name="T">The type of entities to return.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>An enumerable collection of entities.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null) where T : class
        {
            return await _dbConnection.QueryAsync<T>(query, parameters);
        }

        /// <summary>
        /// Executes a query and returns the first result or the default value if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>The first entity that matches the query, or <c>null</c> if no result is found.</returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null) where T : class
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, parameters);
        }


        /// <summary>
        /// Deletes an existing entity in the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity to delete.</typeparam>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            return await _dbConnection.DeleteAsync(entity);
        }

        public async Task<(TFirst, TSecond)> QueryMultipleAsync<TFirst, TSecond>(
            string query,
            object parameters = null
        ) where TFirst : class where TSecond : class
        {
            using (var multi = await _dbConnection.QueryMultipleAsync(query, parameters))
            {
                var first = await multi.ReadFirstOrDefaultAsync<TFirst>();
                var second = await multi.ReadFirstOrDefaultAsync<TSecond>();

                return (first, second);
            }
        }

        public async Task<(IEnumerable<TFirst>, IEnumerable<TSecond>)> QueryMultipleListsAsync<TFirst, TSecond>(
            string query,
            object parameters = null
        ) where TFirst : class where TSecond : class
        {
            using (var multi = await _dbConnection.QueryMultipleAsync(query, parameters))
            {
                var firstList = (await multi.ReadAsync<TFirst>()).ToList();
                var secondList = (await multi.ReadAsync<TSecond>()).ToList();

                return (firstList, secondList);
            }
        }
    }
}
