namespace TitanFx.DependencyProperty.Wpf.Model;

internal record AttachedDependencyPropertyInfo : DependencyPropertyInfoBase
{
    public required string TargetType { get; init; }
    public required bool IsReadOnly { get; init; }
}
