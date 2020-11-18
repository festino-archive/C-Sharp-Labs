using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Lab1
{
    abstract class V1Data : IEnumerable<DataItem>
    {
        protected static readonly CultureInfo DATE_FORMAT = CultureInfo.GetCultureInfo("ru");

        public string Info { get; }
        public DateTime Date { get; }

        public V1Data(string info, DateTime date)
        {
            Info = info;
            Date = date;
        }

        public abstract float[] NearZero(float eps);
        public override string ToString()
        {
            return $"[{Date.ToString(DATE_FORMAT)}] {Info}";
        }
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
