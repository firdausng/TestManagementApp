using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace AppCore.Services.TestRepository.Commands
{
    public class UpdateStepsToScenarioCommand : IRequest
    {
        public Guid ScenarioId { get; }
        public Guid ProjectId { get; }
        public List<StepDto> Steps { get; } = new List<StepDto>();
        public UpdateStepsToScenarioCommand(Guid scenarioId, Guid projectId, List<StepDto> steps)
        {
            ScenarioId = scenarioId;
            ProjectId = projectId;
            Steps = steps;
        }

        public class Handler : IRequestHandler<UpdateStepsToScenarioCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateStepsToScenarioCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.Scenarios
                    .Include(s => s.Project)
                    .Include(s => s.StepsList)
                    .Where(p => p.Project.Id.Equals(request.ProjectId));

                var scenarioEntity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.ScenarioId), cancellationToken);

                if (!scenarioEntity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Scenario), request.ScenarioId);
                }

                var stepList = request.Steps.Select(s => new Step(s.Order, s.Description, scenarioEntity)).ToList();

                scenarioEntity.StepsList.Clear();

                await db.SaveChangesAsync(cancellationToken);

                scenarioEntity.AddSteps(stepList);

                db.Scenarios.Attach(scenarioEntity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<UpdateStepsToScenarioCommand>
        {
            public Validator()
            {
                RuleFor(v => v.ScenarioId)
                    .NotNull()
                    .NotEmpty();

                RuleFor(v => v.ProjectId)
                    .NotNull()
                    .NotEmpty();

            }
        }

        public class StepDto
        {
            public StepDto(int order, string description)
            {
                Order = order;
                Description = description;
            }

            public int Order { get; }
            public string Description { get; }
        }
    }
}
