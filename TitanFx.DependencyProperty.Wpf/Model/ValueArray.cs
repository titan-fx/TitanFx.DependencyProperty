using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TitanFx.DependencyProperty.Wpf.Model;

internal static class ValueArray
{
    public static ValueArray<T> CreateReverse<T>(IEnumerable<T> source)
    {
        return ValueArray<T>.CreateReverse(source);
    }

    public static ValueArray<T> Create<T>(IEnumerable<T> source)
    {
        return new(source);
    }
}

internal sealed class ValueArray<T> : IReadOnlyList<T>, IEquatable<ValueArray<T>>
{
    private readonly T[] _data;

    public T this[int index] => _data[index];

    public int Count => _data.Length;

    public ValueArray(IEnumerable<T> items)
    {
        _data = [.. items];
    }

    public ValueArray(ReadOnlySpan<T> items)
    {
        _data = [.. items];
    }

    private ValueArray(T[] items)
    {
        _data = items;
    }

    public static ValueArray<T> CreateReverse(IEnumerable<T> source)
    {
        var result = source.ToArray();
        Array.Reverse(result);
        return new(result);
    }

    public bool Equals([NotNullWhen(true)] ValueArray<T>? other)
    {
        return other is not null && _data.SequenceEqual(other._data);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ValueArray<T>);
    }

    public override int GetHashCode()
    {
        var result = 0;
        foreach (var item in _data)
            result = ValueTuple.Create(result, item).GetHashCode();
        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return $"[{string.Join(",", _data)}]";
    }
}
