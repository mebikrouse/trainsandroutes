namespace Solution
{
    //Хранит информацию о столкновении маршрутов на некоторой станции.
    class CollisionAtStationCase
    {
        //Станция, на которой произошло столкновение.
        private int stationKey;
        //Столкнувшиеся маршруты.
        private IRoute routeA;
        private IRoute routeB;

        public int StationKey => stationKey;

        public IRoute RouteA => routeA;

        public IRoute RouteB => routeB;

        public CollisionAtStationCase(int stationKey, IRoute routeA, IRoute routeB)
        {
            this.stationKey = stationKey;
            this.routeA = routeA;
            this.routeB = routeB;
        }

        public override string ToString()
        {
            return "Routes " + routeA.ToString() + " and " + routeB.ToString() + " at station " + stationKey + ".";
        }
    }
}
