using System;

namespace WebApiUtilities.Abstract
{
    public abstract class AuditableEntity<TId> : Entity<TId>
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
