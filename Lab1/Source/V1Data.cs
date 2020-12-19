using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Lab1
{
    abstract class V1Data : IEnumerable<DataItem>, INotifyPropertyChanged
    {
        protected static readonly CultureInfo DATE_FORMAT = CultureInfo.GetCultureInfo("ru");
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected string info;
        protected DateTime date;

        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public V1Data(string info, DateTime date)
        {
            Info = info;
            Date = date;
        }

        public abstract float[] NearZero(float eps);
        public override string ToString()
        {
            return $"[{Date.ToString(DATE_FORMAT)}] {Info}";
        }
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
