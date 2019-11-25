namespace Dappery.Core.Breweries.Commands.DeleteBrewery
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Exceptions;
    using MediatR;

    public class DeleteBreweryCommandHandler : IRequestHandler<DeleteBreweryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBreweryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteBreweryCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the brewery and invalidate the request if none is found
            var breweryToDelete = await _unitOfWork.BreweryRepository.GetBreweryById(request.BreweryId, cancellationToken);

            if (breweryToDelete is null)
            {
                throw new DapperyApiException($"No brewery was found with ID {request.BreweryId}", HttpStatusCode.NotFound);
            }
            
            // Delete the brewery from the database
            await _unitOfWork.BreweryRepository.DeleteBrewery(request.BreweryId, cancellationToken);
            _unitOfWork.Commit();
            
            return Unit.Value;
        }
    }
}