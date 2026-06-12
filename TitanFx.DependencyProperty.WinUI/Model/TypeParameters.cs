using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal sealed record TypeParameters : IReadOnlyList<string>
{
    private readonly string[] _source;

    public TypeParameters(IEnumerable<string> source)
    {
        _source = [..source];
    }

    public string this[int index] => _source[index];

    public int Count => _source.Length;

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var item in _source)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals([NotNullWhen(true)]TypeParameters? other)
    {
        return other is not null && _source.SequenceEqual(other._source);
    }

    public override int GetHashCode()
    {
        var result = 0;
        foreach (var item in _source)
            result = ValueTuple.Create(result, item).GetHashCode();
        return result;
    }

    public override string ToString()
    {
        if (_source is [])
            return "";
        return $"<{string.Join(", ", _source)}>";
    }
}