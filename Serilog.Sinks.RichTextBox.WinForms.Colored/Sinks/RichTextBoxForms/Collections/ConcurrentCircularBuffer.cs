using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Collections
{
    internal sealed class ConcurrentCircularBuffer<T>
    {
        private readonly object _sync = new();
        private readonly T[] _buffer;
        private readonly int _capacity;
        private int _head;
        private int _count;

        public ConcurrentCircularBuffer(int capacity)
        {
            _capacity = capacity > 0 ? capacity : 1;
            _buffer = new T[_capacity];
        }

        public void Add(T item)
        {
            lock (_sync)
            {
                var tail = _head + _count;
                if (tail >= _capacity)
                {
                    tail -= _capacity;
                }

                _buffer[tail] = item;
                if (_count == _capacity)
                {
                    if (++_head == _capacity)
                    {
                        _head = 0;
                    }
                }
                else
                {
                    ++_count;
                }
            }
        }

        public void TakeSnapshot(List<T> target)
        {
            lock (_sync)
            {
                target.Clear();

                for (var i = 0; i < _count; ++i)
                {
                    var index = _head + i;
                    if (index >= _capacity)
                    {
                        index -= _capacity;
                    }

                    target.Add(_buffer[index]);
                }
            }
        }
    }
}
