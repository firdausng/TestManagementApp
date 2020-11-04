using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.TestRepository;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Commands
{
    public class CreateFeatureCommand : IRequest<CreatedItemDto>
    {
        public string Name { get; }
        public Guid ProjectId { get; }
        public string Description { get; set; }

        public CreateFeatureCommand(string Name, Guid ProjectId)
        {
            this.Name = Name;
            this.ProjectId = ProjectId;
        }

        public class Handler : IRequestHandler<CreateFeatureCommand, CreatedItemDto>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<CreatedItemDto> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
            {
                var projectEntity = await db.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(request.ProjectId), cancellationToken);

                if (!projectEntity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Project), request.ProjectId);
                }

                var entity = Feature.Factory(request.Name, projectEntity);

                if (!entity.IsEntityExist())
                {
                    throw new EntityCreateFailureException(nameof(Feature), request.ProjectId, "Entity Creation failed");
                }

                projectEntity.AddFeature(entity);

                db.Projects.Attach(projectEntity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id);
            }
        }

        public class CreateTestProjectCommandValidator : AbstractValidator<CreateFeatureCommand>
        {
            public CreateTestProjectCommandValidator()
            {
                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Project name is required.")
                    .WithMessage("Project name already exists.");

                //RuleFor(v => v.IsEnabled)
                //    .Must(v => v == false || v == true)
                //    .WithName("IsEnabled")
                //    .NotNull()
                //    .WithName("IsEnabled");
            }
        }
    }
}
