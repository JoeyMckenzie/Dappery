namespace Dappery.Core.Extensions
{
    using Domain.Dtos.Beer;
    using Domain.Entities;

    public static class BeerExtensions
    {
        public static BeerDto ToBeerDto(this Beer beer)
        {
             return new BeerDto
            {
                Id = beer.Id,
                Name = beer.Name,
                Style = beer.BeerStyle.ToString(),
                Brewery = beer.Brewery?.ToBreweryDto(false),
            };
        }
    }
}