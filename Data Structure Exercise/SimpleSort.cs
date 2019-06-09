using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Structure_Exercise;

namespace Data_Structure_Exercise
{
    static public class SimpleSort<T> where T : IComparable<T>
    {
        public delegate int SelectPivot(int left, int right, ref T[] arr);

        static public void BubbleSort(ref T[] arr, int len)
        {
            T buf;
            for (int i = len; i > 1; i--)
            {
                for (int j = 1; j < i; j++)
                {
                    if (arr[j - 1].CompareTo(arr[j]) > 0)
                    {
                        buf = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = buf;
                    }
                }
            }
        }

        static public void SelectionSort(ref T[] arr, int len)
        {
            for (int i = 0; i < len; i++)
            {
                int argmax = i;//최대우선순위값을 담는 변수

                //최대 우선순위값 탐색
                for (int j = i + 1; j < len; j++)
                    if (arr[argmax].CompareTo(arr[j]) > 0) argmax = j;

                T buf = arr[i];
                arr[i] = arr[argmax];
                arr[argmax] = buf;
            }
        }

        static public void InsertionSort(ref T[] arr, int len)
        {
            int i;
            int j;
            T temp;
            for (i = 1; i < len; i++)
            {
                temp = arr[i];
                for (j = i - 1; j >= 0; j--)
                {
                    if (temp.CompareTo(arr[j]) < 0)
                        arr[j + 1] = arr[j];
                    else
                        break;
                }
                arr[j + 1] = temp;
            }
        }

        static public void HeapSort(ref T[] arr, int len)
        {
            SimpleHeap<T> heap = new SimpleHeap<T>();
            for (int i = 0; i < len; i++) heap.HInsert(arr[i]);
            for (int i = 0; i < len; i++) arr[i] = heap.HDelete();
        }

        static public void MergeSort(ref T[] arr, int len)
        {
            MergeSort(ref arr, 0, len - 1);
        }

        static private void MergeSort(ref T[] arr, int left, int right)
        {
            int mid = (left + right) / 2;

            if (left < right)
            {
                MergeSort(ref arr, left, mid);
                MergeSort(ref arr, mid + 1, right);

                Merge2Areas(ref arr, left, mid, right);
            }
        }

        static private void Merge2Areas(ref T[] arr, int left, int mid, int right)
        {
            int lidx = left;
            int ridx = mid + 1;
            int sidx = left;

            T[] sortArr = new T[right + 1];

            while (lidx <= mid && ridx <= right)
            {
                if (arr[lidx].CompareTo(arr[ridx]) < 0)
                    sortArr[sidx++] = arr[lidx++];
                else
                    sortArr[sidx++] = arr[ridx++];
            }

            while (lidx <= mid)
                sortArr[sidx++] = arr[lidx++];

            while (ridx <= right)
                sortArr[sidx++] = arr[ridx++];

            for (int i = left; i <= right; i++) arr[i] = sortArr[i];
            /*
            int i;
            int j;
            T temp;
            for (i=mid+1; i<=right; i++)
            {
                temp = arr[i];
                for (j=i-1; j>=left; j--)
                {
                    if (temp.CompareTo(arr[j]) < 0)
                        arr[j + 1] = arr[j];
                    else
                        break;
                }
                arr[j + 1] = temp;
            }

            삽입정렬을 병합에 이용하는 것은 {{1,2,3,4,5,6}, {2,2,3,3,4,4,5}} 같은 케이스 때문에 더 느려짐.
            */
        }

        static public void QuickSort(ref T[] arr, int len)
        {
            QuickSort(ref arr, 0, len-1, (int left, int right ,ref T[] _arr)=> { return right; });
        }

        static public void QuickSort(ref T[] arr, int len, SelectPivot sp)
        {
            QuickSort(ref arr, 0, len-1, sp);
        }

        static public void Swap(ref T a, ref T b)
        {
            T buf = a;
            a = b;
            b = buf;
        }

        static private int PartitionQS(ref T[] arr, int left, int right, SelectPivot sp)
        {
            Swap(ref arr[sp(left,right,ref arr)],ref arr[right]);
            T x = arr[right];
            int i = left;//i는 x가 들어가야할 자리.
            for (int j=left; j<right;j++)
            {
                if (arr[j].CompareTo(x) < 0)//j번째가 x보다 우선순위가 낮다면
                {
                    Swap(ref arr[i], ref arr[j]);
                    //만약 먼저 검사했던 모든 원소들이 x보다 작거나 x와 같았다면, swap은 일어나지 않는다.
                    //가장 최근에 검사한 x보다 큰 원소의 index를 k라 하자.
                    //k는 x보다 큰데 x의 자리인 i보다 더 왼쪽에 있으므로,
                    //x보다 작은 원소인 j번째 원소와 교환시켜야 제자리를 찾게 되는 것이다.
                    i++;//j번째가 x보다 앞에 들어가야 하므로, i를 증가시킨다.
                }
            }
            Swap(ref arr[i], ref arr[right]);
            return i;
        }

        static private int PartitionQSHo(ref T[] arr, int p, int r, SelectPivot sp)
        {
            Swap(ref arr[sp(p, r, ref arr)], ref arr[p]);
            T x = arr[p];
            int i = p;
            int j = r;

            while (true)
            {
                for (;j>=p+1 ; j--) if (arr[j].CompareTo(x) < 0) break;
                for (;i<=r-1 ;i++) if (arr[i].CompareTo(x) > 0) break;
                if (i < j)
                    Swap(ref arr[i], ref arr[j]);
                else
                {
                    Swap(ref arr[p], ref arr[j]);
                    return j;
                }
                    
            }
        }

        static public void LSDRadixSort(uint digit,uint[] arr)
        {
            int al = arr.Length;
            uint d;
            SimpleCircularQueue<uint>[] bucket = new SimpleCircularQueue<uint>[10];
            for (int i = 0; i < 10; i++)
                bucket[i] = new SimpleCircularQueue<uint>(al);
            uint p = 1;
            for (int i=1; i<=digit; i++)
            {
                for (int j=0; j<al; j++)
                {
                    d = (arr[j] %(p*10))/p; //or arr[j]/(p*10) % 10
                    bucket[d].enQueue(arr[j]);
                }
                int idx = 0;
                for (int j=0; j<10; j++)
                {
                    while (!bucket[j].IsEmpty())
                    {
                        arr[idx] = bucket[j].deQueue();
                        idx++;
                    }
                }
                p *= 10;
            }
        }

        static public void LSDRadixSort(uint digit, int[] arr)
        {
            int l = arr.Length;
            uint[] buf = new uint[l];
            for (int i = 0; i < l; i++)
                buf[i] = (uint)arr[i];
            LSDRadixSort(digit,buf);
            for (int i = 0; i < l; i++)
                arr[i] = (int)buf[i];
        }


        static private void QuickSort(ref T[] arr, int left, int right, SelectPivot sp)
        {
            if (left<right)
            {
                int q = PartitionQSHo(ref arr, left, right, sp);
                QuickSort(ref arr, left, q-1,sp);
                QuickSort(ref arr, q+1, right, sp);
            }
            //밑 : 본래의 것보다 좀 지저분해지긴 했지만 쨌든 윤성우식.
            /*
            if (left >= right) return;
            //Partition
            int pivot = sp.Invoke(left,right, ref arr);
            int low = (pivot == left) ? left+1 : left;
            int high = (pivot == right) ? right - 1 : right;
            while (true)
            {
                for (;low<=right;low++)
                {
                    if (arr[low].CompareTo(arr[pivot]) > 0) break;
                }
                for (;high>=left;high--)
                {
                    if (arr[high].CompareTo(arr[pivot]) < 0) break;
                }
                if (low > right) { Swap(ref arr[pivot], ref arr[right]); QuickSort(ref arr, left, right - 1, sp); return;}
                if (high < left) { Swap(ref arr[pivot], ref arr[left]); QuickSort(ref arr, left+1, right, sp); return; }
                if (low <= high) Swap(ref arr[low], ref arr[high]); else break;
            }
            Swap(ref arr[pivot], ref arr[high]);

            pivot = high;
            //Partition

            QuickSort(ref arr, left, pivot - 1, sp);
            QuickSort(ref arr, pivot+1, right, sp);
            //... 좀 지저분하게 구현해버렸다?
            */
        }
        
    }

}
