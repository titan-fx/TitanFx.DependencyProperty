using System.Runtime.CompilerServices;

namespace TitanFx.DependencyProperty.Wpf.UnitTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}
