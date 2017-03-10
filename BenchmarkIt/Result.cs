using System.Collections.Generic;

namespace BenchmarkIt
{
    public class Result
    {
        public string Label { get; internal set; }
        public int BatchSize { get; internal set; }
        public int BatchTime { get; internal set; }
        public IList<Run> Runs { get; internal set; }

        public double OperationsPerSecond
        {
            get
            {
                return Runs[Runs.Count - 1].OperationsPerSecond;
            }
        }

        public double MeanTicksPerBatch
        {
            get
            {
                return Runs[Runs.Count - 1].MeanTicks;
            }
        }

        public double Variance
        {
            get
            {
                return Runs[Runs.Count - 1].Variance;
            }
        }

        public double StandardDeviation
        {
            get
            {
                return Runs[Runs.Count - 1].StandardDeviation;
            }
        }

        public double StandardErrorsMean
        {
            get
            {
                return Runs[Runs.Count - 1].StandardErrorsMean;
            }
        }

        public double Error
        {
            get
            {
                return Runs[Runs.Count - 1].Error;
            }
        }
    }
}