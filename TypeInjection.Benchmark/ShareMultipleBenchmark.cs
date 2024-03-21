using BenchmarkDotNet.Attributes;
using TypeInjection.Sharing;
using ParameterInjection;

using P = ParameterInjection;
using E = ParameterInjection.Encodings;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class ShareMultipleBenchmark
{
    private readonly Serializer tSerializer = Serializer.With<HomogeniseNewLines>();
    private readonly AllocFreeSerializer tFreeSerializer = AllocFreeSerializer.With<Flatten>();

    private readonly P.Sharing.Serializer pSerializerA = new(E.HomogeniseNewLines);
    private readonly P.Sharing.Serializer pSerializerB = new(E.Flatten);

    [Benchmark(Baseline = true)]
    public ITextProcessor Parameterised() => new TextProcessor(new Combo(this.pSerializerA.Encoder, this.pSerializerB.Encoder));

    [Benchmark]
    public ITextProcessor TypeInjection()
    {
        var rightInjector = this.tFreeSerializer.Inject(ComboProcessingCreator.Item);
        return this.tSerializer.Injector.Inject(rightInjector);
    }
}

/*
// * Summary *

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean      | Error     | Ratio | Allocated | 
|-------------- |----------:|----------:|------:|----------:|
| Parameterised |  7.548 ns | 0.1763 ns |  1.00 |      56 B |
| TypeInjection | 28.267 ns | 0.5373 ns |  3.76 |         - | 
*/