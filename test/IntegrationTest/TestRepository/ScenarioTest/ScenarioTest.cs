using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries.GetScenario;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository.ScenarioTest
{
    using static SliceFixture;
    public class ScenarioTest: BaseScenarioTest
    {
        [Fact]
        public async Task ShouldCreateAndDeleteScenario()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldNotBeNull();
            scenarioEntity.Description.ShouldBe("s1");

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);

            scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldUpdateScenario()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var updateScenarioCommand = new UpdateScenarioCommand(createScenarioDto.Id, createProjectDto.Id, "description 1");
            await SendAsync(updateScenarioCommand);

            var scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldNotBeNull();
            scenarioEntity.Description.ShouldBe("description 1");

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);

            scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldUpdateStepsIntoScenario()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var updateScenarioCommand = new UpdateStepsToScenarioCommand(createScenarioDto.Id, createProjectDto.Id, new List<UpdateStepsToScenarioCommand.StepDto> 
            { 
                new UpdateStepsToScenarioCommand.StepDto(1, "step 1"),
                new UpdateStepsToScenarioCommand.StepDto(2, "step 2"),
                new UpdateStepsToScenarioCommand.StepDto(3, "step 3"),
            });
            await SendAsync(updateScenarioCommand);

            var scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .Include(s => s.StepsList)
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldNotBeNull();
            scenarioEntity.StepsList.Count.ShouldBe(3);

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);

            scenarioEntity = await ExecuteDbContextAsync(db => db.Scenarios
                .SingleOrDefaultAsync(p => p.Id.Equals(createScenarioDto.Id))
            );

            scenarioEntity.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldGetScenario()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var getScenarioCommand = new GetScenarioQuery(createScenarioDto.Id, createProjectDto.Id, false);
            var getScenarioDto = await SendAsync(getScenarioCommand);

            getScenarioDto.ShouldNotBeNull();
            getScenarioDto.StepList.Count.ShouldBe(0);
            getScenarioDto.Description.ShouldBe("s1");
            getScenarioDto.ProjectId.ShouldBe(createProjectDto.Id);

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);
        }

        [Fact]
        public async Task ShouldGetScenarioWithSteps()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var updateStepsToScenarioCommand = new UpdateStepsToScenarioCommand(createScenarioDto.Id, createProjectDto.Id, new List<UpdateStepsToScenarioCommand.StepDto> 
            { 
                new UpdateStepsToScenarioCommand.StepDto(1, "step 1"),
                new UpdateStepsToScenarioCommand.StepDto(2, "step 2"),
                new UpdateStepsToScenarioCommand.StepDto(3, "step 3"),
            });

            await SendAsync(updateStepsToScenarioCommand);

            var getScenarioCommand = new GetScenarioQuery(createScenarioDto.Id, createProjectDto.Id, true);
            var getScenarioDto = await SendAsync(getScenarioCommand);

            getScenarioDto.ShouldNotBeNull();
            getScenarioDto.StepList.ShouldNotBeNull();
            getScenarioDto.StepList.Count.ShouldBe(3);
            getScenarioDto.Description.ShouldBe("s1");
            getScenarioDto.ProjectId.ShouldBe(createProjectDto.Id);

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);
        }

        [Fact]
        public async Task ShouldGetScenarioList()
        {
            var createScenarioCommand = new CreateScenarioCommand("s1", createProjectDto.Id);
            var createScenarioDto = await SendAsync(createScenarioCommand);

            var getScenarioListCommand = new GetScenarioListQuery(createProjectDto.Id, false, false);
            var getScenarioListDto = await SendAsync(getScenarioListCommand);

            getScenarioListDto.ShouldNotBeNull();
            getScenarioListDto.Data.ShouldNotBeNull();
            getScenarioListDto.Count.ShouldNotBe(0);
            getScenarioListDto.Data.ShouldBeOfType<List<GetScenarioDto>>();

            var deleteScenarioCommand = new DeleteScenarioCommand(createScenarioDto.Id, createProjectDto.Id);
            await SendAsync(deleteScenarioCommand);

        }
    }
}
