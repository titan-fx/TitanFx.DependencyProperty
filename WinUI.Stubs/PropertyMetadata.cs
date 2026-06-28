namespace Microsoft.UI.Xaml;

public sealed class PropertyMetadata
{
    public PropertyMetadata(object defaultValue) { }

    public PropertyMetadata(
        object defaultValue,
        DependencyPropertyChangedEventHandler propertyChangedCallback
    ) { }

    public static PropertyMetadata Create(object defaultValue) => new(defaultValue);

    public static PropertyMetadata Create(
        object defaultValue,
        DependencyPropertyChangedEventHandler propertyChangedCallback
    ) => new(defaultValue, propertyChangedCallback);

    public static PropertyMetadata Create(CreateDefaultValueCallback createDefaultValueCallback) =>
        new(null);

    public static PropertyMetadata Create(
        CreateDefaultValueCallback createDefaultValueCallback,
        DependencyPropertyChangedEventHandler propertyChangedCallback
    ) => new(null, propertyChangedCallback);
}
