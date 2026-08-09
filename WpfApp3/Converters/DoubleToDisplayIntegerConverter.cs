using System;
using System.Globalization;
using System.Windows.Data;

namespace HaruaConvert.Converters
{
    public sealed class DoubleToDisplayIntegerConverter : IValueConverter
    {
        public double Scale { get; set; } = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return ((int)Math.Round(number * Scale, MidpointRounding.AwayFromZero)).ToString(culture);
            }

            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
