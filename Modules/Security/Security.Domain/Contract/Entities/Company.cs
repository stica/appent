using Dapper.Contrib.Extensions;

namespace Security.Domain.Contract.Entities
{
    [Table("[SecurityManagement].[Company]")]
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Addresss { get; set; }

        public string PhoneNumber { get; set; }

        public string State { get; set; }

        public int NumberOfTrucsAllowed { get; set; }

        public string ShortName { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAdmin { get; set; }
    }
}
