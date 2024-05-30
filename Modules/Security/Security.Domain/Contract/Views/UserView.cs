namespace Security.Domain.Contract.Views
{
    public class UserView
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public int CompanyId { get; set; }

        public bool IsAdmin { get; set; }
    }
}
