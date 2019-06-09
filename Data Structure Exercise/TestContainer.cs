using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCQueueI = Data_Structure_Exercise.SimpleCircularQueue<int>;
using Data_Structure_Exercise;
using SBTnodeI = Data_Structure_Exercise.SimpleBinaryTreeNode;
using SLLstackI = Data_Structure_Exercise.SimpleLinkedListBasedStack<int>;
using SSCLCLTR = Data_Structure_Exercise.SimpleStackCalculator;
using System.Collections;
using Data_Structure_Exercise.SimpleSearch;

namespace Data_Structure_Exercise
{

    using SLLStackC2 = Data_Structure_Exercise.SimpleLinkedListBasedStack<C2>;
    public class C1 { }
    public interface I1 { }

    public abstract class A1 { }
    public abstract class A2 : A1 { }
    public class C3 : A2 { }

    public class C2 : C1, I1
    {

        public int a; public C2() : this(0) { }
        public C2(int a) { this.a = a; }

        public override string ToString()
        {
            return String.Format("C2 - {0}", a);
        }
    }

    public class C4<T> { }


    public class TestContainer
    {

        public delegate void ArrSort<T>(ref T[] arr, int len) where T : IComparable<T>;

        public delegate double SDL(double a, double b);

        public static void GenericTest()
        {
            var c4 = new C4<C2>();
            Console.WriteLine("c4 is C4<C2> : {0}", c4 is C4<C2>);//c4 is C4는 ㅅㅂ 안통하네
            Console.WriteLine("c4 is C4<I1> : {0}", c4 is C4<I1>);
            Console.WriteLine("c4 is C4<C1> : {0}", c4 is C4<C1>);
            Console.WriteLine("c4.GetType() == typeof(C4<>) : {0}", c4.GetType() == typeof(C4<>));
            Console.WriteLine("c4 is C4.GetType().IsGenericType : {0}", c4.GetType().IsGenericType);
            Console.WriteLine("c4.GetType().GetGenericTypeDefinition()==typeof(C4<>) : {0}", c4.GetType().GetGenericTypeDefinition() == typeof(C4<>));
            //Result : TFFFTT
            //C4<I1> c5 = (C4<I1>)c4; casting 불가
            //C4<C2> c6 = (c4<C2>)c4; casting 불가
            Console.WriteLine(c4.GetType().BaseType);
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<>), c4));
            Console.WriteLine(c4.GetType().GetGenericTypeDefinition());
            Console.WriteLine("{0}, {1}", c4.GetType().GetGenericArguments()[0], c4.GetType().GetGenericArguments().Length);
            Console.WriteLine(c4.GetType().GetGenericTypeDefinition() == typeof(C4<C1>));
            Console.WriteLine(c4.GetType().GetGenericTypeDefinition() == typeof(C4<C2>));
            Console.WriteLine(c4 is C4<C2>);
            Console.WriteLine(c4.GetType() == typeof(C4<C2>));
            Console.WriteLine(c4.GetType().GetGenericTypeDefinition() == typeof(C4<>));
            Console.WriteLine(typeof(C4<C1>));
            Console.WriteLine(typeof(C4<C1>).GetGenericArguments()[0]);
            Console.WriteLine(typeof(C4<>).GetGenericArguments().Length);
            Console.WriteLine(typeof(C4<>).GetGenericArguments()[0]);
            Console.WriteLine(typeof(C4<>) == typeof(C4<>));
            Console.WriteLine(typeof(C4<int>) == typeof(C4<>));
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<>), c4));
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<C2>), c4));
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<C1>), c4));
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<C3>), c4));
            Console.WriteLine(Convenience.IsGenericTypeOf(typeof(C4<I1>), c4));
            Console.WriteLine("--------------------");
            var c2 = new C2();
            Console.WriteLine(c2.GetType().GetInterfaces().Contains<Type>(typeof(I1)));
        }

        public static void SSCTest()
        {
            string s1 = "33 + 7 - 9";
            string s2 = "(3+10)/99*((50*7+3)-900)";
            Console.WriteLine(StkCalculTest(s1));
            Console.WriteLine(StkCalculTest(s2));
        }

        public static string StkCalculTest(string infix)
        {
            StringBuilder sb = new StringBuilder();
            var posfix = SSCLCLTR.ConvertInfixToPosfixTest(infix);
            foreach (var e in posfix)
            {
                if (e is double) sb.AppendFormat("{0} ", e);
                if (e is SSCLCLTR.SimpleBinaryOperator) sb.AppendFormat("{0} ", ((SSCLCLTR.SimpleBinaryOperator)e).expr);
            }
            return sb.ToString();
        }

        public static void SDLTest()
        {
            SDL d = new SDL((double a, double b) => { return a + b; });
            Console.WriteLine(d(1, 2));
            Console.WriteLine(d(2, 3));
        }

        public static void SCTest1()
        {
            String s = "1 + 2 / 3";
            char[] cs = s.ToCharArray();
            Console.WriteLine("----------");
            foreach (char c in cs)
                Console.WriteLine("{0}, {1}", c, (int)c);
            Console.WriteLine("----------");
            foreach (char c in s)
            {
                try
                {
                    Console.WriteLine("{0}, {1}", c, int.Parse(c.ToString()));
                }
                catch (Exception e)//FormatException
                {
                    Console.WriteLine("{0},{1}", e.GetType(), e.Message);
                }
            }
            Console.WriteLine("----------");
            foreach (char c in s)
            {
                int i;
                bool success = int.TryParse(c.ToString(), out i);
                if (success) Console.WriteLine(i); else Console.WriteLine(c);
            }
            Console.WriteLine("----------");
            Console.WriteLine("{0}, {1}", 32, (char)32);//성공적으로 casting. 결과, 32, (공백문자)
            Console.WriteLine("{0}, {1}", 43, (char)43);//성공적으로 castring. 결과, 43, +
        }

        public static void StackTest1()
        {
            SLLstackI s = new SLLstackI();
            s.Push(1);
            s.Push(2);
            s.Push(3);
            s.Push(4);
            Console.WriteLine(s.Peek);
            while (true)
            {
                Console.WriteLine(s.Pop());
            }
            //ㅇㅋ EmptyStackException 제대로 발동된다.
        }

        public static void StackTest2()
        {
            SLLStackC2 s = new SLLStackC2();
            C2 c2 = new Data_Structure_Exercise.C2();
            s.Push(c2);
            Console.WriteLine(c2);
            Console.WriteLine(s.Peek);
            c2.a = 10;
            Console.WriteLine(c2);
            Console.WriteLine(s.Peek);
            //Result : 0, 0, 10, 10.
            //Node class의 _data field의 accessibility는 internal.
            //그러나 Node head의 data의 class인 C2의 접근성은 public. a의 acessibility도 public.
            //따라서 c2.a에 접근해서 수정하는 것이 가능.
        }

        public static void StackTest3()
        {
            SLLstackI stk = new SLLstackI();
            Console.WriteLine(stk.Count);
            stk.Push(3);
            Console.WriteLine(stk.Count);
            stk.Push(4);
            Console.WriteLine(stk.Count);
            stk.Pop();
            stk.Pop();
            Console.WriteLine(stk.Count);
            var stk2 = new SLLstackI(4);
            Console.WriteLine(stk2.Count);
        }

        public static void Class_Inheritance_Test()
        {
            C1 c1 = new C1();
            C2 c2 = new C2();
            C3 c3 = new C3();
            Console.WriteLine(c1 is C1);
            Console.WriteLine(c1 is C2);
            Console.WriteLine(c2 is C1);
            Console.WriteLine(c2 is C2);
            Console.WriteLine(c2 is I1);
            Console.WriteLine("c3 is C3? : {0}", c3 is C3);
            Console.WriteLine("c3 is A2? : {0}", c3 is A2);
            Console.WriteLine("c3 is A1? : {0}", c3 is A1);
            //Result : TFTTTTTT
        }

        public static void SBT_Test1()
        {
            SBTnodeI A = new SBTnodeI(typeof(int), 10);
            //Console.WriteLine(A.Left);
            Console.WriteLine(A.Data);
            Console.WriteLine(A.DataType);
            Console.WriteLine(A.Data.GetType());
            A.Data = 30;
            Console.WriteLine(A.Data);
            //A.Data = 30.5;
            //Console.WriteLine(A.Data); 캐스팅오류 제대로 발생해주시구연~~~~~~~
            Console.WriteLine("{0}, {1}, {2}", A.IsLeftEmpty(), A.IsRightEmpty(), A.IsTerminal());
            A.Left = new SBTnodeI(typeof(double), 20.5);
            Console.WriteLine("{0}, {1}, {2}", A.IsLeftEmpty(), A.IsRightEmpty(), A.IsTerminal());
            A.Right = new SBTnodeI(typeof(string), "퍄퍄");
            Console.WriteLine("{0}, {1}, {2}", A.IsLeftEmpty(), A.IsRightEmpty(), A.IsTerminal());
            Console.WriteLine(A.Left.Data.GetType());
            Console.WriteLine(A.Right.Data.GetType());
            //A.Right.Data = 20; 기대한대로 캐스팅오류 제대로 발생해주시구연~~~~~~
        }

        public static void SBT_Test2()
        {
            SBTnodeI B = new SBTnodeI(typeof(int), 1);
            SBTnodeI A = new SBTnodeI(typeof(int), 10);
            A.Left = B;
            Console.WriteLine(A.Left);
            Console.WriteLine(A.Right);
        }

        public static void SBT_Test3()
        {
            SBTnodeI B = new SBTnodeI(typeof(int), 1);
            SBTnodeI A = new SBTnodeI(typeof(int), 10);
            A.Right = B;
            Console.WriteLine(A.Right);
            Console.WriteLine(A.Left);
        }

        public static void SBT_Test4()
        {
            SBTnodeI B = new SBTnodeI(typeof(int), 1);
            SBTnodeI A = new SBTnodeI(typeof(int), 10, B);
            Console.WriteLine(A);
            Console.WriteLine(A.Left);
        }

        public static void SBT_Test5()
        {
            var intt = typeof(int);
            SBTnodeI nd1 = new SBTnodeI(intt, 1);
            SBTnodeI nd2 = new SBTnodeI(intt, 2);
            SBTnodeI nd3 = new SBTnodeI(intt, 3);
            SBTnodeI nd4 = new SBTnodeI(intt, 4);
            nd1.Left = nd2;
            nd1.Right = nd3;
            nd2.Left = nd4;
            ArrayList trvlst;
            trvlst = nd1.Traverse(SBTnodeI.InOrder);
            foreach (int e in trvlst) Console.WriteLine(e);
        }

        public static void SBT_Test6()
        {
            var intt = typeof(int);
            SBTnodeI nd1 = new SBTnodeI(intt, 1);
            SBTnodeI nd2 = new SBTnodeI(intt, 2);
            SBTnodeI nd3 = new SBTnodeI(intt, 3);
            SBTnodeI nd4 = new SBTnodeI(intt, 4);
            nd1.Left = nd2;
            nd1.Right = nd3;
            nd2.Left = nd4;
            ArrayList trvlst;
            trvlst = nd1.Traverse(SBTnodeI.InOrder, (SimpleBinaryTreeNode node) => { Console.WriteLine(node.Data); });
        }

        public static void SBT_Test7()
        {
            SBTnodeI nd1 = new SBTnodeI(1,null,null);
            SBTnodeI nd2 = new SBTnodeI(3.5);
            Console.WriteLine("{0}, {1}",nd1.Data,nd1.DataType);
            Console.WriteLine("{0}, {1}", nd2.Data, nd2.DataType);
            Console.WriteLine(nd1.Data + nd2.Data);
            /* result : 
             * 1, System.Int32
             * 3.5, System.Double
             * 4.5
             * 계속하려면 아무 키나 누르십시오 . . .
             */
        }

        public static void SBT_Test8()
        {
            SBTnodeI sbt1 = new SBTnodeI(1);
            SBTnodeI sbt2 = new SBTnodeI(2);
            SBTnodeI sbt3 = new SBTnodeI(3);
            SBTnodeI sbt4 = new SBTnodeI(4);
            sbt1.ChangeLeftSubtree(sbt2);
            sbt1.ChangeRightSubtree(sbt3);
            Console.WriteLine("{0}, {1}", sbt1.Left.Data, sbt1.Right.Data);
            sbt1.ChangeLeftSubtree(null);
            Console.WriteLine(sbt1.IsLeftEmpty());
            sbt1.ChangeLeftSubtree(sbt4);
            sbt3.ChangeRightSubtree(sbt2);
            Console.WriteLine(sbt1.Height);
            Console.WriteLine(sbt3.Height);
            Console.WriteLine(sbt2.Height);
            Console.WriteLine(sbt1.GetHeightDiff());
            /*
             * result :
             * 2, 3
             * True
             * 2
             * 1
             * 0
             * -1
             */
        }

        public static void QBS_Test(int capacity)
        {
            BurgerSimulationQ bsq = new BurgerSimulationQ(1800, 100, 15, 10);
            bsq.Run();
        }

        public static void SCQ_Test()
        {
            var q = new SCQueueI();
            int[] arr = new int[4] { 1, 2, 3, 4 };
            foreach (int e in arr)
            {
                Console.WriteLine(q.IsEmpty());
                q.enQueue(e);
                Console.WriteLine("{0}:{1}", e, q.Count);
                Console.WriteLine(q.IsFull());
            }
            while (!(q.IsEmpty()))
            {
                Console.WriteLine("{0}:{1}", q.deQueue(), q.Count);
            }
            Console.WriteLine(q.IsEmpty());
        }

        public static void IsTest()
        {
            double[] i = new double[1] { 1 };
            int[] j = new int[1] { 2 };

            foreach (dynamic e in i)
            {
                Console.WriteLine(e is double);
                Console.WriteLine(e.GetType());
            }

            foreach (dynamic e in j)
            {
                Console.WriteLine(e is double);
                Console.WriteLine(e is int);
                Console.WriteLine(e is object);
                Console.WriteLine(e.GetType());
            }
            //result : T, double, F, T, T, int.
        }

        public static void CalTest1()
        {
            string exp = "1 + 3 - (6 - 3 *7)/3";
            ArrayList posfix = SSCLCLTR.ConvertInfixToPosfixTest(exp);
            SimpleBinaryTreeNode exptree = SSCLCLTR.MakeExpTree(posfix);
            exptree.Traverse(SimpleBinaryTreeNode.PostOrder, (SimpleBinaryTreeNode nd)=> { Console.Write(nd.Data); });
            Console.WriteLine();
            exptree.Traverse(SimpleBinaryTreeNode.InOrder, (SimpleBinaryTreeNode nd) => { Console.Write(nd.Data); });
            Console.WriteLine();
            exptree.Traverse(SimpleBinaryTreeNode.PreOrder, (SimpleBinaryTreeNode nd) => { Console.Write(nd.Data); });
            Console.WriteLine();
            Console.WriteLine(SSCLCLTR.EvaluateExpTreeP2(exptree));
            Console.WriteLine(SSCLCLTR.EvaluateExpTreeP(exptree));
            //EvaluateExpTreeP에 의해서 exptree가 수정되기 때문에 P 쓰고 P2 쓰면 에러가 났던 것.
            //exptree가 수정되지 않는 EvaluateExpTreeP를 생각해봐야...
            Console.WriteLine(1 + 3 - (6 - 3 * 7) / 3);
        }

        public static void CompareTest()
        {
            Console.WriteLine(3.CompareTo(5));//-1
            Console.WriteLine(5.CompareTo(3));//1
            Console.WriteLine(3.CompareTo(3));//0
        }

        public static void HeapTest1()
        {
            Random rnd = new Random();
            SimpleHeap<int> sh = new SimpleHeap<int>((int a, int b)=> { return (-a.CompareTo(b)); });
            for (int i = 1; i<=10; i++)
            {
                int r = rnd.Next(20);
                Console.WriteLine("{0} insertion : {1}",i,r);
                sh.HInsert(r);
            }
            Console.WriteLine();
            for (int i = 1; i <= 10; i++)
                Console.WriteLine("sh[{0}] : {1}",i,sh[i]);

            Console.WriteLine("Root Node is Deleted : {0}",sh.HDelete());

            for (int i = 1; i <= 9; i++)
                Console.WriteLine("sh[{0}] : {1}", i, sh[i]);

            for (int i = 2; i <= 10; i++)
                Console.WriteLine("{0}th deletion : {1}",i,sh.HDelete());
            //힙에서 첫번째 인덱스만 deque함수(equivalent to HDelete)를 통해 볼 수 있게 만들어놓아서 큐의 모양새를 갖춘 것이 우선순위큐이다.
            //우선순위큐의 구현? 그런 거 귀찮아영.
        }

        public static void SortTest1()
        {
            int len = 10;
            int[] arr = new int[len];
            Random rnd = new Random();
            for (int i = 0; i < len; i++) { Console.WriteLine(arr[i] = rnd.Next(20)); }
            SimpleSort<int>.BubbleSort(ref arr, len);
            foreach (int i in arr)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
            for (int i = 0; i < len; i++) Console.WriteLine(arr[i] = rnd.Next(20));
            SimpleSort<int>.SelectionSort(ref arr, len);
            foreach (int i in arr)
                Console.WriteLine(i);
        }

        public static void BreakTest1()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.WriteLine("{0}, {1}", i, j);
                    break;
                }
                Console.WriteLine(i);
                //break가 break하는 for문은 안쪽의 for문 뿐.
            }
        }

        public static void BreakTest2()
        {
            int i;
            int j;
            for (i = 0; i < 3; i++)
            {
                Console.WriteLine("i : {0}",i);
                for (j = 0; j<3; j++)
                {
                    //if (j > 1)
                    //    break;
                    Console.WriteLine(j);
                }
                Console.WriteLine("{0},{1}",i,j);
                //정상. break에 의해 for문이 종료될 때는 j가 증가하지 않음.
                //안쪽 for문이 종료될 때 j=3이고, j=3이기에 j<3의 조건에 걸려서 안쪽 for문 종료됨.
            }
        }

        public static void InsrtTest()
        {
            int len = 10;
            int[] arr = new int[len];
            Random rnd = new Random();
            for (int i = 0; i < len; i++) { arr[i] = rnd.Next(20); Console.WriteLine(arr[i]); }
            Console.WriteLine();
            SimpleSort<int>.InsertionSort(ref arr,len);
            foreach (int i in arr) Console.WriteLine(i);
        }

        public static void HPSRT_Test()
        {
            int len = 10;
            int[] arr = new int[len];
            Random rnd = new Random();
            for (int i = 0; i < len; i++) { arr[i] = rnd.Next(20); Console.WriteLine(arr[i]); }
            Console.WriteLine();
            SimpleSort<int>.HeapSort(ref arr, len);
            foreach (int i in arr) Console.WriteLine(i);
        }

        public static void SortingTest(ArrSort<int> arrsort, int len, int range)
        {
            int[] arr = new int[len];
            Random rnd = new Random();
            for (int i = 0; i < len; i++) { arr[i] = rnd.Next(range); Console.WriteLine(arr[i]); }
            Console.WriteLine();
            arrsort.Invoke(ref arr,len);
            foreach (int i in arr) Console.WriteLine(i);
        }

        public static void MergeSortTest()
        {
            int len = 10;
            int range = 20;
            SortingTest(SimpleSort<int>.MergeSort, len, range);
        }

        public static void ArrTest()
        {
            int[] arr = new int[3];
            int[] arr2 = new int[3] {1,2,3 };
            int i = 0;
            int k = 0;
            arr[i++] = arr2[k++]; 
            for (int j = 0; j<3; j++) Console.WriteLine("{0} : {1}",j,arr[j]);//1, 0, 0
            i = 0; k = 0;
            arr[++i] = arr2[k++];
            for (int j = 0; j < 3; j++) Console.WriteLine("{0} : {1}", j, arr[j]);//1,1,0
            i = 0; k = 0;
            arr[i++] = arr2[++k];
            for (int j = 0; j < 3; j++) Console.WriteLine("{0} : {1}", j, arr[j]);//2,1,0
            i = 0; k = 0;
            arr[++i] = arr2[++k];
            for (int j = 0; j < 3; j++) Console.WriteLine("{0} : {1}", j, arr[j]);//2,2,0
        }

        public static void SwapTest()
        {
            int a = 10;
            int b = 20;
            int[] arr = new int[3] { 1, 2, 3 };
            SimpleSort<int>.Swap(ref a, ref b);
            SimpleSort<int>.Swap(ref arr[0], ref arr[2]);
            Console.WriteLine("{0}, {1}",a,b);
            Console.WriteLine("{0}, {1}, {2}", arr[0], arr[1], arr[2]);
        }

        public static void InterPolationSearchTest()
        {
            int len = 10;
            int range = 1000;
            Random rnd = new Random();
            int[] arr = new int[len];
            for (int i = 0; i < len; i++)
                arr[i] = rnd.Next(range);
            SimpleSort<int>.QuickSort(ref arr, len);
            foreach (int i in arr)
                Console.Write("{0} ", i);
            while (true)
            {
                Console.WriteLine("\n\n Search : ");
                int key;

                try { key = int.Parse(Console.ReadLine()); }
                catch { break; }
                
                Console.WriteLine("\nOk. Search {0}.", key);
                InterpolationSearch its = new InterpolationSearch(arr, key, (idx) => Console.WriteLine("The Index is {0}.", idx));
                its.Work();
                Console.WriteLine("-------------");
            }
        }

        public static void BST_Test()
        {
            BinarySearchTree<int, int> bst = new BinarySearchTree<int, int>((int data)=> { return data; });
            BinarySearchTreeNode<int> node;
            Random rnd = new Random();
            int rndint;
            int keyvalue;
            for (int i = 0; i<10; i++)
            {
                rndint = rnd.Next(1000);
                Console.Write("{0}/",rndint);
                bst.Insert(rndint);
            }
            Console.WriteLine();
            ArrayList trvlst = new ArrayList();
            bst.Traverse(ref trvlst, SimpleBinaryTreeNode.InOrder, (data) => Console.Write("{0}/", data));

            while(true)
            {
                Console.WriteLine("\nSearch : ");
                try { keyvalue = int.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Stop Search."); break; }
                try {
                    node = bst.Search(keyvalue);
                    bst.Traverse(ref trvlst, SimpleBinaryTreeNode.InOrder, (data) => Console.Write("{0}/", data));
                    Console.WriteLine();
                }
                catch (NotFound) { Console.WriteLine("Not Found."); }
            }

            while (true)
            {
                Console.WriteLine("\nRemove : ");
                try { keyvalue = int.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Stop Remove."); break; }
                try {
                    node = bst.RemoveNode(keyvalue);
                    bst.Traverse(ref trvlst, SimpleBinaryTreeNode.InOrder, (data) => Console.Write("{0}/", data));
                    Console.WriteLine();
                }
                catch (NotFound) { Console.WriteLine("Not Found."); }
            }
            bst.Traverse(ref trvlst, SimpleBinaryTreeNode.InOrder, (data) => Console.WriteLine(data));
        }

        public static void LineBreakTest()
        {
            Console.WriteLine("{0}, {1}",Console.LargestWindowWidth, Console.WindowLeft);
            Console.WindowWidth = 240;//240이 최대
            Console.SetWindowPosition(0,0);
            Console.SetWindowPosition(0,240);
            int n;
            while (int.TryParse(Console.ReadLine(),out n))
            {
                Console.WriteLine();
                for (int i = 1; i <= n; i++)
                {
                    for (int j = 0; j < i; j++)
                        Console.Write("a");
                    Console.WriteLine();
                }
            }
        }

        public static void ArrIniNullTest()
        {
            string[] s = new string[5];
            s[1] = "s";
            Console.WriteLine("{0}, {1}", s[0] == null, s[1]);
            //result : true, s;
        }

        public static void ListIniNullTest()
        {
            List < string > l = new List<string>();
            l[2] = "2";
            Console.WriteLine("{0}", l[1]);
            //Not operate.
        }

        public class TV_Block
        {
            public int depth;
            public int rel;
            public string str;

            public TV_Block(string str, int depth, int rel) { this.str = str; this.depth = depth; this.rel = rel; }
        }

        public static void buildtv(List<TV_Block> tv, SBTnodeI nd, int depth, int rel, ref int cont_depth, ref int cont_rel)//rel = right edge length.
        {
            if (cont_depth > depth)
            {
                tv.Add(new TV_Block(String.Format("({0})", nd.Data.ToString()), depth, ++cont_rel));
            }
            else
            {
                tv.Add(new TV_Block(String.Format("({0})", nd.Data.ToString()), depth, rel));
                cont_rel = rel;
            }
            cont_depth = depth;

            if (!(nd.IsLeftEmpty())) buildtv(tv, nd.Left, depth+1, cont_rel, ref cont_depth, ref cont_rel);

            if (!(nd.IsRightEmpty())) buildtv(tv, nd.Right, depth+1, cont_rel+1, ref cont_depth, ref cont_rel);
        }

        public static void tempTV()
        {
            Console.WindowWidth = 240;//240이 최대
            SBTnodeI[] bt = new SBTnodeI[10];
            for (int i = 0; i < 10; i++)
                bt[i] = new SBTnodeI(i);
            bt[0].Left = bt[1];
            bt[1].Left = bt[3];
            bt[3].Left = bt[4];
            bt[3].Right = bt[5];
            bt[0].Right = bt[2];
            bt[2].Left = bt[6];
            bt[2].Right = bt[7];
            bt[7].Right = bt[8];
            bt[8].Left = bt[9];

            int h = bt[0].Height;
            int cont_depth = 0;
            int cont_rel = 0;
            List<TV_Block> tv = new List<TV_Block>();
            buildtv(tv, bt[0], 0, 0, ref cont_depth, ref cont_rel);
            foreach (TV_Block e in tv)
                Console.WriteLine("{0} : (depth : {1}), (rel : {2})",e.str, e.depth, e.rel);
        }

        public static void BTV_Test()
        {
            BinarySearchTree<int, int> bst = new BinarySearchTree<int, int>((int data) => { return data; });
            BinarySearchTreeNode<int> node;
            Random rnd = new Random();
            int rndint;
            int keyvalue;
            for (int i = 0; i < 10; i++)
            {
                rndint = rnd.Next(1000);
                Console.Write("{0}/", rndint);
                bst.Insert(rndint);
            }
            Console.WriteLine();
            ArrayList trvlst = new ArrayList();
            bst.Traverse(ref trvlst, SimpleBinaryTreeNode.InOrder, (data) => Console.Write("{0}/", data));
            Console.WriteLine();

            bst.ShowVData();
        }

    }
}
