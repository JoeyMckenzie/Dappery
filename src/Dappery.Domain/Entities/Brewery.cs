namespace Dappery.Domain.Entities
{
    using System.Collections.Generic;

    public class Brewery : TimeStampedEntity
    {
        public string Name { get; set; }

        public Address Address { get; set; }

        public ICollection<Beer> Beers { get; set; }

        public int BeerCount => Beers.Count;
    }
}