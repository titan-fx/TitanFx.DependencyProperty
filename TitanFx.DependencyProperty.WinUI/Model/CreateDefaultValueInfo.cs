using System.Linq;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.WinUI.Model;

internal record CreateDefaultValueInfo
{
    public required string Name { get; init; }
    public required bool ReturnsReferenceType { get; init; }
    public required bool IsMethod { get; init; }

    internal static CreateDefaultValueInfo? Capture(
        INamedTypeSymbol containingType,
        string? memberName
    )
    {
        if (memberName is null)
            return null;

        var rank = containingType
            .GetMembers(memberName)
            .Select(static s =>
                s switch
                {
                    IMethodSymbol { IsStatic: true, Parameters: [], TypeParameters: [] } m =>
                        m.ReturnType switch
                        {
                            { IsReferenceType: true } => MemberRank.ReferenceMethod,
                            { IsValueType: true } => MemberRank.ValueMethod,
                            _ => MemberRank.Unsupported,
                        },
                    IPropertySymbol { IsStatic: true, Parameters: [] } p => p.Type switch
                    {
                        { IsReferenceType: true } => MemberRank.ReferenceProperty,
                        { IsValueType: true } => MemberRank.ValueProperty,
                        _ => MemberRank.Unsupported,
                    },
                    _ => MemberRank.Unsupported,
                }
            )
            .Order()
            .FirstOrDefault();

        return new()
        {
            Name = memberName,
            ReturnsReferenceType = rank is MemberRank.ReferenceMethod,
            IsMethod = rank is MemberRank.ReferenceMethod or MemberRank.ValueMethod,
        };
    }
}

file enum MemberRank
{
    ReferenceMethod,
    ValueMethod,
    ReferenceProperty,
    ValueProperty,
    Unsupported,
}
