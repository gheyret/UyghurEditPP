using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UyghurEditPP
{
    internal class JsonArray
    {
        private InternalObject[] _array = new InternalObject[4];

        public int Count { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Add(InternalObject obj)
        {
            if (Count == _array.Length)
                Array.Resize(ref _array, _array.Length * 2);
            _array[Count++] = obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RemoveAt(int index)
        {
            Array.Copy(_array, index + 1, _array, index, --Count);
        }

        internal InternalObject this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                return _array[index];
            }
            set
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array."); // unreachable
                _array[index] = value;
            }
        }

        internal class Enumerator
        {
            private readonly InternalObject[] _array;
            private readonly int _count;
            private InternalObject _current;

            public int Position { get; private set; }  // = -1;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(JsonArray array)
            {
                _array = array._array;
                _count = array.Count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                Position++;
                if (Position == _count)
                    return false;
                _current = _array[Position];
                return true;
            }

            public InternalObject Current{
            	get{return _current;}
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public IEnumerable<InternalObject> GetEnumerable()
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