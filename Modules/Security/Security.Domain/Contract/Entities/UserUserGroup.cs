using Dapper.Contrib.Extensions;

namespace Security.Domain.Contract.Entities
{
    [Table("SecurityManagement.UserUserGroup")]
    public class UserUserGroup
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int UserGroupId { get; set; }
    }
}
