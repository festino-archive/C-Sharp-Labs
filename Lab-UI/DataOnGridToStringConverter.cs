using Lab1;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab_UI
{
    class DataOnGridToStringConverter : IValueConverter
    {
        readonly static string Format = "f3";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as V1DataOnGrid == null)
                return value?.ToString(); //"Не выбран V1DataOnGrid " + value?.ToString();
            if (parameter as int? == null)
                return "parameter error : " + parameter?.ToString() + " (" + parameter?.GetType() + ")";
            int param = (int)parameter;
            V1DataOnGrid data = (V1DataOnGrid)value;
            if (param < 0)
                param = data.Grid.Count + param;
            if (param < 0 || param >= data.Grid.Count)
                return "V1DataOnGrid пуста";
            return data.Grid.GetTime(param).ToString(Format) + " : " + data.Values[param].ToString(Format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
