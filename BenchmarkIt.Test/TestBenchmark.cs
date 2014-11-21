using NUnit.Framework;
using System;

namespace BenchmarkIt.Test
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestIterations ()
		{
			var result = Benchmark.This (() => {
			})
				.For (10).Iterations.Run ();

			Assert.AreEqual (10, result.TotalIterations, "Failed to run for 10 iterations");
		}

		[Test ()]
		public void TestTime ()
		{
			var start = DateTime.Now;
			var result = Benchmark.This (() => {
			})
				.For (2).Seconds.Run ();

			Assert.IsTrue (result.Stopwatch.ElapsedMilliseconds >= 1000, "Benchmark did not run for at least 1 second, only ran for " + result.Stopwatch.ElapsedMilliseconds + " ms");
		}

		[Test()]
		public void TestCacheWarmup() {
			var result = Benchmark.This (() => {
			}).For (10).Iterations.WithCacheWarmup (10).Run ();

			Assert.AreEqual (10, result.TotalIterations, "Failed to run for 10 iterations");
		}
	}
}

