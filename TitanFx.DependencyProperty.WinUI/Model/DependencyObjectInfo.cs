using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static TitanFx.DependencyProperty.WinUI.Model.Constants;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal partial record DependencyObjectInfo : RootTypeInfo
{
    public required string Type { get; init; }
    public required ValueArray<DependencyPropertyInfo> Properties { get; init; }

    public static IncrementalValuesProvider<DependencyObjectInfo> Capture(
        IncrementalGeneratorInitializationContext context
    )
    {
        return context
            .SyntaxProvider.ForAttributeWithMetadataName(
                $"{Constants.Namespace}.{DependencyPropertyAttribute}",
                static (node, _) =>
                    node
                        is PropertyDeclarationSyntax
                        {
                            Parent: not ExtensionBlockDeclarationSyntax
                        },
                static (ctx, token) =>
                {
                    if (
                        ctx.TargetSymbol is not IPropertySymbol { IsStatic: false } property
                        || ctx.TargetNode is not PropertyDeclarationSyntax node
                    )
                        return null;
                    var propertyType = property.Type.ToDisplayString(
                        SymbolDisplayFormat.FullyQualifiedFormat
                    );
                    var syntax = property
                        .DeclaringSyntaxReferences.Select(x => x.GetSyntax(token))
                        .OfType<PropertyDeclarationSyntax>()
                        .ToList();

                    var namedArguments = property
                        .GetAttributes()
                        .Where(static a =>
                            a.AttributeClass
                                is {
                                    Name: DependencyPropertyAttribute,
                                    ContainingType: null,
                                    ContainingNamespace:
                                    {
                                        Name: Constants.WinUI,
                                        ContainingNamespace:
                                        {
                                            Name: Constants.DependencyProperty,
                                            ContainingNamespace:
                                            {
                                                Name: Constants.TitanFx,
                                                ContainingNamespace.IsGlobalNamespace: true
                                            }
                                        }
                                    }
                                }
                        )
                        .SelectMany(static a => a.NamedArguments)
                        .ToLookup(static a => a.Key, static a => a.Value);

                    var onValueChangedName = namedArguments[OnValueChanged]
                        .Where(static x => x.Kind is TypedConstantKind.Primitive)
                        .Select(static x => x.Value)
                        .OfType<string>()
                        .FirstOrDefault();
                    var createDefaultValueFn = namedArguments[CreateDefaultValue]
                        .Where(static x => x.Kind is TypedConstantKind.Primitive)
                        .Select(static x => x.Value)
                        .OfType<string>()
                        .FirstOrDefault();

                    return new
                    {
                        Parent = Capture(property.ContainingType),
                        property.Name,
                        SetterModifiers = GetModifiers(syntax, SyntaxKind.SetAccessorDeclaration),
                        InitModifiers = GetModifiers(syntax, SyntaxKind.InitAccessorDeclaration),
                        GetterModifiers = GetModifiers(syntax, SyntaxKind.GetAccessorDeclaration),
                        Modifiers = GetModifiers(syntax),
                        PropertyType = propertyType,
                        InitialValue = node.Initializer switch
                        {
                            null => $"default({propertyType})",
                            var v => $"({propertyType})({node.Initializer.Value})",
                        },
                        CreateDefaultValue = createDefaultValueFn,
                        Nullable = property.NullableAnnotation is NullableAnnotation.Annotated,
                        OnValueChanged = onValueChangedName is null
                            ? null
                            : OnValueChangedInfo.Capture(
                                property.ContainingType,
                                onValueChangedName
                            ),
                    };
                }
            )
            .Where(static x => x is not null)
            .Select(static (x, _) => x!)
            .Collect()
            .SelectMany(
                static (x, _) =>
                    x.GroupBy(
                        static v => v.Parent,
                        static (parent, properties) =>
                            new DependencyObjectInfo
                            {
                                Type = FullyQualify(parent),
                                Path = parent.Path,
                                Namespace = parent.Namespace,
                                Properties = new(
                                    properties.Select(static p => new DependencyPropertyInfo
                                    {
                                        Name = p.Name,
                                        SetterModifiers = p.SetterModifiers,
                                        InitModifiers = p.InitModifiers,
                                        GetterModifiers = p.GetterModifiers,
                                        Modifiers = p.Modifiers,
                                        InitialValue = p.InitialValue,
                                        CreateDefaultValue = p.CreateDefaultValue,
                                        PropertyType = p.PropertyType,
                                        OnValueChanged = p.OnValueChanged,
                                    })
                                ),
                            }
                    )
            );
    }

    private static string FullyQualify(RootTypeInfo type)
    {
        var sb = new StringBuilder();
        _ = sb.Append("global::");
        if (type.Namespace is { } ns)
            _ = sb.Append($"{ns}.");
        foreach (var t in type.Path.SkipLast(1))
            _ = sb.Append($"{t.Name}{t.TypeParameters}.");
        foreach (var t in type.Path.TakeLast(1))
            _ = sb.Append($"{t.Name}{t.TypeParameters}");
        return sb.ToString();
    }

    private static Modifiers GetModifiers(IReadOnlyCollection<PropertyDeclarationSyntax> property)
    {
        if (property.Count == 0)
            return new Modifiers { Accessibility = null, Other = new([]) };

        return GetModifiers(property.SelectMany(static x => x.Modifiers));
    }

    private static Modifiers? GetModifiers(
        IReadOnlyCollection<PropertyDeclarationSyntax> property,
        SyntaxKind accessor
    )
    {
        if (property.Count == 0)
            return null;

        var accessors = property
            .SelectMany(static x => x.AccessorList?.Accessors ?? [])
            .Where(x => x.Kind() == accessor)
            .ToList();
        if (accessors.Count == 0)
            return null;

        return GetModifiers(accessors.SelectMany(static x => x.Modifiers));
    }

    private static Modifiers GetModifiers(IEnumerable<SyntaxToken> modifiers)
    {
        var accessibility = null as string;
        var other = new List<string>();
        foreach (var modifier in modifiers.DistinctBy(static x => x.Kind()))
        {
            switch (modifier.Kind())
            {
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.ProtectedKeyword:
                case SyntaxKind.PrivateKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.FileKeyword:
                    accessibility = modifier.ToString();
                    break;
                default:
                    other.Add(modifier.ToString());
                    break;
            }
        }

        return new() { Accessibility = accessibility, Other = new(other) };
    }
}

internal sealed record Modifiers
{
    public required string? Accessibility { get; init; }
    public required ValueArray<string> Other { get; init; }
}
