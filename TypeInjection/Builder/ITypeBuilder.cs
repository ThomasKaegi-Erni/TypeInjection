namespace TypeInjection.Builder;

public interface ITypeInjector<out TResult>
    where TResult : ITypeInjector<TResult>
{
    // this is the type injection part :-)
    TResult Inject<TEncoding>() where TEncoding : IEncoding; // IEncoding can't be abstracted away :-(
}

public interface ITypeBuilder : IBuilder<ITextProcessor>, ITypeInjector<ITypeBuilder> { }
