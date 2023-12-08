using TypeInjection;

namespace ParameterInjection;

internal sealed class TextProcessor : ITextProcessor
{
    private readonly IEncoder encoder;
    internal TextProcessor(IEncoder encoder) => this.encoder = encoder;
    public String Compact(IEnumerable<String> text, IDelimitation delimitation)
    {
        return delimitation.Join(text.Select(l => this.encoder.Encode(l)));
    }
    public IEnumerable<String> Expand(String text, IDelimitation delimitation)
    {
        return delimitation.Split(text); // Not doing the inverse decoding right now...
    }
}
