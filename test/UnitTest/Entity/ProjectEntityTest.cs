using AppCore.Domain.Entities.TestRepository;
using Shouldly;
using Xunit;

namespace UnitTest.Entity
{
    public class ProjectEntityTest
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
    }
}
