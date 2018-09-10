using System.Collections.Generic;

namespace Solution
{
    interface ITransportNetwork
    {
        bool ContainsStation(int stationKey);
        bool AreStationsConnected(int stationAKey, int stationBKey);
        int GetConnectionLengthBetweenStations(int stationAKey, int stationBKey);
        IEnumerable<int> GetAllStations();
        IEnumerable<int> GetAllConnectedToTargetStationStations(int stationKey);
    }
}
