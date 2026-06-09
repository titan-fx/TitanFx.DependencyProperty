using System;
using System.CodeDom.Compiler;

namespace TitanFx.DependencyProperty.WinUI;

internal static class Util
{
    public static IDisposable WriteBlock(IndentedTextWriter writer)
    {
        writer.WriteLine("{");
        writer.Indent++;

        return new CloseBlock(writer);
    }

    private sealed class CloseBlock(IndentedTextWriter writer) : IDisposable
    {
        public void Dispose()
        {
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
