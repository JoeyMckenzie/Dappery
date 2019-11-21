namespace Dappery.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Beers.Commands.CreateBeer;
    using Core.Beers.Commands.UpdateBeery;
    using Core.Beers.Queries.GetBeers;
    using Core.Beers.Queries.RetrieveBeer;
    using Domain.Dtos.Beer;
    using Domain.Media;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class BeersController : DapperyControllerBase
    {
        private readonly ILogger<BeersController> _logger;

        public BeersController(ILogger<BeersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<BeerResourceList> GetAllBeers()
        {
            _logger.LogInformation("Retrieving all beers from the database for request");
            return await Mediator.Send(new GetBeersQuery());
        }

        [HttpGet("{id}")]
        public async Task<BeerResource> GetBeerById(int id)
        {
            _logger.LogInformation($"Retrieving beer from ID {id}");
            return await Mediator.Send(new RetrieveBeerQuery(id));
        }

        [HttpPost]
        public async Task<BeerResource> CreateBeer(CreateBeerDto beerDto)
        {
            _logger.LogInformation($"Creating beer for brewery ID {beerDto}");
            return await Mediator.Send(new CreateBeerCommand(beerDto));
        }

        [HttpPut("{id}")]
        public async Task<BeerResource> UpdateBeer(UpdateBeerDto beerDto)
        {
            return await Mediator.Send(new UpdateBeerCommand(beerDto))
        }
    }
}