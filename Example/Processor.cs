using TypeInjection;

namespace Example;

public sealed class Processor
{
    private readonly IDelimitation delimitation;
    private readonly ITextProcessor textProcessor;
    public Processor(ITextProcessor textProcessor, IDelimitation delimitation)
    {
        this.textProcessor = textProcessor;
        this.delimitation = delimitation;
    }
    public String Process(IEnumerable<String> text)
    {
        return this.textProcessor.Compact(text, this.delimitation);
    }
}