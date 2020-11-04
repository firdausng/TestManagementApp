using System.Collections.Generic;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Project : AuditableEntity
    {
        private Project()
        {

        }
        public Project(string Name, bool IsEnabled)
        {
            this.Name = Name;
            this.IsEnabled = IsEnabled;
        }
        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public List<Feature> Features { get; } = new List<Feature>();
        public List<Tag> Tags { get; } = new List<Tag>();
        //public List<ProjectUser> Members { get; set; } = new List<ProjectUser>();
    }
}
