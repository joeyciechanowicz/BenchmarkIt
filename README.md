BenchmarkIt
===========

Simple easy .NET benchmarking for little bits of code.

```csharp
Benchmark.This(()="abcdef".Contains("ab"))
  .For(10).Seconds.Run("containslel");
```
