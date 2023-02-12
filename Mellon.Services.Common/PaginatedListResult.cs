using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Common.Services
{
    public class PaginatedListResult<T>
    {
        public PaginatedListResult(int? start, int? length, int? total, IEnumerable<T> items)
        {
            Start = start;
            Length = length;
            Total = total;
            Data = items ?? Enumerable.Empty<T>();
        }

        public int? Start { get; }
        public int? Length { get; }
        public int? Total { get; }
        public IEnumerable<T> Data { get; }
    }
}
