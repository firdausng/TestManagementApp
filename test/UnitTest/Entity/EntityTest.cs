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

    }
}
