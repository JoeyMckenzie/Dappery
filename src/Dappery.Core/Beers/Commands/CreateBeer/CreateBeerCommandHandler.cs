namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Media;
    using MediatR;

    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, BeerResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBeerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<BeerResource> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}