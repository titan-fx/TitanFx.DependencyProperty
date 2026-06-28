using System;

namespace TitanFx.DependencyProperty.Wpf.Model;

[Flags]
internal enum CoerceValueSignature
{
    NotFound = 0,

    /// <summary>Anything else</summary>
    Unsupported = -1,

    /// <summary>static TProperty Handler(DependencyObject d, object? value);</summary>
    DependencyObject = 1 << 0,

    /// <summary>static TProperty Handler(TTarget d, object? value);</summary>
    Target = 1 << 1,

    /// <summary>TProperty Handler(object? value);</summary>
    Owner = 1 << 2,
}
