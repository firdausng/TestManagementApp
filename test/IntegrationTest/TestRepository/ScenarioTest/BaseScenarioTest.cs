using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.TestRepository.ScenarioTest
{
    using static SliceFixture;
    public class BaseScenarioTest: IntegrationTestBase
    {
        protected CreateProjectCommand createProjectCommand;
        protected CreatedItemDto createProjectDto;

        protected CreateFeatureCommand createFeatureCommand;
        protected CreatedItemDto createFeatureDto;

        public override async Task InitializeAsync()
        {
            await base.DisposeAsync();
            createProjectCommand = new CreateProjectCommand("project1", true);
            createProjectDto = await SendAsync(createProjectCommand);

            createFeatureCommand = new CreateFeatureCommand("feature1", createProjectDto.Id);
            createFeatureDto = await SendAsync(createFeatureCommand);
        }

        public override async Task DisposeAsync()
        {
            var deleteFeatureCommand = new DeleteFeatureCommand(createFeatureDto.Id, createProjectDto.Id);
            await SendAsync(deleteFeatureCommand);

            var deleteProjectCommand = new DeleteProjectCommand(createProjectDto.Id);
            await SendAsync(deleteProjectCommand);

            await base.DisposeAsync();
        }
    }
}
