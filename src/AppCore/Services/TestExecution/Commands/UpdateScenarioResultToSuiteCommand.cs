using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestExecution;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestExecution.Commands
{
    public class UpdateScenarioResultToSuiteCommand : IRequest
    {
        public Guid TestSuiteId { get; }
        public Guid ScenarioId { get; }
        public Guid ProjectId { get; }
        public List<ScenarioResultDto> ScenarioResults { get; } = new List<ScenarioResultDto>();
        public UpdateScenarioResultToSuiteCommand(Guid testSuiteId, Guid projectId, List<ScenarioResultDto> scenarioResults, Guid scenarioId)
        {
            TestSuiteId = testSuiteId;
            ProjectId = projectId;
            ScenarioResults = scenarioResults;
            ScenarioId = scenarioId;
        }

        public class Handler : IRequestHandler<UpdateScenarioResultToSuiteCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateScenarioResultToSuiteCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.TestSuites
                    .Include(s => s.ResultList)
                    .Where(p => p.ProjectId.Equals(request.ProjectId));

                var testSuiteEntity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.TestSuiteId), cancellationToken);

                EntityGuard.NullGuard(testSuiteEntity, new EntityNotFoundException(nameof(TestSuite), request.TestSuiteId));

                var resultList = request.ScenarioResults.Select(s => new ResultSnapshot()).ToList();

                testSuiteEntity.ResultList.AddRange(resultList);

                db.TestSuites.Attach(testSuiteEntity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<UpdateScenarioResultToSuiteCommand>
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

        public class ScenarioResultDto
        {
            public Guid ScenarioId { get; set; }
            public DateTime ExecutionDate { get; set; }
            public TimeSpan TestDuration { get; set; }
            public TestStatus TestStatus { get; set; }
            public string StatusReason { get; set; }

            public ScenarioResultDto()
            {
            }
        }
    }

}
