namespace Solution
{
    class CollisionOnConnectionBetweenStationsCase
    {
        public class Connection
        {
            private int stationAKey;
            private int stationBKey;

            public int StationAKey => stationAKey;

            public int StationBKey => stationBKey;

            public Connection(int stationAKey, int stationBKey)
            {
                this.stationAKey = stationAKey;
                this.stationBKey = stationBKey;
            }
        }

        private Connection stations;
        private IRoute routeA;
        private IRoute routeB;

        public Connection Stations => stations;

        public IRoute RouteA => routeA;

        public IRoute RouteB => routeB;

        public CollisionOnConnectionBetweenStationsCase(Connection stations, IRoute routeA, IRoute routeB)
        {
            this.stations = stations;
            this.routeA = routeA;
            this.routeB = routeB;
        }

        public override string ToString()
        {
            return "Routes " + routeA.ToString() + " and " + routeB.ToString() + " on connection between stations " + stations.StationAKey + " and " + stations.StationBKey + ".";
        }
    }
}
