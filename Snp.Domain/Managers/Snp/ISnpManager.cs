using Snp.Domain.Contract.Commands;
using Snp.Domain.Contract.Views;
using System.Numerics;

namespace Snp.Domain.Managers.Snp
{
    /// <summary>
    /// Interface for managing customer operations in the SNP system.
    /// </summary>
    public interface ISnpManager
    {
        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerCommand">The command containing customer details to be created.</param>
        /// <returns>The unique identifier of the newly created customer.</returns>
        Task<int> CreeateCustomer(CreateCustomerCommand createCustomerCommand);

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A collection of customer views representing all customers.</returns>
        Task<IEnumerable<CustomerView>> GetAllCustomers();

        /// <summary>
        /// Retrieves a specific customer by their identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to retrieve.</param>
        /// <returns>A <see cref="CustomerView"/> object representing the customer, or null if the customer does not exist.</returns>
        Task<CustomerView> GetCustomer(int customerId);

        /// <summary>
        /// Deletes a customer by their identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to delete.</param>
        /// <returns><c>true</c> if the customer was successfully deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteCustomer(int customerId);
    }
}
