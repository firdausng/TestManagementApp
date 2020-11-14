using AppCore.Domain.Entities.TestRepository;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Entity
{
    public class ScenarioEntityTest
    {
        [Fact]
        public void Should_Create_Scenario_Entity()
        {
            var project = Project.Factory("project1", true);
            var scenario = Scenario.Factory("test", project, Feature.Factory("feature", project));
            scenario.ShouldBeOfType<Scenario>();
            scenario.Feature.ShouldNotBeNull();
            scenario.Feature.Name.ShouldBe("feature"); 
        }

        [Fact]
        public void Should_Create_Scenario_Entity_When_feature_null()
        {
            var project = Project.Factory("project1", true);
            var scenario = Scenario.Factory("test", project, null);
            scenario.ShouldBeOfType<Scenario>();
            scenario.Feature.ShouldBeNull();
        }

        [Fact]
        public void Should_update_Scenario_Entity_With_Steps()
        {
            var project = Project.Factory("project1", true);
            var scenario = Scenario.Factory("test", project, null);
            scenario.AddSteps(new List<Step> 
            { 
                new Step(1, "1", scenario),
                new Step(1, "2", scenario),
                new Step(3, "3", scenario),
            });

            scenario.ShouldBeOfType<Scenario>();
            scenario.Feature.ShouldBeNull();
        }
    }
}
