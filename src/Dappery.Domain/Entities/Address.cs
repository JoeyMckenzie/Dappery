namespace Dappery.Domain.Entities
{
    public class Address : TimeStampedEntity
    {
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }
    }
}