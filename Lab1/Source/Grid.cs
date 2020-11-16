namespace Lab1
{
    struct Grid
    {
        public float TimeStart { get; }
        public float TimeStep { get; }
        public int Count { get; }

        public Grid(float timeStart, float timeStep, int count)
        {
            TimeStart = timeStart;
            TimeStep = timeStep;
            Count = count;
        }

        public float GetTime(int n)
        {
            return TimeStart + n * TimeStep;
        }

        public string ToString(string format)
        {
            string timeStart = TimeStart.ToString(format);
            string timeStep = TimeStep.ToString(format);
            return GetType().Name + $"{{{timeStart}, {timeStep}, {Count}}}";
        }

        public override string ToString()
        {
            return ToString(null);
        }
    }
}
