using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace IsbnConverter.Views;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    public override void Show()
    {
        base.Show();
        App.OnThemeChanged(this, EventArgs.Empty);
    }

    private void Paste10_OnClick(object sender, RoutedEventArgs e)
    {
        Isbn10.SelectAll();
        Isbn10.Paste();
    }

    private void Copy10_OnClick(object sender, RoutedEventArgs e)
    {
        Isbn10.SelectAll();
        Isbn10.Copy();
    }

    private void Paste13_OnClick(object sender, RoutedEventArgs e)
    {
        Isbn13.SelectAll();
        Isbn13.Paste();
    }

    private void Copy13_OnClick(object sender, RoutedEventArgs e)
    {
        Isbn13.SelectAll();
        Isbn13.Copy();
    }

    private void Isbn10_OnGotFocus(object sender, GotFocusEventArgs e) => Isbn10?.SelectAll();
    private void Isbn13_OnGotFocus(object sender, GotFocusEventArgs e) => Isbn13?.SelectAll();
    private void Isbn10_OnPointerReleased(object? sender, PointerReleasedEventArgs e) => Isbn10?.SelectAll();
    private void Isbn13_OnPointerReleased(object? sender, PointerReleasedEventArgs e) => Isbn13?.SelectAll();
}