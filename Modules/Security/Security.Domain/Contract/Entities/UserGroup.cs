using Dapper.Contrib.Extensions;

namespace Security.Domain.Contract.Entities
{
    [Table("SecurityManagement.UserGroup")]
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
