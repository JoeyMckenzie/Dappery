namespace Dappery.Core.Beers.Commands.DeleteBeer
{
    using MediatR;

    public class DeleteBeerCommand : IRequest<Unit>
    {
        public DeleteBeerCommand(int beerId) => BeerId = beerId;
        
        public int BeerId { get; }
    }
}