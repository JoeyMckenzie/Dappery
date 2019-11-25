namespace Dappery.Domain.Dtos.Beer
{
    public class UpdateBeerDto
    {
        public string? Name { get; set; }

        public string? Style { get; set; }

        public int? BreweryId { get; set; }
    }
}