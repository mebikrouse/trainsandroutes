using System.Collections.Generic;

namespace Solution
{
    //Предоставляет информаю о транспортной сети.
    interface ITransportNetwork
    {
        //Содержит ли транспортная сеть станцию с ключом stationKey.
        bool ContainsStation(int stationKey);
        //Соединены ли станции с ключами stationAKey и stationBKey путем.
        bool AreStationsConnected(int stationAKey, int stationBKey);
        //Узнать длину пути между станциями с ключами stationAKey и stationBKey.
        int GetConnectionLengthBetweenStations(int stationAKey, int stationBKey);
        //Получить все станции транспортной сети.
        IEnumerable<int> GetAllStations();
        //Получить все смежные с данной станции.
        IEnumerable<int> GetAllConnectedToTargetStationStations(int stationKey);
    }
}
