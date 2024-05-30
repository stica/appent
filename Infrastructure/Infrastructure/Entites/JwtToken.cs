using System.Security.Principal;

namespace Start.Infrastructure.Entites
{
    public class JwtToken : GenericIdentity
    {
        public JwtToken()
            : base(string.Empty)
        {

        }

        public string UserName { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
