using System;
using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace TitanFx.DependencyProperty.Wpf;

internal static class Util
{
    public static IDisposable WriteBlock(
        IndentedTextWriter writer,
        string open = "{",
        string close = "}"
    )
    {
        writer.WriteLine(open);
        writer.Indent++;

        return new WriteBlockHandle(writer, close);
    }

    public static IDisposable Indent(IndentedTextWriter writer)
    {
        writer.Indent++;

        return new IndentHandle(writer);
    }

    private sealed class WriteBlockHandle(IndentedTextWriter writer, string close) : IDisposable
    {
        public void Dispose()
        {
            writer.Indent--;
            writer.WriteLine(close);
        }
    }

    private sealed class IndentHandle(IndentedTextWriter writer) : IDisposable
    {
        public void Dispose()
        {
            writer.Indent--;
        }
    }

    public static SymbolDisplayFormat FullyQualifiedNullableFormat { get; } =
        new SymbolDisplayFormat(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
                | SymbolDisplayMiscellaneousOptions.UseSpecialTypes
                | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
        );
}
