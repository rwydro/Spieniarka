﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Xml;
using TOReportApplication.Logic.Enums;
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;

namespace TOReportApplication.Views
{
    /// <summary>
    ///     Interaction logic for AdminModeView.xaml
    /// </summary>
    public partial class AdminModeView : UserControl
    {
        private ContextMenu contextMenu;
        private XmlDocument xmldoc;
        private string fileName = "";

        public AdminModeView()
        {
            InitializeComponent();
            DataGridName.Visibility = Visibility.Visible;
        }




        private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (IAdminModeViewModel) DataContext;
            viewModel.SearchButtonClickedAction += OnSearchButtonClicked;
        }

        private void OnSearchButtonClicked(object machine)
        {
            contextMenu = new ContextMenu();
            xmldoc = null;
            switch (machine)
            {
                case DataContextEnum.FormViewModel:
                    fileName = "FormaKolumny.xml";
                    break;
                case DataContextEnum.BlowingMachineViewModel:
                    fileName = "SpieniarkaKolumny.xml";
                    break;
                case DataContextEnum.ContinuousBlowingMachineViewModel:
                    fileName = "SpieniarkaCiaglaKolumny.xml";
                    break;
            }

            var file = new FileInfo(fileName);
            if (!file.Exists || file.Length == 0) return;

            xmldoc = new XmlDocument();
            var fs = new FileStream(
                fileName,
                FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            fs.Close();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            var dataGridTextColumn = e.Column as DataGridTextColumn;
            if (e.PropertyType == typeof(DateTime))
                if (dataGridTextColumn != null)
                    dataGridTextColumn.Binding.StringFormat = "{0:dd-MM-yyyy HH:mm:ss}";

            if (!string.IsNullOrEmpty(displayName)) e.Column.Header = displayName;
        }

        private void SaveColumnsVisibilityInFile()
        {
            using (var settingwriter = new XmlTextWriter(
                fileName,
                null))
            {
                settingwriter.WriteStartDocument();
                settingwriter.WriteStartElement("Machine");
                foreach (var element in DataGridName.Columns)
                {
                    var newColumnHeader = element.Header.ToString().Replace(" ", "_").Replace("[", "_")
                        .Replace("]", "_").Replace("%", "_");
                    settingwriter.WriteStartElement(newColumnHeader);
                    settingwriter.WriteStartElement("Visibility");
                    settingwriter.WriteString(element.Visibility.ToString());
                    settingwriter.WriteEndElement();
                    settingwriter.WriteEndElement();
                }

                settingwriter.Close();
            }
        }

        private void DataGrid_OnAutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (var element in DataGridName.Columns)
            {
                element.Visibility = Visibility.Visible;
                var newColumnHeader = element.Header.ToString().Replace(" ", "_").Replace("[", "_").Replace("]", "_")
                    .Replace("%", "_");
                var menuItem = new MenuItem
                {
                    IsChecked = true,
                    Header = element.Header
                };

                contextMenu.Items.Add(menuItem);
                menuItem.Click += OnMenuItemClicked;
                menuItem.Checked += OnMenuItemChecked;
                menuItem.Unchecked += OnMenuItemUnchecked;

                if (xmldoc != null)
                {
                    var xmlNode = xmldoc.GetElementsByTagName(newColumnHeader);
                    ;
                    var elementVisibilityText = xmlNode[0].InnerText;
                    element.Visibility = elementVisibilityText == "Visible" ? Visibility.Visible : Visibility.Hidden;
                    menuItem.IsChecked = element.Visibility == Visibility.Visible;
                }
            }

            DataGridName.Visibility = Visibility.Visible;
        }

        private void OnMenuItemUnchecked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            foreach (var column in DataGridName.Columns)
            {
                if (!column.Header.ToString().Contains(menuItem.Header.ToString())) continue;
                column.Visibility = Visibility.Hidden;
                return;
            }
        }

        private void OnMenuItemChecked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            foreach (var column in DataGridName.Columns)
            {
                if (!column.Header.ToString().Contains(menuItem.Header.ToString())) continue;
                column.Visibility = Visibility.Visible;
                return;
            }
        }

        private void OnMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null) menuItem.IsChecked = !menuItem.IsChecked;
            SaveColumnsVisibilityInFile();
        }


        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            var columnHeader = sender as DataGridColumnHeader;
            columnHeader.ContextMenu = contextMenu;
        }

        private string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default) return displayName.DisplayName;
            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    var attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (var i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                            return displayName.DisplayName;
                    }
                }
            }

            return null;
        }

        private void AdminMode_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((IAdminModeViewModel)DataContext).Dispose();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            // navigate up to the header 
            var columnHeader = sender as DataGridColumnHeader;
            columnHeader.ContextMenu = contextMenu;


        }

        private void DataGridName_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
        }

    }
}