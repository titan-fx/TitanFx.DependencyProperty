using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.Wpf.Model;

internal record CreateDefaultValueInfo
{
    public required string Name { get; init; }
    public required bool IsMethod { get; init; }

    internal static CreateDefaultValueInfo? Capture(
        INamedTypeSymbol containingType,
        string? memberName
    )
    {
        if (memberName is null)
            return null;

        return new()
        {
            Name = memberName,
            IsMethod = containingType.GetMembers(memberName).OfType<IMethodSymbol>().Any(),
        };
    }
}
