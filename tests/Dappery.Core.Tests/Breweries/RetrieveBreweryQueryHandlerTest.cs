namespace Dappery.Core.Tests.Breweries
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Breweries.Queries.RetrieveBrewery;
    using Domain.Media;
    using Exceptions;
    using Shouldly;
    using Xunit;

    public class RetrieveBreweryQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task RetrieveBreweryHandler_GivenExistingBreweryId_ReturnsBreweryWithBeers()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new RetrieveBreweryQuery(1);
            var handler = new RetrieveBreweryQueryHandler(unitOfWork);
            
            // Act
            var response = await handler.Handle(query, CancellationTestToken);
            
            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<BreweryResource>();
            response.Self.ShouldNotBeNull();
            response.Self.Address?.ShouldNotBeNull();
            response.Self.Beers.ShouldNotBeNull();
            response.Self.Beers.ShouldNotBeEmpty();
            response.Self.Name.ShouldBe("Fall River Brewery");
            response.Self.BeerCount?.ShouldNotBeNull();
            response.Self.BeerCount?.ShouldBe(3);
        }
        
        [Fact]
        public async Task RetrieveBreweryHandler_GivenNonExistingBreweryId_ReturnsApiException()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new RetrieveBreweryQuery(11);
            var handler = new RetrieveBreweryQueryHandler(unitOfWork);
            
            // Act
            var response = await Should.ThrowAsync<DapperyApiException>(async () => await handler.Handle(query, CancellationTestToken));
            
            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<DapperyApiException>();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}