namespace Dappery.Domain.Dtos.Beer
{
    using Brewery;
    using Entities;

    public class BeerDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public BeerStyle Style { get; set; }

        public BreweryDto Brewery { get; set; }
    }
}