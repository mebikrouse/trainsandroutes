using System;
using System.Collections.Generic;

namespace Solution
{
    //Передает вызывателю входные данные задачи. Входные данные могут быть считаны с файла, консоли или быть сгенерированы.
    interface ITaskDataProvider
    {
        //Можно ли получить входные данные.
        bool HasNextTaskData();
        //Получить входные данные.
        Tuple<ITransportNetwork, IEnumerable<IRoute>> GetTaskData();
    }
}
