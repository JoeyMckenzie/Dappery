namespace Dappery.Core.Tests.Breweries
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Breweries.Commands.DeleteBrewery;
    using Exceptions;
    using MediatR;
    using Shouldly;
    using Xunit;

    public class DeleteBreweryCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidDeleteRequest_WhenBreweryExists_IsRemovedFromDatabaseIncludingAllBeers()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var deleteCommand = new DeleteBreweryCommand(1);
            var handler = new DeleteBreweryCommandHandler(unitOfWork);
            
            // Act
            var result = await handler.Handle(deleteCommand, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Unit>();
        }
        
        [Fact]
        public async Task GivenValidDeleteRequest_WhenDoesNotBreweryExist_IsNotRemovedFromDatabaseAndExceptionIsThrown()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var deleteCommand = new DeleteBreweryCommand(11);
            var handler = new DeleteBreweryCommandHandler(unitOfWork);
            
            // Act
            var result = await Should.ThrowAsync<DapperyApiException>(async () => await handler.Handle(deleteCommand, CancellationTestToken));

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}