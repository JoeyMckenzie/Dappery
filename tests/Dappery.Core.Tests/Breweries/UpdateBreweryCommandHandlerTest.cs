namespace Dappery.Core.Tests.Breweries
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Breweries.Commands.UpdateBrewery;
    using Domain.Dtos;
    using Domain.Dtos.Brewery;
    using Exceptions;
    using Shouldly;
    using Xunit;

    public class UpdateBreweryCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidUpdateRequest_WhenBreweryExists_ReturnsUpdatedMappedBrewery()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            const int breweryId = 1;
            var updateCommand = new UpdateBreweryCommand(new UpdateBreweryDto
            {
                Id = breweryId,
                Address = new AddressDto
                {
                    City = "Updated City",
                    State = "Updated State",
                    StreetAddress = "Updated Street Address",
                    ZipCode = "12345"
                },
                Name = "Updated Brewery Name"
            }, breweryId);
            
            // Act
            var commandHandler = new UpdateBreweryCommandHandler(unitOfWork);
            var result = await commandHandler.Handle(updateCommand, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.Self.ShouldNotBeNull();
            result.ApiVersion.ShouldNotBeNull();
            result.Self.Id.ShouldBe(breweryId);
            result.Self.Address?.ShouldNotBeNull();
            result.Self.Address?.City.ShouldBe(updateCommand.Dto.Address?.City);
            result.Self.Address?.State.ShouldBe(updateCommand.Dto.Address?.State);
            result.Self.Address?.StreetAddress.ShouldBe(updateCommand.Dto.Address?.StreetAddress);
            result.Self.Address?.ZipCode.ShouldBe(updateCommand.Dto.Address?.ZipCode);
            result.Self.Name.ShouldBe(updateCommand.Dto.Name);
        }
        
        [Fact]
        public async Task GivenValidUpdateRequest_WhenBreweryDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            const int breweryId = 11;
            var updateCommand = new UpdateBreweryCommand(new UpdateBreweryDto
            {
                Id = breweryId,
                Address = new AddressDto
                {
                    City = "Doesn't Exist!",
                    State = "Doesn't Exist!",
                    StreetAddress = "Doesn't Exist!",
                    ZipCode = "Doesn't Exist!"
                },
                Name = "Doesn't Exist!"
            }, breweryId);
            
            // Act
            var commandHandler = new UpdateBreweryCommandHandler(unitOfWork);
            var result = await Should.ThrowAsync<DapperyApiException>(async () => await commandHandler.Handle(updateCommand, CancellationTestToken));

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task GivenValidUpdateRequest_WhenBreweryDoesExistAndAddressIsNotUpdated_ReturnsMappedBreweryWithNoUpdatedAddress()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            const int breweryId = 1;
            var updateCommand = new UpdateBreweryCommand(new UpdateBreweryDto
            {
                Id = breweryId,
                Name = "Cedar Crest Brewery"
            }, breweryId);
            
            // Act
            var commandHandler = new UpdateBreweryCommandHandler(unitOfWork);
            var result = await commandHandler.Handle(updateCommand, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeNull();
            result.Self.ShouldNotBeNull();
            result.ApiVersion.ShouldNotBeNull();
            result.Self.Id.ShouldBe(breweryId);
            result.Self.Address?.ShouldNotBeNull();
            result.Self.Address?.StreetAddress.ShouldBe("1030 E Cypress Ave Ste D");
            result.Self.Address?.City.ShouldBe("Redding");
            result.Self.Address?.State.ShouldBe("CA");
            result.Self.Address?.ZipCode.ShouldBe("96002");
            result.Self.Name.ShouldBe(updateCommand.Dto.Name);
        }
    }
}