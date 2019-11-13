namespace Dappery.Domain.Media
{
    using System.Collections.Generic;
    using Dtos.Brewery;

    public class BreweryResourceList : ResourceList<BreweryDto>
    {
        public BreweryResourceList(IEnumerable<BreweryDto> items) 
            : base(items)
        {
        }
    }
}