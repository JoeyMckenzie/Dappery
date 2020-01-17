namespace Dappery.Core.Tests.Breweries
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Breweries.Queries.GetBreweries;
    using Domain.Media;
    using Shouldly;
    using Xunit;

    public class GetBreweriesQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GetBreweriesQueryHandler_WhenBreweriesExist_ReturnsListOfBreweriesWithBeers()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var handler = new GetBreweriesQueryHandler(unitOfWork);

            // Act
            var response = await handler.Handle(new GetBreweriesQuery(), CancellationTestToken);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<BreweryResourceList>();
            response.Items.ShouldNotBeNull();
            response.Items.ShouldNotBeEmpty();
            response.Count.ShouldBe(2);
            response.Items.FirstOrDefault(b => b.Id == 1)?.Address.ShouldNotBeNull();
            response.Items.FirstOrDefault(b => b.Id == 1)?.Beers.ShouldNotBeEmpty();
            response.Items.FirstOrDefault(b => b.Id == 1)?.BeerCount.ShouldNotBeNull();
            response.Items.FirstOrDefault(b => b.Id == 1)?.BeerCount.ShouldBe(3);
            response.Items.FirstOrDefault(b => b.Id == 2)?.Address.ShouldNotBeNull();
            response.Items.FirstOrDefault(b => b.Id == 2)?.Beers.ShouldNotBeEmpty();
            response.Items.FirstOrDefault(b => b.Id == 2)?.BeerCount.ShouldNotBeNull();
            response.Items.FirstOrDefault(b => b.Id == 2)?.BeerCount.ShouldBe(2);
        }
        
        [Fact]
        public async Task GetBreweriesQueryHandler_WhenNoBreweriesExist_ReturnsEmptyListOfBreweries()
        {
            // Arrange, remove all breweries from the test database
            using var unitOfWork = UnitOfWork;
            await UnitOfWork.BreweryRepository.DeleteBrewery(1, CancellationTestToken);
            await UnitOfWork.BreweryRepository.DeleteBrewery(2, CancellationTestToken);
            var handler = new GetBreweriesQueryHandler(unitOfWork);

            // Act
            var response = await handler.Handle(new GetBreweriesQuery(), CancellationTestToken);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<BreweryResourceList>();
            response.Items.ShouldNotBeNull();
            response.Items.ShouldBeEmpty();
            response.Count.ShouldBe(0);
        }
    }
}