using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class SievePrimeFinder
    {
        static void Main()
        {
            int maxNumber = 100;
            List<int> primes = FindPrimes(maxNumber);
            PrintPrimes(primes);
        }

        private static List<int> FindPrimes(int upperLimit)
        {
            bool[] isPrime = new bool[upperLimit + 1];
            InitializeArray(isPrime);

            for (int currentNumber = 2; currentNumber * currentNumber <= upperLimit; currentNumber++)
            {
                if (isPrime[currentNumber])
                {
                    MarkMultiples(isPrime, currentNumber);
                }
            }

            return CollectPrimes(isPrime);
        }

        private static void InitializeArray(bool[] isPrime)
        {
            for (int i = 2; i < isPrime.Length; i++)
            {
                isPrime[i] = true;
            }
        }

        private static void MarkMultiples(bool[] isPrime, int number)
        {
            for (int multiple = number * 2; multiple < isPrime.Length; multiple += number)
            {
                isPrime[multiple] = false;
            }
        }

        private static List<int> CollectPrimes(bool[] isPrime)
        {
            List<int> primes = new List<int>();
            for (int i = 2; i < isPrime.Length; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
            return primes;
        }

        private static void PrintPrimes(List<int> primes)
        {
            Console.WriteLine("2~100之间的素数：");
            foreach (int prime in primes)
            {
                Console.Write(prime + " ");
            }
        }
    }
}
