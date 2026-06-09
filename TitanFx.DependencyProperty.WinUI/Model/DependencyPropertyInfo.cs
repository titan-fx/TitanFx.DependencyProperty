namespace TitanFx.DependencyProperty.WinUI.Model;

internal record DependencyPropertyInfo
{
    public required string Name { get; init; }
    public required Modifiers? SetterModifiers { get; init; }
    public required Modifiers? InitModifiers { get; init; }
    public required Modifiers? GetterModifiers { get; init; }
    public required Modifiers Modifiers { get; init; }
    public required string PropertyType { get; init; }
    public required string InitialValue { get; init; }
    public required string? CreateDefaultValue { get; init; }
    public required OnValueChangedInfo? OnValueChanged { get; init; }
}
