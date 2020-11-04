namespace AppCore.Domain.Entities.TestRepository
{
    public class Tag : AuditableEntity
    {
        public Tag(string Name)
        {
            this.Name = Name;
        }
        public string Name { get; private set; }
        public Project Project { get; set; }
    }
}
