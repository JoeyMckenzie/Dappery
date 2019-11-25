namespace Dappery.Core.Beers.Commands.UpdateBeery
{
    using Domain.Dtos.Beer;
    using Domain.Media;
    using MediatR;

    public class UpdateBeerCommand : IRequest<BeerResource>
    {
        public UpdateBeerCommand(UpdateBeerDto beerDto, int requestId) => (Dto, BeerId) = (beerDto, requestId);
        
        public UpdateBeerDto Dto { get; }

        public int BeerId { get; }
    }
}