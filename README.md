BenchmarkIt
===========

Simple easy .NET benchmarking for little bits of code.

```csharp
Benchmark.This("string.Contains", () => "abcdef".Contains("ef"))
    .Against.This("string.IndexOf", () => "abcdef".IndexOf("ef"))
    .For(5)
    .Seconds().PrintComparison();
```
```
Name    Iterations      Percent
string.Contains 26168118        100%
string.IndexOf  19055089        72.8179573326595%
```


# TODO
* Investigate manually unrolling the benchmark loops a bit
* Speed up time loop
* Generate graphs?
