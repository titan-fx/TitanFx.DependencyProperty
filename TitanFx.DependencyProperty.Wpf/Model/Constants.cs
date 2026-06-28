namespace TitanFx.DependencyProperty.Wpf.Model;

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
    public const string Wpf = nameof(Wpf);
    public const string Namespace = $"{TitanFx}.{DependencyProperty}.{Wpf}";
    public const string OnValueChanged = nameof(OnValueChanged);
    public const string CreateDefaultValue = nameof(CreateDefaultValue);
    public const string ValidateValue = nameof(ValidateValue);
    public const string CoerceValue = nameof(CoerceValue);
    public const string ReadOnly = nameof(ReadOnly);

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
        public const string NotImplementedException = "global::System.NotImplementedException";
        public const string DependencyPropertyAttribute =
            $"global::{Namespace}.{Constants.DependencyPropertyAttribute}";
        public const string AttachedDependencyPropertyAttribute =
            $"global::{Namespace}.{Constants.AttachedDependencyPropertyAttribute}";

        public const string DependencyProperty = "global::System.Windows.DependencyProperty";
        public const string DependencyPropertyKey = "global::System.Windows.DependencyPropertyKey";
        public const string DependencyPropertyChangedEventArgs =
            "global::System.Windows.DependencyPropertyChangedEventArgs";
        public const string DependencyObject = "global::System.Windows.DependencyObject";
        public const string PropertyMetadata = "global::System.Windows.PropertyMetadata";
    }
}
