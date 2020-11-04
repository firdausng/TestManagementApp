using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Scenario : TaggableEntity
    {
        public Scenario(string Description)
        {
            this.Description = Description;
        }
        public string Description { get; private set; }
        public string Status { get; set; }
        public List<Step> StepsList { get; set; } = new List<Step>();
    }
}
