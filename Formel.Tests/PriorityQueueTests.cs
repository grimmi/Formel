using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Formel.Tests
{
    public class PriorityQueueTests
    {
        [Fact]
        public void Insert_OneItem_LengthShouldBeOne()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);

            Assert.Equal(1, queue.Length);
        }

        [Fact]
        public void Insert_TwoItems_LengthShouldBeTwo()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(2);

            Assert.Equal(2, queue.Length);
        }

        [Fact]
        public void Pop_TwoItemsInserted_LengthShouldBeOne()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(2);
            var item = queue.Pop();

            Assert.Equal(1, queue.Length);
        }

        [Fact]
        public void Pop_TwoItemsInsertedInOrder_TheBiggerShouldBeReturned()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(2);
            var item = queue.Pop();

            Assert.Equal(2, item);
        }

        [Fact]
        public void Pop_PopTwiceAfterInsertingTwoItemsInOrder_ShouldReturnItemsInDescendingOrder()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(2);

            var item1 = queue.Pop();
            var item2 = queue.Pop();

            Assert.Equal(2, item1);
            Assert.Equal(1, item2);
        }

        [Fact]
        public void Pop_ItemsInsertedInDescendingOrder_ShouldBeReturnedInSameOrder()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(2);
            queue.Push(1);

            var item1 = queue.Pop();
            var item2 = queue.Pop();

            Assert.Equal(2, item1);
            Assert.Equal(1, item2);
        }

        [Fact]
        public void Insert_InsertingItemsInMixedOrder_ShouldReturnInDescendingOrder()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(3);
            queue.Push(2);

            Assert.Equal(3, queue.Length);

            var i1 = queue.Pop();
            var i2 = queue.Pop();
            var i3 = queue.Pop();

            Assert.Equal(3, i1);
            Assert.Equal(2, i2);
            Assert.Equal(1, i3);
        }

        [Fact]
        public void Enumerate_InsertingThreeItemsInMixedOrder_AreReturnedInDescendingOrder()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(10);
            queue.Push(5);
            queue.Push(20);

            var expected = new[] { 20, 10, 5 };
            var counter = 0;
            foreach(var item in queue)
            {
                Assert.Equal(expected[counter++], item);
            }
        }

        [Fact]
        public void Pop_InsertingAfterPoppingFirstElement_ShouldReturnTheRestInDescendingOrder()
        {
            var queue = new PriorityQueue<int>();
            queue.Push(30);
            queue.Push(20);
            queue.Push(10);

            var i1 = queue.Pop();
            Assert.Equal(30, i1);
            Assert.Equal(2, queue.Length);

            queue.Push(25);
            Assert.Equal(3, queue.Length);

            var i2 = queue.Pop();
            var i3 = queue.Pop();
            var i4 = queue.Pop();
            Assert.Equal(25, i2);
            Assert.Equal(20, i3);
            Assert.Equal(10, i4);
        }
    }
}
