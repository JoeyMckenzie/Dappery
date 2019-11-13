namespace Dappery.Domain.Dtos.Beer
{
    using Entities;

    public class CreateBeerDto
    {
        public string? Name { get; set; }

        public BeerStyle Style { get; set; }
    }
}