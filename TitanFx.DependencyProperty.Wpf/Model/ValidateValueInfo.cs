using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.Wpf.Model;

internal record ValidateValueInfo
{
    public required string MethodName { get; init; }
    public required bool AcceptsObject { get; init; }
    public required bool Exists { get; init; }

    internal static ValidateValueInfo? Capture(INamedTypeSymbol ownerType, string? methodName)
    {
        if (methodName is null)
            return null;
        return new ValidateValueInfo
        {
            MethodName = methodName,
            AcceptsObject = ownerType
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .Any(v => v.Parameters is [{ Type.SpecialType: SpecialType.System_Object }]),
            Exists = ownerType
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .Any(v =>
                    v
                        is {
                            IsStatic: true,
                            ReturnType.SpecialType: SpecialType.System_Boolean,
                            Parameters: [{ }]
                        }
                ),
        };
    }
}
