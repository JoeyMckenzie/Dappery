namespace Dappery.Core.Tests.Extensions
{
    using System.Linq;
    using Core.Extensions;
    using Domain.Entities;
    using Shouldly;
    using Xunit;

    public class BreweryExtensionsTest
    {
        [Fact]
        public void ToBreweryDto_GivenValidBreweryWithAListOfBeers_ReturnsMappedBreweryDto()
        {
            // Arrange
            var breweryToMap = new Brewery
            {
                Id = 1,
                Name = "Test Brewery",
                Address = new Address
                {
                    City = "Redding",
                    State = "CA",
                    ZipCode = "96002",
                    StreetAddress = "123 Redding St."
                }
            };
            
            // Add our beers since they are initialized within the domain
            breweryToMap.Beers.Add(new Beer { Id = 1, Name = "Test Beer 1", BeerStyle = BeerStyle.Lager });
            breweryToMap.Beers.Add(new Beer { Id = 2, Name = "Test Beer 2", BeerStyle = BeerStyle.Ipa });
            breweryToMap.Beers.Add(new Beer { Id = 3, Name = "Test Beer 3", BeerStyle = BeerStyle.DoubleIpa });
            
            // Act
            var mappedBrewery = breweryToMap.ToBreweryDto();
            
            // Assert
            mappedBrewery.ShouldNotBeNull();
            mappedBrewery.Id.ShouldBe(breweryToMap.Id);
            mappedBrewery.Name.ShouldBe(breweryToMap.Name);
            mappedBrewery.Address?.ShouldNotBeNull();
            mappedBrewery.Address?.City.ShouldBe(breweryToMap.Address?.City);
            mappedBrewery.Address?.State.ShouldBe(breweryToMap.Address?.State);
            mappedBrewery.Address?.StreetAddress.ShouldBe(breweryToMap.Address?.StreetAddress);
            mappedBrewery.Address?.ZipCode.ShouldBe(breweryToMap.Address?.ZipCode);
            mappedBrewery.Beers.ShouldNotBeNull();
            mappedBrewery.Beers.ShouldNotBeEmpty();
            mappedBrewery.Beers.First(b => b.Id == 1).Name.ShouldBe("Test Beer 1");
            mappedBrewery.Beers.First(b => b.Id == 1).Style.ShouldBe("Lager");
            mappedBrewery.Beers.First(b => b.Id == 2).Name.ShouldBe("Test Beer 2");
            mappedBrewery.Beers.First(b => b.Id == 2).Style.ShouldBe("Ipa");
            mappedBrewery.Beers.First(b => b.Id == 3).Name.ShouldBe("Test Beer 3");
            mappedBrewery.Beers.First(b => b.Id == 3).Style.ShouldBe("DoubleIpa");
            mappedBrewery.BeerCount.ShouldNotBeNull();
            mappedBrewery.BeerCount.ShouldBe(3);
        }
        
        [Fact]
        public void ToBreweryDto_GivenValidBreweryWithoutListOfBeers_ReturnsMappedBreweryDtoWithEmptyBeerListAndZeroCount()
        {
            // Arrange
            var breweryToMap = new Brewery
            {
                Id = 1,
                Name = "Test Brewery",
                Address = new Address
                {
                    City = "Redding",
                    State = "CA",
                    ZipCode = "96002",
                    StreetAddress = "123 Redding St."
                }
            };
            
            // Act
            var mappedBrewery = breweryToMap.ToBreweryDto();
            
            // Assert
            mappedBrewery.ShouldNotBeNull();
            mappedBrewery.Id.ShouldBe(breweryToMap.Id);
            mappedBrewery.Name.ShouldBe(breweryToMap.Name);
            mappedBrewery.Address?.ShouldNotBeNull();
            mappedBrewery.Address?.City.ShouldBe(breweryToMap.Address?.City);
            mappedBrewery.Address?.State.ShouldBe(breweryToMap.Address?.State);
            mappedBrewery.Address?.StreetAddress.ShouldBe(breweryToMap.Address?.StreetAddress);
            mappedBrewery.Address?.ZipCode.ShouldBe(breweryToMap.Address?.ZipCode);
            mappedBrewery.Beers.ShouldNotBeNull();
            mappedBrewery.Beers.ShouldBeEmpty();
            mappedBrewery.BeerCount.ShouldNotBeNull();
            mappedBrewery.BeerCount.ShouldBe(0);
        }
        
        [Fact]
        public void ToBreweryDto_GivenValidBreweryWithoutBeerListIncluded_ReturnsMappedBreweryDtoWithoutBeerListOrCount()
        {
            // Arrange
            var breweryToMap = new Brewery
            {
                Id = 1,
                Name = "Test Brewery",
                Address = new Address
                {
                    City = "Redding",
                    State = "CA",
                    ZipCode = "96002",
                    StreetAddress = "123 Redding St."
                }
            };
            
            // Add our beers since they are initialized within the domain
            breweryToMap.Beers.Add(new Beer { Id = 1, Name = "Test Beer 1", BeerStyle = BeerStyle.Lager });
            breweryToMap.Beers.Add(new Beer { Id = 2, Name = "Test Beer 2", BeerStyle = BeerStyle.Ipa });
            breweryToMap.Beers.Add(new Beer { Id = 3, Name = "Test Beer 3", BeerStyle = BeerStyle.DoubleIpa });

            // Act
            var mappedBrewery = breweryToMap.ToBreweryDto(false);
            
            // Assert
            mappedBrewery.ShouldNotBeNull();
            mappedBrewery.Id.ShouldBe(breweryToMap.Id);
            mappedBrewery.Name.ShouldBe(breweryToMap.Name);
            mappedBrewery.Address?.ShouldNotBeNull();
            mappedBrewery.Address?.City.ShouldBe(breweryToMap.Address?.City);
            mappedBrewery.Address?.State.ShouldBe(breweryToMap.Address?.State);
            mappedBrewery.Address?.StreetAddress.ShouldBe(breweryToMap.Address?.StreetAddress);
            mappedBrewery.Address?.ZipCode.ShouldBe(breweryToMap.Address?.ZipCode);
            mappedBrewery.Beers.ShouldBeNull();
            mappedBrewery.BeerCount.ShouldBeNull();
        }
        
        [Fact]
        public void ToBreweryDto_GivenValidBreweryWithNoBeerListAndWithoutBeerListIncluded_ReturnsMappedBreweryDtoWithoutBeerListOrCount()
        {
            // Arrange
            var breweryToMap = new Brewery
            {
                Id = 1,
                Name = "Test Brewery",
                Address = new Address
                {
                    City = "Redding",
                    State = "CA",
                    ZipCode = "96002",
                    StreetAddress = "123 Redding St."
                }
            };
            
            // Act
            var mappedBrewery = breweryToMap.ToBreweryDto(false);
            
            // Assert
            mappedBrewery.ShouldNotBeNull();
            mappedBrewery.Id.ShouldBe(breweryToMap.Id);
            mappedBrewery.Name.ShouldBe(breweryToMap.Name);
            mappedBrewery.Address?.ShouldNotBeNull();
            mappedBrewery.Address?.City.ShouldBe(breweryToMap.Address?.City);
            mappedBrewery.Address?.State.ShouldBe(breweryToMap.Address?.State);
            mappedBrewery.Address?.StreetAddress.ShouldBe(breweryToMap.Address?.StreetAddress);
            mappedBrewery.Address?.ZipCode.ShouldBe(breweryToMap.Address?.ZipCode);
            mappedBrewery.Beers.ShouldBeNull();
            mappedBrewery.BeerCount.ShouldBeNull();
        }
    }
}