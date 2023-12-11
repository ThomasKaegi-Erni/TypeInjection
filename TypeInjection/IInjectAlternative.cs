namespace TypeInjection.Alternative;

public interface IInject
{
    // Injects an encoding into any other type TResult
    TResult Inject<TResult>() where TResult : ICreate<TResult>;
}

public interface ICreate<out TResult>
    where TResult : ICreate<TResult>
{
    static abstract TResult Create<TEncoding>() where TEncoding : IEncoding;
}


// Example implementation usage
public sealed class FileStream<TEncoding> : Alternative.IInject
    where TEncoding : IEncoding
{
    public TResult Inject<TResult>()
        where TResult : ICreate<TResult>
            => TResult.Create<TEncoding>();

    /* other stream related members */
}
