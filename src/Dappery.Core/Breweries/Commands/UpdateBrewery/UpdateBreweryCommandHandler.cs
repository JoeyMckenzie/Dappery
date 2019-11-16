namespace Dappery.Core.Breweries.Commands.UpdateBrewery
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Dappery.Core.Extensions;
    using Data;
    using Domain.Dtos.Brewery;
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
            var breweryToUpdate = await _unitOfWork.BreweryRepository.GetBreweryById(request.Id);

            // Invalidate the request if no brewery was found
            if (breweryToUpdate == null)
            {
                throw new DapperyApiException($"No brewery was found with ID {request.Id}", HttpStatusCode.NotFound);   
            }
            
            // Update the properties on the brewery entity
            breweryToUpdate.Name = request.Dto.Name;
            var updateBrewery = false;

            if (request.Dto.Address != null && breweryToUpdate.Address != null)
            {
                updateBrewery = true;
                breweryToUpdate.Address.StreetAddress = request.Dto.Address.StreetAddress;
                breweryToUpdate.Address.City = request.Dto.Address.City;
                breweryToUpdate.Address.State = request.Dto.Address.State;
                breweryToUpdate.Address.ZipCode = request.Dto.Address.ZipCode;
            }
            
            // Update the brewery in the database
            await _unitOfWork.BreweryRepository.UpdateBrewery(breweryToUpdate, updateBrewery);
            
            // Grab a reference to the updated brewery
            var updatedBrewery = await _unitOfWork.BreweryRepository.GetBreweryById(request.Id);
            
            // Commit the transaction and clean up our resources
            _unitOfWork.Commit();
            
            // Map and return the brewery
            return new BreweryResource(updatedBrewery.ToBreweryDto());
        }
    }
}