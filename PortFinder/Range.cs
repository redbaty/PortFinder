namespace PortFinder
{
    public class Range
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public Range()
        {
        }

        public Range(int max, int min)
        {
            Max = max;
            Min = min;
        }
    }
}