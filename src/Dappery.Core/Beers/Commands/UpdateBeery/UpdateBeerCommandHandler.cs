namespace Dappery.Core.Beers.Commands.UpdateBeery
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Entities;
    using Domain.Media;
    using Exceptions;
    using Extensions;
    using MediatR;

    public class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, BeerResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBeerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BeerResource> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            // First validate both the beer
            var existingBeer = await _unitOfWork.BeerRepository.GetBeerByIdAsync(request.BeerId, cancellationToken);
            
            // Invalidate the request if no beer was found
            if (existingBeer is null)
            {
                throw new DapperyApiException($"Beer with ID {request.BeerId} was not found", HttpStatusCode.NotFound);
            }
            
            // Attempt to parse the incoming BeerStyle enumeration value (exactly how we parse in the CreateBeerCommandHandler)
            var parsedBeerStyle = Enum.TryParse(request.Dto.Style, true, out BeerStyle beerStyle);
            
            // Update the fields on the existing beer
            existingBeer.BreweryId = request.Dto.BreweryId ?? existingBeer.BreweryId;
            existingBeer.BeerStyle = parsedBeerStyle ? beerStyle : BeerStyle.Other;
            existingBeer.Name = request.Dto.Name;
            existingBeer.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.BeerRepository.UpdateBeerAsync(existingBeer, cancellationToken);
            
            // Grab the newly return beer for the response
            var updatedBeer = await _unitOfWork.BeerRepository.GetBeerByIdAsync(existingBeer.Id, cancellationToken);
            _unitOfWork.Commit();
            
            // Return the response with the updated beer
            return new BeerResource(updatedBeer.ToBeerDto());
        }
    }
}