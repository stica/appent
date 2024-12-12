using Dapper.Contrib.Extensions;
using Snp.Domain.Contract.Enums;
using Start.Common.Interfaces;
using System.Numerics;

namespace Snp.Domain.Contract.Entities
{
    [Table("snp.Customer")]
    public class Customer : IAuditInfo
    {
        /// <summary>
        /// Get or set Customer identifier.
        /// </summary>
        [Key]
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
        /// Get or set contact identifier.
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// Get or set created date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Get or set updated date.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }    
    }
}
