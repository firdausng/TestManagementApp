using AppCore.Services.TestRepository.Commands;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.TestRepository.TagTest
{
    using static SliceFixture;
    public class TagTest: BaseTagTest
    {
        [Fact]
        public async Task ShouldCreateAndDeleteTag()
        {
            var createTagCommand = new CreateTagCommand("t1", createProjectDto.Id);
            var createTagDto = await SendAsync(createTagCommand);

            var tagEntity = await ExecuteDbContextAsync(db => db.Tags
                .SingleOrDefaultAsync(p => p.Id.Equals(createTagDto.Id))
            );

            tagEntity.ShouldNotBeNull();
            tagEntity.Name.ShouldBe("t1");

            var deleteTagCommand = new DeleteTagCommand(createTagDto.Id, createProjectDto.Id);
            await SendAsync(deleteTagCommand);

            tagEntity = await ExecuteDbContextAsync(db => db.Tags
                .SingleOrDefaultAsync(p => p.Id.Equals(createTagDto.Id))
            );

            tagEntity.ShouldBeNull();
        }

        [Fact]
        public async Task ShouldUpdateTag()
        {
            var createTagCommand = new CreateTagCommand("t1", createProjectDto.Id);
            var createTagDto = await SendAsync(createTagCommand);

            var updateTagCommand = new UpdateTagCommand(createTagDto.Id, createProjectDto.Id, "t edit", "description 1");
            await SendAsync(updateTagCommand);

            var scenarioEntity = await ExecuteDbContextAsync(db => db.Tags
                .SingleOrDefaultAsync(p => p.Id.Equals(createTagDto.Id))
            );

            scenarioEntity.ShouldNotBeNull();
            scenarioEntity.Name.ShouldBe("t edit");
            scenarioEntity.Description.ShouldBe("description 1");

            var deleteTagCommand = new DeleteTagCommand(createTagDto.Id, createProjectDto.Id);
            await SendAsync(deleteTagCommand);
        }
    }
}
