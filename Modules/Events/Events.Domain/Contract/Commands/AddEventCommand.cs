namespace Events.Domain.Contract.Commands
{
    public class AddEventCommand
    {
        public string Name { get; set; }

        public int OwnerId { get; set; }

        public int ActivityId { get; set; }

        public string Address { get; set; }

        public DateTime? Time { get; set; }

        public int? CityId { get; set; }

        public int? CountryId { get; set; }

        public bool IsOpenToEveryone { get; set; }

        public int? MinimumPersons { get; set; }

        public int? MaximumPersons { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int? RepetitionType { get; set; }

        public bool AmIAttending { get; set; }
    }
}
