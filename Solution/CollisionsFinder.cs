using System;
using System.Collections.Generic;

namespace Solution
{
    //Содержит методы поиска столкновений на станциях и путях.
    class CollisionsFinder
    {
        //Ищет все столкновения на станциях.
        public static IEnumerable<CollisionAtStationCase> FindCollisionsAtStations(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes)
        {
            //Для каждой станции запоминаем маршруты, проходящие через нее, а также расстояние, которое необходимо пройти по маршруту, чтобы добраться до данной станции.
            BucketDictionary<int, Tuple<IRoute, int>> routesPassingThroughStation = new BucketDictionary<int, Tuple<IRoute, int>>();
            foreach (IRoute route in routes)
                for (int i = 0; i < route.StationsCount; i++)
                    routesPassingThroughStation.Add(route[i], new Tuple<IRoute, int>(route, GetRouteLength(transportNetwork, route, 0, i)));

            //Обходим каждую станцию и смотрим, есть ли такая пара маршрутов, проходящих через данную станцию, расстояние до данной станции которых одинаково. 
            //Такая пара маршрутов столкнется в рассматриваемой станции.
            List<CollisionAtStationCase> collisionAtStationCases = new List<CollisionAtStationCase>();
            //Для каждой пары (Станция; Все маршруты, проходящие через станцию).
            foreach (KeyValuePair<int, List<Tuple<IRoute, int>>> possibleCases in routesPassingThroughStation)
            {
                //Текущая станция.
                int currentStation = possibleCases.Key;
                //Все маршруты, проходящие через текущую станцию.
                List<Tuple<IRoute, int>> routesList = possibleCases.Value;

                //Смотрим все возможные пары машрутов.
                for (int i = 0; i < routesList.Count - 1; i++)
                    for (int j = i + 1; j < routesList.Count; j++)
                        //Если расстояние, которое необходимо пройти по первому маршруту, чтобы добраться до текущей станции, аналогично расстоянию второго маршрута, то маршруты сталкиваются.
                        if (routesList[i].Item2 == routesList[j].Item2)
                            collisionAtStationCases.Add(new CollisionAtStationCase(currentStation, routesList[i].Item1, routesList[j].Item1));
            }

            return collisionAtStationCases;
        }

        //Ищет все столкновения на путях.
        public static IEnumerable<CollisionOnConnectionBetweenStationsCase> FindCollisionsOnConnectionsBetweenStations(ITransportNetwork transportNetwork, IEnumerable<IRoute> routes)
        {
            //Для каждого пути между станциями запоминаем маршруты, проходящие через данный путь.
            BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>> routesPassingThroughConnection = new BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>>();
            //Если существует путь, через который уже прошли некоторые маршруты, и мы встречаем, маршрут, который проходит данный путь в обратном направлении, то данный маршрут записывается
            //в нижеуказанную структуру.
            BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>> routesPassingThroughConnectionInOppositeDirection = new BucketDictionary<Tuple<int, int>, Tuple<IRoute, int>>();

            //Запоминаем, какие маршруты проходят через каждый путь и в каком направлении, а также расстояние, которое необходимо пройти по маршруту, чтобы добраться до пути.
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

            //Обходим каждый путь и смотрим, есть ли такая пара маршрутов, которая прошла данный путь в противоположных друг другу направлениях
            List<CollisionOnConnectionBetweenStationsCase> collisionBetweenStationsCases = new List<CollisionOnConnectionBetweenStationsCase>();
            foreach (KeyValuePair<Tuple<int, int>, List<Tuple<IRoute, int>>> possibleCases in routesPassingThroughConnection)
            {
                Tuple<int, int> connectionKey = possibleCases.Key;
                Tuple<int, int> oppositeConnectionKey = new Tuple<int, int>(connectionKey.Item2, connectionKey.Item1);

                //Если все маршруты, проходящие через рассматриваемый путь, проходят его в одном направлении, то, очевидно, на этом пути никакие маршруты не столкнутся
                //(условием столкновения поездов на пути является их движение навстречу друг к другу).
                if (!routesPassingThroughConnectionInOppositeDirection.ContainsKey(oppositeConnectionKey)) continue;

                List<Tuple<IRoute, int>> routesList = possibleCases.Value;
                List<Tuple<IRoute, int>> oppositeRoutesList = routesPassingThroughConnectionInOppositeDirection[oppositeConnectionKey];

                //Для каждого маршрута, проходящего данный путь, смотрим все маршруты, проходящие данный путь в противоположном направлении.
                int maxAllowedDifferenceInDistance = transportNetwork.GetConnectionLengthBetweenStations(connectionKey.Item1, connectionKey.Item2);
                foreach (Tuple<IRoute, int> route in routesList)
                {
                    foreach (Tuple<IRoute, int> oppositeRoute in oppositeRoutesList)
                    {
                        int distancesToPassDifference = Math.Abs(route.Item2 - oppositeRoute.Item2);
                        //Если расстояние, которое необходимо пройти по первому маршруту, не превышает расстояния, которое необходимо пройти по второму, на величину,
                        //бОльшую длины рассматриваемого пути, то маршруты сталкиваются.
                        if (distancesToPassDifference < maxAllowedDifferenceInDistance)
                            collisionBetweenStationsCases.Add(new CollisionOnConnectionBetweenStationsCase(new CollisionOnConnectionBetweenStationsCase.Connection(connectionKey.Item1, connectionKey.Item2), route.Item1, oppositeRoute.Item1));
                    }
                }
            }

            return collisionBetweenStationsCases;
        }

        //Считает расстояние, которое необходимо пройти по маршруту, чтобы добраться от станции с индексом from до станции с индексом to. Индекс - порядковый номер станции в маршруте.
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
