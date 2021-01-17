using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UyghurEditPP
{
    internal class JsonDictionary
    {
        private static readonly int[] Primes =
        {
            5, 11, 23, 47, 97, 197, 397, 797, 1597, 3203, 6421, 12853, 25717, 51439, 102881, 205763, 411527, 823117,
            1646237, 3292451, 6584911, 13169837, 26339683, 52679369, 105358751, 21071761, 42143573, 84287191, 168574409,
            337148843, 674297699, 1348595401, int.MaxValue
        };

        private int[] _bucket;
        private Entry[] _entries;
        private int _primeIndex;
        private int _capacity;
        private int _free;
        private int _count;

        private class Entry
        {
            public int HashCode;
            public string Key;
            public int Next;
            public InternalObject Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonDictionary()
        {
            _capacity = Primes[_primeIndex++];
            _bucket = new int[_capacity];
            for (var i = 0; i < _capacity; i++)
                _bucket[i] = -1;
            _entries = new Entry[_capacity];
        }

        public int Count{
        	get{return _count - _free;}
        }

        public InternalObject this[string key]
        {
            get
            {
            	InternalObject value;
                if (TryGetValue(key, out value))
                    return value;
                throw new KeyNotFoundException();
            }
            set{
            	Insert(key, value, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(string key, InternalObject value)
        {
            Insert(key, value, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Insert(string key, InternalObject value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException();
            var hashCode = GetHashCode(key);
            var hash = hashCode % _capacity;
            var index = _bucket[hash];
            while (true)
            {
                if (index == -1)
                {
                    if (_count == _capacity)
                    {
                        Resize();
                        hash = hashCode % _capacity;
                    }
                    _entries[_count] = new Entry
                    {
                        HashCode = hashCode,
                        Key = key,
                        Value = value,
                        Next = _bucket[hash]
                    };
                    _bucket[hash] = _count++;
                    return;
                }
                var entry = _entries[index];
                if (entry.HashCode == hashCode && entry.Key == key)
                {
                    if (!add)
                        entry.Value = value;
                    return;
                }
                index = entry.Next;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException();
            var hash = GetHashCode(key);
            var index = hash % _capacity;
            if (_bucket[index] == -1)
                return false;

            var entry = _entries[_bucket[index]];
            var prev = entry;
            while (true)
            {
                if (entry.HashCode == hash && entry.Key == key)
                {
                    entry.HashCode = -1;
                    if (prev == entry)
                    {
                        _bucket[index] = entry.Next;
                    }
                    else
                    {
                        prev.Next = entry.Next;
                    }
                    _free++;
                    return true;
                }
                if (entry.Next == -1)
                    return false;
                prev = entry;
                entry = _entries[entry.Next];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(string key)
        {
        	InternalObject oo;
            return TryGetValue(key, out oo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(string key, out InternalObject value)
        {
        	value = default(InternalObject);
            var hashCode = GetHashCode(key);
            var hash = hashCode % _capacity;
            if (_bucket[hash] == -1)
                return false;
            var entry = _entries[_bucket[hash]];
            while (true)
            {
                if (entry.HashCode == hashCode && entry.Key == key)
                {
                    value = entry.Value;
                    return true;
                }
                if (entry.Next == -1)
                    return false;
                entry = _entries[entry.Next];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetHashCode(string key)
        {
            return key.GetHashCode() & 0x7fffffff;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Resize()
        {
            var newCapacity = Primes[_primeIndex++];
            _bucket = new int[newCapacity];
            for (var i = 0; i < newCapacity; i++)
                _bucket[i] = -1;
            var newEntries = new Entry[newCapacity];
            for (var i = 0; i < _capacity; i++)
            {
                if (_entries[i].HashCode < 0)
                    continue;
                var index = _entries[i].HashCode % newCapacity;
                (newEntries[i] = _entries[i]).Next = _bucket[index];
                _bucket[index] = i;
            }
            _entries = newEntries;
            _capacity = newCapacity;
        }

        public class Enumerator
        {
            private readonly Entry[] _entries;
            private readonly int _count;
            private int _position = -1;
            private KeyValuePair<string, InternalObject> _current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(JsonDictionary dict)
            {
                _entries = dict._entries;
                _count = dict._count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                while (true)
                {
                    _position++;
                    if (_position == _count)
                        return false;
                    var entry = _entries[_position];
                    if (entry.HashCode < 0)
                        continue;
                    _current = new KeyValuePair<string, InternalObject>(entry.Key, entry.Value);
                    return true;
                }
            }

            public KeyValuePair<string, InternalObject> Current {
            	get{return _current;}
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public IEnumerable<KeyValuePair<string, InternalObject>> GetEnumerable()
            {
                while (MoveNext())
                    yield return _current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}