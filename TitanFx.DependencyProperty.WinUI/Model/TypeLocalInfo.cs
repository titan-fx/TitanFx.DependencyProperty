namespace TitanFx.DependencyProperty.WinUI.Model;

internal record TypeLocalInfo
{
    public required string Name { get; init; }
    public required TypeParameters TypeParameters { get; init; }
    public required string Kind { get; init; }
    public required string Modifiers { get; init; }
}
