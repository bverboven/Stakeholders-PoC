namespace Regira.Stakeholders.Core.Entities
{
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Box { get; set; }

        public string PostBox { get; set; }

        public string PostalCode { get; set; }
        public string City { get; set; }

        public string CountryCode { get; set; }
    }
}
