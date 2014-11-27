using System;
using System.Diagnostics;
using System.Linq;
using BenchmarkIt;
using System.Threading;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Benchmark.This("string.Contains", () => "abcdef".Contains("ef"))
            .Against.This("string.IndexOf", () => "abcdef".IndexOf("ef"))
            .For(5)
            .Seconds().PrintComparison();

		    Console.Read();

		    Benchmark.This("Fast", () =>
		    {
		        int i;
		    })
            .Against.This("Median",() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            })
            .Against.This("Slow", () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            })
            .For(100000)
            .Iterations()
            .PrintComparison();
            
            Console.WriteLine();

		    Benchmark.This("Fast", () =>
		    {
		        int i;
		    })
            .Against.This("Median", () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            })
            .Against.This("Slow", () =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    var x = Math.Sin(Math.Cos(i));
                }
            })
            .For(5)
            .Seconds()
            .PrintComparison();

		    Console.Read();
		}
	}
}
