using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal record CreateDefaultValueInfo
{
    public required string Name { get; init; }
    public required bool ReturnsValueType { get; init; }

    internal static CreateDefaultValueInfo? Capture(
        INamedTypeSymbol containingType,
        string? methodName
    )
    {
        if (methodName is null)
            return null;

        return new()
        {
            Name = methodName,
            ReturnsValueType = containingType
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .Where(static m => m is { IsStatic: true, Parameters: [], TypeParameters: [] })
                .Select(static m => m.ReturnType.IsValueType)
                .FirstOrDefault(),
        };
    }
}
