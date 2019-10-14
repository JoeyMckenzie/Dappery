namespace Dappery.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Shouldly;
    using Xunit;

    public class BeerRepositoryTest : TestFixture
    {
        [Fact]
        public async Task GetAllBeers_WhenInvokedAndBeersExist_ReturnsValidListOfBeers()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            
            // Act
            var beers = (await unitOfWork.BeerRepository.GetAllBeers()).ToList();
                
            // Assert
            beers.ShouldNotBeNull();
            beers.ShouldBeOfType<List<Beer>>();
            beers.ShouldNotBeEmpty();
            beers.All(b => b.Brewery != null).ShouldBeTrue();
            beers.All(b => b.Brewery.Address != null).ShouldBeTrue();
            beers.All(b => b.Brewery.Address.BreweryId == b.Brewery.Id).ShouldBeTrue();
            beers.ShouldContain(b => b.Name == "Hexagenia");
            beers.FirstOrDefault(b => b.Name == "Hexagenia")?.BeerStyle.ShouldBe(BeerStyle.Ipa);
            beers.ShouldContain(b => b.Name == "Widowmaker");
            beers.FirstOrDefault(b => b.Name == "Widowmaker")?.BeerStyle.ShouldBe(BeerStyle.DoubleIpa);
            beers.ShouldContain(b => b.Name == "Hooked");
            beers.FirstOrDefault(b => b.Name == "Hooked")?.BeerStyle.ShouldBe(BeerStyle.Lager);
            beers.ShouldContain(b => b.Name == "Pale Ale");
            beers.FirstOrDefault(b => b.Name == "Pale Ale")?.BeerStyle.ShouldBe(BeerStyle.PaleAle);
            beers.ShouldContain(b => b.Name == "Hazy Little Thing");
            beers.FirstOrDefault(b => b.Name == "Hazy Little Thing")?.BeerStyle.ShouldBe(BeerStyle.NewEnglandIpa);
        }
        
        [Fact]
        public async Task GetBeerById_WhenInvokedAndBeerExists_ReturnsValidMappedBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            
            // Act
            var beer = await unitOfWork.BeerRepository.GetBeerById(1);
                
            // Assert, validate a few properties
            beer.ShouldNotBeNull();
            beer.ShouldBeOfType<Beer>();
            beer.Name.ShouldBe("Hexagenia");
            beer.BeerStyle.ShouldBe(BeerStyle.Ipa);
            beer.Brewery.ShouldNotBeNull();
            beer.Brewery.Name.ShouldBe("Fall River Brewery");
            beer.Brewery.Address.ShouldNotBeNull();
            beer.Brewery.Address.City.ShouldBe("Redding");
        }
        
        [Fact]
        public async Task GetBeerById_WhenInvokedAndBeerDoesNotExist_ReturnsNull()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            
            // Act
            var beer = await unitOfWork.BeerRepository.GetBeerById(10);
                
            // Assert, validate a few properties
            beer.ShouldBeNull();
        }
    }
}