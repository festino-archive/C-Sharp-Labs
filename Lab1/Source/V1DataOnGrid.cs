using Lab1.Source;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

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

        public IEnumerator<DataItem> GetEnumerator()
        {
            return new DataEnumerator(Values, Grid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
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
