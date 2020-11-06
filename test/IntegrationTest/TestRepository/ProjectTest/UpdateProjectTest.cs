using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;
    public class UpdateProjectTest : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldUpdateProject()
        {
            var createProjectCommand = new CreateProjectCommand("project1", true);
            var createProjectDto = await SendAsync(createProjectCommand);

            var updatedProjectCommand = new UpdateProjectCommand(createProjectDto.Id, "updated project1", false);
            await SendAsync(updatedProjectCommand);

            var projectEntity = await ExecuteDbContextAsync(db => db.Projects
                .SingleOrDefaultAsync(p => p.Id.Equals(createProjectDto.Id))
            );

            projectEntity.ShouldNotBeNull();
            projectEntity.Name.ShouldBe("updated project1");
            projectEntity.IsEnabled.ShouldBe(false);
        }
    }
}
