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
        public void Should_CreateFeatureEntityUsingFactory()
        {
            var feature = Feature.Factory("test", new Project("project1", true));
            feature.ShouldBeOfType<Feature>();
        }

        [Fact]
        public void Should_Not_CreateFeatureEntityUsingFactory_When_ProjectIsNull()
        {
            var feature = Feature.Factory("test", null);
            feature.ShouldBeNull();
        }

        [Fact]
        public void Should_CreateFeatureEntityWithDescriptionUsingFactory()
        {
            var feature = Feature.Factory("test", new Project("project1", true), "description");
            feature.ShouldBeOfType<Feature>();
            feature.Description.ShouldBe("description");
        }
    }
}
