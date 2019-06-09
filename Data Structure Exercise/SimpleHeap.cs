using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Data_Structure_Exercise
{
    class SimpleHeap<T> where T : IComparable<T>
    {
        const int default_length = 1024;

        public delegate int PriorityTest(T a, T b);//b보다 우선순위가 낮으면 -1, b와 우선순위가 같으면 0, b와 우선순위가 높으면 1.

        private PriorityTest _prio = null;
        private T[] _hdata;
        private int _hleng;
        private int _count;

        public int Count { get { return _count; } }
        public int MaxLen { get { return _hleng; } }
        public bool IsEmpty { get { return (_count == 0); } }

        public T this[int i]
        { get { if (i > _count) throw new SimpleHeapException("The index is out of range.", new ArgumentOutOfRangeException());
                else return _hdata[i]; } }

        public SimpleHeap(int maxlength, PriorityTest prio)
        {
            _hleng = maxlength + 1;
            _hdata = new T[_hleng];
            _hdata[0] = default(T);
            _prio = prio;
            _count = 0;
        }

        public SimpleHeap(int maxlength) : this(maxlength,(T a, T b)=> { return a.CompareTo(b); }) { }
        public SimpleHeap(PriorityTest prio) : this(default_length, prio) { }
        public SimpleHeap() : this(default_length) { }

        private int GetParentIDX(int idx)
        { if (idx == 1) throw new SimpleHeapException("The Root Node has no parent node.");
            if ( idx > MaxLen || idx < 1)
            { var buf = new ArgumentOutOfRangeException(); throw new SimpleHeapException(buf.Message, buf); }
            return idx / 2; }
        private int GetLeftIDX(int idx)
        { if (2 * idx > _count)
            { var buf = new ArgumentOutOfRangeException(); throw new SimpleHeapException(buf.Message, buf); }
            return 2 * idx; }
        private int GetRightIDX(int idx) {if (2 * idx + 1 > _count)
            { var buf = new ArgumentOutOfRangeException(); throw new SimpleHeapException(buf.Message, buf); }
            return 2 * idx + 1;}

        private bool IsTerminalIDX(int idx)
        {
            return (2 * idx > _count && _count <= idx);
        }
        
        int GetHiPriChildIDX(int idx)
        {

            int left = GetLeftIDX(idx);
            int right = GetRightIDX(idx);

            if (_hdata[left].CompareTo(_hdata[right]) < 0) return left; else return right;
        }

        public void HInsert(T data)
        {
            if (_hleng<=(_count+1)) { var buf = new ArgumentOutOfRangeException(); throw new SimpleHeapException(buf.Message, buf); }
            int idx = _count + 1;
            _hdata[idx] = data;
            try
            {
                int prntidx = GetParentIDX(idx);
                T buf = _hdata[prntidx];
                while (_prio(data,buf) < 0)
                {
                    _hdata[prntidx] = data;
                    _hdata[idx] = buf;
                    idx = prntidx;
                    prntidx = GetParentIDX(idx);
                    buf = _hdata[prntidx];
                }
            }//먼데 시발.
            catch (SimpleHeapException) { if (idx <= 1)  _hdata[idx] = data; }
            _count++;
        }
        

        // Temperature otherTemperature = obj as Temperature;

        public T HDelete()
        {
            if (IsEmpty) throw new SimpleHeapException("The heap is empty.",new ArgumentNullException());

            int idx = 1;
            T hbuf = _hdata[1];
            T last = _hdata[_count];
            _hdata[idx] = last;

            int prichld = 1;
            T buf = _hdata[prichld];
            while (_prio(last,buf) >= 0)
            {
                if (2 * idx + 1 > _count) break;
                prichld = GetHiPriChildIDX(idx);
                buf = _hdata[prichld];
                _hdata[prichld] = last;
                _hdata[idx] = buf;
                idx = prichld;
            }
            _hdata[_count] = default(T);
            _count--;
            return hbuf;
        }

        public override string ToString()
        {
            if (IsEmpty) return "";
            StringBuilder sb = new StringBuilder();
            int h = 1;
            for (int i=1; i<=_count;i++)
            {
                if (i >= (int)Math.Pow(2, h))
                { sb.Append("\n"); h++; }
                sb = sb.Append(_hdata[i].ToString());
                sb = sb.Append(" ");
            }
            return sb.ToString();
        }
    }

    public class SimpleHeapException : Exception
    {
        public SimpleHeapException(string message) : base(message) { }

        public SimpleHeapException(string message, Exception innerException) : base(message, innerException) { }

        public class TerminalNodeException : SimpleHeapException
        {
            public TerminalNodeException() : base("You tried an process that cannot use terminal node.") { }
        }
    }

}

