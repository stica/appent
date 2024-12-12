using Snp.Domain.Contract.Commands;
using Snp.Domain.Contract.Entities;
using Snp.Domain.Contract.Views;
using Start;
using Start.Common.Classes;
using Start.Infrastructure.Entites;

namespace Snp.Domain.Managers.Snp
{
    /// <summary>
    /// Provides implementation for managing customers in the SNP system.
    /// </summary>
    public class SnpManager : BaseManager, ISnpManager
    {
        public SnpManager(CustomConfigurationProvider configuration) : base(configuration)
        {
        }

        /// <inheritdoc/>
        public Task<int> CreeateCustomer(CreateCustomerCommand createCustomerCommand)
        {
            return ExecuteInTransactionWithResultAsync<int>(async (ctx) =>
            {
                var contactId = await ctx.InsertAsync<Contact>(new Contact
                {
                    PhoneNumber = createCustomerCommand.PhoneNumber,
                });

                var customerId = await ctx.InsertAsync(new Customer
                {
                    ContactId = (int)contactId,
                    Name = createCustomerCommand.Name,
                    Status = createCustomerCommand.Status,
                });

                return (int)customerId;
            }, MethodInfo.Create("CreeateCustomer", createCustomerCommand));
        }

        /// <inheritdoc/>
        public Task<bool> DeleteCustomer(int customerId)
        {
            return ExecuteInTransactionWithResultAsync(async ctx =>
            {
                var entityToDelete = await ctx.GetAsync<Customer>(customerId);

                if (entityToDelete == null) {
                    throw new ArgumentException($"Customer with identifier {customerId} does not exist.");
                }

                return await ctx.DeleteAsync(entityToDelete);
            }, MethodInfo.Create("DeleteCustomer", customerId));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<CustomerView>> GetAllCustomers()
        {
            return ExecuteWithResultAsync(async ctx =>
            {
                var query = @"
                    SELECT * FROM [snp].Customer;
                    SELECT * FROM [snp].Contact;";

                var (customers, contacts) = await ctx.QueryMultipleListsAsync<Customer, Contact>(query);

                var customerViews = customers.Select(customer =>
                {
                    var contact = contacts.FirstOrDefault(c => c.Id == customer.ContactId);

                    return new CustomerView
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Status = customer.Status,
                        CreatedAt = customer.CreatedAt,
                        UpdatedAt = customer.UpdatedAt,
                        Contact = contact ?? new Contact()
                    };
                });

                return customerViews;
            }, MethodInfo.Create("GetAllCustomers"));
        }

        /// <inheritdoc/>
        public Task<CustomerView> GetCustomer(int customerId)
        {
            return ExecuteWithResultAsync(async ctx =>
            {
                var query = @"
                    SELECT * FROM [snp].Customer WHERE Id = @CustomerId;
                    SELECT c.* 
                    FROM [snp].Contact c
                    INNER JOIN [snp].Customer cust ON c.Id = cust.ContactId
                    WHERE cust.Id = @CustomerId;";

                var result = await ctx.QueryMultipleAsync<Customer, Contact>(query, new { CustomerId = customerId });

                if (result.Item1 == null)
                {
                    throw new ArgumentException($"Customer with identifier {customerId} does not exist.");
                }

                return new CustomerView
                {
                    Contact = result.Item2,
                    CreatedAt = result.Item1.CreatedAt,
                    Id = result.Item1.Id,
                    Name = result.Item1.Name,
                    Status = result.Item1.Status,
                    UpdatedAt = result.Item1.UpdatedAt,
                };

            }, MethodInfo.Create("GetAllCustomers"));
        }
    }
}
