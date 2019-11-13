namespace Dappery.Core.Extensions
{
    using System.Linq;
    using Domain.Dtos;
    using Domain.Dtos.Beer;
    using Domain.Dtos.Brewery;
    using Domain.Entities;

    public static class BreweryExtensions
    {
        public static BreweryDto ToBreweryDto(this Brewery brewery, bool includeBeerList = true)
        {
            return new BreweryDto
            {
                Id = brewery.Id,
                Name = brewery.Name,
                Beers = includeBeerList ? brewery.Beers.Select(b => new BeerDto
                {
                    Id = b.BreweryId,
                    Name = b.Name,
                    Style = b.BeerStyle.ToString()
                }) : default,
                Address = new AddressDto
                {
                    City = brewery.Address?.City,
                    State = brewery.Address?.State,
                    StreetAddress = brewery.Address?.StreetAddress,
                    ZipCode = brewery.Address?.ZipCode
                },
                BeerCount = includeBeerList ? brewery.BeerCount : (int?) null
            };
        }
    }
}