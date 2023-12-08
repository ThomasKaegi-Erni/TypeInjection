
namespace TypeInjection.Builder;

public static class Builder
{
    public static IBuilder With<TEncoding>()
        where TEncoding : IEncoding => Build<TEncoding>.Item;

    private sealed class Build<TEncoding> : IBuilder
        where TEncoding : IEncoding
    {
        public static Build<TEncoding> Item { get; } = new Build<TEncoding>();
        public IBuilder Inject<TEncodingArg>()
            where TEncodingArg : IEncoding => Build<Combo<TEncoding, TEncodingArg>>.Item;
        ITextProcessor IBuilder.Build() => Processor.With<TEncoding>();
    }
}