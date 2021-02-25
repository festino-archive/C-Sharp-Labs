using Lab1;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab_UI
{
    class DataItemToCoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as DataItem? == null)
                return "object error";
            return ((DataItem)value).T;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
