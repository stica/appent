using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Snp.Api.Contract.Requests;
using Snp.Api.SnpCache;
using Snp.Domain.Contract.Commands;
using Snp.Domain.Managers.Snp;

namespace Snp.Api.GraphQL.Mutations
{
    /// <summary>
    /// Represents the main GraphQL mutation class for handling customer-related operations.
    /// </summary>
    public class Mutation
    {
        private readonly ISnpManager _snpManager;
        private readonly IMapper _mapper;
        private readonly SnpCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mutation"/> class.
        /// </summary>
        /// <param name="snpManager">The manager responsible for customer-related operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping request objects to command objects.</param>
        public Mutation(
            ISnpManager snpManager,
            IMapper mapper,
            SnpCacheService cacheService)
        {
            _snpManager = snpManager;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Creates a new customer based on the provided request.
        /// </summary>
        /// <param name="request">The request containing the details of the customer to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the created customer.</returns>
        public async Task<int> CreateCustomer(CreateCustomerRequest request)
        {
            try
            {
                var createCustomerCommand = _mapper.Map<CreateCustomerCommand>(request);
                var result = await _snpManager.CreeateCustomer(createCustomerCommand);
                await _cacheService.InvalidateAllCustomers();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Deletes a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                var result = await _snpManager.DeleteCustomer(id);

                await _cacheService.InvalidateAllCustomers();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
