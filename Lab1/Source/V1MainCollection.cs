using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    class V1MainCollection : IEnumerable<V1Data>
    {
        private List<V1Data> DataSets = new List<V1Data>();

        public int Count { get => DataSets.Count; }
        public float MaxLength {
            get => DataSets.Max(dataSet =>
                dataSet.Count() == 0 ? 0 : dataSet.Max(x => x.Value.Length())
            );
        }
        public DataItem MaxValue {
            get
            {
                var united = DataSets.SelectMany(x => x);
                if (united.Count() == 0) return new DataItem();
                return united.Aggregate((max, v) => v.Value.LengthSquared() > max.Value.LengthSquared() ? v : max);
            }
        }
        public IEnumerable<float> Dublicates
        {
            get
            {
                return DataSets.SelectMany(dataSet => dataSet)
                        .GroupBy(v => v.T)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key);
            }
        }

        public void Add(V1Data item)
        {
            DataSets.Add(item);
        }

        public bool Remove(string id, DateTime dateTime)
        {
            bool res = false;
            for (int i = DataSets.Count - 1; i >= 0; i--)
            {
                if (DataSets[i].Info.Equals(id) && DataSets[i].Date.Equals(dateTime))
                {
                    DataSets.RemoveAt(i);
                    res = true;
                }
            }
            return res;
        }

        public void AddDefaults()
        {
            int onGridCount = 2;
            int collectionCount = 2;
            for (int i = 0; i < onGridCount; i++)
            {
                V1DataOnGrid data = new V1DataOnGrid($"id={i}", DateTime.Now, new Grid(0, 0.1f, 10));
                data.InitRandom(-1, 1);
                Add(data);
            }
            for (int i = 1; i <= collectionCount; i++)
            {
                V1DataCollection data = new V1DataCollection($"id={i}", DateTime.Now);
                data.InitRandom(10 + 2 * i, 0, 100, -1, 1);
                Add(data);
            }
            Add(new V1DataOnGrid($"DoG_empty", DateTime.Now, new Grid(0, 1f, 0)));
            Add(new V1DataCollection($"DC_empty", DateTime.Now));
        }

        public override string ToString()
        {
            string res = string.Join("\n\t", DataSets);
            return $"{GetType().Name} {{\n\t{res}\n}}";
        }

        public string ToLongString(string format)
        {
            string res = string.Join("\n\t", from dataSet in DataSets select dataSet.ToLongString(format));
            return $"{GetType().Name} {{\n\t{res}\n}}";
        }

        public string ToLongString()
        {
            return ToLongString(null);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)DataSets).GetEnumerator();
        }

        IEnumerator<V1Data> IEnumerable<V1Data>.GetEnumerator()
        {
            return ((IEnumerable<V1Data>)DataSets).GetEnumerator();
        }
    }
}
