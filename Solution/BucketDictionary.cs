using System.Collections;
using System.Collections.Generic;

namespace Solution
{
    class BucketDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, List<TValue>>>
    {
        private Dictionary<TKey, List<TValue>> dictionary;

        public List<TValue> this[TKey key]
        {
            get { return dictionary[key]; }
        }

        public BucketDictionary()
        {
            dictionary = new Dictionary<TKey, List<TValue>>();
        }

        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key)) dictionary.Add(key, new List<TValue>());
            dictionary[key].Add(value);
        }

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
