using System;
using System.Runtime.CompilerServices;

namespace UyghurEditPP
{
	internal class TypeDictionary<T>
	{
		// ReSharper disable once StaticMemberInGenericType
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
		private int _count;

		private class Entry
		{
			public int HashCode;
			public Type Key;
			public int Next;
			public T Value;
		}

		public TypeDictionary()
		{
			_capacity = Primes[_primeIndex++];
			_bucket = new int[_capacity];
			for (var i = 0; i < _capacity; i++)
				_bucket[i] = -1;
			_entries = new Entry[_capacity];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Insert(Type key, T value)
		{
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
					return value;
				}
				var entry = _entries[index];
				if (entry.HashCode == hashCode && entry.Key == key)
				{
					entry.Value = value;
					return value;
				}
				index = entry.Next;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetValue(Type key, out T value)
		{
			value = default(T);
			var hashCode = GetHashCode(key);
			var hash = hashCode % _capacity;
			if (_bucket[hash] == -1){
				return false;
			}
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
		private int GetHashCode(Type key)
		{
			return key.GetHashCode() & 0x7fffffff;
		}

		private void Resize()
		{
			var newCapacity = Primes[_primeIndex++];
			_bucket = new int[newCapacity];
			for (var i = 0; i < newCapacity; i++)
				_bucket[i] = -1;
			Array.Resize(ref _entries, newCapacity);
			for (var i = 0; i < _capacity; i++)
			{
				var index = _entries[i].HashCode % newCapacity;
				_entries[i].Next = _bucket[index];
				_bucket[index] = i;
			}
			_capacity = newCapacity;
		}
	}
}