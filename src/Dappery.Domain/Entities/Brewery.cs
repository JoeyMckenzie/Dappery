namespace Dappery.Domain.Entities
{
    using System.Collections.Generic;

    public class Brewery : TimeStampedEntity
    {
        public Brewery()
        {
            Beers = new List<Beer>();
        }

        public string? Name { get; set; }

        public Address? Address { get; set; }

        public ICollection<Beer> Beers { get; }

        public int BeerCount => Beers.Count;
    }
}