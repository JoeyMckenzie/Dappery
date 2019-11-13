namespace Dappery.Core.Tests.Breweries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Breweries.Commands.CreateBrewery;
    using Domain.Dtos;
    using Domain.Dtos.Brewery;
    using Shouldly;
    using Xunit;

    public class CreateBreweryCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task CreateBreweryCommandHandler_GivenAValidRequest_CreatesBrewery()
        {
            // Arrange
            var createBreweryDto = new CreateBreweryDto
            {
                Name = "Pizza Port Brewing Company",
                Address = new AddressDto
                {
                    City = "San Diego",
                    State = "CA",
                    StreetAddress = "123 San Diego St.",
                    ZipCode = "92109"
                }
            };

            // Act
            var handler = new CreateBreweryCommandHandler(UnitOfWork);
            var result = await handler.Handle(new CreateBreweryCommand(createBreweryDto), CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Brewery.ShouldNotBeNull();
            result.Brewery.ShouldBeOfType<BreweryDto>();
            result.Brewery.Name.ShouldNotBeNull();
            result.Brewery.Name.ShouldBe(createBreweryDto.Name);
            result.Brewery.Address.ShouldNotBeNull();
            result.Brewery.Address.City.ShouldBe(createBreweryDto.Address.City);
            result.Brewery.Address.State.ShouldBe(createBreweryDto.Address.State);
            result.Brewery.Address.StreetAddress.ShouldBe(createBreweryDto.Address.StreetAddress);
            result.Brewery.Address.ZipCode.ShouldBe(createBreweryDto.Address.ZipCode);
            result.Brewery.Beers.ShouldBeEmpty();
            result.Brewery.BeerCount.ShouldBe(0);
        }
    }
}