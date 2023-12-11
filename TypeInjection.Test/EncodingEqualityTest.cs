using ParameterInjection;
using ParameterInjection.Builder;
using TypeInjection.Builder;

using BuilderT = TypeInjection.Builder.Builder;
using BuilderP = ParameterInjection.Builder.Builder;

namespace TypeInjection.Test;

// Note: These tests are rather quick and dirty. They're mostly here to illustrate that the two paradigms do the same...
public class EncodingEqualityTest
{
    private static readonly IDelimitation delimitation = Delimitations.None;

    [Theory]
    [MemberData(nameof(SingleEncodings))]
    public void SingularProcessorsCompactTheSame(ITextProcessor typed, ITextProcessor parametrised)
    {
        String fromTypedProcessor = typed.Compact(Text(), delimitation);
        String fromParametrisedProcessor = parametrised.Compact(Text(), delimitation);

        Assert.Equal(fromTypedProcessor, fromParametrisedProcessor);
    }

    [Theory]
    [MemberData(nameof(MultiEncodings))]
    public void MultipleProcessorsCompactTheSame(ITextProcessor typed, ITextProcessor parametrised)
    {
        String fromTypedProcessor = typed.Compact(Text(), delimitation);
        String fromParametrisedProcessor = parametrised.Compact(Text(), delimitation);

        Assert.Equal(fromTypedProcessor, fromParametrisedProcessor);
    }
    public static IEnumerable<Object[]> SingleEncodings()
    {
        return AllEncodings().Select(i => new Object[] { i.typed, i.parametrised });
        static IEnumerable<(ITextProcessor typed, ITextProcessor parametrised)> AllEncodings()
        {
            yield return (Processor.With<None>(), new TextProcessor(Encodings.None));
            yield return (Processor.With<UpperCase>(), new TextProcessor(Encodings.UpperCase));
            yield return (Processor.With<LowerCase>(), new TextProcessor(Encodings.LowerCase));
            yield return (Processor.With<HomogeniseNewLines>(), new TextProcessor(Encodings.HomogeniseNewLines));
            yield return (Processor.With<Flatten>(), new TextProcessor(Encodings.Flatten));
            yield return (Processor.With<Trim>(), new TextProcessor(Encodings.Trim));
            yield return (Processor.With<Separate>(), new TextProcessor(Encodings.Separate));
        }
    }
    public static IEnumerable<Object[]> MultiEncodings()
    {
        return SomeVariants().Select(i => new Object[] { i.typed.Build(), i.parametrised.Build() });
        static IEnumerable<(ITypeBuilder typed, IParameterizedBuilder parametrised)> SomeVariants()
        {
            yield return (BuilderT.With<HomogeniseNewLines>().Inject<Flatten>(),
                          BuilderP.With(Encodings.HomogeniseNewLines).Add(Encodings.Flatten));
            yield return (BuilderT.With<LowerCase>().Inject<Flatten>().Inject<Separate>(),
                          BuilderP.With(Encodings.LowerCase).Add(Encodings.Flatten).Add(Encodings.Separate));
        }
    }
    private static IEnumerable<String> Text()
    {
        yield return " This is \n some";
        yield return "  WEIrd \t text. Full    ";
        yield return "\nof whitespace    ";
        yield return "  and \n \r\n newLine Characters.    ";
    }
}