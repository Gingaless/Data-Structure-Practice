using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Exercise
{
    namespace SimpleSearch
    {
        public abstract  class SimpleSearch<T, U> where U : IComparable
        {
            public delegate Boolean Condition(T element, U skey);
            public Condition condition;
            public T[] data;
            public U skey;
            public Action<T> act;

            public SimpleSearch(T[] data, U skey, Action<T> act, Condition condition)
            {
                this.data = data;
                this.skey = skey;
                this.condition = condition;
                this.act = act;
            }

            public SimpleSearch(T[] data, U skey, Action<T> act) :
                this(data, skey, act, (T element, U key) => { if (key is T) { return element.Equals(key); } else return false; }) { }

            public abstract void Work();
        }

        public class InterpolationSearch : SimpleSearch<int,int>
        {
            public InterpolationSearch(int[] data, int skey, Action<int> act, Condition condition) : base(data, skey, act, condition) { }
            public delegate void SimpleAction(int data, int idx);

            public InterpolationSearch(int[] data, int skey, Action<int> act) :
                this(data,skey, act, (int element, int key)=> { return element==key; }) { }

            public override void Work()
            {
                try
                {
                    int idx = Search(0, data.Length-1);
                    act.Invoke(idx);
                }
                catch (NotFound) { Console.WriteLine("Not Found."); }
            }

            private int Search(int p, int r)
            {
                if ((data[p]>skey)||(data[r]<skey)) throw new NotFound();
                //탈출조건은 (p>r)가 아니다!
                int Q = skey;
                double QP = Q - data[p];
                double RP = data[r] - data[p];
                int round = (int)Math.Round((QP / RP) * (r - p));
                int q = round + p;
                if (Q < data[q])
                    return Search(p, q - 1);
                else if (Q > data[q])
                    return Search(q + 1, r);
                else return q;
            }
        }

        public class NotFound : Exception { }
    }
}
