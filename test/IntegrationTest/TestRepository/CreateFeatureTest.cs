using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;

    public class CreateFeatureTest : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateFeature()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);

            var featureEntity = await ExecuteDbContextAsync(db => db.Features
                .SingleOrDefaultAsync(p => p.Id.Equals(createFeatureDto.Id))
            );

            featureEntity.ShouldNotBeNull();
            featureEntity.Name.ShouldBe("feature1");
            featureEntity.Description.ShouldBeNull();
        }
    }
}
