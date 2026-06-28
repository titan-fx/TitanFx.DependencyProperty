namespace System.Windows;

public sealed class DependencyProperty
{
    public static DependencyProperty Register(
        string name,
        Type propertyType,
        Type ownerType,
        PropertyMetadata typeMetadata,
        ValidateValueCallback validateValueCallback
    ) => null;

    public static DependencyProperty RegisterAttached(
        string name,
        Type propertyType,
        Type ownerType,
        PropertyMetadata typeMetadata,
        ValidateValueCallback validateValueCallback
    ) => null;

    public static DependencyPropertyKey RegisterReadOnly(
        string name,
        Type propertyType,
        Type ownerType,
        PropertyMetadata typeMetadata,
        ValidateValueCallback validateValueCallback
    ) => null;

    public static DependencyPropertyKey RegisterAttachedReadOnly(
        string name,
        Type propertyType,
        Type ownerType,
        PropertyMetadata typeMetadata,
        ValidateValueCallback validateValueCallback
    ) => null;
}
