BenchmarkIt
===========

Simple easy .NET benchmarking for POCs

'''csharp
Benchmark.This(()="abcdef".Contains("ab"))
  .For(10).Seconds.Run("containslel");
'''
