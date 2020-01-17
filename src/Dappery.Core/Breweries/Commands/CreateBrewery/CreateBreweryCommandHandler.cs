namespace Dappery.Core.Breweries.Commands.CreateBrewery
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Entities;
    using Domain.Media;
    using Extensions;
    using MediatR;

    public class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, BreweryResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBreweryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BreweryResource> Handle(CreateBreweryCommand request, CancellationToken cancellationToken)
        {
            var breweryToCreate = new Brewery
            {
                Name = request.Dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    StreetAddress = request.Dto.Address?.StreetAddress,
                    City = request.Dto.Address?.City,
                    State = request.Dto.Address?.State,
                    ZipCode = request.Dto.Address?.ZipCode,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            
            // Create our brewery, retrieve the brewery so we can map it to the response, and clean up our resources
            var breweryId = await _unitOfWork.BreweryRepository.CreateBrewery(breweryToCreate, cancellationToken);
            var insertedBrewery = await _unitOfWork.BreweryRepository.GetBreweryById(breweryId, cancellationToken);
            _unitOfWork.Commit();
            
            // Map and return the response
            return new BreweryResource(insertedBrewery.ToBreweryDto());
        }
    }
}