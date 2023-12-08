namespace TypeInjection;

public interface IDelimitation
{
    String Join(IEnumerable<String> lines);
    IEnumerable<String> Split(String block);
}
