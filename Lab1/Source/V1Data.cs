﻿using System;

namespace Lab1
{
    abstract class V1Data
    {
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
            return $"[{Date}] {Info}";
        }
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
    }
}
