namespace Dappery.Core.Beers.Commands.UpdateBeery
{
    using Domain.Dtos.Beer;
    using Domain.Media;
    using MediatR;

    public class UpdateBeerCommand : IRequest<BeerResource>
    {
        public UpdateBeerCommand(CreateBeerDto beerDto) => Dto = beerDto;
        
        public CreateBeerDto Dto { get; }
    }
}