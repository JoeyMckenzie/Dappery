namespace Dappery.Domain.Media
{
    using System.Collections.Generic;
    using System.Linq;

    public class ResourceList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Count => Items.Count();
    }
}