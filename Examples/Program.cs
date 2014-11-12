using System;
using System.Diagnostics;
using BenchmarkIt;
using System.Threading;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
		    var quickIterationResult = Benchmark.This(() =>
		    {
		        int i;
		    }).For(100000).Iterations.Run("Fast");

            var meadianIterationResult = Benchmark.This(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            }).For(100000).Iterations.Run("Median");

            var slowIterationResult = Benchmark.This(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            }).For(100000).Iterations.Run("Slow");

            quickIterationResult.Compare(meadianIterationResult, slowIterationResult);
		    
            Console.WriteLine();

		    var quickTimeResult = Benchmark.This(() =>
		    {
		        int i;
		    }).For(5).Seconds.Run("Fast");

            var medianTimeResult = Benchmark.This(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            }).For(5).Seconds.Run("Median");

            var slowTimeResult = Benchmark.This(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            }).For(5).Seconds.Run("Slow");

            Result.PrintComparison(quickTimeResult, medianTimeResult, slowTimeResult);
		    Console.Read();
		}
	}
}
