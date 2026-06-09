using System.Runtime.CompilerServices;

namespace TitanFx.DependencyProperty.WinUI.UnitTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}
