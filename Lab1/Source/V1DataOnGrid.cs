using Lab1.Source;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Runtime.Serialization;

namespace Lab1
{
    [Serializable]
    public class V1DataOnGrid : V1Data, IEnumerable<DataItem>, ISerializable
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
         * date ("ru" format)
         * grid (start step count)
         * vector values (3 float each line, "count" times)*/
        public static V1DataOnGrid FromFile(string filename)
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

                parsingArg = istream.ReadLine();
                if (parsingArg == null)
                    throw new Exception("no date");
                DateTime date = DateTime.Parse(parsingArg, DATE_FORMAT);

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

        public string ToString(string format)
        {
            return $"{GetType().Name} {{{base.ToString()}, {Grid.ToString(format)}}}";
        }

        public override string ToString()
        {
            return ToString(null);
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
            return $"{ToString(format)} : {{{gridStr}}}";
        }

        public override string ToLongString()
        {
            return ToLongString(null);
        }

        public V1DataOnGrid(SerializationInfo info, StreamingContext context)
                : base(info.GetString("base_Info"), info.GetDateTime("base_Date"))
        {
            Grid = (Grid)info.GetValue("grid", typeof(Grid));
            Values = new Vector3[Grid.Count];
            for (int i = 0; i < Grid.Count; i++)
            {
                float x = info.GetSingle("v_" + i.ToString() + "_X");
                float y = info.GetSingle("v_" + i.ToString() + "_Y");
                float z = info.GetSingle("v_" + i.ToString() + "_Z");
                Values[i] = new Vector3(x, y, z);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("base_Info", Info);
            info.AddValue("base_Date", Date);
            info.AddValue("grid", Grid, typeof(Grid));
            for (int i = 0; i < Grid.Count; i++)
            {
                info.AddValue("v_" + i.ToString() + "_X", Values[i].X);
                info.AddValue("v_" + i.ToString() + "_Y", Values[i].Y);
                info.AddValue("v_" + i.ToString() + "_Z", Values[i].Z);
            }
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
