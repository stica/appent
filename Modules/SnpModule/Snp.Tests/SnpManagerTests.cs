using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Start.Common.Classes;
using System.Reflection;
using Xunit.Sdk;
using Snp.Domain.Managers.Snp;
using Start.Infrastructure;
using Snp.Domain.Contract.Commands;
using System.Transactions;

namespace Snp.Tests
{
    /// <summary>
    /// Provides unit tests for the <see cref="SnpManager"/> class.
    /// </summary>
    [TestClass]
    public class SnpManagerTests
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private CustomConfigurationProvider _customConfigurationProvider;

        private List<int> _createdCustomers;
        private SnpManager _manager;

        /// <summary>
        /// Initializes the test environment and dependencies before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _createdCustomers = new List<int>();
            var services = new ServiceCollection();

            _serviceProvider = services.BuildServiceProvider();
            _configuration = GetConfiguration();
            _customConfigurationProvider = new CustomConfigurationProvider(_configuration);
            _manager = new SnpManager(_customConfigurationProvider);
            TransactionManager.ImplicitDistributedTransactions = true;
        }

        /// <summary>
        /// Cleans up resources and database entries created during the test.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            using (DapperContext ctx = new DapperContext(_customConfigurationProvider.ConnectionString))
            {
                foreach (var customerId in _createdCustomers)
                {
                    ctx.Execute($"DELETE FROM [snp].[Customer] WHERE Id = @Id", new { Id = customerId });
                }
            }
        }

        /// <summary>
        /// Tests that a customer can be created successfully.
        /// </summary>
        [TestMethod]
        public async Task Create_Customer_Works_Ok()
        {
            var createCustomer = new CreateCustomerCommand
            {
                Name = "Test",
                PhoneNumber = "1234567890",
                Status = Domain.Contract.Enums.CustomerStatus.Active,
            };

            var createdCustomerId = await _manager.CreeateCustomer(createCustomer);
            _createdCustomers.Add(createdCustomerId);

            var createdCustomer = await _manager.GetCustomer(createdCustomerId);

            Assert.IsNotNull(createdCustomer);
            Assert.AreEqual(createdCustomerId, createdCustomer.Id);
            Assert.AreEqual("Test", createdCustomer.Name);
            Assert.AreEqual("1234567890", createdCustomer.Contact.PhoneNumber);
        }

        /// <summary>
        /// Tests that all customers can be retrieved successfully.
        /// </summary>
        [TestMethod]
        public async Task Get_All_Customers_Works_Ok()
        {
            var createCustomer = new CreateCustomerCommand
            {
                Name = "Test",
                PhoneNumber = "1234567890",
                Status = Domain.Contract.Enums.CustomerStatus.Active,
            };

            var firstCustomerId = await _manager.CreeateCustomer(createCustomer);
            _createdCustomers.Add(firstCustomerId);

            createCustomer = new CreateCustomerCommand
            {
                Name = "Test2",
                PhoneNumber = "1234567891",
                Status = Domain.Contract.Enums.CustomerStatus.Inactive,
            };

            var secondCustomerId = await _manager.CreeateCustomer(createCustomer);
            _createdCustomers.Add(secondCustomerId);

            var customers = await _manager.GetAllCustomers();

            Assert.IsTrue(customers.Count() > 0);

            var customer1 = customers.FirstOrDefault(x => x.Id == firstCustomerId);
            var customer2 = customers.FirstOrDefault(x => x.Id == secondCustomerId);

            Assert.AreEqual("Test", customer1.Name);
            Assert.AreEqual("1234567890", customer1.Contact.PhoneNumber);

            Assert.AreEqual("Test2", customer2.Name);
            Assert.AreEqual("1234567891", customer2.Contact.PhoneNumber);
        }

        /// <summary>
        /// Tests that a customer can be deleted successfully.
        /// </summary>
        [TestMethod]
        public async Task Delete_Customer_Works_Ok()
        {
            var createCustomer = new CreateCustomerCommand
            {
                Name = "Test",
                PhoneNumber = "1234567890",
                Status = Domain.Contract.Enums.CustomerStatus.Active,
            };

            var createdCustomerId = await _manager.CreeateCustomer(createCustomer);
            _createdCustomers.Add(createdCustomerId);

            await _manager.DeleteCustomer(createdCustomerId);
            string error = string.Empty;

            try
            {
                var customer = await _manager.GetCustomer(createdCustomerId);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            Assert.AreEqual($"Customer with identifier {createdCustomerId} does not exist.", error);
        }

        /// <summary>
        /// Builds the configuration for the test environment.
        /// </summary>
        /// <returns>The configuration instance.</returns>
        private IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.Test.json")
                .Build();
        }
    }
}
