using Dapper.Contrib.Extensions;
using Security.Domain.Contract.Documents;
using Start.Common.Classes;

namespace Security.Domain.Contract.Entities
{
    [Table("SecurityManagement.Policy")]
    public class Policy : BaseDocument<PolicyDocument>
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
