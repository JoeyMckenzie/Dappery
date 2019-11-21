namespace Dappery.Domain.Dtos.Beer
{
    public class UpdateBeerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Style { get; set; }

        public int BreweryId { get; set; }
    }
}