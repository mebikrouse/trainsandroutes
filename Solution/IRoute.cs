using System.Collections.Generic;

namespace Solution
{
    interface IRoute : IEnumerable<int>
    {
        int this[int index] { get; }
        int StationsCount { get; }
    }
}
