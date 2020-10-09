using Lab1.Source;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab1
{
    class V1DataCollection : V1Data
    {
        public List<DataItem> Values { get; } = new List<DataItem>();

        public V1DataCollection(string info, DateTime date)
                : base(info, date) { }

        public void InitRandom(int nItems, float tmin, float tmax, float minValue, float maxValue)
        {
            for (int i = 0; i < nItems; i++)
            {
                float x = RandomUtils.GetFloat(minValue, maxValue);
                float y = RandomUtils.GetFloat(minValue, maxValue);
                float z = RandomUtils.GetFloat(minValue, maxValue);
                float time = RandomUtils.GetFloat(tmin, tmax);
                DataItem item = new DataItem(time, new Vector3(x, y, z));
                Values.Add(item);
            }
        }

        public override float[] NearZero(float eps)
        {
            List<float> res = new List<float>();
            foreach (DataItem item in Values)
            {
                if (item.Value.Length() < eps)
                    res.Add(item.T);
            }
            return res.ToArray();
        }

        public override string ToString()
        {
            return $"{GetType().Name} {{{base.ToString()}, {Values.Count}}}";
        }

        public override string ToLongString()
        {
            string str = "\n";
            foreach (DataItem item in Values)
            {
                str += $"\t{{{item.T} : {item.Value}}}\n";
            }
            return $"{ToString()} : {{{str}}}";
        }
    }
}
