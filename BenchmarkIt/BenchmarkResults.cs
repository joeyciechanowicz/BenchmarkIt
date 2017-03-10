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

        public void PrintStats()
        {
            Console.WriteLine(GetFormattedString());
        }

        public string GetFormattedString()
        {
            var fastest = results.OrderBy(x => x.OperationsPerSecond).First();

            var output = String.Format("{0,-10}|{1,-10}|{2,-25}|{3,-15}\r\n", "Label", "Runs", "Ops/Sec", "% Slower");
            output += String.Format("{0,-10}+{0,-10}+{1,-25}+{2,-15}\r\n", new String('-', 10), new String('-', 25), new String('-', 15));
            foreach (var result in results)
            {
                var amountSlower = ((result.OperationsPerSecond / fastest.OperationsPerSecond) * 100.0d) - 100.0d;
                output += String.Format("{0,-10}|{1,-10}|{2,-25}|{3,-15}\r\n", 
                    result.Label, 
                    result.Runs.Count(), 
                    SencibleDouble(result.OperationsPerSecond) + " +/-" + SencibleDouble(result.Error) + "%", 
                    amountSlower == 0.0d ? "fastest" : SencibleDouble(amountSlower));
            }
            
            return output;
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