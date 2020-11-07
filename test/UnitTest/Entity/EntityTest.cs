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
    public class EntityTest
    {
        [Fact]
        public void Should_Create_Project()
        {
            var project = Project.Factory("test", true);
            project.ShouldBeOfType<Project>();
            project.Name.ShouldBe("test");
            project.IsEnabled.ShouldBe(true);
        }

        [Fact]
        public void Should_Not_Create_Project_When_Name_null()
        {
            var project = Project.Factory(null, true);
            project.ShouldBeNull();
        }

        [Fact]
        public void Should_Not_Create_Project_When_Name_empty()
        {
            var project = Project.Factory("", true);
            project.ShouldBeNull();
        }

        [Fact]
        public void Should_Not_Create_Project_When_Name_whitespace()
        {
            var project = Project.Factory(" ", true);
            project.ShouldBeNull();
        }

        [Fact]
        public void Should_Create_Feature_Entity()
        {
            var feature = Feature.Factory("test", Project.Factory("project1", true));
            feature.ShouldBeOfType<Feature>();
        }

        [Fact]
        public void Should_Not_Create_Feature_Entity_When_Project_Null()
        {
            var feature = Feature.Factory("test", null);
            feature.ShouldBeNull();
        }

        [Fact]
        public void Should_Create_Feature_Entity_With_Description()
        {
            var feature = Feature.Factory("test", Project.Factory("project1", true), "description");
            feature.ShouldBeOfType<Feature>();
            feature.Description.ShouldBe("description");
        }

        [Fact]
        public void Should_Update_Feature_Entity_With_Description()
        {
            var feature = Feature.Factory("test", Project.Factory("project1", true), "description");

            feature.UpdateInfo("update", "update description");

            feature.ShouldBeOfType<Feature>();
            feature.Description.ShouldBe("update description");
            feature.Name.ShouldBe("update");
        }

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
