namespace Dappery.Core.Breweries.Commands.UpdateBrewery
{
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using MediatR;

    public class UpdateBreweryCommand : IRequest<BreweryResource>
    {
        public UpdateBreweryCommand(UpdateBreweryDto dto, int breweryId) => (Dto, BreweryId) = (dto, breweryId);

        public int BreweryId { get; }
            
        public UpdateBreweryDto Dto { get; }
    }
}