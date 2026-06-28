namespace System.Windows;

public abstract class DependencyObject
{
    public void SetValue(DependencyProperty property, object value) { }

    public object GetValue(DependencyProperty property) => null!;
}
