using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    using static SliceFixture;
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        private static readonly AsyncLock _mutex = new AsyncLock();

        private static bool _initialized;

        public virtual Task DisposeAsync() => Task.CompletedTask;

        public virtual async Task InitializeAsync()
        {
            if (_initialized)
                return;

            using (await _mutex.LockAsync())
            {
                if (_initialized)
                    return;

                await SliceFixture.ResetCheckpoint();

                _initialized = true;
            }
        }
    }
}
