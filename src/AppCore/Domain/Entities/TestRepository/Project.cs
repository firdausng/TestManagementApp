using System.Collections.Generic;
using System.Linq.Expressions;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Project : AuditableEntity
    {
        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public List<Feature> Features { get; } = new List<Feature>();
        public List<Tag> Tags { get; } = new List<Tag>();
        //public List<ProjectUser> Members { get; set; } = new List<ProjectUser>();

        private Project()
        {}

        private Project(string Name, bool IsEnabled)
        {
            this.Name = Name;
            this.IsEnabled = IsEnabled;
        }
        public static Project Factory(string name, bool IsEnabled)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var project = new Project(name, IsEnabled);

            return project;
        }

        public void AddFeature(Feature feature)
        {
            Features.Add(feature);
        }

        public void Update(string Name, bool IsEnabled)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new System.ArgumentException($"'{nameof(Name)}' cannot be null or empty", nameof(Name));
            }

            this.Name = Name;
            this.IsEnabled = IsEnabled;
        }
    }
}
