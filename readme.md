# Type Injection

A somewhat esoteric programming technique discovered using .Net 7. while working on my project [Quantities](https://github.com/atmoos/Quantities). Having said that, type injection is sloley an alternate way of solving a problem. By all means not necessarily the best.

This builds on the new C# capability called "[static abstract interface members](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface#static-abstract-and-virtual-members)".

## Scenario

Encodings is an example that lends itself well to type injection, as encoding itself is stateless in that all we'd want to do is change the encoding algorithm.

The encoding interface might look something like this:

```csharp
public interface IEncoding
{
    static abstract IEnumerable<Byte> Encode(IEnumerable<Char> data);
    static abstract IEnumerable<Char> Decode(IEnumerable<Byte> data);
}
```

With that it be natural to define serializers (etc.) as follows:

```csharp
public interface ISerialize
{
    IEnumerable<Byte> Serialize(Object item);
    TResult Deserialize<TResult>(IEnumerable<Byte> data);
}

public sealed class Serializer<TEncoding> : ISerialize
    where TEncoding : IEncoding
{
    public TResult Deserialize<TResult>(IEnumerable<Byte> data)
    {
        var json = TEncoding.Decode(data); // json is only an example
        return (TResult)(/* object from json */);
    }
}
```

This pattern would result in very readable code:

```csharp

ISerializer serializer = new Serializer<Utf8>(/* serialization options... */);

// or similar
Stream fileStream = new FileStream<Ascii>("/some/path.log");
```

## Technique

### Problem Statement

With the above example in mind, how would we go about using the same encoding for both a serializer and the file stream? (Without one knowing about the other, that is.)

Had we used the classical way of setting the encoding by using some variable, sharing the encoding could be quite simple:

```csharp
// A) Using a shared parameter
Encoding encoding = Encoding.Utf8;
ISerializer serializer = new Serializer(encoding); // not the impl. from above...

Stream fileStream = new FileStream("/some/path.log", encoding); // not the impl. from above...

// B) or, by using a property on the serializer to instantiate the file stream
Serializer serializer = new Serializer(encoding); // not the impl. from above...

Stream fileStream = new FileStream("/some/path.log", serializer.Encoding); // not the impl. from above...
```

Option A) translates fairly easily to our implementations from above (it's not type injection yet, though):

```csharp
// A) Analogue using a shared type parameter
public (ISerializer, Stream) Initialize<TEncoding>()
    where TEncoding : IEncoding
{
    // these are the same types as in the leading example
    return (new Serializer<TEncoding>(), new FileStream<TEncoding>("/some/path.log"));
}
```

The issue with this is that the serializer and the file stream are co-located: i.e. they are instantiated at the same time. So what should we do when we cannot instantiate them at the same time?

### Type Injection Solution

Scenario B) where the encoding used by the serializer can be passed to the stream is not trivial to achieve using static interface members. We need an intermediary process by which the type parameter of one type can be transparently shared with some other entirely unrelated type.

Let's first introduce two new interfaces:

```csharp
public interface IInject
{
    TResult Inject<TResult>(IEncodingInjector<TResult> injector);
}

public interface IEncodingInjector<out TResult>
{
    // this is the type injection part :-)
    TResult Inject<TEncoding>() where TEncoding : IEncoding;
}
```

The serializer now only needs to implement the `IInject` interface to allow for others to be injected with it's underlying encoding type. Let's add that implementation to the serializer:

```csharp
public sealed class Serializer<TEncoding> : ISerialize, IInject
    where TEncoding : IEncoding
{
    public TResult Inject<TResult>(IEncodingInjector<TResult> injector)
    {
        return injector.Inject<TEncoding>();
    }

    /* ISerialize members here */
}
```

What we need next is a type that knows how to create a file stream, by using that injected type parameter:

```csharp
public sealed class FileStreamCreator : IEncodingInjector<Stream>
{
    private readonly String path; // constructor injected...

    public Stream Inject<TEncoding>()
        where TEncoding : IEncoding
    {
        return new FileStream<TEncoding>(this.path);
    }
}
```

Now we can share the type parameter that the serializer uses to create a (file) stream that uses the same encoding type without either the `Serializer` or the `FileStream` knowing about each other.

```csharp
// B) or, by using a property on the serializer to instantiate the file stream
IInject serializer = new Serializer<Utf8>(encoding); // for simplicity only showing injection use case...

// somewhere completely different...
IEncodingInjector<Stream> streamCreator = new FileStreamCreator("/some/path.log");
Stream fileStream = serializer.Inject(streamCreator);
```

## Discussion

Type injection is in general not very elegant. It's strength is in that it alleviates abstract interface members to full class citizens of the C# language to be used not only in numeric math but also in business logic that goes beyond math and algorithms.

Potential benefits:

- allocation free programming
- performance improvements
- making the singleton pattern safe to use

When used carefully, it can also improve readability.

The downsides are numerous though:

- Added complexity to share/inject type parameterisation
- Currently unfamiliar coding pattern
- Difficulty combining normal parameters with type parameters
- Injecting one type parameter is hard enough, injecting two or more quickly becomes unwieldy
- Inability to fully abstract the injection interfaces
  - The injected base type (`IEncoding`) cannot be parameterized as of .Net 8

Hence, currently, be careful when employing this pattern.

Note also that this is very similar to the visitor pattern using types instead of values.
