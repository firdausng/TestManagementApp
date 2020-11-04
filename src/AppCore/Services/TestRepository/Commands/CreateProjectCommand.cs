using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                var entity = new Project(request.Name, request.IsEnabled);

                db.Projects.Add(entity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id);
            }
        }

        public class CreateTestProjectCommandValidator : AbstractValidator<CreateProjectCommand>
        {
            public CreateTestProjectCommandValidator()
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
}
