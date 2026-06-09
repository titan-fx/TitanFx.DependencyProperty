using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal record CreateDefaultValueInfo
{
    public required string Name { get; init; }
    public required bool ReturnsValueType { get; init; }

    internal static CreateDefaultValueInfo Capture(
        INamedTypeSymbol containingType,
        string createDefaultValue
    )
    {
        var method = containingType
            .GetMembers(createDefaultValue)
            .OfType<IMethodSymbol>()
            .FirstOrDefault(static m =>
                m is { IsStatic: true, Parameters: [], TypeParameters: [] }
            );

        return new()
        {
            Name = createDefaultValue,
            ReturnsValueType = method is { ReturnType.IsValueType: true },
        };
    }
}
