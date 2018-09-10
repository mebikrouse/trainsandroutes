using System;
using System.Collections.Generic;

namespace Solution
{
    class CollisionsFinder
    {
        public static IEnumerable<CollisionAtStationCase> FindCollisionsAtStations(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes)
        {
            BucketDictionary<int, Tuple<IRoute, int>> routesPassingThroughStation = new BucketDictionary<int, Tuple<IRoute, int>>();

            foreach (IRoute route in routes)
                for (int i = 0; i < route.StationsCount; i++)
                    routesPassingThroughStation.Add(route[i], new Tuple<IRoute, int>(route, GetRouteLength(transportNetwork, route, 0, i)));

            List<CollisionAtStationCase> collisionAtStationCases = new List<CollisionAtStationCase>();
            foreach (KeyValuePair<int, List<Tuple<IRoute, int>>> possibleCases in routesPassingThroughStation)
            {
                int currentStation = possibleCases.Key;
                List<Tuple<IRoute, int>> routesList = possibleCases.Value;

                for (int i = 0; i < routesList.Count - 1; i++)
                    for (int j = i + 1; j < routesList.Count; j++)
                        if (routesList[i].Item2 == routesList[j].Item2)
                            collisionAtStationCases.Add(new CollisionAtStationCase(currentStation, routesList[i].Item1, routesList[j].Item1));
            }

            return collisionAtStationCases;
        }

        public static IEnumerable<CollisionOnConnectionBetweenStationsCase> FindCollisionsOnConnectionsBetweenStations(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes)
        {
            BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>> routesPassingThroughConnection = new BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>>();
            BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>> routesPassingThroughConnectionInOppositeDirection = new BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>>();

            foreach (IRoute route in routes)
            {
                for (int i = 0; i < route.StationsCount - 1; i++)
                {
                    Tuple<int, int> connectionKey = new Tuple<int, int>(route[i], route[i + 1]);
                    Tuple<int, int> oppositeConnectionKey = new Tuple<int, int>(route[i + 1], route[i]);

                    Tuple<IRoute, int> passingCase = new Tuple<IRoute, int>(route, GetRouteLength(transportNetwork, route, 0, i));

                    if (routesPassingThroughConnection.ContainsKey(oppositeConnectionKey)) routesPassingThroughConnectionInOppositeDirection.Add(connectionKey, passingCase);
                    else routesPassingThroughConnection.Add(connectionKey, passingCase);
                }
            }

            List<CollisionOnConnectionBetweenStationsCase> collisionBetweenStationsCases = new List<CollisionOnConnectionBetweenStationsCase>();
            foreach (KeyValuePair<Tuple<int, int>, List<Tuple<IRoute, int>>> possibleCases in routesPassingThroughConnection)
            {
                Tuple<int, int> connectionKey = possibleCases.Key;
                Tuple<int, int> oppositeConnectionKey = new Tuple<int, int>(connectionKey.Item2, connectionKey.Item1);

                if (!routesPassingThroughConnectionInOppositeDirection.ContainsKey(oppositeConnectionKey)) continue;

                List<Tuple<IRoute, int>> routesList = possibleCases.Value;
                List<Tuple<IRoute, int>> oppositeRoutesList = routesPassingThroughConnectionInOppositeDirection[oppositeConnectionKey];

                int maxAllowedDifferenceInDistance = transportNetwork.GetConnectionLengthBetweenStations(connectionKey.Item1, connectionKey.Item2);
                foreach (Tuple<IRoute, int> route in routesList)
                {
                    foreach (Tuple<IRoute, int> oppositeRoute in oppositeRoutesList)
                    {
                        int distancesToPassDifference = Math.Abs(route.Item2 - oppositeRoute.Item2);
                        if (distancesToPassDifference < maxAllowedDifferenceInDistance)
                            collisionBetweenStationsCases.Add(new CollisionOnConnectionBetweenStationsCase(new CollisionOnConnectionBetweenStationsCase.Connection(connectionKey.Item1, connectionKey.Item2), route.Item1, oppositeRoute.Item1));
                    }
                }
            }

            return collisionBetweenStationsCases;
        }

        private static int GetRouteLength(ITransportNetwork transportNetwork, IRoute route, int from, int to)
        {
            if (from > to) throw new ArgumentException("Value of from argument must not be lesser than value of to argument.");

            int length = 0;
            for (int i = from; i < to; i++)
                length += transportNetwork.GetConnectionLengthBetweenStations(route[i], route[i + 1]);

            return length;
        }
    }
}
