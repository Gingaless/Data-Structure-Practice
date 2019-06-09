using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Data_Structure_Exercise
{
    /*
    public class SimpleBinaryTreeNode<T>
    {
        private T _data;
        private SimpleBinaryTreeNode<T> _left = null;
        private SimpleBinaryTreeNode<T> _right = null;

        public T Data { get { return _data; } set { _data = value; } }
        public SimpleBinaryTreeNode<T> Left { get { if (_left != null) return _left; else throw new SBTNodeEmptyException(false); } set { _left = value; } }
        public SimpleBinaryTreeNode<T> Right { get { if (_right == null) throw new SBTNodeEmptyException(true); else return _right; } set { _right = value; } }

        public SimpleBinaryTreeNode (T data, SimpleBinaryTreeNode<T> left, SimpleBinaryTreeNode<T> right)
        { _data = data; _left = left; _right = right; }

        public SimpleBinaryTreeNode(T data, SimpleBinaryTreeNode<T> left) : this(data, left, null) { }
        public SimpleBinaryTreeNode(T data) : this(data, null, null){ }

        //overloading : 한 클래스 내부에서의 여러 type으로의 method 선언
        //override : 자식 클래스가 부모 클래스의 method를 재정의하는 것.


        public override string ToString()
        {
            return _data.ToString();
        }
    }
    애초에 복잡한 연산을 수행해야 하는 tree node에 쓸데없이 정적인 generic을 넣은 것이 문제였다... 니미샹
    */

    public class SimpleBinaryTreeNode
    {

        public delegate void Ordering(ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order, TraverseEvent act, NodeEventArgs args);

        public delegate void OrderAction(SimpleBinaryTreeNode node);

        public delegate void TraverseEvent(SimpleBinaryTreeNode node, NodeEventArgs args);

        public readonly static Ordering PreOrder;
        public readonly static Ordering InOrder;
        public readonly static Ordering PostOrder;

        static SimpleBinaryTreeNode()
        {
            //Ordering emp = ((ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order, OrderAction act) => { if (rtnd.IsTerminal()) trvlst.Add(rtnd.Data); });

            Ordering c = ((ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order,TraverseEvent act, NodeEventArgs args) =>
            { if (act != null) act.Invoke(rtnd,args); trvlst.Add(rtnd.Data); });
            //슈ㅣ바... 람다식 써서 편하게 해보려다가 괜히 더 복잡해지고 중복되는 코드가 생겨난 거 같네 애미

            Ordering l = ((ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order, TraverseEvent act, NodeEventArgs args) => 
            { if (!(rtnd.IsLeftEmpty())) Traverse(ref trvlst, rtnd.Left, order,act, args); });

            Ordering r = ((ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order, TraverseEvent act, NodeEventArgs args) => 
            { if (!(rtnd.IsRightEmpty())) Traverse(ref trvlst, rtnd.Right,order,act,args); });

            PreOrder += c;
            PreOrder += l;
            PreOrder += r;

            InOrder += l;
            InOrder += c;
            InOrder += r;

            PostOrder += l;
            PostOrder += r;
            PostOrder += c;

            //...머여 이거 뭔가 카드 돌려막기처럼 되어버렸는데. 제대로 작동하나 이거?
        }



        private object _data;
        private Type _dtype = typeof(double);
        private SimpleBinaryTreeNode _left = null;
        private SimpleBinaryTreeNode _right = null;

        

        public dynamic Data
        { get { return Convert.ChangeType(_data,_dtype); }
            set { if (value.GetType() != _dtype) throw new InvalidCastException(); _data = value; } }

        public SimpleBinaryTreeNode Left { get { return _left; }
            set { _left = value; } }

        public SimpleBinaryTreeNode Right { get { return _right; }
            set { _right = value; } }

        public Type DataType { get { return _dtype; } }

        public int Height { get { return this.GetHeight(); } }

        public SimpleBinaryTreeNode(Type type, object data, SimpleBinaryTreeNode left, SimpleBinaryTreeNode right)
        { _dtype = type; _data = data; _left = left; _right = right; }

        public SimpleBinaryTreeNode(dynamic data, SimpleBinaryTreeNode left, SimpleBinaryTreeNode right) : this((Type)data.GetType(), (object)data, left, right){ }
        public SimpleBinaryTreeNode(dynamic data) : this((Type)data.GetType(),(object)data,null,null) { }

        public SimpleBinaryTreeNode(Type type, object data, SimpleBinaryTreeNode left) : this(type, data ,left, null) { }
        public SimpleBinaryTreeNode(Type type, object data) : this(type, data, null, null) { }

        //overloading : 한 클래스 내부에서의 여러 type으로의 method 선언
        //override : 자식 클래스가 부모 클래스의 method를 재정의하는 것.

        public bool IsLeftEmpty() { return (_left==null); }
        public bool IsRightEmpty() { return (_right==null); }
        public bool IsTerminal() { return (IsLeftEmpty() && IsRightEmpty()); }

        public static void Traverse(ref ArrayList trvlst, SimpleBinaryTreeNode rtnd,Ordering order, TraverseEvent act,NodeEventArgs args)//trvlist = traversal list, rtnd = root node.
        {
            order.Invoke(ref trvlst, rtnd, order, act,args);
        }

        public static void Traverse(ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order, OrderAction act)
        {
            order.Invoke(ref trvlst, rtnd, order, (SimpleBinaryTreeNode nd, NodeEventArgs args)=>{ act.Invoke(nd); },null);
        }

        public static void Traverse(ref ArrayList trvlst, SimpleBinaryTreeNode rtnd, Ordering order)
        {
            order.Invoke(ref trvlst, rtnd, order, null,null);
        }


        public ArrayList Traverse(Ordering order, TraverseEvent act, NodeEventArgs args)//trvlist = traversal list, rtnd = root node.
        {
            ArrayList trvlst = new ArrayList();
            SimpleBinaryTreeNode.Traverse(ref trvlst, this, order, act, args);
            return trvlst;
        }

        public ArrayList Traverse(Ordering order, OrderAction act)
        {
            return Traverse(order,(SimpleBinaryTreeNode rtnd, NodeEventArgs args)=> { act.Invoke(rtnd); },null);
        }

        public ArrayList Traverse(Ordering order)
        {
            ArrayList trvlst = new ArrayList();
            SimpleBinaryTreeNode.Traverse(ref trvlst, this, order, null);
            return trvlst;
        }

        public SimpleBinaryTreeNode RemoveLeftSubtree()
        {
            if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);
            SimpleBinaryTreeNode buf = _left;
            _left = null;
            return buf;
        }

        public SimpleBinaryTreeNode RemoveRightSubtree()
        {
            if (IsRightEmpty()) throw new SBTNodeEmptyException(true);
            SimpleBinaryTreeNode buf = _right;
            _right = null;
            return buf;
        }

        public void ChangeLeftSubtree(SimpleBinaryTreeNode substitute)
        {
            //if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);
            _left = substitute;
        }

        public void ChangeRightSubtree(SimpleBinaryTreeNode substitute)
        {
            //if (IsRightEmpty()) throw new SBTNodeEmptyException(true);
            _right = substitute;
        }

        public void Dispose()
        {
            _data = null;
            _left = null;
            _right = null;
            _dtype = null;
        }

        public void Set(dynamic data)
        {
            _dtype = data.GetType();
            _data = data;
        }

        private int GetHeight()
        {
            int leftH;
            int rightH;

            leftH = (this.IsLeftEmpty()) ? -1 : this.Left.GetHeight();
            rightH = (this.IsRightEmpty()) ? -1 : this.Right.GetHeight();

            if (leftH > rightH)
                return leftH + 1;
            else
                return rightH + 1;
        }

        public int GetHeightDiff()
        {
            int leftH = (this.IsLeftEmpty()) ? -1 : this.Left.GetHeight();
            int rightH = (this.IsRightEmpty()) ? -1 : this.Right.GetHeight();

            return leftH - rightH;
        }

        public void ShowVData()
        {
            List<VData> vdl = new List<VData>();

            int cont_depth = 0;
            int cont_rel = 0;

            this.BuildVData(vdl, 0, 0, ref cont_depth, ref cont_rel);

            foreach (VData vd in vdl)
                Console.WriteLine("{0} : (depth = {1}), (rel = {2})", vd.str, vd.depth, vd.rel);
        }

        private void BuildVData(List<VData> vdl, int depth, int rel, ref int cont_depth, ref int cont_rel)//rel = right edge length.
        {
            if (cont_depth > depth)
            {
                vdl.Add(new VData(String.Format("({0})", this.Data.ToString()), depth, ++cont_rel));
            }
            else
            {
                vdl.Add(new VData(String.Format("({0})", this.Data.ToString()), depth, rel));
                cont_rel = rel;
            }
            cont_depth = depth;

            if (!(this.IsLeftEmpty())) this.Left.BuildVData(vdl, depth + 1, cont_rel, ref cont_depth, ref cont_rel);

            if (!(this.IsRightEmpty())) this.Right.BuildVData(vdl, depth + 1, cont_rel + 1, ref cont_depth, ref cont_rel);
        }

        public override string ToString()
        {
            return _data.ToString();
        }

        public class NodeEventArgs
        {
            protected object _args = null;
            private Type _argtype = null;
            public dynamic Args { get { return Convert.ChangeType(_args, _argtype); } }
            public Type ArgType { get { return _argtype; } }
            public NodeEventArgs() { }
            public NodeEventArgs(dynamic args) { _args = args; _argtype = args.GetType(); }
        }

        public class VData
        {
            public int depth;
            public int rel;
            public string str;

            public VData(string str, int depth, int rel) { this.str = str; this.depth = depth; this.rel = rel; }
        }

    }

    public class SBTNodeEmptyException : Exception//false면 left, true이면 right
    {
        public SBTNodeEmptyException(bool dir) : base((!dir) ? "The left child node is empty set node." : "The right child node is empty set node.") {}
    }
}

//condition ? consequence : alternative