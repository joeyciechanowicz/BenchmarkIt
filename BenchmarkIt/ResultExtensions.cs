namespace BenchmarkIt
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Compares all the results and prints a comparison
        /// </summary>
        /// <param name="results"></param>
        public static void PrintComparison(this Result[] results)
        {
            Result.PrintComparison(results);
        }
    }
}
