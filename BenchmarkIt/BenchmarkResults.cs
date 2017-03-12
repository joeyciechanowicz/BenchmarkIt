using System;
using System.Collections.Generic;
using System.Linq;

namespace BenchmarkIt
{
    public class BenchmarkResults
    {
        private Result[] results;

        internal BenchmarkResults(Result[] results)
        {
            this.results = results;
        }

        public Result this[int i]
        {
            get
            {
                return results[i];
            }
        }
        
        public int Length { get { return results.Length; } }

        public void PrintBasic()
        {
            var fastest = results.OrderBy(x => x.OperationsPerSecond).First();

            var output = new List<string>();
            var fastestRow = 0;
            var prevColor = Console.ForegroundColor;

            output.Add(String.Format("{0,-10}|{1,-10}|{2,-25}|{3,-15}", "Label", "Runs", "Ops/Sec", "% Slower"));
            output.Add(String.Format("{0,-10}+{0,-10}+{1,-25}+{2,-15}", new String('-', 10), new String('-', 25), new String('-', 15)));
            foreach (var result in results)
            {
                var amountSlower = ((result.OperationsPerSecond / fastest.OperationsPerSecond) * 100.0d) - 100.0d;

                if (amountSlower == 0.0d)
                {
                    prevColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                output.Add(String.Format("{0,-10}|{1,-10}|{2,-25}|{3,-15}",
                    result.Label,
                    result.Runs.Count(),
                    SencibleDouble(result.OperationsPerSecond) + " +/-" + SencibleDouble(result.Error) + "%",
                    amountSlower == 0.0d ? "fastest" : SencibleDouble(amountSlower)));
                fastestRow = amountSlower == 0.0d ? output.Count() - 1 : fastestRow;

                Console.ForegroundColor = prevColor;
            }
        }

        private string SencibleDouble(double input)
        {
            if (input > 1000)
            {
                return String.Format("{0:n0}", input);
            }
            return String.Format("{0:n3}", input);
        }
    }
}