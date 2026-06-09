namespace Microsoft.UI.Xaml;

public delegate void DependencyPropertyChangedEventHandler(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e
);

#nullable disable
public sealed class DependencyPropertyChangedEventArgs
{
    public object NewValue { get; }
    public object OldValue { get; }
}
