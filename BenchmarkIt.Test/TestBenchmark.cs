using System.Threading;
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
			var result = Benchmark.This ("", () => {}).For (10).Iterations();

		    Assert.AreEqual(1, result.Length);
			Assert.AreEqual (10, result[0].TotalIterations, "Failed to run for 10 iterations");

            // Make sure this doesn't throw an exception
            result[0].PrintStats();
		}

		[Test ()]
		public void TestTime ()
		{
			var start = DateTime.Now;
			var result = Benchmark.This("",() => {}).For (2).Seconds();

            Assert.AreEqual(1, result.Length);
			Assert.IsTrue (result[0].Stopwatch.ElapsedMilliseconds >= 1000, "Benchmark did not run for at least 1 second, only ran for " + result[0].Stopwatch.ElapsedMilliseconds + " ms");
		}

		[Test()]
		public void TestCacheWarmup() {
			var result = Benchmark.This ("",() => {}).WithWarmup (10).For (10).Iterations();

			Assert.AreEqual (10, result[0].TotalIterations, "Failed to run for 10 iterations");
		}

        [Test()]
        public void TestMultipleIterations()
        {
            var result = Benchmark
                .This("", () => { })
                .Against.This("", () => Thread.Sleep(10))
                .For(10).Iterations();

            Assert.AreEqual(2, result.Length);
            Assert.Greater(result[1].Stopwatch.ElapsedTicks, result[0].Stopwatch.ElapsedTicks);

            // check that no extension is thrown printing results
            result.PrintComparison();
        }

        [Test()]
        public void TestMultipleTimes()
        {
            var result = Benchmark
                .This("", () => { })
                .Against.This("", () => Thread.Sleep(10))
                .For(1).Seconds();

            Assert.AreEqual(2, result.Length);
            Assert.Greater(result[0].TotalIterations, 1);
            Assert.Greater(result[1].TotalIterations, 1);
            Assert.Greater(result[0].TotalIterations, result[1].TotalIterations);

            // check that no extension is thrown printing results
            result.PrintComparison();
        }

        [Test()]
        public void TestMinutes()
        {
            var result = Benchmark
                .This("", () => { })
                .For(0).Minutes();

            Assert.Greater(result[0].Stopwatch.ElapsedTicks, 0);
        }

        [Test()]
        public void TestHours()
        {
            var result = Benchmark
                .This("", () => { })
                .For(0).Hours();

            Assert.Greater(result[0].Stopwatch.ElapsedTicks, 0);
        }

        [Test()]
        public void TestStats()
        {
            var result = Benchmark
                .This("", () => { })
                .For(10).Iterations();

            Assert.IsTrue(result[0].Stats.Contains(result[0].Stopwatch.ElapsedMilliseconds.ToString("D")));
        }
	}
}

