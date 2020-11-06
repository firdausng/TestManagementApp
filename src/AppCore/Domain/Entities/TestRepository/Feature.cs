using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Feature : TaggableEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Project Project { get; private set; }
        public List<Scenario> Scenarios { get; private set; } = new List<Scenario>();

        public static Feature Factory(string name, Project project, string description = null)
        {
            if (project == null)
            {
                return null;
            }
            var feature = new Feature(name, project);
            if (description != null)
            {
                feature.Description = description;
            }
            return feature;
        }
        private Feature()
        {}

        private Feature(string Name, Project Project, string Description = null)
        {
            this.Name = Name;
            this.Project = Project;

            if (!string.IsNullOrEmpty(Description))
            {
                this.Description = Description;
            }
        }

        public void AddDescription(string Description)
        {
            this.Description = Description;
        }

        public void UpdateInfo(string Name, string Description = null)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new System.ArgumentException($"'{nameof(Name)}' cannot be null or empty", nameof(Name));
            }
            this.Name = Name;

            if (!string.IsNullOrEmpty(Description))
            {
                this.Description = Description;
            }
        }
    }
}
