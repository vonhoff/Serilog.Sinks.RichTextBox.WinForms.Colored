using Serilog.Sinks.RichTextBoxForms.Collections;
using Xunit;

namespace Serilog.Tests.Collections
{
    public class ConcurrentCircularBufferTests
    {
        [Fact]
        public void TakeSnapshot_WithItemsLessThanCapacity_ReturnsAllItemsInOrder()
        {
            var buffer = new ConcurrentCircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);

            var snapshot = new List<int>();
            buffer.TakeSnapshot(snapshot);

            Assert.Equal(new[] { 1, 2 }, snapshot);
        }

        [Fact]
        public void AddBeyondCapacity_OverwritesOldestItemsAndMaintainsOrder()
        {
            var buffer = new ConcurrentCircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);
            buffer.Add(4); // Should overwrite the oldest item (1)

            var snapshot = new List<int>();
            buffer.TakeSnapshot(snapshot);

            Assert.Equal(new[] { 2, 3, 4 }, snapshot);
        }

        [Fact]
        public void Clear_FollowedByAdds_SnapshotContainsOnlyNewItems()
        {
            var buffer = new ConcurrentCircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);

            // After clear, snapshot should be empty
            buffer.Clear();
            var snapshotAfterClear = new List<int>();
            buffer.TakeSnapshot(snapshotAfterClear);
            Assert.Empty(snapshotAfterClear);

            // Add new item and verify snapshot contains only the new item
            buffer.Add(4);
            var snapshotAfterOneAdd = new List<int>();
            buffer.TakeSnapshot(snapshotAfterOneAdd);
            Assert.Equal(new[] { 4 }, snapshotAfterOneAdd);

            // Add two more items to fill the buffer again
            buffer.Add(5);
            buffer.Add(6);
            var snapshotAfterMoreAdds = new List<int>();
            buffer.TakeSnapshot(snapshotAfterMoreAdds);
            Assert.Equal(new[] { 4, 5, 6 }, snapshotAfterMoreAdds);
        }

        [Fact]
        public void Restore_AfterClear_ReturnsAllItemsAgain()
        {
            var buffer = new ConcurrentCircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);
            buffer.Clear();

            // Add two new items while buffer is in cleared state
            buffer.Add(4);
            buffer.Add(5);

            // Restore should make all items visible again
            buffer.Restore();
            var snapshot = new List<int>();
            buffer.TakeSnapshot(snapshot);

            Assert.Equal(new[] { 3, 4, 5 }, snapshot);
        }
    }
}