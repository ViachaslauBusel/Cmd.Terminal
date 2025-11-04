using Cmd.Terminal.Debugger.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal
{
    internal struct BlockingEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private object m_locker;
        private LinkedListNode<T> m_node;

        internal BlockingEnumerator(LinkedList<T> colliction, object locker)
        {
            m_node = colliction.First;
            m_locker = locker;
            Monitor.Enter(m_locker);
        }

        public T Current
        {
            get
            {
                T value = m_node.Value;
                m_node = m_node.Next;
                return value;
            }
        }
        object IEnumerator.Current => Current;

        public bool MoveNext() => m_node != null;

        public void Reset()
        {

        }

        public void Dispose()
        {
            Monitor.Exit(m_locker);
        }

        public IEnumerator<T> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
