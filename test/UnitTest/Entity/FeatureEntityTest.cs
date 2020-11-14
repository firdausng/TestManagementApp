using AppCore.Domain.Entities.TestRepository;
using Shouldly;
using Xunit;

namespace UnitTest.Entity
{
    public class FeatureEntityTest
    {
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
    }
}
