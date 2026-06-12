namespace TitanFx.DependencyProperty.WinUI.Model;

internal record AttachedDependencyPropertyInfo : DependencyPropertyInfoBase
{
    public required string TargetType { get; init; }
}
