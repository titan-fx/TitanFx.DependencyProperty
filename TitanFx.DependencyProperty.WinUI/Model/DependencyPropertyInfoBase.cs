namespace TitanFx.DependencyProperty.WinUI.Model;

internal abstract record DependencyPropertyInfoBase
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string RuntimeType { get; init; }
    public required CreateDefaultValueInfo? CreateDefaultValue { get; init; }
    public required OnValueChangedInfo? OnValueChanged { get; init; }
}
