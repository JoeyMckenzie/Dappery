namespace Dappery.Domain.Media
{
    public class Resource<T>
    {
        public Resource(T resource) => Self = resource;

        public T Self { get; }

        public string ApiVersion => "v1";
    }
}