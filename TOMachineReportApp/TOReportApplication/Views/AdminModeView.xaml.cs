﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using TOReportApplication.ViewModels.interfaces;

namespace TOReportApplication.Views
{
    /// <summary>
    ///     Interaction logic for AdminModeView.xaml
    /// </summary>
    public partial class AdminModeView : UserControl
    {
        private readonly ContextMenu contextMenu = new ContextMenu();
        private XmlDocument xmldoc;


        public AdminModeView()
        {
            InitializeComponent();
        }


        private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (IAdminModeViewModel) DataContext;
            viewModel.SearchButtonClickedAction += OnSearchButtonClicked;
        }

        private void OnSearchButtonClicked()
        {
            var file = new FileInfo(
                "FormaKolumny.xml");
            if (file.Exists && file.Length != 0)
            {
                xmldoc = new XmlDocument();

                var fs = new FileStream(
                    "FormaKolumny.xml",
                    FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
            }
        }

        private void AdminMode_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((IAdminModeViewModel) DataContext).Dispose();
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
                "FormaKolumny.xml",
                null))
            {
                settingwriter.WriteStartDocument();
                settingwriter.WriteStartElement("Form");
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
    }
}