using System.Collections.Generic;

namespace Solution
{
    interface ISolutionExporter
    {
        void Export(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes, IEnumerable<CollisionAtStationCase> collisionAtStationCases,
            IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases);
    }
}
