namespace Dappery.Domain.Dtos.Brewery
{
    using System.Collections.Generic;
    using Entities;

    public class BreweryDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public AddressDto Address { get; set; }

        public IEnumerable<Beer> Beers { get; set; }

        public int BeerCount { get; set; }
    }
}