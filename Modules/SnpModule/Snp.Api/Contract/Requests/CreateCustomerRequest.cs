using Snp.Domain.Contract.Enums;

namespace Snp.Api.Contract.Requests
{
    public class CreateCustomerRequest
    {
        /// <summary>
        /// Get or set customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set customer status.
        /// </summary>
        public CustomerStatus Status { get; set; }

        /// <summary>
        /// Get or set created date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Get or set updated date.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Get or set phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
