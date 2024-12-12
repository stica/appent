using Snp.Domain.Contract.Entities;
using Snp.Domain.Contract.Enums;
using System.Numerics;

namespace Snp.Domain.Contract.Views
{
    public class CustomerView
    {
        public CustomerView() {
            Contact = new Contact();
        }

        /// <summary>
        /// Get or set Customer identifier.
        /// </summary>
        public int Id { get; set; }

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

        public Contact Contact { get; set; }
    }
}
