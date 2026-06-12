namespace TitanFx.DependencyProperty.WinUI.Model;

internal static class Constants
{
    public const string TitanFx = nameof(TitanFx);
    public const string DependencyProperty = nameof(DependencyProperty);
    public const string DependencyObject = nameof(DependencyObject);
    public const string DependencyPropertyChangedEventArgs = nameof(
        DependencyPropertyChangedEventArgs
    );
    public const string DependencyPropertyAttribute = nameof(DependencyPropertyAttribute);
    public const string AttachedDependencyPropertyAttribute = nameof(
        AttachedDependencyPropertyAttribute
    );
    public const string WinUI = nameof(WinUI);
    public const string Namespace = $"{TitanFx}.{DependencyProperty}.{WinUI}";
    public const string OnValueChanged = nameof(OnValueChanged);
    public const string CreateDefaultValue = nameof(CreateDefaultValue);

    public static class Types
    {
        public const string EmbeddedAttribute = "global::Microsoft.CodeAnalysis.EmbeddedAttribute";
        public const string ConditionalAttribute =
            "global::System.Diagnostics.ConditionalAttribute";
        public const string ObseleteAttribute = "global::System.ObsoleteAttribute";
        public const string AttributeUsageAttribute = "global::System.AttributeUsageAttribute";
        public const string AttributeTargets = "global::System.AttributeTargets";
        public const string NotNullWhenAttribute =
            "global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute";
        public const string Attribute = "global::System.Attribute";
        public const string Int32 = "global::System.Int32";
        public const string String = "global::System.String";
        public const string Boolean = "global::System.Boolean";
        public const string Object = "global::System.Object";
        public const string Delegate = "global::System.Delegate";
        public const string Unsafe = "global::System.Runtime.CompilerServices.Unsafe";
        public const string MethodInfo = "global::System.Reflection.MethodInfo";
        public const string WeakReference = "global::System.WeakReference";
        public const string InvalidOperationException = "global::System.InvalidOperationException";
        public const string NotSupportedException = "global::System.NotSupportedException";
        public const string DependencyPropertyAttribute =
            $"global::{Namespace}.{Constants.DependencyPropertyAttribute}";
        public const string AttachedDependencyPropertyAttribute =
            $"global::{Namespace}.{Constants.AttachedDependencyPropertyAttribute}";

        public const string DependencyProperty = "global::Microsoft.UI.Xaml.DependencyProperty";
        public const string DependencyPropertyChangedEventArgs =
            "global::Microsoft.UI.Xaml.DependencyPropertyChangedEventArgs";
        public const string DependencyObject = "global::Microsoft.UI.Xaml.DependencyObject";
        public const string PropertyMetadata = "global::Microsoft.UI.Xaml.PropertyMetadata";
    }
}
