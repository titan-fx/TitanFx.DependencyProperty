using Microsoft.CodeAnalysis;
using TitanFx.DependencyProperty.WinUI.Model;

namespace TitanFx.DependencyProperty.WinUI;

[Generator]
public sealed class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        UtilGenerator.Emit(context);
        var dependencyObjects = DependencyObjectInfo.Capture(context);

        DependencyPropertyGenerator.Emit(context, dependencyObjects);
    }
}
