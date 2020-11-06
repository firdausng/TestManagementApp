using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries.GetFeature;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Collections.Generic;
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

    public class GetFeatureTest : BaseFeatureTest
    {
        [Fact]
        public async Task ShouldGetFeature()
        {
            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);

            var getFeatureCommand = new GetFeatureQuery(createFeatureDto.Id, createProjectDto.Id);
            var getFeatureDto = await SendAsync(getFeatureCommand);

            getFeatureDto.ShouldNotBeNull();
            getFeatureDto.Description.ShouldBeNull();
            getFeatureDto.Name.ShouldBe("feature1");
            getFeatureDto.ProjectId.ShouldBe(createProjectDto.Id);

            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);

        }

        [Fact]
        public async Task ShouldGetFeatureList()
        {
            var createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            var createFeatureDto = await SendAsync(createFeatureCommand);

            var getFeatureListCommand = new GetFeatureListQuery(createProjectDto.Id);
            var getFeatureListDto = await SendAsync(getFeatureListCommand);

            getFeatureListDto.ShouldNotBeNull();
            getFeatureListDto.Data.ShouldNotBeNull();
            getFeatureListDto.Count.ShouldNotBe(0);
            getFeatureListDto.Data.ShouldBeOfType<List<GetFeatureDto>>();

            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);

        }
    }
}
