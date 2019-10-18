namespace Dappery.Domain.Dtos.Brewery
{
    using Entities;

    public class UpdateBreweryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AddressDto Type { get; set; }
    }
}