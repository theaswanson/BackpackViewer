using System.Diagnostics.CodeAnalysis;

namespace BackpackViewer.Core.Caching;

public class CacheResult<T> where T : class
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool Found { get; }
    public T? Value { get; }

    private CacheResult(bool found, T? value)
    {
        Found = found;
        Value = value;
    }
    
    public static CacheResult<T> Success(T value) => new(true, value);
    public static CacheResult<T> NotFound() => new(false, null);
}