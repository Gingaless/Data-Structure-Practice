using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Exercise
{
    public class SimpleLinkedListBasedStack<T>
    {
        private Node<T> _head;
        private int _count;

        public T Peek {get { return _head._data; } }
        public int Count { get { return _count; } }
        //public Node<T> Head { get { return _head; }  }
        //Inconsistent accessibility : internal class Node가 property Head보다 접근성이 낮기 때문에 컴파일 에러가 생긴다.

        public SimpleLinkedListBasedStack() { _head = null; _count = 0; }

        public SimpleLinkedListBasedStack(T data)
        {
            _head = new Node<T>(data, null);
            _count = 1;
        }

        public Boolean IsEmpty()
        {
            return (_head==null);
        }

        public void Push(T data)
        {
            Node<T> buf = _head;
            _head = new Node<T>(data,buf);
            _count++;
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new EmptyStackException();

            T buf = _head._data;
            _head = _head._next;
            _count--;
            return buf;
        }

        internal class Node<T>
        {
            internal T _data;
            internal Node<T> _next;

            internal Node() : this(default(T), null) { }

            internal Node(T data, Node<T> next_node)
            {
                _data = data;
                _next = next_node;
            }
        }
    }

    public class EmptyStackException : Exception
    {
        public EmptyStackException() : base("The stack is empty.") { }
    }
}
