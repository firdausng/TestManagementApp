using System;
using System.Text;

namespace AppCore.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public virtual bool IsEntityExist()
        {
            if (this == null)
            {
                return false;
            }
            return true;
        }
    }
}
