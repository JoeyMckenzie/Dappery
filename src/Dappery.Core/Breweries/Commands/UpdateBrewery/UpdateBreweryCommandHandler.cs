namespace Dappery.Core.Breweries.Commands.UpdateBrewery
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using Data;
    using Domain.Media;
    using Exceptions;
    using MediatR;

    public class UpdateBreweryCommandHandler : IRequestHandler<UpdateBreweryCommand, BreweryResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBreweryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BreweryResource> Handle(UpdateBreweryCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the brewery on the request
            var breweryToUpdate = await _unitOfWork.BreweryRepository.GetBreweryById(request.BreweryId, cancellationToken);

            // Invalidate the request if no brewery was found
            if (breweryToUpdate is null)
            {
                throw new DapperyApiException($"No brewery was found with ID {request.BreweryId}", HttpStatusCode.NotFound);   
            }
            
            // Update the properties on the brewery entity
            breweryToUpdate.Name = request.Dto.Name;
            var updateBrewery = false;

            // If the request contains an address, set the flag for the persistence layer to update the address table
            if (request.Dto.Address != null && breweryToUpdate.Address != null)
            {
                updateBrewery = true;
                breweryToUpdate.Address.StreetAddress = request.Dto.Address.StreetAddress;
                breweryToUpdate.Address.City = request.Dto.Address.City;
                breweryToUpdate.Address.State = request.Dto.Address.State;
                breweryToUpdate.Address.ZipCode = request.Dto.Address.ZipCode;
            }
            
            // Update the brewery in the database, retrieve it, and clean up our resources
            await _unitOfWork.BreweryRepository.UpdateBrewery(breweryToUpdate, cancellationToken, updateBrewery);
            var updatedBrewery = await _unitOfWork.BreweryRepository.GetBreweryById(request.BreweryId, cancellationToken);
            _unitOfWork.Commit();
            
            // Map and return the brewery
            return new BreweryResource(updatedBrewery.ToBreweryDto());
        }
    }
}