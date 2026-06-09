using Microsoft.CodeAnalysis;
using TitanFx.DependencyProperty.WinUI.Model;

namespace TitanFx.DependencyProperty.WinUI;

[Generator]
public sealed class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        AttributeGenerator.Emit(context);
        var dependencyObjects = DependencyObjectInfo.Capture(context);

        DependencyPropertyGenerator.Emit(context, dependencyObjects);
    }
}
