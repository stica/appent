using Dapper.Contrib.Extensions;

namespace Events.Domain.Entites
{
    [Table("[Events].[EventRating]")]
    public class EventRating
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public decimal Rating { get; set; }
    }
}