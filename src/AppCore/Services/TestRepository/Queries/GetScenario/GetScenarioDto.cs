using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Services.TestRepository.Queries.GetScenario
{
    public class GetScenarioDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid FeatureId { get; set; }
        public string Description { get; set; }
        public List<Step> StepList { get; set;  } = new List<Step>();

        public class Step
        {
            public Step(int order, string description)
            {
                Order = order;
                Description = description;
            }
            public int Order { get; set; }
            public string Description { get; set; }
        }
    }
}
