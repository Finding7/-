using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{


    // 链表节点
    public class Node<T>
    {
        public Node<T> Next { get; set; }
        public T Data { get; set; }

        public Node(T t)
        {
            Next = null;
            Data = t;
        }
    }

    // 泛型链表类
    public class GenericList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public GenericList()
        {
            tail = head = null;
        }

        public Node<T> Head
        {
            get { return head; }
        }

        public void Add(T t)
        {
            Node<T> n = new Node<T>(t);
            if (tail == null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next = n;
                tail = n;
            }
        }

        //ForEach方法
        public void ForEach(Action<T> action)
        {
            Node<T> current = head;
            while (current != null)
            {
                action(current.Data);
                current = current.Next;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 整型List
            GenericList<int> intlist = new GenericList<int>();
            for (int x = 0; x < 10; x++)
            {
                intlist.Add(x);
            }

            // 使用ForEach打印元素
            Console.WriteLine("整数链表元素：");
            intlist.ForEach(x => Console.WriteLine(x));

            // 求和
            int sum = 0;
            intlist.ForEach(x => sum += x);
            Console.WriteLine($"总和：{sum}");

            // 求最大值
            int max = int.MinValue;
            intlist.ForEach(x => { if (x > max) max = x; });
            Console.WriteLine($"最大值：{max}");

            // 求最小值
            int min = int.MaxValue;
            intlist.ForEach(x => { if (x < min) min = x; });
            Console.WriteLine($"最小值：{min}");

            // 字符串型List
            GenericList<string> strList = new GenericList<string>();
            for (int x = 0; x < 10; x++)
            {
                strList.Add("str" + x);
            }

            // 使用ForEach打印字符串元素
            Console.WriteLine("\n字符串链表元素：");
            strList.ForEach(s => Console.WriteLine(s));
        }
    }
    
}
