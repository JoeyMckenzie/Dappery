namespace Dappery.Domain.Media
{
    using System.Collections.Generic;
    using System.Linq;

    public class ResourceList<T>
    {
        public ResourceList(IEnumerable<T>? items) => Items = items;
        
        public IEnumerable<T> Items { get; }

        public int Count => Items?.Count() ?? 0;
    }
}