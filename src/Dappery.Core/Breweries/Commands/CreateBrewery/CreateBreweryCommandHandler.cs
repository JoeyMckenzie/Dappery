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
            
            // Create the brewery and commit the transaction against the database and clean up our resources
            var breweryId = await _unitOfWork.BreweryRepository.CreateBrewery(breweryToCreate, cancellationToken);

            // Grab a reference to our newly inserted brewery
            var insertedBrewery = await _unitOfWork.BreweryRepository.GetBreweryById(breweryId, cancellationToken);
            
            // Commit the transaction and clean up our resources
            _unitOfWork.Commit();
            
            // Map and return the response
            return new BreweryResource(insertedBrewery.ToBreweryDto());
        }
    }
}