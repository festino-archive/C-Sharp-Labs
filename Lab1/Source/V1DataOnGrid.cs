using Lab1.Source;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Globalization;

namespace Lab1
{
    class V1DataOnGrid : V1Data, IEnumerable<DataItem>
    {
        public Grid Grid { get; }
        public Vector3[] Values { get; }

        public V1DataOnGrid(string info, DateTime date, Grid grid)
                : base(info, date)
        {
            Grid = grid;
            Values = new Vector3[grid.Count];
        }

        /* format:
         * info line
         * date ("")
         * grid (start step count)
         * vector values (3 float each line, "count" times)*/
        public static V1DataOnGrid FromFile(string filename) // no base constructor
        {
            FileStream fs = null;
            V1DataOnGrid dataSet = null;
            string parsingArg = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                StreamReader istream = new StreamReader(fs);

                string info = istream.ReadLine();
                if (info == null)
                    throw new Exception("no info");

                //System.Console.WriteLine(new DateTime(2004, 10, 2).ToString(CultureInfo.GetCultureInfo("ru")));
                parsingArg = istream.ReadLine();
                if (parsingArg == null)
                    throw new Exception("no date");
                DateTime date = DateTime.Parse(parsingArg, CultureInfo.GetCultureInfo("ru"));

                string[] gridInfo = istream.ReadLine().Split(' ');
                if (gridInfo.Length != 3)
                    throw new Exception($"grid line parts count {gridInfo.Length} != 3");
                parsingArg = gridInfo[0];
                float timeStart = float.Parse(parsingArg);
                parsingArg = gridInfo[1];
                float timeStep = float.Parse(parsingArg);
                parsingArg = gridInfo[2];
                int count = int.Parse(parsingArg);
                Grid grid = new Grid(timeStart, timeStep, count);

                dataSet = new V1DataOnGrid(info, date, grid);
                for (int i = 0; i < dataSet.Values.Length; i++)
                {
                    string[] vecComponents = istream.ReadLine().Split(' ');
                    if (vecComponents.Length != 3)
                        throw new Exception($"{i+1} Values line component count {vecComponents.Length} != 3");
                    parsingArg = vecComponents[0];
                    float v1 = float.Parse(parsingArg);
                    parsingArg = vecComponents[1];
                    float v2 = float.Parse(parsingArg);
                    parsingArg = vecComponents[2];
                    float v3 = float.Parse(parsingArg);
                    dataSet.Values[i] = new Vector3(v1, v2, v3);
                }
            }
            catch (Exception e)
            {
                dataSet = null;
                System.Console.WriteLine($"Parse error: {e.Message}\n(in V1DataOnGrid, on \"{filename}\")");
                if (parsingArg != null && e is FormatException)
                    System.Console.WriteLine($"(while parsing \"{parsingArg}\")");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return dataSet;
        }

        public void InitRandom(float minValue, float maxValue)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i] = new Vector3();
                Values[i].X = RandomUtils.GetFloat(minValue, maxValue);
                Values[i].Y = RandomUtils.GetFloat(minValue, maxValue);
                Values[i].Z = RandomUtils.GetFloat(minValue, maxValue);
            }
        }

        public static explicit operator V1DataCollection(V1DataOnGrid data)
        {
            V1DataCollection dataNew = new V1DataCollection(data.Info, data.Date);
            for (int i = 0; i < data.Values.Length; i++)
            {
                DataItem item = new DataItem(data.Grid.GetTime(i), data.Values[i]);
                dataNew.Values.Add(item);
            }
            return dataNew;
        }

        public override float[] NearZero(float eps)
        {
            List<float> res = new List<float>();
            for (int i = 0; i < Values.Length; i++) {
                Vector3 vec = Values[i];
                if (vec.Length() < eps)
                    res.Add(Grid.GetTime(i));
            }
            return res.ToArray();
        }

        public override string ToString()
        {
            return $"{GetType().Name} {{{base.ToString()}, {Grid}}}";
        }

        public override string ToLongString(string format)
        {
            string gridStr = "\n";
            for (int i = 0; i < Values.Length; i++)
            {
                string time = Grid.GetTime(i).ToString(format);
                string value = Values[i].ToString(format);
                string length = Values[i].Length().ToString(format);
                gridStr += $"\t{{{time} : {value} ({length})}}\n"; // DataItem.ToString(format) duplicate
            }
            return $"{ToString()} : {{{gridStr}}}"; // ToString(format) for Grid data?
        }

        public override string ToLongString()
        {
            return ToLongString(null);
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return new DataEnumerator(Values, Grid);
        }

        private class DataEnumerator : IEnumerator<DataItem>
        {
            private readonly Grid grid;
            private Vector3[] values;
            private int position = -1;

            object IEnumerator.Current => Current;
            public DataItem Current
            {
                get
                {
                    return new DataItem(grid.GetTime(position), values[position]);
                }
            }

            public DataEnumerator(Vector3[] values, Grid grid)
            {
                this.values = values;
                this.grid = grid;
            }

            public bool MoveNext()
            {
                position++;
                return position < values.Length;
            }

            public void Reset()
            {
                position = -1;
            }

            public void Dispose()
            {
                values = null;
            }
        }
    }
}
