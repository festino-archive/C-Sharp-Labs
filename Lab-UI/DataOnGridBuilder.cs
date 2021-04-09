using Lab1;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lab_UI
{
    class DataOnGridBuilder : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Error {
            get
            {
                string msg = this["Name"];
                string msg2 = this["Count"];
                string msg3 = this["MinValue"];
                if (msg == null) msg = msg2;
                else if (msg2 != null) msg += ", " + msg2;
                if (msg == null) msg = msg3;
                else if (msg3 != null) msg += ", " + msg3;
                return msg;
            }
        }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "Name":
                        if (Name == null) msg = $"Name cannot be null";
                        else if (Name.Length < 1) msg = $"Name length must be at least 1";
                        else if (MainColl == null) msg = $"No MainCollection";
                        else if (MainColl.Contains(Name)) msg = $"MainCollection is already contains \"{Name}\"";
                        break;
                    case "Count":
                        if (Count <= 2) msg = "Count must be greater than 2";
                        break;
                    case "MinValue":
                    case "MaxValue":
                        if (MinValue >= MaxValue) msg = "MinValue must be less than MaxValue";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public int Count
        {
            get => count;
            set
            {
                count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }
        public float MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinValue"));
            }
        }
        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
            }
        }

        private string name = "id";
        private int count = 10;
        private float minValue = -1, maxValue = 1;

        private V1MainCollection MainColl;

        public DataOnGridBuilder()
        {
        }
        public DataOnGridBuilder(V1MainCollection mainColl) : this()
        {
            MainColl = mainColl;
        }

        public void SetMainCollection(V1MainCollection mainColl)
        {
            MainColl = mainColl;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
        }

        public void BuildAndAdd()
        {
            string info = Name;
            DateTime date = DateTime.Now;
            Grid grid = new Grid(0, 0.1f, count);
            V1DataOnGrid data = new V1DataOnGrid(info, date, grid);
            data.InitRandom(MinValue, MaxValue);
            MainColl.Add(data);
        }
    }
}
