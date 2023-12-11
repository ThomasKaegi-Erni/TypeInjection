namespace TypeInjection.Sharing;

internal sealed class Inject<TEncoding> : IInject
where TEncoding : IEncoding
{
    public static IInject Item { get; } = new Inject<TEncoding>();
    private Inject() { } // prevents not using the allocation free Item.
    TResult IInject.Inject<TResult>(IEncodingInjector<TResult> injector)
    {
        return injector.Inject<TEncoding>();
    }
}
