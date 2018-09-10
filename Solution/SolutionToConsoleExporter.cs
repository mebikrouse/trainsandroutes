using System;
using System.Linq;
using System.Collections.Generic;

namespace Solution
{
    class SolutionToConsoleExporter : ISolutionExporter
    {
        private int solutionsCount = 0;

        public void Export(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes, IEnumerable<CollisionAtStationCase> collisionAtStationCases, IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases)
        {
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("                                     TASK #" + ++solutionsCount);
            Console.WriteLine("------------------------------------------------------------------------------------");

            PrintTransportNetwork(transportNetwork);
            PrintRoutes(routes);
            PrintCollisionAtStationCases(collisionAtStationCases);
            PrintCollisionOnConnectionBetweenStationsCases(collisionOnConnectionBetweenStationsCases);
        }

        private void PrintTransportNetwork(ITransportNetwork transportNetwork)
        {
            Console.WriteLine("  Task's transport network:");

            Console.WriteLine("    Stations:");
            IEnumerable<int> transportNetworkStations = transportNetwork.GetAllStations();
            Console.Write("      ");
            foreach (int station in transportNetworkStations)
                Console.Write(station + " ");
            Console.WriteLine();

            Console.WriteLine("    Connections:");
            foreach (int station in transportNetworkStations)
            {
                Console.Write("      ");
                foreach (int connectedStation in transportNetwork.GetAllConnectedToTargetStationStations(station))
                    Console.Write(station + "<->" + connectedStation + " ");
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void PrintRoutes(IEnumerable<IRoute> routes)
        {
            Console.WriteLine("  Task's routes:");

            foreach (IRoute route in routes)
                Console.WriteLine("    " + route.ToString());

            Console.WriteLine();
        }

        private void PrintCollisionAtStationCases(IEnumerable<CollisionAtStationCase> collisionAtStationCases)
        {
            Console.WriteLine("  Collisions that may happen at stations:");

            if (collisionAtStationCases == null || !collisionAtStationCases.Any()) Console.WriteLine("    There are no such stations.");
            else
            {
                foreach (CollisionAtStationCase collisionCase in collisionAtStationCases)
                    Console.WriteLine("    " + collisionCase.ToString());
            }

            Console.WriteLine();
        }

        private void PrintCollisionOnConnectionBetweenStationsCases(IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases)
        {
            Console.WriteLine("  Collisions that may happen on connections between stations:");

            if (collisionOnConnectionBetweenStationsCases == null || !collisionOnConnectionBetweenStationsCases.Any()) Console.WriteLine("    There are no such connections.");
            else
            {
                foreach (CollisionOnConnectionBetweenStationsCase collisionCase in collisionOnConnectionBetweenStationsCases)
                    Console.WriteLine("    " + collisionCase.ToString());
            }

            Console.WriteLine();
        }
    }
}
