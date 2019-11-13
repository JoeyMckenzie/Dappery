using System.Linq;
using Dappery.Core.Extensions;

namespace Dappery.Core.Breweries.Queries.GetBreweries
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using MediatR;

    public class GetBreweriesQueryHandler : IRequestHandler<GetBreweriesQuery, BreweryResourceList>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBreweriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BreweryResourceList> Handle(GetBreweriesQuery request, CancellationToken cancellationToken)
        {
            // Grab a reference to all breweries in the database
            var breweries = await _unitOfWork.BreweryRepository.GetAllBreweries();
            
            _unitOfWork.Commit();
            
            // Map each brewery to its corresponding DTO
            return new BreweryResourceList(breweries.Select(b => b.ToBreweryDto()));
        }
    }
}