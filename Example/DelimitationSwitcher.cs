using TypeInjection;
using TypeInjection.Builder;

namespace Example;

public sealed class DelimitationSwitcher : ISwitcher
{
    private static readonly Dictionary<String, IDelimitation> delimitations = new()
    {
        ["-"] = Delimitations.None,
        ["s"] = Delimitations.Space,
        ["*"] = Delimitations.Asterisk,
        ["nl"] = Delimitations.NewLine
    };

    private readonly ITextProcessor textProcessor;
    private IDelimitation delimitation = Delimitations.None;
    public ISwitcher Current => this;
    public DelimitationSwitcher(ITextProcessor textProcessor)
    {
        var options = String.Join(", ", delimitations.Keys);
        Console.WriteLine($"Delimitations: {options}");
        this.textProcessor = textProcessor;
    }
    public Processor Build() => new(this.textProcessor, this.delimitation);
    public Boolean MoveNext()
    {
        Console.Write("Add delimitation: ");
        var choice = Console.ReadLine()?.Trim().ToLowerInvariant() ?? String.Empty;
        if (delimitations.TryGetValue(choice, out var delimitation))
        {
            this.delimitation = delimitation;
            return false;
        }
        return true;
    }
}