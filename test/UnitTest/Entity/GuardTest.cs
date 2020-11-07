using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestRepository;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Entity
{
    public class GuardTest
    {
        [Fact]
        public void Should_Throw_Exception_When_Entity_Null ()
        {
            var project = Project.Factory("test", true);
            project = null;

            Should.Throw<Exception>(() => EntityGuard.NullGuard(project, new Exception()));
        }
    }
}
