namespace Dappery.Core.Tests.Beers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Beers.Queries.RetrieveBeer;
    using Exceptions;
    using Shouldly;
    using Xunit;

    public class RetrieveBeerQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenBeerExists_ReturnsMappedBeer()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new RetrieveBeerQuery(1);
            var handler = new RetrieveBeerQueryHandler(unitOfWork);
            
            // Act
            var result = await handler.Handle(query, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
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
        }
        
        [Fact]
        public async Task GivenValidRequest_WhenBeerDoesNotExist_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new RetrieveBeerQuery(11);
            var handler = new RetrieveBeerQueryHandler(unitOfWork);
            
            // Act
            var result = await Should.ThrowAsync<DapperyApiException>(async () => await handler.Handle(query, CancellationTestToken));

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}