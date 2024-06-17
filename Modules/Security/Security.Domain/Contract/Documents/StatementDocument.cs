using System.Linq;

namespace Security.Domain.Contract.Documents
{
    public class StatementDocument
    {
        public string[] Actions { get; set; }

        public bool IsMatched(string module, string action)
        {
            return Actions.Any(x => x == module + ":" + action);
        }
    }
}
