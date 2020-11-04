using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;

    public class DeleteProjectTest : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldDeleteProject()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var deleteProjectCommand = new DeleteProjectCommand(createProjectDto.Id);
            await SendAsync(deleteProjectCommand);

            var projectEntity = await ExecuteDbContextAsync(db => db.Projects
                .SingleOrDefaultAsync(p => p.Id.Equals(createProjectDto.Id))
            );

            projectEntity.ShouldBeNull();
        }
    }
}
