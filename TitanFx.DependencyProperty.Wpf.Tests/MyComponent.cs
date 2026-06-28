using System;
using System.Windows;

namespace TitanFx.DependencyProperty.Wpf.Tests;

[AttachedDependencyProperty<FrameworkElement, bool>("IsSuccess")]
[AttachedDependencyProperty<FrameworkElement, bool>("ReadOnlyAttached", ReadOnly = true)]
internal partial class MyComponent : DependencyObject
{
    [DependencyProperty]
    public partial int Id { get; set; }

    [DependencyProperty(OnValueChanged = nameof(HandleTextChanged))]
    public partial string? Text { get; set; }

    private void HandleTextChanged(string? newValue, string? oldValue)
    {
        Console.WriteLine($"[{Id}]Text changed: {oldValue} => {newValue}");
    }

    [DependencyProperty(CreateDefaultValue = nameof(InitializeScore))]
    public partial int Score { get; set; }

    private static int InitializeScore() => 0;

    [DependencyProperty(CreateDefaultValue = nameof(InitializeHint))]
    public partial string Hint { get; set; }

    private static string InitializeHint => "abc";

    [DependencyProperty(ValidateValue = nameof(GreaterThanZero))]
    public partial int Validate { get; set; }

    private static bool GreaterThanZero(int value) => value > 0;

    [DependencyProperty(CoerceValue = nameof(ToInteger))]
    public partial int Coerce { get; set; }

    private int ToInteger(object? value) => (int)(value ?? 0) + Validate;

    [DependencyProperty]
    public partial int ReadOnlyInstance { get; }
}
