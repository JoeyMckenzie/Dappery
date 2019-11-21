namespace Dappery.Core.Beers.Commands.UpdateBeery
{
    using Domain.Dtos.Beer;
    using Domain.Media;
    using MediatR;

    public class UpdateBeerCommand : IRequest<BeerResource>
    {
        public UpdateBeerCommand(UpdateBeerDto beerDto) => Dto = beerDto;
        
        public UpdateBeerDto Dto { get; }
    }
}