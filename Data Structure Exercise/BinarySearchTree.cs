using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Structure_Exercise;

namespace Data_Structure_Exercise
{
    namespace SimpleSearch
    {
        public class BinarySearchTree<T,U> where U : IComparable
        {
            public delegate U GetKey(T data);
            public GetKey _gkey;
            public BinarySearchTreeNode<T> _rtnd = null;

            public BinarySearchTree(BinarySearchTreeNode<T> rtnd, GetKey gkey) { _rtnd = rtnd; _gkey = gkey; }
            public BinarySearchTree(GetKey gkey) : this(null, gkey) { }

            public void Traverse(ref ArrayList trvlist, SimpleBinaryTreeNode.Ordering ordering, SimpleBinaryTreeNode.OrderAction orderAction)
            {
                SimpleBinaryTreeNode.Traverse(ref trvlist, _rtnd, ordering,orderAction);
            }

            public void Insert(T data)
            {
                BinarySearchTreeNode<T> new_node = new BinarySearchTreeNode<T>(data);
                if (_rtnd == null) { _rtnd = new_node; return; }
                BinarySearchTreeNode<T> parent = _rtnd;
                U dkey = _gkey(data);
                U pkey = _gkey(parent.Data);

                while(true)
                {
                    if (dkey.CompareTo(pkey) < 0)
                    {
                        if (parent.IsLeftEmpty()) { parent.Left = new_node; return; }
                        else { parent = parent.Left; pkey = _gkey(parent.Data); }
                    }
                    else if (dkey.CompareTo(pkey) > 0)
                    {
                        if (parent.IsRightEmpty()) { parent.Right = new_node; return; }
                        else { parent = parent.Right; pkey = _gkey(parent.Data); }
                    }
                    else { throw new KeyOverlapException(); }
                }
            }

            public BinarySearchTreeNode<T> Search(U targetkey)
            {
                BinarySearchTreeNode<T> parent = _rtnd;
                U pkey;
                while (parent != null)
                {
                    pkey = _gkey(parent.Data);
                    if (targetkey.CompareTo(pkey) < 0) parent = parent.Left;
                    else if (targetkey.CompareTo(pkey) > 0) parent = parent.Right;
                    else return parent;
                }
                throw new NotFound();
            }

            public pcnPair<T> SearchPair(U targetkey, BinarySearchTreeNode<T> vNode)
            {
                BinarySearchTreeNode<T> parent = vNode;
                BinarySearchTreeNode<T> child = _rtnd;
                U ckey;
                while (child != null)
                {
                    ckey = _gkey(child.Data);
                    if (targetkey.CompareTo(ckey) < 0) { parent = child; child = parent.Left; }
                    else if (targetkey.CompareTo(ckey) > 0) { parent = child; child = parent.Right; }
                    else
                    {
                        pcnPair<T> r = new pcnPair<T>(parent, child);
                        return r;
                    }
                }
                throw new NotFound();
            }

            public BinarySearchTreeNode<T> RemoveNode(U targetkey)
            {
                if (_rtnd == null) throw new NotFound();
                BinarySearchTreeNode<T> vNode = new BinarySearchTreeNode<T>(_rtnd.Data, null, _rtnd); //virtual node that is the parent of root node;
                pcnPair<T> delPair = SearchPair(targetkey, vNode);
                if (delPair.cNode.IsTerminal())
                {
                    if (delPair.pNode.Left == delPair.cNode)
                        return (BinarySearchTreeNode<T>)delPair.pNode.RemoveLeftSubtree();
                    else
                        return (BinarySearchTreeNode<T>)delPair.pNode.RemoveRightSubtree();
                }
                else if (delPair.cNode.IsLeftEmpty())
                {
                    delPair.cNode.Data = delPair.cNode.DeleteRightMinKeyNode().cNode.Data;
                }
                else
                {
                    delPair.cNode.Data = delPair.cNode.DeleteLeftMaxKeyNode().cNode.Data;
                }
                _rtnd = vNode.Right;
                vNode.Dispose();
                return delPair.cNode;
            }

            public void ShowVData()
            {
                _rtnd.ShowVData();
            }

        }

        public class pcnPair<T> //parent-child node pair
        {
            public BinarySearchTreeNode<T> pNode; public BinarySearchTreeNode<T> cNode;

            public pcnPair(BinarySearchTreeNode<T> pNode, BinarySearchTreeNode<T> cNode) { this.pNode = pNode; this.cNode = cNode; }
            public pcnPair(BinarySearchTreeNode<T> cNode) : this(null, cNode) { }
        }

        public class BinarySearchTreeNode<T> : SimpleBinaryTreeNode
        {
            public BinarySearchTreeNode<T> Left
            { get { return (BinarySearchTreeNode<T>)(base.Left); }
            set { base.Left = (SimpleBinaryTreeNode)(value); } }

            public BinarySearchTreeNode<T> Right
            {
                get { return (BinarySearchTreeNode<T>)(base.Right); }
                set { base.Right = (SimpleBinaryTreeNode)(value); }
            }


            public BinarySearchTreeNode(T data) : base(data) { }
            public BinarySearchTreeNode(T data, BinarySearchTreeNode<T> left, BinarySearchTreeNode<T> right) : base(data, left, right) {}


            public BinarySearchTreeNode<T> GetMaxKeyNode()
            {
                BinarySearchTreeNode<T> cNode = this; //current node.
                while (!(cNode.IsRightEmpty())) cNode = cNode.Right;
                return cNode;
            }

            public pcnPair<T> GetLeftMaxKeyNodePair()
            {
                if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);
                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = this.Left;
                while (!(cNode).IsRightEmpty()) { pNode = cNode; cNode = pNode.Right; }
                return new pcnPair<T>(pNode, cNode);
            }

            public pcnPair<T> DeleteLeftMaxKeyNode()
            {
                if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);
                if (this.Left.IsRightEmpty())
                {
                    BinarySearchTreeNode<T> buf = this.Left;
                    this.ChangeLeftSubtree(buf.Left);
                    return new pcnPair<T>(this,buf);
                }
                else
                {
                    pcnPair<T> lmnPair = GetLeftMaxKeyNodePair();
                    if (lmnPair.cNode.IsLeftEmpty()) { lmnPair.pNode.RemoveRightSubtree(); return lmnPair; }
                    else { lmnPair.pNode.ChangeRightSubtree(lmnPair.cNode.Left); return lmnPair; }
                }
            }

            public BinarySearchTreeNode<T> GetMinKeyNode()
            {
                BinarySearchTreeNode<T> cNode = this; //current node.
                while (!(cNode.IsLeftEmpty())) cNode = cNode.Left;
                return cNode;
            }

            public pcnPair<T> GetRightMinKeyNodePair()
            {
                if (IsRightEmpty()) throw new SBTNodeEmptyException(true);
                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = this.Right;
                while (!(cNode).IsLeftEmpty()) { pNode = cNode; cNode = pNode.Left; }
                return new pcnPair<T>(pNode, cNode);
            }

            public pcnPair<T> DeleteRightMinKeyNode()
            {
                if (IsRightEmpty()) throw new SBTNodeEmptyException(true);
                if (this.Right.IsLeftEmpty())
                {
                    BinarySearchTreeNode<T> buf = this.Right;
                    this.ChangeRightSubtree(buf.Right);
                    return new pcnPair<T>(this, buf);
                }
                else
                {
                    pcnPair<T> rmnPair = GetRightMinKeyNodePair();
                    if (rmnPair.cNode.IsRightEmpty()) { rmnPair.pNode.RemoveLeftSubtree(); return rmnPair; }
                    else { rmnPair.pNode.ChangeLeftSubtree(rmnPair.cNode.Right); return rmnPair; }
                }
            }

            public BinarySearchTreeNode<T> RotateLL()
            {
                if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);

                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = pNode.Left;

                pNode.ChangeLeftSubtree(cNode.Right);
                cNode.ChangeRightSubtree(pNode);

                return cNode;
            }

            public BinarySearchTreeNode<T> RotateRR()
            {
                if (IsRightEmpty()) throw new SBTNodeEmptyException(true);

                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = pNode.Right;

                pNode.ChangeRightSubtree(cNode.Left);
                cNode.ChangeLeftSubtree(pNode);

                return cNode;
            }

            public BinarySearchTreeNode<T> RotateLR()
            {
                if (IsLeftEmpty()) throw new SBTNodeEmptyException(false);
                else if (Left.IsRightEmpty()) throw new SBTNodeEmptyException(true);

                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = pNode.Left;

                pNode.ChangeLeftSubtree(RotateRR());
                pNode.RotateLL();

                return cNode;
            }

            public BinarySearchTreeNode<T> RotateRL()
            {
                if (IsRightEmpty()) throw new SBTNodeEmptyException(true);
                else if (Left.IsLeftEmpty()) throw new SBTNodeEmptyException(false);

                BinarySearchTreeNode<T> pNode = this;
                BinarySearchTreeNode<T> cNode = pNode.Right;

                pNode.ChangeRightSubtree(RotateLL());
                pNode.RotateRR();

                return cNode;
            }

        }

        public class KeyOverlapException : Exception { public KeyOverlapException() : base("Overlapping key is identified.") {}}
    }
}
