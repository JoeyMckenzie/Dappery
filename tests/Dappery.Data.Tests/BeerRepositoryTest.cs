namespace Dappery.Data.Tests
{
    using System;
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
            unitOfWork.Commit();
                
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
        public async Task GetBeerById_WhenInvokedAndBeerExists_ReturnsValidBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            
            // Act
            var beer = await unitOfWork.BeerRepository.GetBeerById(1);
            unitOfWork.Commit();
                
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
            unitOfWork.Commit();
                
            // Assert, validate a few properties
            beer.ShouldBeNull();
        }
        
        [Fact]
        public async Task CreateBeer_WhenBeerIsValid_ReturnsNewlyInsertedBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var beerToInsert = new Beer
            {
                Name = "Lazy Hazy",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = 1,
                BeerStyle = BeerStyle.NewEnglandIpa
            };
            
            // Act
            var beerId = await unitOfWork.BeerRepository.CreateBeer(beerToInsert);
            var insertedBeer = await unitOfWork.BeerRepository.GetBeerById(beerId);
            unitOfWork.Commit();
            
            insertedBeer.ShouldNotBeNull();
            insertedBeer.ShouldBeOfType<Beer>();
            insertedBeer.Brewery.ShouldNotBeNull();
            insertedBeer.Brewery.Address.ShouldNotBeNull();
            insertedBeer.Brewery.Beers.ShouldNotBeEmpty();
            insertedBeer.Brewery.Beers.Count.ShouldBe(4);
            insertedBeer.Brewery.Beers.ShouldContain(b => b.Id == insertedBeer.Id);
            insertedBeer.Brewery.Beers.FirstOrDefault(b => b.Id == insertedBeer.Id)?.Name.ShouldBe(beerToInsert.Name);
        }
        
        [Fact]
        public async Task UpdateBeer_WhenBeerIsValid_ReturnsUpdateBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var beerToUpdate = new Beer
            {
                Id = 1,
                Name = "Colossus Imperial Stout",
                UpdatedAt = DateTime.UtcNow,
                BeerStyle = BeerStyle.Stout,
                BreweryId = 1,
            };
            
            // Act
            await unitOfWork.BeerRepository.UpdateBeer(beerToUpdate);
            var updatedBeer = await unitOfWork.BeerRepository.GetBeerById(beerToUpdate.Id);
            unitOfWork.Commit();
            
            updatedBeer.ShouldNotBeNull();
            updatedBeer.ShouldBeOfType<Beer>();
            updatedBeer.Brewery.ShouldNotBeNull();
            updatedBeer.Brewery.Address.ShouldNotBeNull();
            updatedBeer.Brewery.Beers.ShouldNotBeEmpty();
            updatedBeer.Brewery.Beers.Count.ShouldBe(3);
            updatedBeer.Brewery.Beers.ShouldContain(b => b.Id == beerToUpdate.Id);
            updatedBeer.Brewery.Beers.ShouldNotContain(b => b.Name == "Hexagenia");
            updatedBeer.Brewery.Beers.FirstOrDefault(b => b.Id == beerToUpdate.Id)?.Name.ShouldBe(beerToUpdate.Name);
        }

        [Fact]
        public async Task DeleteBeer_WhenBeerExists_RemovesBeerFromDatabase()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            (await unitOfWork.BeerRepository.GetAllBeers())?.Count().ShouldBe(5);
            
            // Act
            var removeBeerCommand = await unitOfWork.BeerRepository.DeleteBeer(1);
            var breweryOfRemovedBeer = await unitOfWork.BreweryRepository.GetBreweryById(1);
            (await unitOfWork.BeerRepository.GetAllBeers())?.Count().ShouldBe(4);
            unitOfWork.Commit();
            
            // Assert
            removeBeerCommand.ShouldNotBeNull();
            removeBeerCommand.ShouldBe(1);
            breweryOfRemovedBeer.ShouldNotBeNull();
            breweryOfRemovedBeer.Beers.ShouldNotBeNull();
            breweryOfRemovedBeer.Beers.ShouldNotBeEmpty();
            breweryOfRemovedBeer.Beers.ShouldNotContain(b => b.Name == "Hexagenia");
        }
    }
}