namespace TypeInjection;

public interface ITextProcessor
{
    String Compact(IEnumerable<String> text, IDelimitation delimitation);
    IEnumerable<String> Expand(String text, IDelimitation delimitation);
}
