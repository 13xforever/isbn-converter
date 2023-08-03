using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IsbnConverter.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string tintColor = "#ffffff";

    [NotifyPropertyChangedFor(nameof(CopySymbol))]
    [NotifyPropertyChangedFor(nameof(PasteSymbol))]
    [ObservableProperty] private FontFamily symbolFontFamily = new("avares://IsbnConverter/Assets/Fonts#Font Awesome 6 Free");
    public string CopySymbol => SymbolFontFamily.Name is "Segoe Fluent Icons" ? "\ue8c8" : "\uf0c5"; 
    public string PasteSymbol => SymbolFontFamily.Name is "Segoe Fluent Icons" ? "\ue77f" : "\uf0ea"; 
    
    [ObservableProperty] private string isbn10 = "";
    [ObservableProperty] private string isbn13 = "";
    [ObservableProperty] private bool? isbn10Valid = null;
    [ObservableProperty] private bool? isbn13Valid = null;

    [RelayCommand]
    private async Task Copy(string isbn)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow.Clipboard: {} cb })
        {
            await cb.SetTextAsync(isbn);
        }
    }
    
    [RelayCommand]
    private async Task Paste(string target)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow.Clipboard: {} cb })
        {
            var val = await cb.GetTextAsync();
            if (val is null)
                return;
            
            if (target is nameof(Isbn10))
                Isbn10 = val;
            else if (target is nameof(Isbn13))
                Isbn13 = val;
        }
    }
    
    partial void OnIsbn10Changed(string value)
    {
        var val10 = value.ToUpperInvariant().Replace("-", "").Replace(" ", "");
        if (val10 is not {Length: >=10})
        {
            Isbn10Valid = null;
            return;
        }
        
        if (val10.Length > 10 || !val10.All(c => c is >= '0' and <= '9' or 'X'))
        {
            Isbn10Valid = false;
            return;
        }

        var crcVal = Calc10(val10);
        if (crcVal == val10)
        {
            Isbn10Valid = true;
            Isbn13 = Calc13("978" + val10[..^1]);
        }
        else
            Isbn10Valid = false;
    }

    partial void OnIsbn13Changed(string value)
    {
        var val13 = value.ToUpperInvariant().Replace("-", "").Replace(" ", "");
        if (val13.Length < 13)
        {
            Isbn13Valid = null;
            return;
        }
        if (val13.Length > 13 || !val13.All(c => c is >= '0' and <= '9'))
        {
            Isbn13Valid = false;
            return;
        }

        var crcVal = Calc13(val13);
        if (crcVal == val13)
        {
            Isbn13Valid = true;
            Isbn10 = val13.StartsWith("978") ? Calc10(val13[3..^1]) : "N/A";
        }
        else
        {
            Isbn13Valid = false;
        }
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
}