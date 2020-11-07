using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Commands
{
    public class DeleteScenarioCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DeleteScenarioCommand(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class Handler : IRequestHandler<DeleteScenarioCommand>
        {
            private readonly IAppDbContext context;
            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
            {
                var scenarioEntity = await context.Scenarios
                    .Include(s => s.Project)
                    .Where(t => t.Project.Id.Equals(request.ProjectId))
                    .Where(t => t.Id.Equals(request.Id))
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(scenarioEntity, new EntityNotFoundException(nameof(Scenario), request.Id));

                context.Scenarios.Remove(scenarioEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
