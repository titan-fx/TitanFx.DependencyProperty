namespace System.Windows;

public readonly struct DependencyPropertyChangedEventArgs
{
    public object OldValue { get; }
    public object NewValue { get; }
}
