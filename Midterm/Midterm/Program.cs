using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new int[] { 1, 6, 3, 8, 9, 10, 1};

            Console.WriteLine("The max value is " + numbers.Max());

            Console.Write("Order of the numbers in the array: ");

            var ordered = numbers.OrderByDescending(i => i).ToArray();

            for(int i = 0; i < numbers.Length; i++)
            {
                Console.Write(ordered[i] + ", ");
            }
            Console.WriteLine();

            var temp = (from i in numbers where i == 1 select i).Count();

            Console.WriteLine("The number of entries equal to 1: " + temp);

            var ordered2 = numbers.OrderByDescending(i => i).Take(3).ToArray();

            Console.Write("The first 3 numbers of an orrder of the numbers in the array: ");

            for (int i = 0; i < ordered2.Length; i++)
            {
                Console.Write(ordered2[i] + ", ");
            }
            Console.WriteLine("Average: " + numbers.Average());

            Console.WriteLine("Sum: " + numbers.Sum());

            var list = numbers.ToList();

            Console.ReadLine();
        }
    }
}
