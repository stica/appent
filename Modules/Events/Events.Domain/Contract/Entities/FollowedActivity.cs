using Dapper.Contrib.Extensions;

namespace Events.Domain.Entites
{
    [Table("[Events].[FollowedActivity]")]
    public class FollowedActivity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }
    }
}