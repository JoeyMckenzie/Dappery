namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using Domain.Dtos.Beer;
    using Domain.Media;
    using MediatR;

    public class CreateBeerCommand : IRequest<BeerResource>
    {
        public CreateBeerCommand(BeerDto beerDto) => Beer = beerDto;
        
        public BeerDto Beer { get; }
    }
}