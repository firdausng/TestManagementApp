using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Domain.Entities.TestExecution
{
    public class TestSuite: TestExecutionEntity
    {
        public string Name { get; private set; }
        public List<ResultSnapshot> ResultList { get; set; } = new List<ResultSnapshot>();
        public string TagExpression { get; private set; }

        public TestSuite(string name, Guid projectId, string tagExpression = null)
        {
            Name = name;
            ProjectId = projectId;
            TagExpression = tagExpression;
        }

        public static TestSuite Factory(string name, Guid projectId, string tagExpression = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var testSuite = new TestSuite(name, projectId, tagExpression);

            return testSuite;
        }
    }
}

