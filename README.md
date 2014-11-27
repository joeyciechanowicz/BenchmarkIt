BenchmarkIt
===========

Simple easy .NET benchmarking for little bits of code.

```csharp
Benchmark.This(()=
  {
    "abcdef".Contains("ab")
  })
.For(10).Seconds
.Run("Contains ab");
```


# Future spec
```csharp
Benchmark
.This(Action, [string label])
( .Against.This(Action, [string label]) )*
( .With.CacheWarmup(int amount) )?
.For(int amount)
.( Seconds() | Minutes() | Hours() | Iterations() )
>>>>>>> a73b1ab2c13c05a7b3a88e172da15c77726efdc1
```
