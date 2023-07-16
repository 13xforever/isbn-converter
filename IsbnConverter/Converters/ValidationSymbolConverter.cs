using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace IsbnConverter.Converters;

public class ValidationSymbolConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b ? "\uf058" : "\uf06a";
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}