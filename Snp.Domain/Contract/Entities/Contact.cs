using Dapper.Contrib.Extensions;
using Start.Common.Interfaces;
using System.Numerics;

namespace Snp.Domain.Contract.Entities
{
    [Table("snp.Contact")]
    public class Contact : IAuditInfo
    {
        /// <summary>
        /// Get or set Customer identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Get or set phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

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
