namespace Dappery.Domain.Entities
{
    using System;

    public class TimeStampedEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}