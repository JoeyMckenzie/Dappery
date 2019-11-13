namespace Dappery.Domain.Entities
{
    public class Beer : TimeStampedEntity
    {
        public string Name { get; set; } = default!;
        
        public BeerStyle BeerStyle { get; set; }

        public int BreweryId { get; set; }

        public Brewery Brewery { get; set; } = default!;
    }
}