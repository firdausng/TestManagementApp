using System;

namespace AppCore.Domain.Entities.TestExecution
{
    public abstract class TestExecutionEntity : AuditableEntity
    {
        public Guid ProjectId { get; set; }
    }
}

