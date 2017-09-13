using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formel
{
    public class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private List<T> Queue { get; set; } = new List<T>();

        private bool sorted = false;
        private int currentIndex = 0;

        public int Length => Queue.Count - currentIndex;

        public void Push(T value)
        {
            if (Queue.Any() && value.CompareTo(Queue.Last()) < 0)
            {
                sorted = false;
            }
            Queue.Add(value);
        }

        public T Pop()
        {
            if (!Queue.Any())
            {
                throw new InvalidOperationException("no item left in queue!");
            }

            if (!sorted)
            {
                if(currentIndex > 0)
                {
                    Queue = Queue.Skip(currentIndex).ToList();
                }
                Queue = Queue.OrderByDescending(x => x).ToList();
                currentIndex = 0;
            }

            var item = Queue[currentIndex++];
            return item;
        }

        public T Peek()
        {
            if (!Queue.Any())
            {
                throw new InvalidOperationException("no item left in queue!");
            }

            if (!sorted)
            {
                if (currentIndex > 0)
                {
                    Queue = Queue.Skip(currentIndex).ToList();
                }
                Queue = Queue.OrderByDescending(x => x).ToList();
                currentIndex = 0;
            }

            return Queue[currentIndex];
        }

        public IEnumerator<T> GetEnumerator()
        {
            while(Length > 0)
            {
                yield return Pop();
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
