using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Services.TestRepository.Queries.GetTag
{
    public class GetTagDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<Guid> ScenarioList { get; set; } = new List<Guid>();
        public IReadOnlyCollection<Guid> FeatureList { get; set; } = new List<Guid>();
    }
}
