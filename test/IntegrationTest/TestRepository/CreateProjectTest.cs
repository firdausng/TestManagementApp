using AppCore.Services.TestRepository.Commands;
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
}
