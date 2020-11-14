using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries.GetScenario;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/project/{projectId}/scenario")]
    [ApiController]
    public class ScenarioController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScenarioController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetScenarios))]
        public async Task<ActionResult<GetObjectListDto<GetScenarioDto>>> GetScenarios(Guid projectId, bool includeSteps, bool includeFeature)
        {
            var vm = await mediator.Send(new GetScenarioListQuery(projectId, includeSteps, includeFeature));
            return Ok(vm);
        }

        [HttpGet("{scenarioId}", Name = nameof(GetScenario))]
        public async Task<ActionResult<GetScenarioDto>> GetScenario(Guid scenarioId, Guid projectId, bool includeSteps, bool includeFeature)
        {
            var vm = await mediator.Send(new GetScenarioQuery(scenarioId, projectId, includeSteps, includeFeature));
            return Ok(vm);
        }

        [HttpPost(Name = nameof(NewScenario))]
        public async Task<ActionResult<Guid>> NewScenario(CreateScenarioCommand createScenarioCommand)
        {
            var vm = await mediator.Send(createScenarioCommand);
            if (vm.Id != Guid.Empty)
            {
                var link = Url.Link(nameof(GetScenario), new { scenarioId = vm.Id, projectId= createScenarioCommand.ProjectId });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }

        [HttpDelete("{id}", Name = nameof(DeleteScenario))]
        public async Task<ActionResult> DeleteScenario(Guid id, Guid projectId)
        {
            await mediator.Send(new DeleteScenarioCommand(id, projectId));
            return NoContent();
        }

        [HttpPost("AddSteps", Name = nameof(UpdateStepsToScenario))]
        public async Task<ActionResult<Guid>> UpdateStepsToScenario(UpdateStepsToScenarioCommand updateStepsToScenario)
        {
            await mediator.Send(updateStepsToScenario);
            return NoContent();
        }
    }
}
