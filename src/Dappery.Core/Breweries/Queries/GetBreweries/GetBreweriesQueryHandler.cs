namespace Dappery.Core.Breweries.Queries.GetBreweries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Media;
    using Extensions;
    using MediatR;

    public class GetBreweriesQueryHandler : IRequestHandler<GetBreweriesQuery, BreweryResourceList>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBreweriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BreweryResourceList> Handle(GetBreweriesQuery request, CancellationToken cancellationToken)
        {
            // Grab a reference to all breweries in the database
            var breweries = await _unitOfWork.BreweryRepository.GetAllBreweries(cancellationToken);
            
            // Clean up our resources
            _unitOfWork.Commit();
            
            // Map our breweries from the returned query
            var mappedBreweries = breweries.Select(b => b.ToBreweryDto());
            
            // Map each brewery to its corresponding DTO
            return new BreweryResourceList(mappedBreweries);
        }
    }
}