using Dappery.Domain.Media;
using MediatR;

namespace Dappery.Core.Beers.Queries.GetBeers
{
    public class GetBeersQuery : IRequest<BeerResourceList>
    {
    }
}