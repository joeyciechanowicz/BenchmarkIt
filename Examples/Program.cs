using System;
using BenchmarkIt;
using System.Threading;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var sleepResult = Benchmark
				.This (() => {
					Thread.Sleep (1);
				})
				.For (10).Seconds
				.Run ();

			Console.Write (sleepResult.Stats);
		}
	}
}
