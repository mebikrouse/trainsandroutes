using System;
using System.IO;
using System.Collections.Generic;

namespace Solution
{
    class TaskDataFromStreamProvider : ITaskDataProvider
    {
        private TextReader textReader;

        public TaskDataFromStreamProvider(TextReader textReader)
        {
            this.textReader = textReader;
        }

        public bool HasNextTaskData()
        {
            return textReader.Peek() != -1;
        }

        public Tuple<ITransportNetwork, IEnumerable<IRoute>> GetTaskData()
        {
            if (!HasNextTaskData()) throw new InvalidOperationException("There are no tasks left.");

            ITransportNetwork transportNetwork = ConstructTransportNetwork();
            IEnumerable<IRoute> routes = ConstructRoutes();

            return new Tuple<ITransportNetwork, IEnumerable<IRoute>>(transportNetwork, routes);
        }

        private ITransportNetwork ConstructTransportNetwork()
        {
            List<int> transportNetworkStations = new List<int>();

            int stationsCount = Int32.Parse(textReader.ReadLine());

            string[] stationKeys = textReader.ReadLine().Split(' ');
            if (stationKeys.Length != stationsCount) throw new FormatException("Unexpected format.");

            for (int i = 0; i < stationsCount; i++) 
                transportNetworkStations.Add(Int32.Parse(stationKeys[i]));

            List<Tuple<int, int, int>> transportNetworkConnections = new List<Tuple<int, int, int>>();

            int connectionsCount = Int32.Parse(textReader.ReadLine());
            for (int i = 0; i < connectionsCount; i++)
            {
                string[] connectedStationsKeys = textReader.ReadLine().Split(' ');
                if (connectedStationsKeys.Length != 3) throw new FormatException("Unexpected format.");

                transportNetworkConnections.Add(new Tuple<int, int, int>(Int32.Parse(connectedStationsKeys[0]), Int32.Parse(connectedStationsKeys[1]), Int32.Parse(connectedStationsKeys[2])));
            }

            return new TransportNetwork(transportNetworkStations, transportNetworkConnections);
        }

        private IEnumerable<IRoute> ConstructRoutes()
        {
            List<IRoute> routes = new List<IRoute>();

            int routesCount = Int32.Parse(textReader.ReadLine());
            for (int i = 0; i < routesCount; i++)
            {
                List<int> routeStationsList = new List<int>();

                string[] pathKeys = textReader.ReadLine().Split(' ');
                for (int j = 0; j < pathKeys.Length; j++)
                    routeStationsList.Add(Int32.Parse(pathKeys[j]));

                routes.Add(new Route(routeStationsList));
            }

            return routes;
        }
    }
}
