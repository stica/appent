using AutoMapper;
using Snp.Api.Contract.Responses;
using Snp.Domain.Managers.Snp;
using StackExchange.Redis;
using System.Text.Json;

namespace Snp.Api.SnpCache
{
    /// <summary>
    /// Service for managing customer data caching using Redis.
    /// </summary>
    public class SnpCacheService
    {
        private const string ALL_CUSTOMERS = "snp:allcustomers";
        private const string CUSTOMER = "snp:customer";
        private readonly IDatabase _redis;
        private ISnpManager _manager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnpCacheService"/> class.
        /// </summary>
        /// <param name="muxer">The Redis connection multiplexer.</param>
        /// <param name="manager">The SNP manager for database operations.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public SnpCacheService(IConnectionMultiplexer muxer, ISnpManager manager, IMapper mapper)
        {
            _redis = muxer.GetDatabase();
            _manager = manager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all customers from the cache or database.
        /// </summary>
        /// <returns>A collection of <see cref="CustomerResponse"/> objects.</returns>
        public async Task<IEnumerable<CustomerResponse>> GetAllCustomers()
        {
            string json = await _redis.StringGetAsync(ALL_CUSTOMERS);
            List<CustomerResponse> result = new List<CustomerResponse>();

            if (string.IsNullOrEmpty(json))
            {
                var allCustomers = await _manager.GetAllCustomers();
                result = _mapper.Map<List<CustomerResponse>>(allCustomers.ToList());
                var setTask = _redis.StringSetAsync(ALL_CUSTOMERS, JsonSerializer.Serialize(result));
                var expireTask = _redis.KeyExpireAsync(ALL_CUSTOMERS, TimeSpan.FromSeconds(3600));
                await Task.WhenAll(setTask, expireTask);
            }
            else
            {
                result = JsonSerializer.Deserialize<List<CustomerResponse>>(json);
            }

            return result;
        }


        /// <summary>
        /// Retrieves a specific customer by ID from the cache or database.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>A <see cref="CustomerResponse"/> object representing the customer.</returns>
        public async Task<CustomerResponse> GetCustomer(int id)
        {
            var customerRedis = await _redis.HashGetAsync(CUSTOMER, id);

            if (customerRedis.HasValue)
            {
                return JsonSerializer.Deserialize<CustomerResponse>(customerRedis);
            }
            else
            {
                var customerFromDb = await _manager.GetCustomer(id);

                var result = _mapper.Map<CustomerResponse>(customerFromDb);
                await _redis.HashSetAsync(CUSTOMER, id, JsonSerializer.Serialize(result));
                return result;
            }
        }

        public async Task InvalidateAllCustomers()
        {
            _redis.KeyDeleteAsync(ALL_CUSTOMERS);
        }

        public async Task InvalidateCustomer(int id)
        {
            _redis.HashDeleteAsync(CUSTOMER, id);
        }
    }
}
