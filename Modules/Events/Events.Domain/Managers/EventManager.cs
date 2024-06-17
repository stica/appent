using Events.Domain.Contract.Commands;
using Events.Domain.Entites;

namespace Events.Domain.Managers
{
    public class EventManager : IEventManager
    {
        public Task AddEvent(AddEventCommand addEventCommand)
        {
            throw new NotImplementedException();
        }

        public Task<List<Event>> GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}
