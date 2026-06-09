using System;

namespace TitanFx.DependencyProperty.WinUI.Model;

[Flags]
internal enum OnValueChangedSignature
{
    NotFound = 0,

    /// <summary>Anything else</summary>
    Unsupported = -1,

    /// <summary>static void Handler(DependencyObject d, ...</summary>
    DependencyObject = 1 << 0,

    /// <summary>static void Handler(TOwner d, ...</summary>
    Owner = 1 << 1,

    /// <summary>void Handler(...</summary>
    This = 1 << 2,

    Sender = DependencyObject | Owner | This,

    /// <summary>..., DependencyPropertyChangedEventArgs e);</summary>
    EventArgs = 1 << 3,

    /// <summary>..., TProperty? @new, TProperty? old);</summary>
    NewOld = 1 << 4,

    /// <summary>..., TProperty? @new);</summary>
    New = 1 << 5,

    /// <summary>...);</summary>
    Only = 1 << 6,

    Change = EventArgs | NewOld | New | Only,

    /// <summary>static void Handler(DependencyObject d, DependencyPropertyChangedEventArgs e);</summary>
    DependencyObject_EventArgs = DependencyObject | EventArgs,

    /// <summary>static void Handler(TOwner d, DependencyPropertyChangedEventArgs e);</summary>
    Owner_EventArgs = Owner | EventArgs,

    /// <summary>void Handler(DependencyPropertyChangedEventArgs e);</summary>
    This_EventArgs = This | EventArgs,

    /// <summary>static void Handler(DependencyObject d, TProperty? @new, TProperty? old);</summary>
    DependencyObject_NewOld = DependencyObject | NewOld,

    /// <summary>static void Handler(TOwner d, TProperty? @new, TProperty? old);</summary>
    Owner_NewOld = Owner | NewOld,

    /// <summary>void Handler(TProperty? @new, TProperty? old);</summary>
    This_NewOld = This | NewOld,

    /// <summary>static void Handler(DependencyObject d, TProperty? @new);</summary>
    DependencyObject_New = DependencyObject | New,

    /// <summary>static void Handler(TOwner d, TProperty? @new);</summary>
    Owner_New = Owner | New,

    /// <summary>void Handler(TProperty? @new);</summary>
    This_New = This | New,

    /// <summary>static void Handler(DependencyObject d);</summary>
    DependencyObject_Only = DependencyObject | Only,

    /// <summary>static void Handler(TOwner d);</summary>
    Owner_Only = Owner | Only,

    /// <summary>void Handler();</summary>
    This_Only = This | Only,
}
