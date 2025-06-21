using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Collections
{
    internal sealed class ConcurrentCircularBuffer<T>
    {
        private readonly T[] _buffer;
        private readonly int _capacity;
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