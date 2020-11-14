using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Common.Exceptions;
using System.Linq;

namespace AppCore.Services.TestRepository.Commands
{
    public class UpdateTagCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public UpdateTagCommand(Guid Id, Guid ProjectId, string Name, string Description)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
            this.Name = Name;
            this.Description = Description;
        }

        public class Handler : IRequestHandler<UpdateTagCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.Projects;
                var query = entityQuery.ToQueryString();

                var projectEntity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.ProjectId), cancellationToken);

                EntityGuard.NullGuard(projectEntity, new EntityNotFoundException(nameof(Project), request.ProjectId));

                var tagEntity = await db.Tags
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(tagEntity, new EntityNotFoundException(nameof(Tag), request.Id));

                tagEntity.UpdateInfo(request.Name, request.Description);

                db.Tags.Attach(tagEntity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
        {
            public UpdateTagCommandValidator()
            {
                RuleFor(v => v.Id)
                    .NotNull()
                    .NotEmpty();

                RuleFor(v => v.ProjectId)
                    .NotNull()
                    .NotEmpty();

                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("name is required.");

                RuleFor(v => v.Description)
                    .MaximumLength(150);
            }
        }
    }
}
