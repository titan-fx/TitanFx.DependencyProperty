# TitanFx.DependencyProperty

A simple source generator which reduces the boilerplate for creating `DependencyProperty` instances.

# Usage

## Basic usage

Source:
```csharp
using TitanFx.DependencyProperty.WinUI;

[AttachedDependencyProperty<Image, bool>("Blur")]
public partial class MyComponent : DependencyObject
{
	[DependencyProperty]
	public partial string Text { get; set; }
}
```

Rough output:
```csharp
partial class MyComponent
{
	public static readonly DependencyProperty BlurProperty
		= DependencyProperty.RegisterAttached(
			"Blur",
			typeof(bool),
			typeof(MyComponent),
			new PropertyMetadata(
				defaultValue: default(bool)
			)
		);

	public static partial bool GetBlur(Image target)
	{
		return (bool)target.GetValue(BlurProperty);
	}

	public static partial void SetBlur(Image target, bool value)
	{
		target.SetValue(BlurProperty, value);
	}

	public static readonly DependencyProperty TextProperty
		= DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(MyComponent),
			new PropertyMetadata(
				defaultValue: default(string)
			)
		);

	public partial string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}
}

static partial class MyComponentExtensions
{
	extension(Image target)
	{
		get => MyComponent.GetBlur(target);
		set => MyComponent.SetBlur(target, value);
	}
}
```

### CreateDefaultValue

[Microsoft documentation](https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/custom-dependency-properties#createdefaultvaluecallback)

By setting the `CreateDefaultValue` property on the `[DependencyProperty]` attribute to the name of a static method or property, you can generate a different value for each UI thread.
If you do not set a `CreateDefaultValue` then a `defaultValue: default(TProperty)` will be used instead.

```csharp
public partial class MyComponent : DependencyObject
{

	[DependencyProperty(CreateDefaultValue = nameof(GetDefaultText)]
	public partial string Text { get; set; }

	private static string GetDefaultText() => "This value can be unique per UI thread!";
}
```

### PropertyChangedCallback

[Microsoft documentation](https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/custom-dependency-properties#property-changed-callback-method)

By setting the `OnValueChanged` property on the `[DependencyProperty]` attribute to the name of a method, you can listen to changes to the value of the property.
There are multiple supported signatures for this method listed below. If the method has multiple overloads, the order in which overloads are preferred is the same order as below:

```csharp
public partial class MyComponent : DependencyObject
{

	[DependencyProperty(OnValueChanged = nameof(HandleTextChanged)]
	public partial string Text { get; set; }

	static void HandleTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {}
	static void HandleTextChanged(MyComponent sender, DependencyPropertyChangedEventArgs e) {}
	void HandleTextChanged(DependencyPropertyChangedEventArgs e) {}

	static void HandleTextChanged(DependencyObject sender, string newValue, string oldValue) {}
	static void HandleTextChanged(MyComponent sender, string newValue, string oldValue) {}
	void HandleTextChanged(string newValue, string oldValue) {}

	static void HandleTextChanged(DependencyObject sender, string newValue) {}
	static void HandleTextChanged(MyComponent sender, string newValue) {}
	void HandleTextChanged(string newValue) {}

	static void HandleTextChanged(DependencyObject sender) {}
	static void HandleTextChanged(MyComponent sender) {}
	void HandleTextChanged() {}
}
```

