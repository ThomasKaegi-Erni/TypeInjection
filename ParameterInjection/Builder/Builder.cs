namespace ParameterInjection.Builder;

public sealed class Builder : IParameterizedBuilder
{
    private readonly IEncoder encoder;
    private Builder(IEncoder encoder) => this.encoder = encoder;
    public IParameterizedBuilder Add(IEncoder parameter)
    {
        return new Builder(new Combo(this.encoder, parameter));
    }
    public TypeInjection.ITextProcessor Build() => new TextProcessor(this.encoder);
    public static IParameterizedBuilder With(IEncoder encoder) => new Builder(encoder);
}