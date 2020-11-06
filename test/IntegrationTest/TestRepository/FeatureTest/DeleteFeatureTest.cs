using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;
    public class DeleteFeatureTest : BaseFeatureTest
    {
        [Fact]
        public async Task ShouldDeleteFeature()
        {
            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);

            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);

            var featureEntity = await ExecuteDbContextAsync(db => db.Features
                .SingleOrDefaultAsync(p => p.Id.Equals(createFeatureDto.Id))
            );

            featureEntity.ShouldBeNull();
        }
    }
}
