namespace Dappery.Core.Beers.Queries.GetBeers
{
    using Domain.Media;
    using MediatR;

    public class GetBeersQuery : IRequest<BeerResourceList>
    {
    }
}