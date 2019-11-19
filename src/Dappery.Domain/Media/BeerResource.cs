namespace Dappery.Domain.Media
{
    using Dtos.Beer;

    public class BeerResource : Resource<BeerDto>
    {
        public BeerResource(BeerDto resource) 
            : base(resource)
        {
        }
    }
}
