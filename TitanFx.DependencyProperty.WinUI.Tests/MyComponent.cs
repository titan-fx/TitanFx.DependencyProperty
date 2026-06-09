using System;
using Microsoft.UI.Xaml;

namespace TitanFx.DependencyProperty.WinUI.Tests;

internal partial class MyComponent : DependencyObject
{
    [DependencyProperty]
    public partial int Id { get; set; }

    [DependencyProperty(OnValueChanged = nameof(HandleTextChanged))]
    public partial string Text { get; set; }

    private void HandleTextChanged(string newValue, string oldValue)
    {
        Console.WriteLine($"[{Id}]Text changed: {oldValue} => {newValue}");
    }

    [DependencyProperty(CreateDefaultValue = nameof(InitializeScore))]
    public partial int Score { get; set; }

    private static int InitializeScore() => 0;

    [DependencyProperty(CreateDefaultValue = nameof(InitializeHint))]
    public partial string Hint { get; set; }

    private static string InitializeHint() => "abc";
}
