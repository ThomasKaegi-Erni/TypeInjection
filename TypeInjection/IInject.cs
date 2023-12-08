
namespace TypeInjection.Sharing;

public interface IInject
{
    // Injects an encoding into any other type TResult
    TResult Inject<TResult>(IEncodingInjector<TResult> injector);
}

// The interface that allows for type injection. Here, of encoding types.
public interface IEncodingInjector<out TResult>
{
    // this is the type injection part :-)
    TResult Inject<TEncoding>() where TEncoding : IEncoding; // IEncoding can't be abstracted away :-(
}
