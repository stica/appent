using Events.Domain.Contract.Commands;
using Events.Domain.Entites;

namespace Events.Domain.Managers
{
    public interface IEventManager
    {
        Task<List<Event>> GetEvents();

        Task AddEvent(AddEventCommand addEventCommand);
    }
}
