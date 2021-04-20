using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        public static RoutedCommand AddCustomGridCommand = new RoutedCommand("AddCustomGrid", typeof(MainWindow));
        private V1MainCollection MainColl;

        public MainWindow()
        {
            InitializeComponent();
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

        private void FilterDataOnGrid(object c, FilterEventArgs e) => e.Accepted = e.Item is V1DataOnGrid;
        private void FilterDataCollection(object c, FilterEventArgs e) => e.Accepted = e.Item is V1DataCollection;

        private void SetMainCollection(V1MainCollection coll)
        {
            if (MainColl != null)
                MainColl.CollectionChanged -= OnCollectionChange;
            MainColl = coll;
            MainColl.CollectionChanged += OnCollectionChange;
            GetGridBuilder().SetMainCollection(MainColl);

            OnCollectionChange(this, null);
        }

        private DataOnGridBuilder GetGridBuilder()
        {
            return Resources["GridBuilder"] as DataOnGridBuilder;
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

        // menu event handlers

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
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

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainColl != null && MainColl.HasUnsavedChanges;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
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

        private void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBox_Main?.SelectedItem != null;
        }

        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null)
            {
                V1Data data = (V1Data)listBox_Main.SelectedItem;
                MainColl.Remove(data.Info, data.Date);
            }
        }

        private void CanAddCustomGridCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = GetGridBuilder() != null && GetGridBuilder().Error == null;
        }

        private void AddCustomGridCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            GetGridBuilder().BuildAndAdd();
        }

        private void Item_New_Click(object sender, RoutedEventArgs e)
        {
            if (SuggestSave())
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
