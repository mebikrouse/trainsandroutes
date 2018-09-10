using System;
using System.Linq;
using System.Collections.Generic;

namespace Solution
{
    //ISolutionExporter, выводящий результат решения на консоль.
    class SolutionToConsoleExporter : ISolutionExporter
    {
        //Счетчик показанный решений. Выводится при показе очередного решения.
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

        //Вывести информацию о транспортной сети, для которой осуществлялся поиск столкновений.
        private void PrintTransportNetwork(ITransportNetwork transportNetwork)
        {
            Console.WriteLine("  Task's transport network:");

            //Вывести станции транспортной сети
            Console.WriteLine("    Stations:");
            IEnumerable<int> transportNetworkStations = transportNetwork.GetAllStations();
            Console.Write("      ");
            foreach (int station in transportNetworkStations)
                Console.Write(station + " ");
            Console.WriteLine();

            //Вывести пути между станциями транспортной сети.
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

        //Вывести информацию о маршрутах, которые необходимо проверить на предмет столкновений.
        private void PrintRoutes(IEnumerable<IRoute> routes)
        {
            Console.WriteLine("  Task's routes:");

            //Вывести маршруты
            foreach (IRoute route in routes)
                Console.WriteLine("    " + route.ToString());

            Console.WriteLine();
        }

        //Вывести информацию о столкновениях, которые произошли на станциях.
        private void PrintCollisionAtStationCases(IEnumerable<CollisionAtStationCase> collisionAtStationCases)
        {
            Console.WriteLine("  Collisions that may happen at stations:");

            //Если коллекция пуста, то столкновений не произошло.
            if (collisionAtStationCases == null || !collisionAtStationCases.Any()) Console.WriteLine("    There are no such stations.");
            else
            {
                //В другом случае вывести все столкновения.
                foreach (CollisionAtStationCase collisionCase in collisionAtStationCases)
                    Console.WriteLine("    " + collisionCase.ToString());
            }

            Console.WriteLine();
        }

        //Вывести информацию о столкновениях, которые произошли на путях.
        private void PrintCollisionOnConnectionBetweenStationsCases(IEnumerable<CollisionOnConnectionBetweenStationsCase> collisionOnConnectionBetweenStationsCases)
        {
            Console.WriteLine("  Collisions that may happen on connections between stations:");

            //Если коллекция пуста, то столкновений не произошло.
            if (collisionOnConnectionBetweenStationsCases == null || !collisionOnConnectionBetweenStationsCases.Any()) Console.WriteLine("    There are no such connections.");
            else
            {
                //В другом случае вывести все столкновения.
                foreach (CollisionOnConnectionBetweenStationsCase collisionCase in collisionOnConnectionBetweenStationsCases)
                    Console.WriteLine("    " + collisionCase.ToString());
            }

            Console.WriteLine();
        }
    }
}
