using System.Collections;
using System.Collections.Generic;

namespace Solution
{
    //Структура данных, используемая в алгоритме поиска сталкивающихся путей.
    //Для каждого ключа в коллекции хранится список объектов, имеющих данный ключ.
    class BucketDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, List<TValue>>>
    {
        private Dictionary<TKey, List<TValue>> dictionary;

        //Вернуть список объектов, соответствующих ключу.
        public List<TValue> this[TKey key]
        {
            get { return dictionary[key]; }
        }

        //Конструктор.
        public BucketDictionary()
        {
            dictionary = new Dictionary<TKey, List<TValue>>();
        }

        //Добавить объект value с ключом key/
        public void Add(TKey key, TValue value)
        {
            //Если объектов с ключом key еще нет, то создать список, в котором данные объекты будут храниться.
            if (!ContainsKey(key)) dictionary.Add(key, new List<TValue>());
            //Добавить в список объект value.
            dictionary[key].Add(value);
        }

        //Содержит ли словарь объекты с ключом key.
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
