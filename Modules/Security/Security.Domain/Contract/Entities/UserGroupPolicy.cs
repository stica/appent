using Dapper.Contrib.Extensions;

namespace Security.Domain.Contract.Entities
{
    [Table("SecurityManagement.UserGroupPolicy")]
    public class UserGroupPolicy
    {
        [Key]
        public int Id { get; set; }

        public int PolicyId { get; set; }
        public int UserGroupId { get; set; }
    }
}
