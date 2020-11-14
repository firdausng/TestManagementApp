using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestExecution;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestExecution.Commands
{
    public class DeleteTestSuiteCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DeleteTestSuiteCommand(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class Handler : IRequestHandler<DeleteTestSuiteCommand>
        {
            private readonly IAppDbContext context;
            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteTestSuiteCommand request, CancellationToken cancellationToken)
            {
                var testSuiteEntity = await context.TestSuites
                    .Where(t => t.ProjectId == request.ProjectId)
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(testSuiteEntity, new EntityNotFoundException(nameof(TestSuite), request.Id));

                context.TestSuites.Remove(testSuiteEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
