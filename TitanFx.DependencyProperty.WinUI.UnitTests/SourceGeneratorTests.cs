using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.UI.Xaml;

namespace TitanFx.DependencyProperty.WinUI.UnitTests;

public class SourceGeneratorTests
{
    [Theory]
    [InlineData(
        "Simple integer",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { get; set; }
            }
            """
    )]
    [InlineData(
        "Defaulting integer",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { get; set; } = 123;
            }
            """
    )]
    [InlineData(
        "Complex defaulting integer",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { get; set; } = int.Parse("123") * 456;
            }
            """
    )]
    [InlineData(
        "Getter only",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { get; }
            }
            """
    )]
    [InlineData(
        "Setter only",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { set; }
            }
            """
    )]
    [InlineData(
        "Init only",
        """
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial int Id { init; }
            }
            """
    )]
    [InlineData(
        "Multiple properties",
        """
            [ContentProperty(Name = nameof(Content))]
            public partial class MyComponent
            {
                [DependencyProperty]
                public partial FrameworkElement? Content { get; set; }

                [DependencyProperty]
                public partial int Id { get; set; } = 123;

                [DependencyProperty]
                public partial string Source { get; set; } = "https://test.invalid/";

                public partial Guid InternalId { get; init; }
            }
            """
    )]
    [InlineData(
        "Missing callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = "HandleValueChanged")]
                public partial int Id { get; set; }
            }
            """
    )]
    [InlineData(
        "Static DependencyObject EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static DependencyObject NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new, int old)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static DependencyObject NewOld object callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, object? @new, object? old)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static DependencyObject New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static DependencyObject Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static TOwner EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(MyComponent sender, DependencyPropertyChangedEventArgs e)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static TOwner NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static TOwner New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
            }
            """
    )]
    [InlineData(
        "Static TOwner Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(MyComponent sender)
                {
                }
            }
            """
    )]
    [InlineData(
        "Instance EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private void HandleValueChanged(DependencyPropertyChangedEventArgs e)
                {
                }
            }
            """
    )]
    [InlineData(
        "Instance NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private void HandleValueChanged(int @new, int old)
                {
                }
            }
            """
    )]
    [InlineData(
        "Instance New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private void HandleValueChanged(int @new)
                {
                }
            }
            """
    )]
    [InlineData(
        "Instance Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer DependencyObject EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                }
                private static void HandleValueChanged(DependencyObject sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, DependencyPropertyChangedEventArgs e)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(DependencyPropertyChangedEventArgs e)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer TObject EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, DependencyPropertyChangedEventArgs e)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(DependencyPropertyChangedEventArgs e)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer Instance EventArgs callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(DependencyPropertyChangedEventArgs e)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer DependencyObject NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer TObject NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new, int old)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer Instance NewOld callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new, int old)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer DependencyObject New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender, int @new)
                {
                }
                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer TOwner New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender, int @new)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer Instance New callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged(int @new)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer DependencyObject Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(DependencyObject sender)
                {
                }
                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer TOwner Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private static void HandleValueChanged(MyComponent sender)
                {
                }
                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "Prefer Instance Only callback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(OnValueChanged = nameof(HandleValueChanged))]
                public partial int Id { get; set; }

                private void HandleValueChanged()
                {
                }
            }
            """
    )]
    [InlineData(
        "struct CreateDefaultValueCallback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(CreateDefaultValue = nameof(GetInitialId))]
                public partial int Id { get; set; }

                private static int GetInitialId()
                {
                    return 0;
                }
            }
            """
    )]
    [InlineData(
        "class CreateDefaultValueCallback",
        """
            public partial class MyComponent
            {
                [DependencyProperty(CreateDefaultValue = nameof(GetInitialId))]
                public partial string Id { get; set; }

                private static string GetInitialId()
                {
                    return "ABC";
                }
            }
            """
    )]
    [InlineData(
        "Attached property",
        """
            [AttachedDependencyProperty<FrameworkElement, bool>("IsSuccess")]
            public partial class MyComponent;
            """
    )]
    [InlineData(
        "Attached property with OnValueChanged",
        """
            [AttachedDependencyProperty<FrameworkElement, bool>("IsSuccess", OnValueChanged = nameof(HandleIsSuccessChanged))]
            public partial class MyComponent
            {
                private static void HandleIsSuccessChanged(FrameworkElement target, bool newValue, bool oldValue)
                {
                }
            }
            """
    )]
    [InlineData(
        "Attached property ignores instance OnValueChanged",
        """
            [AttachedDependencyProperty<FrameworkElement, bool>("IsSuccess", OnValueChanged = nameof(HandleIsSuccessChanged))]
            public partial class MyComponent
            {
                private void HandleIsSuccessChanged(FrameworkElement target, bool newValue, bool oldValue)
                {
                }
            }
            """
    )]
    public async Task Run(string name, string source)
    {
        var result = AssertGenerator(
            $"""
            using System;
            using TitanFx.DependencyProperty.WinUI;
            using Microsoft.UI.Xaml;

            {source}
            """,
            TestContext.Current.CancellationToken
        );

        var settings = new VerifySettings();
        settings.UseParameters(name);
        settings.IgnoreGeneratedResult(x =>
            x.HintName is "Microsoft.CodeAnalysis.EmbeddedAttribute.cs" or "Util.g.cs"
        );
        _ = await Verify(result, settings);
    }

    private static GeneratorDriverRunResult AssertGenerator(
        string source,
        CancellationToken cancellationToken
    )
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: cancellationToken);
        var compilation = CSharpCompilation.Create(
            assemblyName: "tests",
            syntaxTrees: [syntaxTree],
            references: _references
        );
        var generator = new SourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var result = driver.RunGenerators(compilation, cancellationToken);
        return result.GetRunResult();
    }

    private static readonly MetadataReference[] _references =
    [
        .. Enumerable
            .Select([typeof(DependencyObject)], static t => t.Assembly)
            .Concat(AppDomain.CurrentDomain.GetAssemblies())
            .Select(static a => a.Location)
            .Distinct()
            .Where(static l => l is not null)
            .Select(static l => MetadataReference.CreateFromFile(l)),
    ];
}
