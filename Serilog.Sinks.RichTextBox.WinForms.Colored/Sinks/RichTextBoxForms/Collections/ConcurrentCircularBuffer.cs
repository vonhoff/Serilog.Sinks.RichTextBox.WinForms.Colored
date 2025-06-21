using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Collections
{
    /// <summary>
    /// Thread-safe circular buffer that overwrites the oldest element when capacity is exceeded.
    /// Optimized for the pattern «many producers / single consumer snapshot». Add() never blocks; the
    /// internal array size is fixed (<see cref="Capacity"/>). Snapshot() returns a snapshot of the
    /// current content in chronological order (oldest → newest).
    /// </summary>
    internal sealed class ConcurrentCircularBuffer<T>
    {
        private readonly T[] _buffer;
        private readonly int _capacity;

        // _head points to the index of the oldest entry. _count is the number of valid items.
        private int _head;
        private int _count;

        private readonly object _sync = new();

        public ConcurrentCircularBuffer(int capacity)
        {
            _capacity = capacity > 0 ? capacity : 1;
            _buffer = new T[_capacity];
            _head = 0;
            _count = 0;
        }

        public int Capacity => _capacity;

        public int Count
        {
            get
            {
                lock (_sync)
                {
                    return _count;
                }
            }
        }

        /// <summary>
        /// Adds an item to the buffer, overwriting the oldest element when the buffer is full.
        /// </summary>
        public void Add(T item)
        {
            lock (_sync)
            {
                var tail = (_head + _count) % _capacity;
                _buffer[tail] = item;

                if (_count == _capacity)
                {
                    _head = (_head + 1) % _capacity;
                }
                else
                {
                    _count++;
                }
            }
        }

        /// <summary>
        /// Fills the provided list with a snapshot of the current content in chronological order.
        /// The list is cleared before being filled.
        /// </summary>
        public void TakeSnapshot(List<T> target)
        {
            lock (_sync)
            {
                target.Clear();
                target.Capacity = _count;

                for (var i = 0; i < _count; i++)
                {
                    target.Add(_buffer[(_head + i) % _capacity]);
                }
            }
        }
    }
}