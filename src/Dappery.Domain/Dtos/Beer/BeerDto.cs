namespace Dappery.Domain.Dtos.Beer
{
    using Brewery;

    public class BeerDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        
        public string? Style { get; set; }

        public BreweryDto? Brewery { get; set; }
    }
}