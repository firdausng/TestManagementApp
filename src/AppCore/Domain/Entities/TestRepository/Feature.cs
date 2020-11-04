using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Feature : TaggableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public List<Scenario> Scenarios { get; set; } = new List<Scenario>();
    }
}
