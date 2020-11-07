using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Step: IEquatable<Step>
    {
        public Step(int Order, string Description, Scenario scenario)
        {
            this.Order = Order;
            this.Description = Description;
            Scenario = scenario;
        }

        // EF Core need this
        private Step()
        {}

        public int Order { get; private set; }
        public string Description { get; private set; }
        public Scenario Scenario { get; set; }

        public bool Equals(Step other)
        {
            if (other.Order == Order)
            {
                return true;
            }
            return false;
        }
    }
}
