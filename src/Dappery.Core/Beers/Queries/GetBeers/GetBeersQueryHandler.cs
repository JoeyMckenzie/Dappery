namespace Dappery.Core.Beers.Queries.GetBeers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Domain.Media;
    using Extensions;
    using MediatR;

    public class GetBeersQueryHandler : IRequestHandler<GetBeersQuery, BeerResourceList>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBeersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BeerResourceList> Handle(GetBeersQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all of our beers from the database
            var beers = await _unitOfWork.BeerRepository.GetAllBeersAsync(cancellationToken);
            
            // Clean up our resources
            _unitOfWork.Commit();
            
            // Map our beers and return the response
            var mappedBeers = beers.Select(b => b.ToBeerDto());

            // Return our mapped beers
            return new BeerResourceList(mappedBeers);
        }
    }
}