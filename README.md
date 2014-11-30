Benchmark.It
===========

Simple easy .NET benchmarking for little bits of code. When you just really want to see if one method is actually faster than another.

## Install
Run the following command in the Package Manager Console (NuGet).
```bash
PM> Install-Package Benchmark.It
```
Or clone and include BenchmarkIt.csproj directly
# Use
Lets say you wanted to see if string.Contains was faster or slower than string.IndexOf. Simply write the following and have it printed out nicely for you to see.
```csharp
Benchmark.This("string.Contains", () => "abcdef".Contains("ef"))
    .Against.This("string.IndexOf", () => "abcdef".IndexOf("ef"))
    .For(5)
    .Seconds().PrintComparison();
```
```
Name    Iterations      Percent
Name                                    Iterations          Percent                       
string.Contains                         23117812            100%                          string.IndexOf                          10852501            46.9%
```

Or you wanted to see if a for loop was actually faster than a foreach loop (it is).
```csharp
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
```
```
Name                                    Milliseconds        Percent                       
for.Count                               34305               920.8%                        for.Length                              3725                100%                          foreach                                 4341                116.5%
```

And why stop there, you can add as many different methods as you want.
```csharp
Benchmark.This("empty 1", () => { })
 .Against.This("empty 2", () => { })
 .Against.This("empty 3", () => { })
 .Against.This("empty 4", () => { })
 .Against.This("empty 5", () => { })
 .Against.This("empty 6", () => { })
 .For(1).Minutes().PrintComparison();
```

You can also just benchmark one method, and on top of that you can specify a number of _warmpup_ loops to perform first.
```csharp
Benchmark.This("string.Contains", () => "abcdef".Contains("ef"))
    .WithWarmup(1000)
    .For(5).Seconds()
    .PrintComparison();
```

# TODO
* Improve result print, clean it up etc.
* Investigate manually unrolling the benchmark loops a bit
* Speed up time loop
* Generate graphs?
