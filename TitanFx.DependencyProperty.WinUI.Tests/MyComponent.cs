using Microsoft.UI.Xaml;

namespace TitanFx.DependencyProperty.WinUI.Tests;

internal partial class MyComponent : DependencyObject
{
    [DependencyProperty]
    public partial int Id { get; set; }
}
