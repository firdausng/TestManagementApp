using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using AppCore.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using AppCore.Domain.Entities.Common.Guards;

namespace AppCore.Services.TestRepository.Commands
{
    public class CreateProjectCommand : IRequest<CreatedItemDto>
    {
        public string Name { get; }
        public bool IsEnabled { get; }

        public CreateProjectCommand(string Name, bool IsEnabled = true)
        {
            this.Name = Name;
            this.IsEnabled = IsEnabled;
        }

        public class Handler : IRequestHandler<CreateProjectCommand, CreatedItemDto>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<CreatedItemDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
            {
                var entity = Project.Factory(request.Name, request.IsEnabled);

                db.Projects.Add(entity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id);
            }
        }

        public class CreateScenarioCommandValidator : AbstractValidator<CreateProjectCommand>
        {
            public CreateScenarioCommandValidator()
            {
                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Project name is required.")
                    .WithMessage("Project name already exists.");

                RuleFor(v => v.IsEnabled)
                    .Must(v => v == false || v == true)
                    .WithName("IsEnabled")
                    .NotNull()
                    .WithName("IsEnabled");
            }
        }
    }

    public class CreateScenarioCommand : IRequest<CreatedItemDto>
    {
        public string Name { get; }
        public Guid ProjectId { get; }
        public Guid FeatureId { get; }
        public string Description { get; }

        public CreateScenarioCommand(string name, Guid projectId, Guid featureId = new Guid())
        {
            Name = name;
            ProjectId = projectId;
            FeatureId = featureId;
        }

        public class Handler : IRequestHandler<CreateScenarioCommand, CreatedItemDto>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<CreatedItemDto> Handle(CreateScenarioCommand request, CancellationToken cancellationToken)
            {
                var projectEntity = await db.Projects
                    .FirstOrDefaultAsync(p => p.Id.Equals(request.ProjectId), cancellationToken);

                EntityGuard.NullGuard(projectEntity, new EntityNotFoundException(nameof(Project), request.ProjectId));

                Feature featureEntity = null;
                if (request.FeatureId != Guid.Empty)
                {
                    featureEntity = await db.Features
                        .FirstOrDefaultAsync(p => p.Id.Equals(request.FeatureId), cancellationToken);

                    EntityGuard.NullGuard(featureEntity, new EntityNotFoundException(nameof(Feature), request.FeatureId));
                }

                var entity = Scenario.Factory(request.Name, projectEntity, featureEntity);

                EntityGuard.NullGuard(entity, new EntityCreateFailureException(nameof(Scenario), request, "Entity Creation failed"));

                projectEntity.AddScenario(entity);

                db.Projects.Attach(projectEntity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id);
            }
        }

        public class CreateScenarioCommandValidator : AbstractValidator<CreateScenarioCommand>
        {
            public CreateScenarioCommandValidator()
            {
                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Project name is required.")
                    .WithMessage("Project name already exists.");
            }
        }
    }
}
