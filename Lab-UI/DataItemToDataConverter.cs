using Lab1;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab_UI
{
    class DataItemToDataConverter : IValueConverter
    {
        readonly static string Format = "f3";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as DataItem? == null)
                return "object error";
            DataItem item = (DataItem)value;
            return item.Value.ToString(Format) + " [" + item.Value.Length().ToString(Format) + "]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
