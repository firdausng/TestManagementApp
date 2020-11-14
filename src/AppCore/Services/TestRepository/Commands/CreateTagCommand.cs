using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Common.Exceptions;

namespace AppCore.Services.TestRepository.Commands
{
    public class CreateTagCommand : IRequest<CreatedItemDto>
    {
        public string Name { get; }
        public string Description { get; }
        public Guid ProjectId { get; }

        public CreateTagCommand(string Name, Guid ProjectId)
        {
            this.Name = Name;
            this.ProjectId = ProjectId;
        }

        public class Handler : IRequestHandler<CreateTagCommand, CreatedItemDto>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<CreatedItemDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
            {
                var projectEntity = await db.Projects
                    .FirstOrDefaultAsync(p => p.Id.Equals(request.ProjectId), cancellationToken);

                EntityGuard.NullGuard(projectEntity, new EntityNotFoundException(nameof(Project), request.ProjectId));

                var entity = Tag.Factory(request.Name, projectEntity, request.Description);

                EntityGuard.NullGuard(entity, new EntityCreateFailureException(nameof(Tag), request.ProjectId, "Entity Creation failed"));

                projectEntity.Tags.Add(entity);

                db.Projects.Attach(projectEntity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id);
            }
        }

        public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
        {
            public CreateTagCommandValidator()
            {
                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Project name is required.")
                    .WithMessage("Project name already exists.");

                RuleFor(v => v.ProjectId)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}
