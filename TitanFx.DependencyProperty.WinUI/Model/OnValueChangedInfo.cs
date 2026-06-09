using System.Linq;
using Microsoft.CodeAnalysis;
using static TitanFx.DependencyProperty.WinUI.Model.Constants;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal record OnValueChangedInfo
{
    public required string MethodName { get; init; }
    public OnValueChangedSignature Signature { get; init; }

    internal static OnValueChangedInfo Capture(
        INamedTypeSymbol ownerType,
        string onValueChangedName
    )
    {
        return new OnValueChangedInfo
        {
            MethodName = onValueChangedName,
            Signature = ownerType
                .GetMembers(onValueChangedName)
                .OfType<IMethodSymbol>()
                .Select(GetSignature)
                .Order()
                .DefaultIfEmpty(OnValueChangedSignature.NotFound)
                .FirstOrDefault(static v => v is not OnValueChangedSignature.Unsupported),
        };
    }

    private static OnValueChangedSignature GetSignature(IMethodSymbol method)
    {
        var parameters = method.Parameters.AsSpan();
        var sender = OnValueChangedSignature.Unsupported;
        if (!method.IsStatic)
            sender = OnValueChangedSignature.This;
        else if (parameters is [{ } s, .. var rest])
        {
            parameters = rest;
            if (IsDependencyObject(s.Type))
                sender = OnValueChangedSignature.DependencyObject;
            else
                sender = OnValueChangedSignature.Owner;
        }

        if (sender is OnValueChangedSignature.Unsupported)
            return sender;
        var eventArgs = parameters switch
        {
            [] => OnValueChangedSignature.Only,
            [{ } p0] when IsEventArgs(p0.Type) => OnValueChangedSignature.EventArgs,
            [{ }] => OnValueChangedSignature.New,
            [{ }, { }] => OnValueChangedSignature.NewOld,
            _ => OnValueChangedSignature.Unsupported,
        };
        return sender | eventArgs;
    }

    private static bool IsDependencyObject(ITypeSymbol type)
    {
        return type
            is INamedTypeSymbol
            {
                Name: DependencyObject,
                ContainingNamespace:
                {
                    Name: "Xaml",
                    ContainingNamespace:
                    {
                        Name: "UI",
                        ContainingNamespace:
                        { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }
                    }
                }
            };
    }

    private static bool IsEventArgs(ITypeSymbol type)
    {
        return type
            is INamedTypeSymbol
            {
                Name: DependencyPropertyChangedEventArgs,
                ContainingNamespace:
                {
                    Name: "Xaml",
                    ContainingNamespace:
                    {
                        Name: "UI",
                        ContainingNamespace:
                        { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }
                    }
                }
            };
    }
}
