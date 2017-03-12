using System;

namespace BenchmarkIt
{
    public class BenchmarkSettings
    {
        /// <summary>
        /// The minimum amount of time a batch should take to run
        /// </summary>
        public int BatchSizeTime { get; set; } = 250;
        
        /// <summary>
        /// The acceptable standard deviation of runs in a batch.
        /// </summary>
        public double MinimumErrorToAccept { get; set; } = 1.0d;

        /// <summary>
        /// The number of batches to keep and to calculate the standard deviation for
        /// </summary>
        private int batchesToWorkAcross = 10;
        public int BatchesToWorkAcross
        {
            get
            {
                return batchesToWorkAcross;
            }
            set
            {
                if (value > TDistribution.Values.Length)
                {
                    throw new ArgumentOutOfRangeException($"Maximum value of batches is {TDistribution.Values.Length} as that is the number of t-distrubtion values stored");
                }
                batchesToWorkAcross = value;
            }
        }

        /// <summary>
        /// Maximum amount of time to benchmark for if we haven't got to a standard deviation within our acceptable amount
        /// </summary>
        public int MaxTime { get; set; } = 5000;
    }
}