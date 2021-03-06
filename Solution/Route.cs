﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Solution
{
    //Реализация интерфейса IRoute
    class Route : IRoute
    {
        //Станции маршрута.
        List<int> stations;

        public Route(IEnumerable<int> stations)
        {
            this.stations = new List<int>(stations);
            //Если маршрут содержит менее одной стацнии, то сообщить об ошибке.
            if (StationsCount < 1) throw new ArgumentException("Route must contain at least one station.");
        }

        public int this[int index] => stations[index];

        public int StationsCount => stations.Count;

        public IEnumerator<int> GetEnumerator()
        {
            return stations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(stations[0]);
            for (int i = 1; i < StationsCount; i++) stringBuilder.Append("->" + stations[i]);
            return stringBuilder.ToString();
        }
    }
}
