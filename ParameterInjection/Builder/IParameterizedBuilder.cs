using TypeInjection;
using TypeInjection.Builder;

namespace ParameterInjection.Builder;

public interface IParameterInjector<out TResult, in TParamType>
    where TResult : IParameterInjector<TResult, TParamType>
{
    // this is the parameter "injection" part :-)
    TResult Add(TParamType parameter);
}
public interface IParameterizedBuilder : IBuilder<ITextProcessor>, IParameterInjector<IParameterizedBuilder, IEncoder> { }
