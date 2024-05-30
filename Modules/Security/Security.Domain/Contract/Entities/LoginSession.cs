using Dapper.Contrib.Extensions;
using System;

namespace Security.Domain.Contract.Entities
{
    [Table("SecurityManagement.LoginSession")]
    public class LoginSession
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }

        public string IpAddress { get; set; }

        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
