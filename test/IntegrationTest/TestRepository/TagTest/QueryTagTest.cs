using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries.GetTag;
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
    public class QueryTagTest: BaseTagTest
    {

        [Fact]
        public async Task ShouldGetTag()
        {
            var createTagCommand = new CreateTagCommand("t1", createProjectDto.Id);
            var createTagDto = await SendAsync(createTagCommand);

            var getTagQuery = new GetTagQuery(createTagDto.Id, createProjectDto.Id);
            var getTagDto = await SendAsync(getTagQuery);

            getTagDto.ShouldNotBeNull();
            getTagDto.Name.ShouldBe("t1");
            getTagDto.Description.ShouldBeNull();
            getTagDto.ProjectId.ShouldBe(createProjectDto.Id);

            var deleteTagCommand = new DeleteTagCommand(createTagDto.Id, createProjectDto.Id);
            await SendAsync(deleteTagCommand);
        }

        [Fact]
        public async Task ShouldGetTagList()
        {
            var createTagCommand = new CreateTagCommand("t1", createProjectDto.Id);
            var createTagDto = await SendAsync(createTagCommand);

            var getTagListQuery = new GetTagListQuery(createProjectDto.Id);
            var getTagListDto = await SendAsync(getTagListQuery);

            getTagListDto.ShouldNotBeNull();
            getTagListDto.Data.ShouldNotBeNull();
            getTagListDto.Count.ShouldNotBe(0);
            getTagListDto.Data.ShouldBeOfType<List<GetTagDto>>();

            var deleteTagCommand = new DeleteTagCommand(createTagDto.Id, createProjectDto.Id);
            await SendAsync(deleteTagCommand);

        }
    }
}
