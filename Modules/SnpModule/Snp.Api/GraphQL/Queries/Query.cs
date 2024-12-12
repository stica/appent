using AutoMapper;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;
using Snp.Api.Contract.Responses;
using Snp.Api.SnpCache;

namespace Snp.Api.GraphQL.Queries
{
    /// <summary>
    /// Represents the main GraphQL query class for handling customer-related queries.
    /// </summary>
    public class Query
    {
        private readonly SnpCacheService _snpCacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        /// <param name="snpCacheService">The service for caching and retrieving customer data.</param>
        public Query(SnpCacheService snpCacheService)
        {
            _snpCacheService = snpCacheService;
        }

        /// <summary>
        /// Retrieves a filtered and sorted list of customers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="CustomerResponse"/>.</returns>
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<CustomerResponse>> GetCustomers()
        {
            return await _snpCacheService.GetAllCustomers();
        }

        /// <summary>
        /// Retrieves a single customer by their unique identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CustomerResponse"/> for the specified customer.</returns>
        public async Task<CustomerResponse> GetCustomer(int customerId)
        {
            return await _snpCacheService.GetCustomer(customerId);
        }
    }

    /// <summary>
    /// Defines sorting rules for the <see cref="Query"/> type.
    /// </summary>
    public class CourseSortType : SortInputType<Query>
    {
        /// <summary>
        /// Configures the sorting rules for the <see cref="Query"/> type.
        /// </summary>
        /// <param name="descriptor">The descriptor used to define sorting rules.</param>
        protected override void Configure(ISortInputTypeDescriptor<Query> descriptor)
        {
            base.Configure(descriptor);
        }
    }

    /// <summary>
    /// Defines filtering rules for the <see cref="Query"/> type.
    /// </summary>
    public class CourseFilterType : FilterInputType<Query>
    {
        /// <summary>
        /// Configures the filtering rules for the <see cref="Query"/> type.
        /// </summary>
        /// <param name="descriptor">The descriptor used to define filtering rules.</param>
        protected override void Configure(IFilterInputTypeDescriptor<Query> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}
