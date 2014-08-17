using System;
using System.Diagnostics;
using System.Text;

namespace BenchmarkIt
{
	public class Result
	{
		public Stopwatch Stopwatch { get; internal set;}

		public int TotalIterations { get; internal set; }

		public string Stats {
			get {
				var sb = new StringBuilder ();

				sb.AppendLine ("Total time: " + Stopwatch.Elapsed);
				sb.AppendLine ("Ticks per execute: " + Stopwatch.ElapsedTicks / TotalIterations);
				sb.AppendLine ("Miliseconds per execute: " + Stopwatch.ElapsedMilliseconds / TotalIterations);

				return sb.ToString ();
			}
		}
	}
}

