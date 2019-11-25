namespace Dappery.Core.Beers.Commands.DeleteBeer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using MediatR;

    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBeerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the beer from the request
            var existingBeer = await _unitOfWork.BeerRepository.GetBeerByIdAsync(request.BeerId, cancellationToken);
            
            // Invalidate the request if no beer is found 

            return Unit.Value;
        }
    }
}