using System;
using System.Linq;
using BenchmarkIt;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            //Benchmark.This(() => Math.Sin(1.23), "Math.Sin")
            //    .Against(() => Math.Cos(1.23), "Math.Cos")
            //    .PrintStats();

            //Benchmark.This(() => Math.Sin(1.23), "Math.Sin")
            //    .Against(() => Math.Cos(1.23), "Math.Cos")
            //    .PrintStats();

            Benchmark.These(new(Action, string)[]{
                (() => Math.Sin(1.23), "Sin1"),
                (() => Math.Sin(1.23), "Sin2"),
                (() => Math.Sin(1.23), "Sin3"),
                //(() => Math.Sin(1.23), "Sin4"),
                //(() => Math.Sin(1.23), "Sin5"),
            }).PrintBasic();

            Console.Read();
		}
	}
}
