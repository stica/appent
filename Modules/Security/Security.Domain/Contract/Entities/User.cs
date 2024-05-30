using Dapper.Contrib.Extensions;
using Start.Common.Interfaces;

namespace Security.Domain.Contract.Entities
{
    [Table("[SecurityManagement].[User]")]
    public class User : IAuditable
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public int CompanyId { get; set; }

        public bool IsAdmin { get; set; }

        public bool ChangePasswordOnNextLogin { get; set; }
    }
}
