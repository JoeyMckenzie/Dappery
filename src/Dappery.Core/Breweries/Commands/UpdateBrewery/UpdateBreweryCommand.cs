namespace Dappery.Core.Breweries.Commands.UpdateBrewery
{
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using MediatR;

    public class UpdateBreweryCommand : IRequest<BreweryResource>
    {
        public UpdateBreweryCommand(UpdateBreweryDto dto, int id) => (Dto, Id) = (dto, id);

        public int Id { get; }
            
        public UpdateBreweryDto Dto { get; }
    }
}