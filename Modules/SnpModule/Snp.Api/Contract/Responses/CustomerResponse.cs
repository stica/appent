using Snp.Api.Contract.Dtos;
using Snp.Api.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snp.Api.Contract.Responses
{
    public class CustomerResponse
    {
        public CustomerResponse()
        {
            Contact = new ContactDto();
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
        public CustomerStatusDto Status { get; set; }

        /// <summary>
        /// Get or set created date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Get or set updated date.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Get or set Contact data.
        /// </summary>
        public ContactDto Contact { get; set; }
    }
}
