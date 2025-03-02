using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        {
            int[] numbers = GetNumbersFromUser();
            var results = CalculateStatistics(numbers);
            DisplayResults(results);
        }

        private static int[] GetNumbersFromUser()
        {
            while (true)
            {
                Console.Write("请输入整数（用空格分隔）：");
                string input = Console.ReadLine();

                try
                {
                    return input.Split(' ')
                               .Select(int.Parse)
                               .ToArray();
                }
                catch
                {
                    Console.WriteLine("输入格式错误，请重新输入！");
                }
            }
        }

        private static (int max, int min, double average, int sum) CalculateStatistics(int[] numbers)
        {
            int max = numbers[0];
            int min = numbers[0];
            int sum = 0;

            foreach (int num in numbers)
            {
                if (num > max) max = num;
                if (num < min) min = num;
                sum += num;
            }

            double average = (double)sum / numbers.Length;
            return (max, min, average, sum);
        }

        private static void DisplayResults((int max, int min, double avg, int sum) results)
        {
            Console.WriteLine($"最大值：{results.max}");
            Console.WriteLine($"最小值：{results.min}");
            Console.WriteLine($"平均值：{results.avg:F2}");
            Console.WriteLine($"元素总和：{results.sum}");
        }
    }
}
