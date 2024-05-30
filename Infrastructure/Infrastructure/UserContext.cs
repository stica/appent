using System.Threading;

namespace Start.Infrastructure
{
    public class UserContext
    {
        private static AsyncLocal<UserContext> _current = new AsyncLocal<UserContext>();

        public static UserContext Current
        {
            get
            {
                return _current.Value;
            }
        }

        public string IpAddress { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsAdmin { get; set; }

        public static void CreateUserContext(int userId, int companyId, string ipAddress, bool isAdmin)
        {
            _current.Value = new UserContext
            {
                UserId = userId,
                CompanyId = companyId,
                IpAddress = ipAddress,
                IsAdmin = isAdmin
            };
        }

        public static void CreateAnonymousUserContext(string ipAddress)
        {
            _current.Value = new UserContext
            {
                IpAddress = ipAddress
            };
        }

        public static void InvalidateUserContext()
        {
            _current.Value = null;
        }
    }
}
