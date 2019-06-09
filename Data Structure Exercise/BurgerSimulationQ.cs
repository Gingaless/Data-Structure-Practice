using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = Data_Structure_Exercise.Order;
using SCQueueO = Data_Structure_Exercise.SimpleCircularQueue<Data_Structure_Exercise.Order>;
using Data_Structure_Exercise;
using qState = Data_Structure_Exercise.SCQException.qState;

namespace Data_Structure_Exercise
{

    public enum Burger { cheese, bulgogi, Shanghai };//버거종류

    public class Order
        //이거 struct로 정의하면 _wtime이 데이터타입으로 취급되어서, 만약 구조체 선언시에 _wtime이 10이었다면, _wtime--를 반복해도
        //_wtime은 9가 나온다. 시발 괜히 struct로 선언해서 시간낭비 존나했네.
    {
        public int _onum; public Burger burger; public int _wtime;
        public Order(int o, Burger b, int wtime) { _onum = o; burger = b; _wtime = wtime; }
    }
    // _onum : 주문번호. 즉, order number.

    public class BurgerSimulationQ
    {

        static readonly List<int> cooktime = new List<int> { 12, 15, 24 };//버거별 요리시간. 치즈, 불고기, 상하이 순.

        private int _bhour;//영업시간
        private SCQueueO orders;//주문리스트. 이것의 Length가 대기실의 최대수용인원이다.
        private int sec1;
        private int odel;//order delay.

        private int _ronum = 0;//recent order number. 최신주문번호.
        private int _rhour = 0;//recent hour. 현재시간.
        private int _goback = 0;//대기실이 꽉 차서 그냥 돌아간 손님들.
        private int s_che = 0;//팔린 치즈버거 수
        private int s_bul = 0;//팔린 불고기버거 수
        private int s_sha = 0;//팔린 상하이버거 수
        private int v_cstmr = 0;//사먹었든 그냥 돌아갔든지 간에 일단 가게에 들른 손님들 수

        public int Capacity { get { return orders.Length; } }
        public int WatingCustomer { get { return orders.Count; } }

        public BurgerSimulationQ(int business_hour, int capacity, int order_delay, int sec1)
        {
            _bhour = business_hour;
            odel = order_delay;
            this.sec1 = sec1;
            orders = new SCQueueO(capacity);
        }

        public BurgerSimulationQ(int business_hour, int capacity):this(business_hour, capacity, 100) { }

        public BurgerSimulationQ(int business_hour, int capacity, int sec1) : this(business_hour, capacity, 15, sec1) { }

        public BurgerSimulationQ(int capacity):this(3600,capacity) { }

        void TakeOrder(Burger menu)
        {
            if (orders.IsFull()) throw new SCQException(qState.full);
            _ronum++;
            Order no = new Order(_ronum, menu, cooktime[(int)menu]);//New Order
            orders.enQueue(no);
            switch ((int)menu)//치즈 불고기 상하이
            {
                case 0:
                    Console.WriteLine("{0}번째 고객님께서 치즈버거를 주문하셨습니다.\n현재 대기인원 : {1}\n", _ronum, orders.Count);
                    break;
                case 1:
                    Console.WriteLine("{0}번째 고객님께서 불고기버거를 주문하셨습니다.\n현재 대기인원 : {1}\n", _ronum, orders.Count);
                    break;
                default:
                    Console.WriteLine("{0}번째 고객님께서 상하이버거를 주문하셨습니다.\n현재 대기인원 : {1}\n", _ronum, orders.Count);
                    break;
            }
        }

        Order BurgerOrdered()
        {
            if (orders.IsEmpty()) throw new SCQException(qState.empty);

            Order bo = orders.deQueue();

            //Console 글자 색깔 바꿀 수 있음 ㅎ.
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            switch ((int)bo.burger)
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("{0}번째 고객님, 주문하신 치즈버거 나왔습니다.", bo._onum);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                    s_che++;
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("{0}번째 고객님, 주문하신 불고기버거 나왔습니다.", bo._onum);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                    s_bul++;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("{0}번째 고객님, 주문하신 상하이버거 나왔습니다.", bo._onum);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                    s_sha++;
                    break;

            }
            Console.ResetColor();
            //색 바꾼 후 reset을 잊지 말자.
            return bo;
        }

        public void Run()
        {
            Random rnd = new Random();
            int h = _bhour / 3600;
            int m = (_bhour - h * 3600) / 60;
            int cf = 0;//customer frequency.
            Order fo;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("영업 시작했습니다! 앞으로 {0}시간 {1}분 동안 영업합니다!\n", h,m);
            Console.WriteLine("수용가능인원: {0}\n\n",Capacity);
            Console.ResetColor();
            while(_rhour < _bhour)
            {
                System.Threading.Thread.Sleep(sec1);
                _rhour++;
                cf++;
                if (cf < odel) cf++;
                else
                {
                    cf = 0;//시바 설마 숫자 초기화를 안 하는 실수를 저지를 줄이야;;; 데헷 와타시쟝 완전 덜렁이인것이어요
                    v_cstmr++;
                    try
                    { TakeOrder((Burger)rnd.Next(3)); }
                    catch (SCQException scqe)
                    {
                        if (scqe.state == qState.full)
                        {
                            _goback++;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\n더이상 손님을 받을 수 없습니다!\n대기실 꽉 차서 돌아간 손님들 수 : {0}\n", _goback);
                            Console.ResetColor();
                        }
                        else
                            throw scqe;
                    }
                    
                        };
                if (!(orders.IsEmpty()))
                {
                    fo = orders.Qpeek();
                    if (fo._wtime > 0) fo._wtime--;
                    else
                        BurgerOrdered();
                }
                else
                    continue;
            }
            Console.WriteLine("\n영업 끝났습니다! 모두 수고하셨습니다!\n");
            Console.WriteLine("팔린 치즈버거 수 : {0}", s_che);
            Console.WriteLine("팔린 불고기버거 수 : {0}", s_bul);
            Console.WriteLine("팔린 상하이버거 수 : {0}", s_sha);
            Console.WriteLine("대기실 꽉 차서 돌아간 손님들 수 : {0}", _goback);
            Console.WriteLine("아직 대기실 안에 있는 손님들 수 : {0}", orders.Count);
            Console.WriteLine("방문한 총 손님수 : {0}", v_cstmr);
            Console.WriteLine("검산 : {0}", s_che + s_bul + s_sha + _goback + orders.Count);
        }
    }
}
