namespace Dappery.Core.Breweries.Commands.CreateBrewery
{
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using MediatR;

    public class CreateBreweryCommand : IRequest<BreweryResource>
    {
        public CreateBreweryCommand(CreateBreweryDto dto) => Dto = dto;

        public CreateBreweryDto Dto { get; }
    }
}