namespace Dappery.Domain.Dtos.Brewery
{
    using Entities;

    public class UpdateBeerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public BeerStyle Style { get; set; }
    }
}