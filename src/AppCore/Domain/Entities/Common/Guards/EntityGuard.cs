using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Domain.Entities.Common.Guards
{
    public static class EntityGuard
    {
        public static void NullGuard(Entity entity, Exception exception)
        {
            if (entity == null)
                throw exception;

        }
    }
}
