namespace Dappery.Domain.Dtos
{
    public class ErrorDto
    {
        public ErrorDto(string? description, object? details = null)
        {
            Description = string.IsNullOrWhiteSpace(description) ? "An expected error has occured." : description;
            Details = details;
        }

        public string Description { get; }

        public object? Details { get; set; }
    }
}