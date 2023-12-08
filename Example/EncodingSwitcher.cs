using TypeInjection;
using TypeInjection.Builder;

namespace Example;

public sealed class EncodingSwitcher : ISwitcher
{
    private const String bail = "n";
    private static readonly Dictionary<String, Func<ITypeBuilder, ITypeBuilder>> encodings = new()
    {
        ["u"] = b => b.Inject<UpperCase>(),
        ["l"] = b => b.Inject<LowerCase>(),
        ["s"] = b => b.Inject<Separate>(),
        ["h"] = b => b.Inject<HomogeniseNewLines>(),
        ["f"] = b => b.Inject<Flatten>(),
        ["t"] = b => b.Inject<Trim>(),
    };
    private ITypeBuilder builder = Builder.With<None>();
    public ISwitcher Current { get; private set; }
    public EncodingSwitcher()
    {
        var options = String.Join(", ", encodings.Keys);
        Console.WriteLine($"Encodings: {options}");
        Console.WriteLine($"Bail out : {bail}");
        Current = this;
    }
    public Processor Build() => new Processor(this.builder.Build(), Delimitations.None);
    public Boolean MoveNext()
    {
        Console.Write("Add encoding: ");
        var encoding = Console.ReadLine()?.Trim().ToLowerInvariant() ?? String.Empty;
        if (encoding == bail)
        {
            Current = new DelimitationSwitcher(this.builder.Build());
        }
        if (encodings.TryGetValue(encoding, out var append))
        {
            this.builder = append(this.builder);
        }
        return true;
    }
}