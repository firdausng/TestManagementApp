using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestExecution;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestExecution.Commands
{
    public class CreateTestSuiteCommand : IRequest<CreatedItemDto>
    {
        public Guid ProjectId { get; set; }
        public string Name { get; private set; }
        public string TagExpression { get; private set; }
        public CreateTestSuiteCommand(string name, string tagExpression)
        {
            TagExpression = tagExpression;
            Name = name;
        }

        public class Handler : IRequestHandler<CreateTestSuiteCommand, CreatedItemDto>
        {
            private readonly IAppDbContext db;

            public Handler(IAppDbContext db)
            {
                this.db = db;
            }
            public async Task<CreatedItemDto> Handle(CreateTestSuiteCommand request, CancellationToken cancellationToken)
            {
                var entity = TestSuite.Factory(request.Name, request.ProjectId);

                EntityGuard.NullGuard(entity, new EntityCreateFailureException(nameof(TestSuite), request.ProjectId, "Entity Creation failed"));

                db.TestSuites.Attach(entity);
                await db.SaveChangesAsync(cancellationToken);

                return new CreatedItemDto(entity.Id); 
            }
        }

        public class CreateTestSuiteCommandValidator : AbstractValidator<CreateTestSuiteCommand>
        {
            public CreateTestSuiteCommandValidator()
            {
                RuleFor(v => v.Name)
                    .MaximumLength(50)
                    .NotEmpty()
                    .WithMessage("Suite name is required.")
                    .WithMessage("Suite name already exists.");

                RuleFor(v => v.ProjectId)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }

}
