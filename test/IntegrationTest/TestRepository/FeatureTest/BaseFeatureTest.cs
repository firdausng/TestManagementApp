using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.TestRepository
{
    using static SliceFixture;
    public class BaseFeatureTest : IntegrationTestBase
    {
        protected CreateProjectCommand createProjectCommand;
        protected CreatedItemDto createProjectDto;
        public override async Task InitializeAsync()
        {
            await base.DisposeAsync();
            createProjectCommand = new CreateProjectCommand("project1", true);
            createProjectDto = await SendAsync(createProjectCommand);
        }

        public override async Task DisposeAsync()
        {
            var deleteProjectCommand = new DeleteProjectCommand(createProjectDto.Id);
            await SendAsync(deleteProjectCommand);

            await base.DisposeAsync();
        }
    }
}
