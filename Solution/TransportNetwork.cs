using System;
using System.Collections.Generic;

namespace Solution
{
    class TransportNetwork : ITransportNetwork
    {
        private class ConnectionBetweenStations
        {
            int connectedStationKey;
            int connectionLength;

            public int ConnectedStationKey => connectedStationKey;

            public int ConnectionLength => connectionLength;

            public ConnectionBetweenStations(int connectedStationKey, int connectionLength)
            {
                this.connectedStationKey = connectedStationKey;
                this.connectionLength = connectionLength;
            }
        }

        Dictionary<int, LinkedList<ConnectionBetweenStations>> adjacencyLists; 

        public TransportNetwork(IEnumerable<int> stationKeys, IEnumerable<Tuple<int, int, int>> connectionsBetweenStations)
        {
            adjacencyLists = new Dictionary<int, LinkedList<ConnectionBetweenStations>>();

            foreach (int stationKey in stationKeys)
                if (!adjacencyLists.ContainsKey(stationKey))
                    adjacencyLists.Add(stationKey, new LinkedList<ConnectionBetweenStations>());
                else throw new ArgumentException("There is already a station with " + stationKey + " key.");

            foreach(Tuple<int, int, int> connection in connectionsBetweenStations)
            {
                int stationAKey = connection.Item1;
                int stationBKey = connection.Item2;
                int connectionLength = connection.Item3;
                
                if (AreStationsConnected(stationAKey, stationBKey)) throw new ArgumentException("There is already a connection between stations " + stationAKey + " and " + stationBKey + ".");
                if (stationAKey == stationBKey) throw new ArgumentException("Loops are not supported in current implementation.");
                if (connectionLength <= 0) throw new ArgumentException("Length of connection between stations must be a positive value.");

                adjacencyLists[stationAKey].AddFirst(new ConnectionBetweenStations(stationBKey, connectionLength));
                adjacencyLists[stationBKey].AddFirst(new ConnectionBetweenStations(stationAKey, connectionLength));
            }
        }

        public bool ContainsStation(int stationKey)
        {
            return adjacencyLists.ContainsKey(stationKey);
        }

        public bool AreStationsConnected(int stationAKey, int stationBKey)
        {
            if (!ContainsStation(stationAKey)) throw new ArgumentException("There is no station with " + stationAKey + " key.");
            if (!ContainsStation(stationBKey)) throw new ArgumentException("There is no station with " + stationBKey + " key.");

            foreach (ConnectionBetweenStations connection in adjacencyLists[stationAKey])
                if (connection.ConnectedStationKey == stationBKey)
                    return true;

            return false;
        }

        public int GetConnectionLengthBetweenStations(int stationAKey, int stationBKey)
        {
            if (!ContainsStation(stationAKey)) throw new ArgumentException("There is no station with " + stationAKey + " key.");
            if (!ContainsStation(stationBKey)) throw new ArgumentException("There is no station with " + stationBKey + " key.");

            foreach (ConnectionBetweenStations connection in adjacencyLists[stationAKey])
                if (connection.ConnectedStationKey == stationBKey)
                    return connection.ConnectionLength;

            throw new ArgumentException("There is no connection between stations " + stationAKey + " and " + stationBKey + ".");
        }

        public IEnumerable<int> GetAllStations()
        {
            return adjacencyLists.Keys;
        }

        public IEnumerable<int> GetAllConnectedToTargetStationStations(int stationKey)
        {
            if (!ContainsStation(stationKey)) throw new ArgumentException("There is no station with " + stationKey + " key.");

            List<int> connectedStations = new List<int>();
            foreach (ConnectionBetweenStations connection in adjacencyLists[stationKey])
                connectedStations.Add(connection.ConnectedStationKey);

            return connectedStations;
        }
    }
}
