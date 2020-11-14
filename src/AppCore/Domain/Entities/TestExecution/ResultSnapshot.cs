using System;
using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestExecution
{
    public class ResultSnapshot : TestExecutionEntity
    {
        public ResultSnapshot()
        {

        }

        public DateTime ExecutionDate { get; set; }
        public TimeSpan TestDuration { get; set; }
        public List<ScenarioResult> ScenarioResultList { get; private set; } = new List<ScenarioResult>();
    }
}

