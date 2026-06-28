namespace TitanFx.DependencyProperty.Wpf.Model;

internal record DependencyPropertyInfo : DependencyPropertyInfoBase
{
    public required Modifiers? SetterModifiers { get; init; }
    public required Modifiers? InitModifiers { get; init; }
    public required Modifiers? GetterModifiers { get; init; }
    public required Modifiers Modifiers { get; init; }
}
