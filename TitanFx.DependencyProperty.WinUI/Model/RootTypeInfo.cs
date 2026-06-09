using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal record RootTypeInfo
{
    public required string? Namespace { get; init; }
    public required ValueArray<TypeLocalInfo> Path { get; init; }

    protected static RootTypeInfo Capture(INamedTypeSymbol type)
    {
        return new()
        {
            Namespace = type.ContainingNamespace is { IsGlobalNamespace: false } ns
                ? ns.ToDisplayString()
                : null,
            Path = ValueArray.CreateReverse(
                Traverse(type)
                    .Select(static x => new TypeLocalInfo
                    {
                        Name = x.Name,
                        TypeParameters = x.TypeParameters is []
                            ? ""
                            : $"<{string.Join(", ", x.TypeParameters.Select(static p => p.Name))}>",
                        Kind = x switch
                        {
                            { TypeKind: TypeKind.Interface } => "interface",
                            { TypeKind: TypeKind.Struct, IsRecord: true } => "record struct",
                            { TypeKind: TypeKind.Struct, IsRecord: false } => "struct",
                            { TypeKind: TypeKind.Class, IsRecord: true } => "record class",
                            { TypeKind: TypeKind.Class, IsRecord: false } => "class",
                            _ => "UNKNOWN",
                        },
                        Modifiers = x.IsRefLikeType ? "ref" : "",
                    })
            ),
        };
    }

    private static IEnumerable<INamedTypeSymbol> Traverse(INamedTypeSymbol? type)
    {
        while (type is not null)
        {
            yield return type;
            type = type.ContainingType;
        }
    }
}
