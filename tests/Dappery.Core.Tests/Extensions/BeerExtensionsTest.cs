namespace Dappery.Core.Tests.Extensions
{
    using Core.Extensions;
    using Domain.Entities;
    using Shouldly;
    using Xunit;

    public class BeerExtensionsTest
    {
        [Fact]
        public void ToBeerDto_GivenValidBeer_ReturnsMappedBeerDtoWithoutBreweryBeerListAndCount()
        {
            // Arrange
            var beerToMap = new Beer
            {
                Id = 1,
                Name = "Test Beer",
                BeerStyle = BeerStyle.Ipa,
                Brewery = new Brewery
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
                }
            };

            // Act
            var mappedBeer = beerToMap.ToBeerDto();

            // Assert
            mappedBeer.ShouldNotBeNull();
            mappedBeer.Id.ShouldBe(beerToMap.Id);
            mappedBeer.Name.ShouldBe(beerToMap.Name);
            mappedBeer.Style.ShouldBe(beerToMap.BeerStyle.ToString());
            mappedBeer.Brewery?.ShouldNotBeNull();
            mappedBeer.Brewery?.Id.ShouldBe(beerToMap.Brewery.Id);
            mappedBeer.Brewery?.Name.ShouldBe(beerToMap.Brewery.Name);
            mappedBeer.Brewery?.Address?.ShouldNotBeNull();
            mappedBeer.Brewery?.Address?.City.ShouldBe(beerToMap.Brewery.Address?.City);
            mappedBeer.Brewery?.Address?.State.ShouldBe(beerToMap.Brewery.Address?.State);
            mappedBeer.Brewery?.Address?.ZipCode.ShouldBe(beerToMap.Brewery.Address?.ZipCode);
            mappedBeer.Brewery?.Address?.StreetAddress.ShouldBe(beerToMap.Brewery.Address?.StreetAddress);
            
            // Validate the beers were not recursively mapped
            mappedBeer.Brewery?.Beers.ShouldBeNull();
            mappedBeer.Brewery?.BeerCount.ShouldBeNull();
        }
        [Fact]
        public void ToBeerDto_GivenValidBeerWithoutBrewery_ReturnsMappedBeerDtoWithoutBrewery()
        {
            // Arrange
            var beerToMap = new Beer
            {
                Id = 1,
                Name = "Test Beer",
                BeerStyle = BeerStyle.Ipa,
            };

            // Act
            var mappedBeer = beerToMap.ToBeerDto();

            // Assert
            mappedBeer.ShouldNotBeNull();
            mappedBeer.Id.ShouldBe(beerToMap.Id);
            mappedBeer.Name.ShouldBe(beerToMap.Name);
            mappedBeer.Style.ShouldBe(beerToMap.BeerStyle.ToString());
            mappedBeer.Brewery?.ShouldBeNull();
        }
    }
}