namespace PortFinder
{
    public class Range
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public Range()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="max">The maximum value.</param>
        /// <param name="min">The minimum value.</param>
        public Range(int max, int min)
        {
            Max = max;
            Min = min;
        }
    }
}