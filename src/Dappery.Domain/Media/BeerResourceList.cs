namespace Dappery.Domain.Media
{
    using System.Collections.Generic;
    using Dtos.Beer;

    public class BeerResourceList : ResourceList<BeerDto>
    {
        public BeerResourceList(IEnumerable<BeerDto> items) 
            : base(items)
        {
        }
    }
}