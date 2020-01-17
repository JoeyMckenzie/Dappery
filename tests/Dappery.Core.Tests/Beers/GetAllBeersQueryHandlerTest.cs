namespace Dappery.Core.Tests.Beers
{
    using System.Threading.Tasks;
    using Core.Beers.Queries.GetBeers;
    using Shouldly;
    using Xunit;

    public class GetAllBeersQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenBeersArePopulated_ReturnsMappedBeerList()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new GetBeersQuery();
            var handler = new GetBeersQueryHandler(unitOfWork);
            
            // Act
            var result = await handler.Handle(query, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Count.ShouldBe(5);
        }
        
        [Fact]
        public async Task GivenValidRequest_WhenBeersAreNotPopulated_ReturnsMappedEmptyBeerList()
        {
            // Arrange
            using var unitOfWork = UnitOfWork;
            var query = new GetBeersQuery();
            var handler = new GetBeersQueryHandler(unitOfWork);
            await unitOfWork.BeerRepository.DeleteBeerAsync(1, CancellationTestToken);
            await unitOfWork.BeerRepository.DeleteBeerAsync(2, CancellationTestToken);
            await unitOfWork.BeerRepository.DeleteBeerAsync(3, CancellationTestToken);
            await unitOfWork.BeerRepository.DeleteBeerAsync(4, CancellationTestToken);
            await unitOfWork.BeerRepository.DeleteBeerAsync(5, CancellationTestToken);
            
            // Act
            var result = await handler.Handle(query, CancellationTestToken);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }
    }
}