using System;
using Avalonia.Controls;
using Avalonia.Media;
using IsbnConverter.ViewModels;

namespace IsbnConverter.Views;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    public override void Show()
    {
        var systemFonts = FontManager.Current.SystemFonts;
        if (systemFonts.TryGetGlyphTypeface("Segoe UI Variable Text", FontStyle.Normal, FontWeight.Normal, FontStretch.Normal, out _))
            FontFamily = new("Segoe UI Variable Text");
        else if (systemFonts.TryGetGlyphTypeface("Segoe UI", FontStyle.Normal, FontWeight.Normal, FontStretch.Normal, out _))
            FontFamily = new("Segoe UI");
        
        if (DataContext is MainWindowViewModel vm)
        {
            if (systemFonts.TryGetGlyphTypeface("Segoe Fluent Icons", FontStyle.Normal, FontWeight.Normal, FontStretch.Normal, out _))
                vm.SymbolFontFamily = new("Segoe Fluent Icons");
        }
        base.Show();
        App.OnThemeChanged(this, EventArgs.Empty);
    }
}