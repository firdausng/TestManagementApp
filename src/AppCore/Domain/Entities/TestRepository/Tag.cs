using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Tag : AuditableEntity
    {
        private Tag()
        {}
        private Tag(string name, Project project)
        {
            Name = name;
            Project = project;
        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Project Project { get; set; }
        public IReadOnlyCollection<Scenario> ScenarioList { get; private set; } = new List<Scenario>();
        public IReadOnlyCollection<Feature> FeatureList { get; private set; } = new List<Feature>();

        public static Tag Factory(string name, Project project, string description = null)
        {
            if (project == null)
            {
                return null;
            }
            var tag = new Tag(name, project);
            if (description != null)
            {
                tag.Description = description;
            }
            return tag;
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
