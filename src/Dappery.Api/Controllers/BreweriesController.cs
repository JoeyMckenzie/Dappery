namespace Dappery.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Breweries.Commands.CreateBrewery;
    using Core.Breweries.Commands.DeleteBrewery;
    using Core.Breweries.Commands.UpdateBrewery;
    using Core.Breweries.Queries.GetBreweries;
    using Core.Breweries.Queries.RetrieveBrewery;
    using Domain.Dtos.Brewery;
    using Domain.Media;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class BreweriesController : DapperyControllerBase
    {
        private readonly ILogger<BreweriesController> _logger;

        public BreweriesController(ILogger<BreweriesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BreweryResource), StatusCodes.Status200OK)]
        public async Task<BreweryResource> CreateBrewery([FromBody] CreateBreweryDto breweryDto)
        {
            _logger.LogInformation("Creating brewery for request {@breweryDto}", breweryDto);
            return await Mediator.Send(new CreateBreweryCommand(breweryDto));
        }

        [HttpGet]
        public async Task<BreweryResourceList> GetBreweries()
        {
            _logger.LogInformation("Retrieving all breweries");
            return await Mediator.Send(new GetBreweriesQuery());
        }

        [HttpGet("{id}")]
        public async Task<BreweryResource> RetrieveBrewery(int id)
        {
            _logger.LogInformation($"Retrieve brewery with ID {id}");
            return await Mediator.Send(new RetrieveBreweryQuery(id));
        }

        [HttpPut("{id}")]
        public async Task<BreweryResource> UpdateBrewery(int id, [FromBody] UpdateBreweryDto breweryDto)
        {
            _logger.LogInformation($"Updating brewery with ID {id}");
            return await Mediator.Send(new UpdateBreweryCommand(breweryDto, id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewery(int id)
        {
            _logger.LogInformation($"Deleting brewery with ID {id}");
            await Mediator.Send(new DeleteBreweryCommand(id));
            return NoContent();
        }
    }
}