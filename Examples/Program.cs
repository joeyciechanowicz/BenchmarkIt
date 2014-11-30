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

			var values = Enumerable.Range(1, 100000).ToArray();
			Benchmark.This("for.Count", () =>
				{
					for (int i = 0; i < values.Count(); i++)
					{
						int x = values[i];
					}
				})
				.Against.This("for.Length", () =>
					{
						for (int i = 0; i < values.Length; i++) {
							int x = values[i];
						}
					})
				.Against.This("foreach", () =>
					{
						foreach (var x in values) ;
					})
				.For(10000)
				.Iterations()
				.PrintComparison();

			Benchmark.This("string.Contains", () => "abcdef".Contains("ef"))
				.WithWarmup(1000)
				.For(5).Seconds()
				.PrintComparison();
		}
	}
}
