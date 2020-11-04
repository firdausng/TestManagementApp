using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;

    public class GetProject: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetProjectList()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var getProjectListQuery = new GetProjectListQuery();
            var getProjectListDto = await SendAsync(getProjectListQuery);

            getProjectListDto.ShouldNotBeNull();
            getProjectListDto.Count.ShouldBe(1);
            getProjectListDto.Data.ShouldNotBeNull();
            getProjectListDto.Data.ShouldBeOfType<List<GetProjectDto>>();
        }

        [Fact]
        public async Task ShouldGetProjectItem()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var getProjectListQuery = new GetProjectQuery(createProjectDto.Id);
            var getProjectListDto = await SendAsync(getProjectListQuery);

            getProjectListDto.ShouldNotBeNull();
            getProjectListDto.Id.ShouldBe(createProjectDto.Id);
            getProjectListDto.IsEnabled.ShouldBe(true);
            getProjectListDto.Name.ShouldBe("project1");
        }
    }
}
