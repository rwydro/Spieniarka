using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentNHibernate.Utils;
using Newtonsoft.Json;
using TOReportApplication.Model;
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;
using UserControl = System.Windows.Controls.UserControl;

namespace TOReportApplication.Views
{
    /// <summary>
    /// Interaction logic for BlockHistoryView.xaml
    /// </summary>
    public partial class BlockHistoryView : UserControl
    {
        public BlockHistoryView()
        {
            InitializeComponent();
        }

        private void BlockHistoryView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((IBlockHistoryViewModel)this.DataContext).Dispose();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

            if ((e.Column is DataGridTextColumn))
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;

                if ((string)column.Header == "Komora" ||
                    (string)column.Header == "Silos" ||
                    (string)column.Header == "Komentarz" ||
                    (string)column.Header == "Gatunek")
                {
                    column.IsReadOnly = false;
                }

            }
        }
        private string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }


        private void DataGrid_OnAutoGeneratingBlowingMachineColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (e.PropertyType == typeof(DateTime))
            {
                DataGridTextColumn dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "{0:dd-MM-yyyy HH:mm:ss}";
                }
            }

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           
                        var saveFileDialog1 = new SaveFileDialog { CreatePrompt = false, Filter = "Csv|*.csv", OverwritePrompt = true };
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            //XmlDocument doc = new XmlDocument();
                            var dataToSave = SaveReportInFile();
                            using (var file = new StreamWriter(saveFileDialog1.FileName))
                            {
                                file.Write(dataToSave);
                                file.Flush();
                            }
                        }
        }

        private string SaveReportInFile()
        {
            StringBuilder sb = new StringBuilder();
            String result = "";

            if (FormRow.Items.Count > 0)
            {
                FormRow.SelectAllCells();
                FormRow.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, FormRow);
                CBlowingRow.UnselectAllCells();
                result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                sb.Append(result);
            }
      

            if (BlowingRow.Items.Count > 0)
            {
                BlowingRow.SelectAllCells();
                BlowingRow.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, BlowingRow);
                BlowingRow.UnselectAllCells();
                result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                sb.Append(result);
            }


            if (CBlowingRow.Items.Count > 0)
            {
                CBlowingRow.SelectAllCells();
                CBlowingRow.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, CBlowingRow);
                CBlowingRow.UnselectAllCells();
                result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                sb.Append(result);
            }


            return sb.ToString();
        }
    }
}
