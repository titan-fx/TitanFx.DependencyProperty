using Microsoft.CodeAnalysis;
using TitanFx.DependencyProperty.Wpf.Model;

namespace TitanFx.DependencyProperty.Wpf;

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
