namespace Dappery.Domain.Media
{
    using Dtos.Brewery;

    public class BreweryResource : Resource<BreweryDto>
    {
        public BreweryResource(BreweryDto resource) 
            : base(resource)
        {
        }
    }
}