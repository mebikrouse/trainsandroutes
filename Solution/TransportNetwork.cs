using System;
using System.Collections.Generic;

namespace Solution
{
    //Реализация интерфейса ITransportNetwork.
    class TransportNetwork : ITransportNetwork
    {
        //Информация о смежности станций и расстоянии между ними.
        private class ConnectionBetweenStations
        {
            //Смежная станция.
            int connectedStationKey;
            //Расстояние до смежной станции.
            int connectionLength;

            public int ConnectedStationKey => connectedStationKey;

            public int ConnectionLength => connectionLength;

            public ConnectionBetweenStations(int connectedStationKey, int connectionLength)
            {
                this.connectedStationKey = connectedStationKey;
                this.connectionLength = connectionLength;
            }
        }

        //Словарь со списками смежности для каждой станции.
        Dictionary<int, LinkedList<ConnectionBetweenStations>> adjacencyLists; 

        public TransportNetwork(IEnumerable<int> stationKeys, IEnumerable<Tuple<int, int, int>> connectionsBetweenStations)
        {
            adjacencyLists = new Dictionary<int, LinkedList<ConnectionBetweenStations>>();

            //Проверим, содержатся ли одинаковые ключи станций в stationKeys. Каждый ключ должен быть уникален. Если ошибок нет, то добавим станции в транспортную сеть.
            foreach (int stationKey in stationKeys)
                //Если данного ключа нет в словаре, то значит, что такой ключ еще не встречался.
                if (!adjacencyLists.ContainsKey(stationKey))
                    adjacencyLists.Add(stationKey, new LinkedList<ConnectionBetweenStations>());
                //В другом случае ключ уже встречался, а дубликаты ключей недопустимы.
                else throw new ArgumentException("There is already a station with " + stationKey + " key.");

            //Проверим на корректность пути между станциями. Если ошибок нет, то добавим пути в транспортную сеть.
            foreach(Tuple<int, int, int> connection in connectionsBetweenStations)
            {
                int stationAKey = connection.Item1;
                int stationBKey = connection.Item2;
                int connectionLength = connection.Item3;
                
                //Если добавляемый путь уже есть в транспортной сети, то сообщить об ошибке.
                if (AreStationsConnected(stationAKey, stationBKey)) throw new ArgumentException("There is already a connection between stations " + stationAKey + " and " + stationBKey + ".");
                //Если добавляемый путь является петлей, то сообщить об ошибке.
                if (stationAKey == stationBKey) throw new ArgumentException("Loops are not supported in current implementation.");
                //Если длина пути являтся отрицательной величиной или равна нулю, то сообщить об ошибке.
                if (connectionLength <= 0) throw new ArgumentException("Length of connection between stations must be a positive value.");

                //Так как граф не является направленным, необходимо добавлять два ребра
                adjacencyLists[stationAKey].AddFirst(new ConnectionBetweenStations(stationBKey, connectionLength));
                adjacencyLists[stationBKey].AddFirst(new ConnectionBetweenStations(stationAKey, connectionLength));
            }
        }

        public bool ContainsStation(int stationKey)
        {
            //Если словарь содержит ключ stationKey, то значит, что станция с таким ключом существует в транспортной сети.
            return adjacencyLists.ContainsKey(stationKey);
        }

        public bool AreStationsConnected(int stationAKey, int stationBKey)
        {
            //Если станции с ключом stationAKey в транспортной сети нет, то сообщить об ошибке.
            if (!ContainsStation(stationAKey)) throw new ArgumentException("There is no station with " + stationAKey + " key.");
            //Если станции с ключом stationBKey в транспортной сети нет, то сообщить об ошибке.
            if (!ContainsStation(stationBKey)) throw new ArgumentException("There is no station with " + stationBKey + " key.");

            //Если обе станции существуют и являются смежными, то stationBKey должна обязательно быть в списке смежности станции stationAKey
            foreach (ConnectionBetweenStations connection in adjacencyLists[stationAKey])
                if (connection.ConnectedStationKey == stationBKey)
                    return true;

            //Если весь список смежности был пройдет, а вхождение stationBKey не было обнаружено, то станции не являются смежными.
            return false;
        }

        public int GetConnectionLengthBetweenStations(int stationAKey, int stationBKey)
        {
            //Если станции с ключом stationAKey в транспортной сети нет, то сообщить об ошибке.
            if (!ContainsStation(stationAKey)) throw new ArgumentException("There is no station with " + stationAKey + " key.");
            //Если станции с ключом stationBKey в транспортной сети нет, то сообщить об ошибке.
            if (!ContainsStation(stationBKey)) throw new ArgumentException("There is no station with " + stationBKey + " key.");

            //Если станции с ключами stationAKey и stationBKey смежны, то stationBKey обязательно должен быть в списке смежности станции stationAKey, где хранится
            //информация о расстянии.
            foreach (ConnectionBetweenStations connection in adjacencyLists[stationAKey])
                if (connection.ConnectedStationKey == stationBKey)
                    return connection.ConnectionLength;

            //Если вхождение stationBKey в списке смежности станции с ключом stationAKey не было найдено, то станции не являются смежными, о чем необходимо сообщить.
            throw new ArgumentException("There is no connection between stations " + stationAKey + " and " + stationBKey + ".");
        }

        public IEnumerable<int> GetAllStations()
        {
            return adjacencyLists.Keys;
        }

        public IEnumerable<int> GetAllConnectedToTargetStationStations(int stationKey)
        {
            //Если станции с ключом stationKey в транспортной сети нет, то сообщить об ошибке.
            if (!ContainsStation(stationKey)) throw new ArgumentException("There is no station with " + stationKey + " key.");

            //В другом случае заполнить список станциями, которые являются смежными с данной.
            List<int> connectedStations = new List<int>();
            foreach (ConnectionBetweenStations connection in adjacencyLists[stationKey])
                connectedStations.Add(connection.ConnectedStationKey);

            return connectedStations;
        }
    }
}
