namespace Dappery.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Shouldly;
    using Xunit;

    public class BreweryRepositoryTest : TestFixture
    {
        [Fact]
        public async Task GetAllBreweries_WhenInvokedAndBreweriesExist_ReturnsValidListOfBreweries()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;

            // Act
            var breweries = (await unitOfWork.BreweryRepository.GetAllBreweries()).ToList();

            // Assert
            breweries.ShouldNotBeNull();
            breweries.ShouldNotBeEmpty();
            breweries.Count.ShouldBe(2);
            breweries.All(br => br.Address != null).ShouldBeTrue();
            breweries.All(br => br.Beers != null).ShouldBeTrue();
            breweries.All(br => br.Beers.Any()).ShouldBeTrue();
            breweries.FirstOrDefault(br => br.Name == "Fall River Brewery")?.Beers
                .ShouldContain(b => b.Name == "Hexagenia");
            breweries.FirstOrDefault(br => br.Name == "Fall River Brewery")?.Beers
                .ShouldContain(b => b.Name == "Widowmaker");
            breweries.FirstOrDefault(br => br.Name == "Fall River Brewery")?.Beers
                .ShouldContain(b => b.Name == "Hooked");
            breweries.FirstOrDefault(br => br.Name == "Sierra Nevada Brewing Company")?.Beers
                .ShouldContain(b => b.Name == "Pale Ale");
            breweries.FirstOrDefault(br => br.Name == "Sierra Nevada Brewing Company")?.Beers
                .ShouldContain(b => b.Name == "Hazy Little Thing");
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

        [Fact]
        public async Task CreateBrewery_WhenBreweryIsValid_ReturnsNewlyInsertedBrewery()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var breweryToInsert = new Brewery
            {
                Name = "Bike Dog Brewing Company",
                Address = new Address
                {
                    StreetAddress = "123 Sacramento St.",
                    City = "Sacramento",
                    State = "CA",
                    ZipCode = "95811",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            // Act
            var insertedBrewery = await unitOfWork.BreweryRepository.CreateBrewery(breweryToInsert);
            
            insertedBrewery.ShouldNotBeNull();
            insertedBrewery.ShouldBeOfType<Brewery>();
            insertedBrewery.Address.ShouldNotBeNull();
            insertedBrewery.Address.StreetAddress.ShouldBe(breweryToInsert.Address.StreetAddress);
            insertedBrewery.Address.BreweryId.ShouldBe(3);
            insertedBrewery.Beers.ShouldBeEmpty();
        }
    }
}