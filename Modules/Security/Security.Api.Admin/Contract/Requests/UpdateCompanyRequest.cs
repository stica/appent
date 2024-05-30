namespace Security.Api.Admin.Contract.Requests
{
    public class UpdateCompanyRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Addresss { get; set; }

        public string PhoneNumber { get; set; }

        public string State { get; set; }

        public int NumberOfTrucsAllowed { get; set; }

        public string ShortName { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAdmin { get; set; }
    }
}
