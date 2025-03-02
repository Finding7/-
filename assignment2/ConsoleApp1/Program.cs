using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Console.Write("输入一个正整数：");
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.Write("输入错误，请重新输入数字：");
            }

            List<int> factors = new List<int>();
            int temp = num;

            for (int i = 2; i <= temp; i++)
            {
                while (temp % i == 0)
                {
                    if (!factors.Contains(i)) 
                    {
                        factors.Add(i);
                    }
                    temp /= i;
                }
            }

            if (factors.Count == 0)
            {
                Console.WriteLine($"{num} 没有素因数");
            }
            else
            {
                Console.Write($"{num} 的素因数是：");
                foreach (int f in factors)
                {
                    Console.Write(f + " ");
                }
            }
        }
    }
}
