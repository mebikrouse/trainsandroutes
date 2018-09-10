using System.Collections.Generic;

namespace Solution
{
    //Отправляет решение получателю. Получателем может быть файл, графический интерфейс, консоль и т.д.
    interface ISolutionExporter
    {
        //Отправить результаты решения.
        void Export(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes, IEnumerable<CollisionAtStationCase> collisionAtStationCases,
            IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases);
    }
}
