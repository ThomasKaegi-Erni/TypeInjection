using TypeInjection.Sharing;

namespace TypeInjection.Builder;

public interface ITypeBuilder : IBuilder<ITextProcessor>, IEncodingInjector<ITypeBuilder> { }
