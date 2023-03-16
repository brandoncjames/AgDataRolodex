namespace AgDataRolodex.Contracts.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street1 { get; set; } = String.Empty;
        public string? Street2 { get; set; }
        public string City { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;

    }
}
