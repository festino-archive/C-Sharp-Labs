using System;
using System.Threading;
using System.Globalization;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");

            V1MainCollection mainColl = new V1MainCollection();
            mainColl.DataChanged += DataChangedEventHandler;
            mainColl.AddDefaults();
            V1Data data = mainColl[2];
            mainColl.Remove(data.Info, data.Date);
            mainColl[2] = data;
            mainColl[1].Info += " (edited)";
        }

        static void DataChangedEventHandler(object source, DataChangedEventArgs args)
        {
            Console.WriteLine(args);
        }
    }
}
