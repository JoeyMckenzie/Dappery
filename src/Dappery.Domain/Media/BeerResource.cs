namespace Dappery.Domain.Media
{
    using Dtos.Beer;

    public class BeerResource
    {
        public BeerResource(BeerDto beer) => Beer = beer;
        
        public BeerDto Beer { get; }
    }
}
