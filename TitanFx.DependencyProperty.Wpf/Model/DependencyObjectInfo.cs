using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static TitanFx.DependencyProperty.Wpf.Model.Constants;

namespace TitanFx.DependencyProperty.Wpf.Model;

internal partial record DependencyObjectInfo : RootTypeInfo
{
    public required string Type { get; init; }
    public required ValueArray<DependencyPropertyInfo> Properties { get; init; }
    public required ValueArray<AttachedDependencyPropertyInfo> AttachedProperties { get; init; }

    public static IncrementalValuesProvider<DependencyObjectInfo> Capture(
        IncrementalGeneratorInitializationContext context
    )
    {
        var dependencyProperties = CaptureDependencyProperties(context).Collect();
        var attachedProperties = CaptureAttachedDependencyProperties(context).Collect();

        return dependencyProperties
            .Combine(attachedProperties)
            .SelectMany(
                static (x, _) =>
                {
                    var direct = x.Left.ToLookup(static v => v.Owner, static v => v.Value);
                    var attached = x.Right.ToLookup(static v => v.Owner, static v => v.Value);
                    var keys = Enumerable.Union(
                        direct.Select(static v => v.Key),
                        attached.Select(static v => v.Key)
                    );
                    return keys.Select(k => (owner: k, direct: direct[k], attached: attached[k]))
                        .Select(static x => new DependencyObjectInfo
                        {
                            Type = FullyQualify(x.owner),
                            Visibility = x.owner.Visibility,
                            Path = x.owner.Path,
                            Namespace = x.owner.Namespace,
                            Properties = new(x.direct),
                            AttachedProperties = new(x.attached),
                        });
                }
            );
    }

    private static T? GetNamedArgument<T>(
        ILookup<string, TypedConstant> namedArguments,
        string name,
        T? fallback = default
    ) =>
        namedArguments[name]
            .Where(static x => x.Kind is TypedConstantKind.Primitive)
            .Select(static x => x.Value)
            .OfType<T>()
            .FirstOrDefault(fallback);

    private static T? GetNamedArgument<T>(
        ILookup<string, TypedConstant> namedArguments,
        string name,
        T? fallback = null
    )
        where T : struct =>
        namedArguments[name]
            .Where(static x => x.Kind is TypedConstantKind.Primitive)
            .Select(static x => x.Value)
            .OfType<T>()
            .Select(static v => new T?(v))
            .FirstOrDefault(fallback);

    private static IncrementalValuesProvider<
        WithOwner<DependencyPropertyInfo>
    > CaptureDependencyProperties(IncrementalGeneratorInitializationContext context)
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
                        return null!;
                    var propertyType = GetTypeText(property.Type);
                    var syntax = property
                        .DeclaringSyntaxReferences.Select(x => x.GetSyntax(token))
                        .OfType<PropertyDeclarationSyntax>()
                        .ToList();

                    var namedArguments = ctx
                        .Attributes.SelectMany(static a => a.NamedArguments)
                        .ToLookup(static a => a.Key, static a => a.Value);

                    var onValueChanged = GetNamedArgument<string>(namedArguments, OnValueChanged);
                    var createDefaultValue = GetNamedArgument<string>(
                        namedArguments,
                        CreateDefaultValue
                    );
                    var validateValue = GetNamedArgument<string>(namedArguments, ValidateValue);
                    var coerceValue = GetNamedArgument<string>(namedArguments, CoerceValue);

                    return new WithOwner<DependencyPropertyInfo>
                    {
                        Owner = Capture(property.ContainingType),
                        Value = new()
                        {
                            Name = property.Name,
                            SetterModifiers = GetModifiers(
                                syntax,
                                SyntaxKind.SetAccessorDeclaration
                            ),
                            InitModifiers = GetModifiers(
                                syntax,
                                SyntaxKind.InitAccessorDeclaration
                            ),
                            GetterModifiers = GetModifiers(
                                syntax,
                                SyntaxKind.GetAccessorDeclaration
                            ),
                            Modifiers = GetModifiers(syntax),
                            Type = propertyType,
                            IsReferenceType = property.Type.IsReferenceType,
                            RuntimeType = property.Type.ToDisplayString(
                                SymbolDisplayFormat.FullyQualifiedFormat
                            ),
                            CreateDefaultValue = CreateDefaultValueInfo.Capture(
                                property.ContainingType,
                                createDefaultValue
                            ),
                            OnValueChanged = OnValueChangedInfo.Capture(
                                property.ContainingType,
                                onValueChanged,
                                staticOnly: false
                            ),
                            CoerceValue = CoerceValueInfo.Capture(
                                property.ContainingType,
                                coerceValue,
                                staticOnly: false
                            ),
                            ValidateValue = ValidateValueInfo.Capture(
                                property.ContainingType,
                                validateValue
                            ),
                        },
                    };
                }
            )
            .Where(static x => x is not null);
    }

    private static IncrementalValuesProvider<
        WithOwner<AttachedDependencyPropertyInfo>
    > CaptureAttachedDependencyProperties(IncrementalGeneratorInitializationContext context)
    {
        return context
            .SyntaxProvider.ForAttributeWithMetadataName(
                $"{Constants.Namespace}.{AttachedDependencyPropertyAttribute}`2",
                static (node, _) =>
                    node
                        is ClassDeclarationSyntax
                            or StructDeclarationSyntax
                            or RecordDeclarationSyntax,
                static (ctx, token) =>
                {
                    if (
                        ctx.TargetSymbol is not INamedTypeSymbol owner
                        || ctx.TargetNode is not TypeDeclarationSyntax node
                    )
                        return null!;

                    var ownerInfo = Capture(owner);
                    return new ValueArray<WithOwner<AttachedDependencyPropertyInfo>>(
                        ctx.Attributes.Select(a =>
                            {
                                if (
                                    a
                                    is not {
                                        ConstructorArguments: [
                                            {
                                                Kind: TypedConstantKind.Primitive,
                                                Value: string name
                                            },
                                        ],
                                        AttributeClass.TypeArguments: [var tTarget, var tValue]
                                    }
                                )
                                {
                                    return null!;
                                }

                                var namedArguments = a.NamedArguments.ToLookup(
                                    static a => a.Key,
                                    static a => a.Value
                                );

                                var onValueChanged = GetNamedArgument<string>(
                                    namedArguments,
                                    OnValueChanged
                                );
                                var createDefaultValue = GetNamedArgument<string>(
                                    namedArguments,
                                    CreateDefaultValue
                                );
                                var validateValue = GetNamedArgument<string>(
                                    namedArguments,
                                    ValidateValue
                                );
                                var coerceValue = GetNamedArgument<string>(
                                    namedArguments,
                                    CoerceValue
                                );
                                var propertyType = GetTypeText(tValue);

                                return new WithOwner<AttachedDependencyPropertyInfo>
                                {
                                    Owner = ownerInfo,
                                    Value = new()
                                    {
                                        Name = name,
                                        TargetType = tTarget.ToDisplayString(
                                            Util.FullyQualifiedNullableFormat
                                        ),
                                        Type = propertyType,
                                        IsReferenceType = tValue.IsReferenceType,
                                        IsReadOnly = GetNamedArgument(
                                            namedArguments,
                                            ReadOnly,
                                            false
                                        ),
                                        RuntimeType = tValue.ToDisplayString(
                                            SymbolDisplayFormat.FullyQualifiedFormat
                                        ),
                                        CreateDefaultValue = CreateDefaultValueInfo.Capture(
                                            owner,
                                            createDefaultValue
                                        ),
                                        OnValueChanged = OnValueChangedInfo.Capture(
                                            owner,
                                            onValueChanged,
                                            staticOnly: true
                                        ),
                                        CoerceValue = CoerceValueInfo.Capture(
                                            owner,
                                            coerceValue,
                                            staticOnly: true
                                        ),
                                        ValidateValue = ValidateValueInfo.Capture(
                                            owner,
                                            validateValue
                                        ),
                                    },
                                };
                            })
                            .Where(static x => x is not null)
                    );
                }
            )
            .SelectMany(static (x, _) => x);
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

    private static string GetTypeText(ITypeSymbol type)
    {
        return type.ToDisplayString(Util.FullyQualifiedNullableFormat);
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

    private sealed record WithOwner<T>
    {
        public required RootTypeInfo Owner { get; init; }
        public required T Value { get; init; }
    }
}

internal sealed record Modifiers
{
    public required string? Accessibility { get; init; }
    public required ValueArray<string> Other { get; init; }
}
