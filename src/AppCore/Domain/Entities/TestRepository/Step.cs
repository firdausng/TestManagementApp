namespace AppCore.Domain.Entities.TestRepository
{
    public class Step
    {
        public Step(int Order, string Description)
        {
            this.Order = Order;
            this.Description = Description;
        }

        public int Order { get; private set; }
        public string Description { get; private set; }
        public string Status { get; set; }
        public Scenario Scenario { get; set; }
    }
}
