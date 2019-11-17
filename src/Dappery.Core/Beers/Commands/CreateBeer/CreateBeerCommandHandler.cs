namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dappery.Domain.Entities;
    using Dappery.Domain.Media;
    using Data;
    using MediatR;

    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, BeerResource>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBeerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<BeerResource> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            // // Attempt to parse the incoming BeerStyle enumeration value
            // var parsedBeerStyle = Enum.TryParse<BeerStyle>(request.Dto.Style, true, out BeerStyle beerStyle);

            // // Let's instantiate a beer instance
            // var beerToAdd = new Beer
            // {
            //     Name = request.Dto.Name,
            //     BeerStyle = parsedBeerStyle ? beerStyle : BeerStyle.Other,
            // };

            throw new System.NotImplementedException();
        }
    }
}