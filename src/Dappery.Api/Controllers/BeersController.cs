using System.Threading.Tasks;
using Dappery.Core.Beers.Queries.GetBeers;
using Dappery.Domain.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dappery.Api.Controllers
{
    using Core.Beers.Queries.RetrieveBeer;

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
    }
}