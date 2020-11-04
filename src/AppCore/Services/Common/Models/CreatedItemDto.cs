using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Services.Common.Models
{
    public class CreatedItemDto
    {
        public CreatedItemDto(Guid Id)
        {
            this.Id = Id;
        }
        public Guid Id { get; }
    }
}
