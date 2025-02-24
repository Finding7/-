using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_SHARP_COTN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("请输入第一个数字：");
            if (!double.TryParse(Console.ReadLine(), out double num1))
            {
                Console.WriteLine("错误：第一个数字输入无效。");
                return;
            }

            Console.Write("请输入运算符（+、-、*、/）：");
            string op = Console.ReadLine();
            if (op != "+" && op != "-" && op != "*" && op != "/")
            {
                Console.WriteLine("错误：无效的运算符。");
                return;
            }

            Console.Write("请输入第二个数字：");
            if (!double.TryParse(Console.ReadLine(), out double num2))
            {
                Console.WriteLine("错误：第二个数字输入无效。");
                return;
            }

            double result = 0;
            bool Error = false;

            switch (op)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("错误：除数不能为零。");
                        Error = true;
                    }
                    else
                    {
                        result = num1 / num2;
                    }
                    break;
            }

            // 输出结果
            if (!Error)
            {
                Console.WriteLine($"计算结果：{result}");
            }
        }
    }
}
