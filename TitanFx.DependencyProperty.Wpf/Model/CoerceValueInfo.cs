using System.Linq;
using Microsoft.CodeAnalysis;
using static TitanFx.DependencyProperty.Wpf.Model.Constants;

namespace TitanFx.DependencyProperty.Wpf.Model;

internal record CoerceValueInfo
{
    public required string MethodName { get; init; }
    public required CoerceValueSignature Signature { get; init; }

    internal static CoerceValueInfo? Capture(
        INamedTypeSymbol ownerType,
        string? methodName,
        bool staticOnly
    )
    {
        if (methodName is null)
            return null;
        return new CoerceValueInfo
        {
            MethodName = methodName,
            Signature = ownerType
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .Select(v => GetSignature(v, staticOnly))
                .Order()
                .DefaultIfEmpty(CoerceValueSignature.NotFound)
                .FirstOrDefault(static v => v is not CoerceValueSignature.Unsupported),
        };
    }

    private static CoerceValueSignature GetSignature(IMethodSymbol method, bool staticOnly)
    {
        var parameters = method.Parameters.AsSpan();
        if (!method.IsStatic)
        {
            if (!staticOnly)
                return CoerceValueSignature.Owner;
        }
        else if (parameters is [{ } s])
        {
            if (IsDependencyObject(s.Type))
                return CoerceValueSignature.DependencyObject;
            return CoerceValueSignature.Target;
        }
        return CoerceValueSignature.Unsupported;
    }

    private static bool IsDependencyObject(ITypeSymbol type)
    {
        return type
            is INamedTypeSymbol
            {
                Name: DependencyObject,
                ContainingNamespace:
                {
                    Name: "Windows",
                    ContainingNamespace:
                    { Name: "System", ContainingNamespace.IsGlobalNamespace: true }
                }
            };
    }
}
