using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        }

        private void FilterDataOnGrid(object c, FilterEventArgs e) => e.Accepted = e.Item is V1DataOnGrid;
        private void FilterDataCollection(object c, FilterEventArgs e) => e.Accepted = e.Item is V1DataCollection;

        private void SetMainCollection(V1MainCollection coll)
        {
            if (MainColl != null)
                MainColl.CollectionChanged -= OnCollectionChange;
            coll.CollectionChanged += OnCollectionChange;
            MainColl = coll;

            OnCollectionChange(this, null);
        }

        private bool Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                MainColl.Save(dialog.FileName);
                return true;
            }
            return false;
        }

        private bool Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;
            dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                V1MainCollection coll = new V1MainCollection();
                coll.Load(dialog.FileName);
                SetMainCollection(coll);
                return true;
            }
            return false;
        }

        private bool SuggestSave()
        {
            if (!MainColl.HasUnsavedChanges)
                return true;

            string caption = "Несохранённые изменения";
            string message = "Данные были изменены. Вы хотите сохранить их?";
            MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
            MessageBoxResult res = MessageBox.Show(message, caption, buttons);
            if (res == MessageBoxResult.No)
                return true;
            if (res == MessageBoxResult.Cancel)
                return false;
            if (res == MessageBoxResult.Yes)
                try
                {
                    return Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            return false;
        }

        private void OnCollectionChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            DataContext = null;
            DataContext = MainColl;
            textBlock_CollProp.Text = "Максимальное значение длины вектора поля: " + MainColl.MaxLength.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");
            SetMainCollection(new V1MainCollection());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!SuggestSave())
                e.Cancel = true;
        }

        // menu event handlers

        private void Item_New_Click(object sender, RoutedEventArgs e)
        {
            if (SuggestSave())
                SetMainCollection(new V1MainCollection());
        }

        private void Item_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SuggestSave())
                    Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Item_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Item_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null)
            {
                V1Data data = (V1Data)listBox_Main.SelectedItem;
                MainColl.Remove(data.Info, data.Date);
            }
        }
    }
}
