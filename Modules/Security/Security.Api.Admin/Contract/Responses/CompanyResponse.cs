namespace Security.Api.Admin.Contract.Responses
{
    public class CompanyResponse
    {
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
