using ParameterInjection;
using TypeInjection.Sharing;

namespace TypeInjection.Test;

public class TypeInjectionTest
{
    [Fact]
    public void TypeInjectionWorks()
    {
        IInject initial = AllocFreeSerializer.With<Flatten>();

        IInject fromInjection = initial.Inject(new Injector());

        Assert.IsType<Inject<Flatten>>(fromInjection); // Different type as serializer, but using "Flatten".
    }

    [Fact]
    public void ParameterisedAnalogue()
    {
        // This test is utterly silly, but is good to showcase the
        // esoteric nature of type injection... 
        ParameterInjection.Sharing.Serializer initial = new(new ParameterInjection.Flatten());

        Parameter fromParameter = new(initial.Encoder);

        // fromParameter is unrelated to "initial", but the Encoder it uses
        // is exactly the same.
        Assert.IsType<ParameterInjection.Flatten>(fromParameter.Encoder);
    }
}

// This is also a disadvantage of type injection
file sealed class Injector : IEncodingInjector<IInject>
{
    public IInject Inject<TEncoding>()
        where TEncoding : IEncoding
            => Sharing.Inject<TEncoding>.Item;
}

file sealed class Parameter
{
    public IEncoder Encoder { get; }
    public Parameter(IEncoder encoder) => Encoder = encoder;
}
