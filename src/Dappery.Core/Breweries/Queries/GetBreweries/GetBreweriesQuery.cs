namespace Dappery.Core.Breweries.Queries.GetBreweries
{
    using Domain.Media;
    using MediatR;

    public class GetBreweriesQuery : IRequest<BreweryResourceList>
    {
    }
}