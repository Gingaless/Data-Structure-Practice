using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Structure_Exercise;

namespace Data_Structure_Exercise
{

    using SDIX = SimpleDoubleIndexer; //DoubleIndexer을 왜 지원 안 하는건데 마이크로소프트 개 씨발놈들아!!!!!!
    using SOpr = SimpleStackCalculator.SimpleBinaryOperator;
    //using SBrk = SimpleStackCalculator.SimpleBracket;
    using SOBrk = SimpleStackCalculator.SimpleOpenBracket;
    using SCBrk = SimpleStackCalculator.SimpleCloseBracket;
    //using SPO = SimpleStackCalculator.PriorityOfOperator;
    using OpStk = SimpleLinkedListBasedStack<ISimplyOperatable>;

    public abstract class ISimplyOperatable
    { public int priority; public char expr; public override string ToString() { return expr.ToString(); } }

    public class SimpleDoubleIndexer { public double a; public double b; }


    public static class SimpleStackCalculator
    {
        public delegate double SimpleOperator(double a, double b);


        public class SimpleBinaryOperator : ISimplyOperatable
        {
            public SimpleOperator oprt;
            public double this[SDIX index] { get { return oprt(index.a, index.b); } }

            public SimpleBinaryOperator(SimpleOperator sopr, int priority, char expr)
            {
                oprt = sopr; base.priority = priority; base.expr = expr;
            }
        }

        public abstract class SimpleBracket : ISimplyOperatable
        {
            public SimpleBracket(char expr) { base.priority = 0; this.expr = expr; }
        }

        public class SimpleOpenBracket : SimpleBracket
        {
            public SimpleOpenBracket() : base('(') { }
        }

        public class SimpleCloseBracket : SimpleBracket
        {
            public SimpleCloseBracket() : base(')') { }
        }


        public static readonly SOpr sum = new SOpr((double a, double b) => { return a + b; }, 1, '+');
        public static readonly SOpr subtraction = new SOpr((double a, double b) => { return a - b; }, 1, '-');
        public static readonly SOpr product = new SOpr((double a, double b) => { return a * b; }, 2, '*');
        public static readonly SOpr division = new SOpr((double a, double b) => { return a / b; }, 2, '/');
        public static readonly SOBrk brkopen = new SOBrk();
        public static readonly SCBrk brkclose = new SCBrk();
        public static readonly ISimplyOperatable[] lso = {sum, subtraction, product, division, brkopen, brkclose };

        //public enum PriorityOfOperator {brkopen = 0, brkclose = 0, sum = 1, subtraction = 1, product = 2, division = 2 }

        //...존나 쓸데없이 구조를 복잡하게 짠 것 같다?

        public static ArrayList ConvertInfixToPosfixTest(String infix)
        {
            OpStk oprstk = new OpStk();
            ArrayList posfix = new ArrayList();
            StringBuilder buf1 = new StringBuilder();
            double buf2 = 0;

            foreach (char c in infix)
            {
                double buf3;
                bool success = double.TryParse(c.ToString(), out buf3);
                if (success) { buf2 = buf2*10 + buf3; continue; }
                else
                {
                    if (buf2 > 0)
                    { posfix.Add(buf2); buf2 = 0; }
                    
                    
                    foreach (ISimplyOperatable op in lso)
                    {
                        if (c == op.expr) { PutOprToStk(ref oprstk, ref posfix, op);  break; }
                    }
                    /*
                switch (c)
                {
                    case ('+'):
                        break;
                    case '-':
                        break;
                    case '*':
                        break;
                    case '/':
                        break;
                    case '(':
                        break;
                    case ')':
                        break;
                    case ((char)(32)):
                        break;
                    default:
                        throw new Exception("니 계산기에 이상한 문자 넣었죠 쉬팔롬아");
                }
                */
                }
                /*
                try
                {
                    lnum.Add(double.Parse(c.ToString()));
                }
                catch(FormatException e)
                {
                    switch(c)
                    {
                        case '+':
                            break;
                        case '-':
                            break;
                        case '*':
                            break;
                        case '/':
                            break;
                        case ((char)(32)):
                            break;
                        default:
                            throw e;
                    }
                }
                */
            }
            if (buf2 > 0) posfix.Add(buf2);
            while (!(oprstk.IsEmpty())) posfix.Add(oprstk.Pop());
            return posfix;
        }

        static void PutOprToStk(ref OpStk stk,ref ArrayList posfix,ISimplyOperatable op)
        {
            if (stk.IsEmpty()) { stk.Push(op); return; }
            // + () 이렇게 괄호가 뒤에 나올 경우에는 괄호의 우선순위를 최하로 하는 방법이 통용되지 않잖아여
            while (!(stk.IsEmpty()))
            {
                if (op is SOBrk) break;
                if ((!(stk.Peek is SCBrk)) && (stk.Peek is SOBrk)) break;
                if (op.priority <= stk.Peek.priority)
                 posfix.Add(stk.Pop());
                else
                    break;
            }
            if (op is SCBrk) return;
            stk.Push(op);
        }
        
        static public SimpleBinaryTreeNode MakeExpTree(ArrayList posfix)
        {
            if (posfix.Count <= 0) throw new ArgumentNullException("The array list for posfix expression is empty.");
            var stack = new SimpleLinkedListBasedStack<SimpleBinaryTreeNode>();
            int pl = posfix.Count;
            foreach (dynamic e in posfix)
            {
                if (e is double) stack.Push(new SimpleBinaryTreeNode(e));
                else if (e is SOpr)
                {
                    if (stack.Count < 2) throw new ArithmeticException("At least two numbers are needed to make a expression subtree.");
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(new SimpleBinaryTreeNode(e,left,right));
                }
            }
            return stack.Pop();
        }

        static public double EvaluateExpTreeP(SimpleBinaryTreeNode rtnd)
        {
            var numbuf = new SimpleLinkedListBasedStack<double>();
            ArrayList trvlist = new ArrayList();
            SimpleBinaryTreeNode.Traverse(ref trvlist, rtnd, SimpleBinaryTreeNode.PostOrder,
                (SimpleBinaryTreeNode nd)=>{
                    if ((nd.Data is SOpr) && (nd.Left.DataType == typeof(double)) && (nd.Right.DataType == typeof(double)))
                    {
                        double a = (double)trvlist[trvlist.Count-2];//stack 쓰는게 더 적절하지 않았겠냐..? 아 trvlist의 작성이...
                        double b = (double)trvlist[trvlist.Count-1];
                        double r = ((SOpr)(nd.Data)).oprt.Invoke(a,b);
                        nd.Set(r);
                        trvlist.RemoveAt(trvlist.Count-1);
                        trvlist.RemoveAt(trvlist.Count - 1);
                    } });
            return (double)trvlist[0];
        }//...책에 있는 방식이 더 간단한데? 애미 ㅋㅋㅋㅋ
        //아래가 책에 나와있는 방식
        static public double EvaluateExpTreeP2(SimpleBinaryTreeNode rtnd)
        {
            double op1 = ((!(rtnd.Left.IsTerminal())) ? EvaluateExpTreeP2(rtnd.Left) : rtnd.Left.Data);
            double op2 = ((!(rtnd.Right.IsTerminal())) ? EvaluateExpTreeP2(rtnd.Right) : rtnd.Right.Data);

            return ((SOpr)(rtnd.Data)).oprt.Invoke(op1, op2);
        }
    }
}
