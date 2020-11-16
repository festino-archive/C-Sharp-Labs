using System.Numerics;

namespace Lab1
{
    struct DataItem
    {
        public float T { get; }
        public Vector3 Value { get; }

        public DataItem(float t, Vector3 value)
        {
            T = t;
            Value = value;
        }

        public string ToString(string format)
        {
            string time = T.ToString(format);
            string value = Value.ToString(format);
            string length = Value.Length().ToString(format);
            return $"({time} : {value} ({length}))";
        }

        public override string ToString()
        {
            return ToString(null);
        }
    }
}
