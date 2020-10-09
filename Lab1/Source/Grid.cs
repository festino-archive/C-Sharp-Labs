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

        public override string ToString()
        {
            return GetType().Name + $"{{{TimeStart}, {TimeStep}, {Count}}}";
        }
    }
}
