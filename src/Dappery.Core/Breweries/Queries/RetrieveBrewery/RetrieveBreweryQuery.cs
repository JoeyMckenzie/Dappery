namespace Dappery.Core.Breweries.Queries.RetrieveBrewery
{
    using Domain.Media;
    using MediatR;

    public class RetrieveBreweryQuery : IRequest<BreweryResource>
    {
        public RetrieveBreweryQuery(int id) => Id = id;
        
        public int Id { get; }
    }
}