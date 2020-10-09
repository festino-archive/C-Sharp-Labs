using System;

namespace Lab1.Source
{
    static class RandomUtils
    {
        static Random random;

        static RandomUtils()
        {
            random = new Random();
        }

        public static float GetFloat(float minValue, float maxValue)
        {
            return (float) (minValue + (maxValue - minValue) * random.NextDouble());
        }

        public static int GetInt(int minValue, int maxValue)
        {
            return (int)(minValue + (maxValue + 1 - minValue) * random.NextDouble());
        }
    }
}
