namespace Dappery.Domain.Media
{
    using Dtos.Brewery;

    public class BreweryResource
    {
        public BreweryResource(BreweryDto brewery) => Brewery = brewery;
        
        public BreweryDto Brewery { get; }
    }
}