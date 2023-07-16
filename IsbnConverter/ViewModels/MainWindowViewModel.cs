using ReactiveUI;

namespace IsbnConverter.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string tintColor = "#ffffff";
    private double tintOpacity = 1.0;
    private double materialOpacity = 0.69;
    private double luminosityOpacity = 1.0;
    
    public string TintColor
    {
        get => tintColor;
        set => this.RaiseAndSetIfChanged(ref tintColor, value);
    }

    public double TintOpacity
    {
        get => tintOpacity;
        set => this.RaiseAndSetIfChanged(ref tintOpacity, value);
    }

    public double MaterialOpacity
    {
        get => materialOpacity;
        set => this.RaiseAndSetIfChanged(ref materialOpacity, value);
    }

    public double LuminosityOpacity
    {
        get => luminosityOpacity;
        set => this.RaiseAndSetIfChanged(ref luminosityOpacity, value);
    }
}