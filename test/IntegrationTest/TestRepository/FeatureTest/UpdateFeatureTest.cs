using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;

    public class UpdateFeatureTest : BaseFeatureTest
    {
        [Fact]
        public async Task ShouldUpdateFeature()
        {
            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);


            var updateFeatureCommand = new UpdateFeatureInfoCommand(createFeatureDto.Id, createProjectDto.Id, "update feature1", "description");
            await SendAsync(updateFeatureCommand);


            var featureEntity = await ExecuteDbContextAsync(db => db.Features
                .SingleOrDefaultAsync(p => p.Id.Equals(createFeatureDto.Id))
            );

            featureEntity.ShouldNotBeNull();
            featureEntity.Name.ShouldBe("update feature1");
            featureEntity.Description.ShouldBe("description");

            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);
        }
    }
}
