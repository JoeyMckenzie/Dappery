namespace Dappery.Core.Tests.Beers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Beers.Commands.CreateBeer;
    using Domain.Dtos.Beer;
    using Domain.Entities;
    using Domain.Media;
    using Exceptions;
    using Shouldly;
    using Xunit;

    public class CreateBeerCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenBreweryExists_ReturnsMappedAndCreatedBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var beerCommand = new CreateBeerCommand(new CreateBeerDto
            {
                Name = "Test Beer",
                Style = "Lager",
                BreweryId = 1
            });
            var handler = new CreateBeerCommandHandler(unitOfWork);
            
            // Act
            var result = await handler.Handle(beerCommand, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<BeerResource>();
            result.Self.ShouldNotBeNull();
            result.Self.Brewery.ShouldNotBeNull();
            result.Self.Brewery?.Address.ShouldNotBeNull();
            result.Self.Brewery?.Address?.StreetAddress.ShouldBe("1030 E Cypress Ave Ste D");
            result.Self.Brewery?.Address?.City.ShouldBe("Redding");
            result.Self.Brewery?.Address?.State.ShouldBe("CA");
            result.Self.Brewery?.Address?.ZipCode.ShouldBe("96002");
            result.Self.Brewery?.Beers.ShouldBeNull();
            result.Self.Brewery?.Id.ShouldBe(1);
            result.Self.Brewery?.Name.ShouldBe("Fall River Brewery");
            result.Self.Id.ShouldNotBeNull();
            result.Self.Name.ShouldBe(beerCommand.Dto.Name);
            result.Self.Style.ShouldBe(beerCommand.Dto.Style);
        }
        
        [Fact]
        public async Task GivenValidRequest_WhenBreweryDoesNotExist_ThrowsApiExceptionForBadRequest()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var beerCommand = new CreateBeerCommand(new CreateBeerDto
            {
                Name = "Test Beer",
                Style = "Lager",
                BreweryId = 11
            });
            var handler = new CreateBeerCommandHandler(unitOfWork);
            
            // Act
            var result = await Should.ThrowAsync<DapperyApiException>(async () => await handler.Handle(beerCommand, CancellationTestToken));

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task GivenValidRequest_WithInvalidBeerStyle_ReturnsMappedAndCreatedBeerWithOtherAsStyle()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var beerCommand = new CreateBeerCommand(new CreateBeerDto
            {
                Name = "Test Beer",
                Style = "Not defined!",
                BreweryId = 1
            });
            var handler = new CreateBeerCommandHandler(unitOfWork);
            
            // Act
            var result = await handler.Handle(beerCommand, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<BeerResource>();
            result.Self.ShouldNotBeNull();
            result.Self.Brewery.ShouldNotBeNull();
            result.Self.Brewery?.Address.ShouldNotBeNull();
            result.Self.Brewery?.Address?.StreetAddress.ShouldBe("1030 E Cypress Ave Ste D");
            result.Self.Brewery?.Address?.City.ShouldBe("Redding");
            result.Self.Brewery?.Address?.State.ShouldBe("CA");
            result.Self.Brewery?.Address?.ZipCode.ShouldBe("96002");
            result.Self.Brewery?.Beers.ShouldBeNull();
            result.Self.Brewery?.Id.ShouldBe(1);
            result.Self.Brewery?.Name.ShouldBe("Fall River Brewery");
            result.Self.Id.ShouldNotBeNull();
            result.Self.Name.ShouldBe(beerCommand.Dto.Name);
            result.Self.Style.ShouldBe(BeerStyle.Other.ToString());
        }
    }
}