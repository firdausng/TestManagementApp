using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;

    public class CreateFeatureTest : BaseFeatureTest
    {
        [Fact]
        public async Task ShouldCreateFeature()
        {
            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);

            var featureEntity = await ExecuteDbContextAsync(db => db.Features
                .SingleOrDefaultAsync(p => p.Id.Equals(createFeatureDto.Id))
            );

            featureEntity.ShouldNotBeNull();
            featureEntity.Name.ShouldBe("feature1");
            featureEntity.Description.ShouldBeNull();

            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);
        }
    }
}
