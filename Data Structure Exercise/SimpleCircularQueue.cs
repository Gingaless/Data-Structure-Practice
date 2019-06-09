using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Structure_Exercise;
using qState = Data_Structure_Exercise.SCQException.qState;

namespace Data_Structure_Exercise
{
    public class SimpleCircularQueue<T>
    {
        const int basiclength = 4;
        private int F;//front index
        private int R;//rear index
        private int leng;//maximun length;
        private int count;//현재 채워진 양
        private T[] data;

        public int Count { get { return this.count; } }
        public int Length { get { return this.leng; } }
        
        public SimpleCircularQueue(int leng)
        {
            this.count = 0;
            this.leng = leng;
            this.F = 0;
            this.R = 0;
            data = new T[leng+1];
        }

        public SimpleCircularQueue() : this(basiclength) { }

        private int NextIdx(int i) { if (i >= leng) return 0; else return i+1; }

        public Boolean IsEmpty() { if (F == R) return true; else return false; }

        public Boolean IsFull() { if (NextIdx(R) == F) return true; else return false; }

        public void enQueue(T input)
        {
            if (IsFull()) throw new SCQException(qState.full);
            R = NextIdx(R);
            data[R] = input;
            this.count++;
        }

        public T deQueue()
        {
            if (IsEmpty()) throw new SCQException(qState.empty);
            F = NextIdx(F);
            count--;
            return data[F];
        }

        public T Qpeek()
        {
            if (IsEmpty()) throw new SCQException(qState.empty);
            return data[NextIdx(F)];
        }
    }

    public class SCQException : Exception
    {
        public enum qState
        {
            [EnumString("empty")]
            empty,
            [EnumString("full")]
            full
        }
        public qState state;
        public SCQException(qState state) : base(String.Join("The queue is {0}", StringEnum.GetStringValue(state))) { this.state = state; }
    }
}