namespace Snp.Api.Contract.Dtos
{
    public class ContactDto
    {
        /// <summary>
        /// Get or set Customer identifier.
        /// </summary>
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
