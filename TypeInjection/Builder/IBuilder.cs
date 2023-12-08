namespace TypeInjection.Builder;

public interface IEncodingInjector<TResult>
    where TResult : IEncodingInjector<TResult>
{
    // this is the type injection part :-)
    TResult Inject<TEncoding>() where TEncoding : IEncoding;
}

public interface IBuilder : IEncodingInjector<IBuilder>
{
    ITextProcessor Build();
}
