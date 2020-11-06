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

namespace AppCore.Services.TestRepository.Commands
{
    public class UpdateScenarioCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid FeatureId { get; private set; }
        public string Description { get; private set; }

        public UpdateScenarioCommand(Guid Id, Guid ProjectId, string Description)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
            this.Description = Description;
        }

        public class Handler : IRequestHandler<UpdateScenarioCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.Scenarios
                    .Include(s => s.Project)
                    .Where(p => p.Project.Id.Equals(request.ProjectId));
                if (request.FeatureId == default)
                {
                    entityQuery.Include(s => s.Feature).Where(p => p.Feature.Id == request.FeatureId);
                }

                var query = entityQuery.ToQueryString();

                var scenarioEntity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (!scenarioEntity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Scenario), request.Id);
                }

                scenarioEntity.UpdateInfo(request.Description);

                db.Scenarios.Attach(scenarioEntity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<UpdateScenarioCommand>
        {
            public Validator()
            {
                RuleFor(v => v.Id)
                    .NotNull()
                    .NotEmpty();

                RuleFor(v => v.ProjectId)
                    .NotNull()
                    .NotEmpty();

                RuleFor(v => v.Description)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Description name is required.");
            }
        }
    }
}
