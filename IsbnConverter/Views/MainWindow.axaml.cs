using System;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace IsbnConverter.Views;

public partial class MainWindow : Window
{
    private const string Good = "✔️";
    private const string Bad = "❌";
    private readonly SemaphoreSlim UpLock = new(1, 1);

    public MainWindow()
    {
        InitializeComponent();
    }
    
    public override void Show()
    {
        base.Show();
        App.OnThemeChanged(this, EventArgs.Empty);
    }
        private void Isbn10_OnTextChanged(object? sender, TextChangedEventArgs? e)
        {
            if (Isbn10Check is null)
                return;

            var isbn10 = Isbn10!.Text!.ToUpperInvariant().Replace("-", "").Replace(" ", "");
            if (isbn10.Length < 10)
            {
                Isbn10Check.Text = "";
                return;
            }
            if (isbn10.Length > 10 || !isbn10.All(c => c is >= '0' and <= '9' or 'X'))
            {
                Isbn10Check.Text = Bad;
                return;
            }

            var crcVal = Calc10(isbn10);
            if (crcVal == isbn10)
            {
                Isbn10Check.Text = Good;
                if (UpLock.Wait(0))
                {
                    var isbn13 = Calc13("978" + isbn10[..^1]);
                    Isbn13.Text = isbn13;
                    UpLock.Release();
                }
            }
            else
                Isbn10Check.Text = Bad;
        }

        private void Isbn13_OnTextChanged(object? sender, TextChangedEventArgs? e)
        {
            if (Isbn13Check is null)
                return;

            var isbn13 = Isbn13!.Text!.ToUpperInvariant().Replace("-", "").Replace(" ", "");
            if (isbn13.Length < 13)
            {
                Isbn13Check.Text = "";
                return;
            }
            if (isbn13.Length > 13 || !isbn13.All(c => c is >= '0' and <= '9'))
            {
                Isbn13Check.Text = Bad;
                return;
            }

            var crcVal = Calc13(isbn13);
            if (crcVal == isbn13)
            {
                Isbn13Check.Text = Good;
                if (UpLock.Wait(0))
                {
                    var isbn10 = isbn13.StartsWith("978") ? Calc10(isbn13[3..^1]) : "N/A";
                    Isbn10.Text = isbn10;
                    UpLock.Release();
                }
            }
            else
                Isbn13Check.Text = Bad;
        }

        private static string Calc10(string isbn10)
        {
            var sum = 0;
            for (int i = 0, w = 10; i < 9; i++, w--)
                sum += (isbn10[i] - '0') * w;
            var m = (11 - (sum % 11)) % 11;
            var csum = m < 10 ? (char)('0' + m) : 'X';
            return isbn10[..9] + csum;
        }
        
        private static string Calc13(string isbn13)
        {
            var sum = 0;
            for (int i = 0; i < 12; i += 2)
                sum += isbn13[i] - '0';
            for (int i = 1; i < 12; i += 2)
                sum += (isbn13[i] - '0') * 3;
            var m = (10 - (sum % 10)) % 10;
            var csum = (char)('0' + m);
            return isbn13[..12] + csum;
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