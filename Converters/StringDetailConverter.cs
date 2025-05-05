using System.Globalization;

namespace BtgClientManager.Converters
{
    public class StringCombinerConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string firstName && parameter is string lastName)
            {
                return $"{firstName} {lastName}";
            }
            
            return value?.ToString() ?? string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}