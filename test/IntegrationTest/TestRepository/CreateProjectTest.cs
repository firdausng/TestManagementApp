using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;
    public class CreateProjectTest : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateProject()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var projectEntity = await ExecuteDbContextAsync(db =>db.Projects
                .SingleOrDefaultAsync(p => p.Id.Equals(createProjectDto.Id))
            );

            projectEntity.ShouldNotBeNull();
            projectEntity.Name.ShouldBe("project1");
            projectEntity.IsEnabled.ShouldBe(true);
        }
    }

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
