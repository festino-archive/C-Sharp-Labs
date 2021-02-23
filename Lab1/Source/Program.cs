using System;
using System.Numerics;
using System.Threading;
using System.Globalization;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");

            // 1 - V1DataOnGrid.FromFile()
            Console.WriteLine("[1]\n");
            V1DataOnGrid dataFromFile = V1DataOnGrid.FromFile("grid-1.txt");
            Console.WriteLine(dataFromFile?.ToLongString("f4"));

            // 2 - V1MainCollection, AddDefaults
            Console.WriteLine("\n\n\n[2]\n");
            V1MainCollection mainColl = new V1MainCollection();
            mainColl.AddDefaults();
            Console.WriteLine(mainColl);
            Console.WriteLine("Max vector length : " + mainColl.MaxLength);
            Console.WriteLine("DataItem with max length : " + mainColl.MaxValue.ToString("f5"));
            Console.WriteLine("Time dublicates : " + string.Join(", ", mainColl.Dublicates));

            Console.WriteLine("\n\n\n[3]\n");
            mainColl.Save("test.dat");
            mainColl.Load("test.dat");
            Console.WriteLine(mainColl);
        }

        static void PrintNearZero(V1MainCollection mainColl, float eps)
        {

            foreach (V1Data data in mainColl)
            {
                float[] time = data.NearZero(eps);
                string str = "";
                for (int i = 0; i < time.Length; i++)
                {
                    str += time[i] + ", ";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 2);
                }
                Console.WriteLine($"{{ {str} }}");
            }
        }
    }
}
