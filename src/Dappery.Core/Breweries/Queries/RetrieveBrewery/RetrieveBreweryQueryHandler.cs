using Dappery.Core.Extensions;

namespace Dappery.Core.Breweries.Queries.RetrieveBrewery
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using Exceptions;
    using MediatR;

    public class RetrieveBreweryQueryHandler : IRequestHandler<RetrieveBreweryQuery, BreweryResource>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RetrieveBreweryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BreweryResource> Handle(RetrieveBreweryQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the brewery
            var brewery = await _unitOfWork.BreweryRepository.GetBreweryById(request.Id);
            
            // Invalidate the request if no brewery is found
            if (brewery == null)
            {
                throw new DapperyApiException($"No brewery found with ID {request.Id}", HttpStatusCode.NotFound);
            }
            
            // Commit the query and clean up our resources
            _unitOfWork.Commit();
            
            // Map and return the brewery
            return new BreweryResource(brewery.ToBreweryDto());
        }
    }
}