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

            // Invalidate the request if no brewery is found
            if (breweryToDelete is null)
            {
                throw new DapperyApiException($"No brewery was found with ID {request.BreweryId}", HttpStatusCode.NotFound);
            }
            
            // Delete the brewery from the database and clean up our resources once we know we have a valid beer
            await _unitOfWork.BreweryRepository.DeleteBrewery(request.BreweryId, cancellationToken);
            _unitOfWork.Commit();
            
            return Unit.Value;
        }
    }
}