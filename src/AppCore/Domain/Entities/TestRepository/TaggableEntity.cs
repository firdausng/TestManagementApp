using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestRepository
{
    public abstract class TaggableEntity: AuditableEntity
    {
        public List<Tag> TagsList { get; set; } = new List<Tag>();
    }
}
