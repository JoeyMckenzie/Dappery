namespace Dappery.Core.Beers.Queries.RetrieveBeer
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Media;
    using Exceptions;
    using Extensions;
    using MediatR;

    public class RetrieveBeerQueryHandler : IRequestHandler<RetrieveBeerQuery, BeerResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RetrieveBeerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BeerResource> Handle(RetrieveBeerQuery request, CancellationToken cancellationToken)
        {
            // Grab the beer from the ID
            var beer = await _unitOfWork.BeerRepository.GetBeerByIdAsync(request.Id, cancellationToken);
            
            // Invalidate the request if no beer is found
            if (beer is null)
            {
                throw new DapperyApiException($"No beer found with ID {request.Id}", HttpStatusCode.NotFound);
            }
            
            // Commit the query and clean up our resources
            _unitOfWork.Commit();
            
            // Map and return the query
            return new BeerResource(beer.ToBeerDto());
        }
    }
}