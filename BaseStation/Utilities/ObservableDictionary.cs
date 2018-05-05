using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HuskyRobotics.Utilities
{
    /// <summary>
    /// A map which can have it's values bound to
    /// K and V must have a default constructor
    /// 
    /// This class is probabaly not suitable for use.
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(DictionaryEntry))]
    public class ObservableDictionary<K, V> : IList<ObservableDictionary<K, V>.Entry>, IList, INotifyPropertyChanged, INotifyCollectionChanged, 
        IEnumerable<ObservableDictionary<K, V>.Entry>
    {
        private OrderedDictionary dictionary = new OrderedDictionary();

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        /// <summary>
        /// Gets the string representation of the value at the given dictionary key. Designed for bindings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V this[K key] {
            get => (V)dictionary[key];
        }

        public bool ContainsKey(K key) => dictionary.Contains(key);

        public V GetValue(K key) => (V)dictionary[key];

        /// <summary>
        /// Returns the value stored in the map if it exists. If it does not, a default
        /// value is provided. This default value is then inserted into the map.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultProvider"></param>
        /// <returns></returns>
        public V GetValueDefault(K key, Func<V> defaultProvider)
        {
            V value;
            if (dictionary.Contains(key))
            {
                value = (V)dictionary[key];
                return value;
            } else
            {
                value = defaultProvider();
                dictionary[key] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
                return value;
            }
        }

        public void PutValue(K key, V value)
        {
            var countChanged = !dictionary.Contains(key);
            dictionary[key] = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[" + key + "]"));
            if (countChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }
        
        public int Add(Entry entry)
        {
            PutValue(entry.Key, entry.Value);
            return Count - 1;
        }

        //public void Add(object entry)
        //{
        //    if (entry is Entry)
        //    {
        //        Add((Entry)entry);
        //    } else
        //    {
        //        throw new ArgumentException("Attempt to insert an entry of type " + entry.GetType() + " when it should be of type " + typeof(Entry));
        //    }
        //}

        public IEnumerator<Entry> GetEnumerator()
        {
            return new DictionaryEnumerator(dictionary.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<Entry>.Add(Entry item) => PutValue(item.Key, item.Value);

        public void Clear() => dictionary.Clear();

        // Hopefully this does what we want
        public bool Contains(Entry item) => dictionary.Contains(item.Key); 

        public void CopyTo(Entry[] array, int arrayIndex) => dictionary.CopyTo(array, arrayIndex);

        public bool IsFixedSize {
            get => false;
        }

        public bool Remove(Entry item)
        {
            if (dictionary.Contains(item.Key))
            {
                dictionary.Remove(item.Key);
                return true;
            } else
            {
                return false;
            }            
        }

        public int IndexOf(Entry item)
        {
            int i = 0;
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key.Equals(item.Key))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public void Insert(int index, Entry item)
        {
            dictionary.Insert(index, item.Key, item.Value);
        }

        public void RemoveAt(int index)
        {
            dictionary.RemoveAt(index);
        }

        public int Add(object value)
        {
            return Add((Entry)value);
        }

        public bool Contains(object value)
        {
            return Contains((Entry)value);
        }

        public int IndexOf(object value)
        {
            return IndexOf((Entry)value);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (Entry)value);
        }

        public void Remove(object value)
        {
            Remove((Entry)value);
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo((Entry[])array, index); // The cast here might not work
        }

        public bool IsReadOnly => false;

        public int Count => dictionary.Count;

        public object SyncRoot => null; // Not a proper implementation

        public bool IsSynchronized => false;

        object IList.this[int index] { get => this[index]; set => this[index] = (Entry)value; }

        public Entry this[int index] {
            get {
                var entry = dictionary.Cast<DictionaryEntry>().ElementAt(index);
                return new Entry((K)entry.Key, (V)entry.Value);
            }
            set {
                // A dumb way to do it, but it works and it preserves indexes when the key changes
                dictionary.RemoveAt(index);
                dictionary.Insert(index, value.Key, value.Value);
            }
        }

        /// <summary>
        /// An entry which is default constructable
        /// </summary>
        public class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public Entry() : this(default(K), default(V)) { }
            public Entry(K key, V value)
            {
                Key = key;
                Value = value;
            }

            public override bool Equals(object other)
            {
                if (other is Entry)
                {
                    var entry = other as Entry;
                    if (entry.Key == null && entry.Value == null)
                    {
                        return (Key == null && Value == null);
                    }
                    return (entry.Key.Equals(this.Key) && entry.Value.Equals(this.Value));
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Key.GetHashCode() ^ Value.GetHashCode();
            }
        }

        private class DictionaryEnumerator : IEnumerator<Entry>
        {
            private IDictionaryEnumerator backingEnum;

            public DictionaryEnumerator(IDictionaryEnumerator dictionaryEnumerator)
            {
                this.backingEnum = dictionaryEnumerator;
            }

            public Entry Current => new Entry((K)backingEnum.Key, (V)backingEnum.Value);

            object IEnumerator.Current => backingEnum.Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return backingEnum.MoveNext();
            }

            public void Reset()
            {
                backingEnum.Reset();
            }
        }
    }
}
