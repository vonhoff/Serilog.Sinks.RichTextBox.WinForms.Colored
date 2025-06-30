using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Collections
{
    public sealed class ConcurrentCircularBuffer<T>
    {
        private readonly object _sync = new();
        private readonly T[] _buffer;
        private readonly int _capacity;
        private int _head;
        private int _count;
        private int _itemsToSkip;

        public ConcurrentCircularBuffer(int capacity)
        {
            _capacity = capacity > 0 ? capacity : 1;
            _buffer = new T[_capacity];
            _itemsToSkip = 0;
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

                    if (_itemsToSkip > 0)
                    {
                        _itemsToSkip--;
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

                var itemsToTake = _count - _itemsToSkip;
                if (itemsToTake <= 0)
                {
                    return;
                }

                for (var i = 0; i < itemsToTake; ++i)
                {
                    var index = _head + _itemsToSkip + i;
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
                _itemsToSkip = _count;
            }
        }

        public void Restore()
        {
            lock (_sync)
            {
                _itemsToSkip = 0;
            }
        }
    }
}
