using Lab1.Source;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    class V1MainCollection : IEnumerable<V1Data>
    {
        private List<V1Data> DataSets = new List<V1Data>();

        public int Count { get => DataSets.Count; }

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
        }

        public override string ToString()
        {
            string res = "\n";

            foreach (V1Data data in DataSets)
            {
                res += "\t" + data.ToString() + "\n";
            }
            return $"{GetType().Name} {{{res}}}";
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
