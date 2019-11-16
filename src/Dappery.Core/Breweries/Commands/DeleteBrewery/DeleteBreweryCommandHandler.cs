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
            var breweryToDelete = await _unitOfWork.BreweryRepository.GetBreweryById(request.Id);

            if (breweryToDelete == null)
            {
                throw new DapperyApiException($"No brewery was found with ID {request.Id}", HttpStatusCode.NotFound);
            }
            
            // Delete the brewery from the database
            await _unitOfWork.BreweryRepository.DeleteBrewery(request.Id);
            _unitOfWork.Commit();
            
            // Commit the transaction and clean up our resources
            
            return Unit.Value;
        }
    }
}