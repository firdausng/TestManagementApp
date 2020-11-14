using System;

namespace AppCore.Domain.Entities.TestExecution
{
    public class ScenarioResult : TestExecutionEntity
    {
        public Guid ScenarioId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public TimeSpan TestDuration { get; set; }
        public TestStatus TestStatus { get; set; }
        public string StatusReason { get; set; }
    }
}

