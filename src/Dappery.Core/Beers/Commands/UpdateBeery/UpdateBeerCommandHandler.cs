namespace Dappery.Core.Beers.Commands.UpdateBeery
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Media;
    using MediatR;

    public class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, BeerResource>
    {
        public async Task<BeerResource> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}