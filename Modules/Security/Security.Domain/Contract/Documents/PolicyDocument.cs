using System.Collections.Generic;
using System.Linq;

namespace Security.Domain.Contract.Documents
{
    public class PolicyDocument
    {
        public List<StatementDocument> Statemens { get; set; }

        public bool CheckPolicy(string module, string action)
        {
            var matched = Statemens.Where(x => x.IsMatched(module, action));
            return matched.Any();
        }
    }
}
