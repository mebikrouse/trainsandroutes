using System;
using System.Collections.Generic;

namespace Solution
{
    interface ITaskDataProvider
    {
        bool HasNextTaskData();
        Tuple<ITransportNetwork, IEnumerable<IRoute>> GetTaskData();
    }
}
