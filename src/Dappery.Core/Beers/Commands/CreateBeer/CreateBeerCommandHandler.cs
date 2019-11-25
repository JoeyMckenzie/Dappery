namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using Domain.Entities;
    using Domain.Media;
    using Data;
    using MediatR;
    using Exceptions;
    using System.Net;

    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, BeerResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBeerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BeerResource> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            // Check to make sure the brewery exists from the given brewery ID on the request
            var existingBrewery = await _unitOfWork.BreweryRepository.GetBreweryById(request.Dto.BreweryId, cancellationToken);

            // Invalidate the request if no corresponding brewery exists
            // Since we're not overloading the '==' operator, let's use the 'is' comparison here
            if (existingBrewery is null)
            {
                throw new DapperyApiException($"No brewery found with ID {request.Dto.BreweryId}", HttpStatusCode.NotFound);
            }

            // Attempt to parse the incoming BeerStyle enumeration value
            var parsedBeerStyle = Enum.TryParse(request.Dto.Style, true, out BeerStyle beerStyle);

            // Let's instantiate a beer instance
            var beerToAdd = new Beer
            {
                Name = request.Dto.Name,
                BeerStyle = parsedBeerStyle ? beerStyle : BeerStyle.Other,
                BreweryId = request.Dto.BreweryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add the record to the database and retrieve the record after we create it
            var createdBeerId = await _unitOfWork.BeerRepository.CreateBeerAsync(beerToAdd, cancellationToken);
            var createdBeer = await _unitOfWork.BeerRepository.GetBeerByIdAsync(createdBeerId, cancellationToken);
            _unitOfWork.Commit();

            // Return the mapped beer
            return new BeerResource(createdBeer.ToBeerDto());
        }
    }
}