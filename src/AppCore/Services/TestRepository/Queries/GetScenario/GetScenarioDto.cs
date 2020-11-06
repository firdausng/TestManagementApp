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
    }
}
