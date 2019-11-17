namespace Dappery.Domain.Media
{
    using Dtos.Brewery;

    public class BreweryResource : Resource<BreweryDto>
    {
        public BreweryResource(BreweryDto brewery) => Brewery = brewery;
        
        public BreweryDto Brewery { get; }
    }
}