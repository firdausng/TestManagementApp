using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Services.TestRepository.Commands
{
    public class UpdateProjectCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; }
        public bool IsEnabled { get; }

        public UpdateProjectCommand(Guid Id, string Name, bool IsEnabled = true)
        {
            this.Id = Id;
            this.Name = Name;
            this.IsEnabled = IsEnabled;
        }

        public class Handler : IRequestHandler<UpdateProjectCommand>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
            {
                var entityQuery = db.Projects;
                var query = entityQuery.ToQueryString();

                var entity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (!entity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Project), request.Id);
                }

                entity.Update(request.Name, request.IsEnabled);

                db.Projects.Attach(entity);
                await db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
        {
            public UpdateProjectCommandValidator()
            {
                RuleFor(v => v.Id)
                    .NotNull()
                    .NotEmpty();

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
}
