using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using IsbnConverter.Utils;
using IsbnConverter.ViewModels;
using IsbnConverter.Views;

namespace IsbnConverter;

public partial class App : Application
{
    private static readonly WindowTransparencyLevel[] DesiredTransparencyHints =
    {
        WindowTransparencyLevel.Mica,
        WindowTransparencyLevel.AcrylicBlur,
        WindowTransparencyLevel.None,
    };

    private readonly Lazy<bool> isMicaCapable = new(() =>
        Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: Window w }
        && w.ActualTransparencyLevel == WindowTransparencyLevel.Mica
    );

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            desktop.MainWindow.Activated += OnActivated;
            desktop.MainWindow.Deactivated += OnDeactivated;
            desktop.MainWindow.ActualThemeVariantChanged += OnThemeChanged;
            if (isMicaCapable.Value && desktop.MainWindow is Window w)
            {
                RenderOptions.SetTextRenderingMode(w, TextRenderingMode.Antialias);
            }
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        if (!isMicaCapable.Value || sender is not Window w)
            return;

        w.TransparencyLevelHint = DesiredTransparencyHints;
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        if (!isMicaCapable.Value || sender is not Window { DataContext: MainWindowViewModel vm } w)
            return;

        w.TransparencyLevelHint = Array.Empty<WindowTransparencyLevel>();
        if (w.ActualThemeVariant == ThemeVariant.Light)
            vm.TintColor = ThemeConsts.LightThemeTintColor;
        else if (w.ActualThemeVariant == ThemeVariant.Dark)
            vm.TintColor = ThemeConsts.DarkThemeTintColor;
    }
    
    internal static void OnThemeChanged(object? sender, EventArgs e)
    {
        if (sender is not Window { DataContext: MainWindowViewModel vm } w)
            return;

        if (w.ActualThemeVariant == ThemeVariant.Light)
            vm.TintColor = ThemeConsts.LightThemeTintColor;
        else if (w.ActualThemeVariant == ThemeVariant.Dark)
            vm.TintColor = ThemeConsts.DarkThemeTintColor;
    }
}