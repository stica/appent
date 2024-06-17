using Dapper.Contrib.Extensions;

namespace Events.Domain.Entites
{
    [Table("[Events].[Activity]")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}