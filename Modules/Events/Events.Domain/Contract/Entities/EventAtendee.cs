using Dapper.Contrib.Extensions;

namespace Events.Domain.Entites
{
    [Table("[Events].[EventAttendee]")]
    public class EventAttendee
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}