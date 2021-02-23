using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace Lab1
{
    [Serializable]
    public struct DataItem : ISerializable
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

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            T = info.GetSingle("T");
            float x = info.GetSingle("Value_X");
            float y = info.GetSingle("Value_Y");
            float z = info.GetSingle("Value_Z");
            Value = new Vector3(x, y, z);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("T", T);
            info.AddValue("Value_X", Value.X);
            info.AddValue("Value_Y", Value.Y);
            info.AddValue("Value_Z", Value.Z);
        }
    }
}
