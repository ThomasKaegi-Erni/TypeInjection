using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace TypeInjection.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
public class CompareTypeInjectionAgainstNewOperator
{
    [Benchmark(Baseline = true)]
    public ITextProcessor NewClass() => new ClassProcessor();

    [Benchmark]
    public ITextProcessor TypeInjection() => Processor.With<LowerCase>();

    [Benchmark]
    public ITextProcessor NewStruct() => new StructProcessor();

    [Benchmark]
    public StructProcessor NewStruct_AvoidingBoxing() => new StructProcessor();
}

file sealed class ClassProcessor : ITextProcessor
{
    public String Compact(IEnumerable<String> text, IDelimitation delimitation) => throw new NotImplementedException();
    public IEnumerable<String> Expand(String text, IDelimitation delimitation) => throw new NotImplementedException();
}

public readonly struct StructProcessor : ITextProcessor
{
    public String Compact(IEnumerable<String> text, IDelimitation delimitation) => throw new NotImplementedException();
    public IEnumerable<String> Expand(String text, IDelimitation delimitation) => throw new NotImplementedException();
}

/*
// * Summary *

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 8.0.103
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


| Method                   | Mean      | Error     | Ratio | Allocated |
|------------------------- |----------:|----------:|------:|----------:|-
| NewClass                 | 2.9483 ns | 0.0370 ns | 1.000 |      24 B |
| TypeInjection            | 0.8329 ns | 0.0524 ns | 0.283 |         - |
| NewStruct                | 3.0193 ns | 0.0213 ns | 1.025 |      24 B |
| NewStruct_AvoidingBoxing | 0.0000 ns | 0.0000 ns | 0.000 |         - |
*/