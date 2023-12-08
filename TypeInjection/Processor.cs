
namespace TypeInjection;

internal static class Processor
{
    public static ITextProcessor With<TEncoding>()
        where TEncoding : IEncoding => Impl<TEncoding>.Item;

    private sealed class Impl<TEncoding> : ITextProcessor
        where TEncoding : IEncoding
    {
        public static Impl<TEncoding> Item { get; } = new Impl<TEncoding>();
        public String Compact(IEnumerable<String> text, IDelimitation delimitation)
        {
            return delimitation.Join(text.Select(l => TEncoding.Encode(l)));
        }

        public IEnumerable<String> Expand(String text, IDelimitation delimitation)
        {
            return delimitation.Split(text); // Not doing the inverse decoding right now...
        }
    }
}
