namespace Dappery.Data.Tests
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Shouldly;
    using Xunit;

    public class BreweryRepositoryTest : TestFixture
    {
        [Fact]
        public void GetAllBreweries_WhenInvokedAndBreweriesExist_ReturnsValidListOfBreweries()
        {
            
        }
        
        [Fact]
        public async Task GetBreweryById_WhenInvokedAndBreweryExist_ReturnsValidBreweryWithBeersAndAddress()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;

            // Act
            var brewery = await unitOfWork.BreweryRepository.GetBreweryById(1);

            // Assert
            brewery.ShouldNotBeNull();
            brewery.ShouldBeOfType<Brewery>();
            brewery.Address.ShouldNotBeNull();
            brewery.Beers.ShouldNotBeNull();
            brewery.Beers.ShouldNotBeEmpty();   
            brewery.Beers.Count.ShouldBe(3);
            brewery.Beers.ShouldContain(b => b.Name == "Hexagenia");
            brewery.Beers.ShouldContain(b => b.Name == "Widowmaker");
            brewery.Beers.ShouldContain(b => b.Name == "Hooked");
        }
        
        [Fact]
        public async Task GetBreweryById_WhenInvokedAndNoBreweryExist_ReturnsNull()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;

            // Act
            var brewery = await unitOfWork.BreweryRepository.GetBreweryById(11);

            // Assert
            brewery.ShouldBeNull();
        }
    }
}