using System.Linq;
using Dappery.Core.Extensions;

namespace Dappery.Core.Breweries.Queries.GetBreweries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Media;
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
            
            // Map each brewery to its corresponding DTO
            return new BreweryResourceList(breweries.Select(b => b.ToBreweryDto()));
        }
    }
}