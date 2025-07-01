#region Copyright 2025 Simon Vonhoff & Contributors

//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

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