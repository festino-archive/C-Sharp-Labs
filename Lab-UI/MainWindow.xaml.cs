using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab1;
using Microsoft.Win32;
using Grid = Lab1.Grid;

namespace Lab_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private V1MainCollection MainColl;

        public MainWindow()
        {
            InitializeComponent();
            SetMainCollection(new V1MainCollection());
        }

        private void SetMainCollection(V1MainCollection coll)
        {
            MainColl = coll;
            DataContext = MainColl;
            coll.CollectionChanged += OnCollectionChange;
            OnCollectionChange(this, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void OnCollectionChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            listBox_Main.Items.Clear();
            foreach (V1Data data in MainColl)
            {
                listBox_Main.Items.Add(data.ToString());
            }
            textBlock_CollProp.Text = "Максимальное значение длины вектора поля: " + MainColl.MaxLength.ToString();
        }

        private void Item_New_Click(object sender, RoutedEventArgs e)
        {
            SetMainCollection(new V1MainCollection());
        }

        private void Item_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                try {
                    V1MainCollection coll = new V1MainCollection();
                    coll.Load(dialog.FileName);
                    SetMainCollection(coll);
                }
                catch (Exception) {

                }
            }
        }

        private void Item_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SetMainCollection(new V1MainCollection());
        }

        private void Item_Defaults_Click(object sender, RoutedEventArgs e)
        {
            MainColl.AddDefaults();
        }

        private void Item_DefaultColl_Click(object sender, RoutedEventArgs e)
        {
            V1DataCollection coll = new V1DataCollection("default DataCollection", DateTime.Now);
            coll.InitRandom(10, 0, 1, 0, 1);
            MainColl.Add(coll);
        }

        private void Item_DefaultGrid_Click(object sender, RoutedEventArgs e)
        {
            V1DataOnGrid coll = new V1DataOnGrid("default DataOnGrid", DateTime.Now, new Grid(0, 0.1f, 11));
            coll.InitRandom(0, 1);
            MainColl.Add(coll);
        }

        private void Item_AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    V1DataOnGrid coll = V1DataOnGrid.FromFile(dialog.FileName);
                    MainColl.Add(coll);
                }
                catch (Exception)
                {

                }
            }
        }

        private void Item_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null)
            {
                int index = listBox_Main.SelectedIndex;
                V1Data data = null;
                foreach (V1Data d in MainColl)
                {
                    if (index == 0)
                    {
                        data = d;
                        break;
                    }
                    index--;
                }
                if (data != null)
                {
                    MainColl.Remove(data.Info, data.Date);
                }
            }
        }
    }
}
