using System.Collections.Generic;

namespace Solution
{
    //Хранит информацию о маршруте - последовательность станций.
    interface IRoute : IEnumerable<int>
    {
        //Возвращает станцию с индексом index.
        int this[int index] { get; }
        //Количество станций в маршруте.
        int StationsCount { get; }
    }
}
