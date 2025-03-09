using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public abstract class Shape
    {
        public abstract double CalculateArea();
        public abstract bool IsValid();
    }

    public class Rectangle : Shape
    {
        public double Length { get; set; }
        public double Width { get; set; }

        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public override double CalculateArea() => Length * Width;
        public override bool IsValid() => Length > 0 && Width > 0;
    }

    public class Square : Shape
    {
        public double Side { get; set; }

        public Square(double side)
        {
            Side = side;
        }

        public override double CalculateArea() => Side * Side;
        public override bool IsValid() => Side > 0;
    }

    public class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double a, double b, double c)
        {
            SideA = a;
            SideB = b;
            SideC = c;
        }

        public override double CalculateArea()
        {
            double s = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        public override bool IsValid()
        {
            return SideA > 0 && SideB > 0 && SideC > 0 &&
                   SideA + SideB > SideC &&
                   SideA + SideC > SideB &&
                   SideB + SideC > SideA;
        }
    }

    // 简单工厂类
    public static class ShapeFactory
    {
        private static readonly Random _random = new Random();

        public static Shape CreateRandomShape()
        {
            int type = _random.Next(0, 3); // 生成0-2的随机数

            
            if (type == 0)
            {
                return new Rectangle(
                    _random.NextDouble() * 10 + 1,
                    _random.NextDouble() * 10 + 1
                );
            }
            else if (type == 1)
            {
                return new Square(
                    _random.NextDouble() * 10 + 1
                );
            }
            else if (type == 2)
            {
                // 三角形生成需要验证合法性
                while (true)
                {
                    double a = _random.NextDouble() * 10 + 1;
                    double b = _random.NextDouble() * 10 + 1;
                    double c = _random.NextDouble() * 10 + 1;

                    if (a + b > c && a + c > b && b + c > a)
                    {
                        return new Triangle(a, b, c);
                    }
                }
            }
            throw new InvalidOperationException("Unexpected shape type");
        }
    }

    class Program
    {
        static void Main()
        {
            double totalArea = 0;
            for (int i = 0; i < 10; i++)
            {
                Shape shape = ShapeFactory.CreateRandomShape();
                totalArea += shape.CalculateArea();

                
                if (shape is Rectangle rect)
                {
                    Console.WriteLine($"Rectangle: {rect.Length:F2}x{rect.Width:F2} Area: {rect.CalculateArea():F2}");
                }
                else if (shape is Square sq)
                {
                    Console.WriteLine($"Square: {sq.Side:F2}x{sq.Side:F2} Area: {sq.CalculateArea():F2}");
                }
                else if (shape is Triangle tri)
                {
                    Console.WriteLine($"Triangle: {tri.SideA:F2},{tri.SideB:F2},{tri.SideC:F2} Area: {tri.CalculateArea():F2}");
                }
            }
            Console.WriteLine($"Total Area: {totalArea:F2}");
        }
    }
}
