using System;
using System.Numerics;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1
            Console.WriteLine("[1]\n");
            float t1 = 0.001f;
            Grid grid1 = new Grid(0, t1, 20);
            DateTime dateTime1 = new DateTime(2015, 11, 14, 17, 01, 47);
            V1DataOnGrid data1 = new V1DataOnGrid("id1", dateTime1, grid1);
            for (int i = 0; i < data1.Values.Length; i++)
            {
                data1.Values[i] = new Vector3(i, i * i, 3 * i - 4) * t1;
            }
            Console.WriteLine(data1.ToLongString());
            V1DataCollection data2 = (V1DataCollection) data1;
            Console.WriteLine(data2.ToLongString());

            // 2
            Console.WriteLine("\n[2]\n");
            V1MainCollection mainColl = new V1MainCollection();
            mainColl.AddDefaults();
            Console.WriteLine(mainColl);

            // 3
            Console.WriteLine("\n[3] eps=0.1\n");
            PrintNearZero(mainColl, 0.1f);
            Console.WriteLine("\n[3] eps=0.5\n");
            PrintNearZero(mainColl, 0.5f);
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
