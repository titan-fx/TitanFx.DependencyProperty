namespace System.Windows;

public sealed class PropertyMetadata
{
    public PropertyMetadata(
        object defaultValue,
        PropertyChangedCallback propertyChangedCallback,
        CoerceValueCallback coerceValueCallback
    ) { }
}
