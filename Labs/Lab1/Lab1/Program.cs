using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1
{
    class Program
    {

        static void Main(string[] args)
        {
            List<string> words = new List<string>();
            List<string> tempList;

            bool running = true;
            string choice;
            string prompt = "Hello World!!! My first C# App\n" +
                "Options\n" +
                "----------\n" +
                "1 - Import Words From File\n" +
                "2 - Bubble Sort words\n" +
                "3 - LINQ/Lambda sort words\n" +
                "4 - Count the Distinct Words\n" +
                "5 - Take the first 10 words\n" +
                "6 - Get the number of words that start with 'j' and display the count\n" +
                "7 - Get and display of words that end with 'd' and display the count\n" +
                "8 - Get and display of words that are greater than 4 characters long, and display the count\n" +
                "9 - Get and display of words that are less than 3 characters long and start with the letter 'a', and display the count\n" +
                "x - Exit\n\n" +
                "Make a selection: ";

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write(prompt);
                choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    choice = "a";
                }

                Console.Clear();

                switch (choice[0])
                {
                    case '1':
                        words = ImportWords(words);
                        break;
                    case '2':
                        tempList = new List<string>(words);
                        BubbleSort(tempList);
                        break;
                    case '3':
                        tempList = new List<string>(words);
                        LambdaSort(tempList);
                        break;
                    case '4':
                        CountDistinct(words);
                        break;
                    case '5':
                        FirstTenWords(words);
                        break;
                    case '6':
                        startJWords(words);
                        break;
                    case '7':
                        endDWords(words);
                        break;
                    case '8':
                        greaterThanFour(words);
                        break;
                    case '9':
                        lessThanThree(words);
                        break;
                    case 'x':
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        break;
                }
                Console.Write("\n\n");
            }
        }

        //1 - Import Words From File
        static List<string> ImportWords(List<string> words)
        {
            int count = 0;

            Console.WriteLine("Reading Words");

            using (StreamReader sR = new StreamReader("Words.txt"))
            {
                string word;

                while ((word = sR.ReadLine()) != null)
                {
                    words.Add(word);
                    count++;
                }
            }

            Console.WriteLine("Reading Words complete\n" +
                "Number of words found: " + count
                );

            return words;
        }

        //2 - Bubble Sort words
        static string[] BubbleSort(List<string> words)
        {
            string temp;
            bool swap;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (words.Count != 0)
            {           
                do
                {
                    swap = false;
                    for (int i = 0; i < words.Count - 1; i++)
                    {
                        if (string.Compare(words[i], words[i + 1]) < 0)
                        {
                            temp = words[i];
                            words[i] = words[i + 1];
                            words[i + 1] = temp;
                            swap = true;
                        }
                    }
                } while (swap);

                watch.Stop();
            }
            Console.WriteLine("Time elapsed: " + watch.ElapsedMilliseconds + "ms");          

            return words.ToArray();
        }

        //3 - LINQ/Lambda sort words
        static string[] LambdaSort(List<string> words)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (words.Count != 0)
            {
                words = words.OrderByDescending(x => x).ToList<string>();

                watch.Stop();
            }
            Console.WriteLine("Time elapsed: " + watch.ElapsedMilliseconds + "ms");          

            return words.ToArray();
        }

        //4 - Count the Distinct Words
        static void CountDistinct(List<string> words)
        {
            int count = 0;

            if (words.Count != 0)
            {
                count = (from x in words select x).Distinct().Count();
            }

            Console.WriteLine("The number of distinct words is: " + count);
        }

        //5 - Take the first 10 words
        static void FirstTenWords(List<string> words)
        {
            if (words.Count != 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(words[i]);
                }
            }
        }

        //6 - Get the number of words that start with 'j' and display the count
        static void startJWords(List<string> words)
        {
            var result = from s in words where s.StartsWith("j") select s;

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine("Number of words that start with 'j': " + result.Count());
        }

        //7 - Get and display of words that end with 'd' and display the count
        static void endDWords(List<string> words)
        {
            var result = from s in words where s.EndsWith("d") select s;

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine("Number of words that ends with 'd': " + result.Count());
        }

        //8 - Get and display of words that are greater than 4 characters long, and display the count
        static void greaterThanFour(List<string> words)
        {
            var result = from s in words where s.Count() > 4 select s;

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine("Number of words longer than 4 characters: " + result.Count());
        }

        //9 - Get and display of words that are less than 3 characters long and start with the letter 'a', and display the count
        static void lessThanThree(List<string> words)
        {
            var result = from s in words where s.Count() < 3 && s.StartsWith("a") select s;

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine("Number of words longer less than 3 characters and start with 'a': " + result.Count());
        }
    }
}
