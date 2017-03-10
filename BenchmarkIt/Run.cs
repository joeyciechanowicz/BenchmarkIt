namespace BenchmarkIt
{
    // This class is a bit confusing. It actually stores the current values for all
    // its props, as calculated across all previous runs. So MeanTicks is the mean ticks across all batches (or last 10)
    public class Run
    {
        public long Ticks { get; internal set; }
        public double MeanTicks { get; internal set; }
        public double Variance { get; internal set; }
        public double StandardDeviation { get; internal set; }
        public double StandardErrorsMean { get; internal set; }
        public double Error { get; internal set; }
        public double OperationsPerSecond { get; internal set; }
    }
}