namespace Dappery.Domain.Entities
{
    public class Beer : TimeStampedEntity
    {
        public string Name { get; set; }
        
        public BeerStyle BeerStyle { get; set; }

        public Brewery Brewery { get; set; }
    }
}