
namespace TypeInjection;

public static class Delimitations
{
    public static IDelimitation None { get; } = new Empty();
    public static IDelimitation Space { get; } = new Delimitation(" ");
    public static IDelimitation NewLine { get; } = new Delimitation(Environment.NewLine);
    public static IDelimitation Asterisk { get; } = new Delimitation("*");

    private sealed class Delimitation : IDelimitation
    {
        private readonly String delimiter;
        public Delimitation(String delimiter) => this.delimiter = delimiter;
        public String Join(IEnumerable<String> lines) => String.Join(this.delimiter, lines);
        public IEnumerable<String> Split(String block) => block.Split(this.delimiter);
    }

    private sealed class Empty : IDelimitation
    {
        public String Join(IEnumerable<String> lines) => String.Join(String.Empty, lines);
        public IEnumerable<String> Split(String block) => new[] { block };
    }
}
