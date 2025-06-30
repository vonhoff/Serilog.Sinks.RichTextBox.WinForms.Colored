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
        private int _clearIndex;

        public ConcurrentCircularBuffer(int capacity)
        {
            _capacity = capacity > 0 ? capacity : 1;
            _buffer = new T[_capacity];
            _clearIndex = 0;
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

                var startIndex = _clearIndex;
                var itemsToShow = _count - startIndex;
                
                for (var i = startIndex; i < _count; ++i)
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

        public void Clear()
        {
            lock (_sync)
            {
                _clearIndex = _count;
                if (_count == _capacity)
                {
                    _head  = 0;
                    _count = 0;
                    _clearIndex = 0;
                }
            }
        }

        public void Restore()
        {
            lock (_sync)
            {
                _clearIndex = 0;
            }
        }
    }
}
