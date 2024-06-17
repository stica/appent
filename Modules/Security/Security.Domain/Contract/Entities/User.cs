using Dapper.Contrib.Extensions;
using System;

namespace Security.Domain.Contract.Entities
{
    [Table("[SecurityManagement].[User]")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsAdmin { get; set; }

        public bool? ChangePasswordOnNextLogin { get; set; }

        public int? CityId { get; set; }

        public int? CountryId { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsEnabled { get; set; }
    }
}
