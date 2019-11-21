namespace Dappery.Domain.Dtos.Brewery
{
    public class UpdateBreweryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AddressDto Address { get; set; }
    }
}