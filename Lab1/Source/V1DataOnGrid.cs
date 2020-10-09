using Lab1.Source;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab1
{
    class V1DataOnGrid : V1Data
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

        public override string ToLongString()
        {
            string gridStr = "\n";
            for (int i = 0; i < Values.Length; i++)
            {
                gridStr += $"\t{{{Grid.GetTime(i)} : {Values[i]}}}\n";
            }
            return $"{ToString()} : {{{gridStr}}}";
        }
    }
}
