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
			var sleepResult = Benchmark
				.This (() => Thread.Sleep (500))
				.For (10).Seconds
				.Run ();

            Debug.Write(sleepResult.Stats);
		}
	}
}
