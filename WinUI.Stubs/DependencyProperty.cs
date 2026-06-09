namespace Microsoft.UI.Xaml;

public sealed class DependencyProperty
{
    public static DependencyProperty Register(
        string name,
        Type valueType,
        Type ownerType,
        PropertyMetadata metadata
    )
    {
        return new();
    }
}
