using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Common.Models
{
    public class GetObjectListDto<T>
    {
        public GetObjectListDto(List<T> Data, int Count)
        {
            this.Data = Data;
            this.Count = Count;
        }
        public IReadOnlyCollection<T> Data { get; private set; } = new List<T>();
        public int Count { get; private set; }
    }
}
