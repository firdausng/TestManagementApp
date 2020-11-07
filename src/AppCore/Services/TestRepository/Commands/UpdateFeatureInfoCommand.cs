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
using AppCore.Domain.Entities.Common.Guards;

namespace AppCore.Services.TestRepository.Commands
{
    public class UpdateFeatureInfoCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public UpdateFeatureInfoCommand(Guid Id, Guid ProjectId, string Name, string Description)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
            this.Name = Name;
            this.Description = Description;
        }

        public class Handler : IRequestHandler<UpdateFeatureInfoCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateFeatureInfoCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.Projects;
                var query = entityQuery.ToQueryString();

                var projectEntity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.ProjectId), cancellationToken);

                EntityGuard.NullGuard(projectEntity, new EntityNotFoundException(nameof(Project), request.ProjectId));

                var featureEntity = await db.Features
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(featureEntity, new EntityNotFoundException(nameof(Feature), request.Id));

                featureEntity.UpdateInfo(request.Name, request.Description);

                db.Features.Attach(featureEntity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class UpdateFeatureInfoCommandValidator : AbstractValidator<UpdateFeatureInfoCommand>
        {
            public UpdateFeatureInfoCommandValidator()
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
