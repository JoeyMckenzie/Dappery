namespace Dappery.Core.Beers.Queries.RetrieveBeer
{
    using Domain.Media;
    using MediatR;

    public class RetrieveBeerQuery : IRequest<BeerResource>
    {
        public RetrieveBeerQuery(int id) => Id = id;
        
        public int Id { get; }
    }
}