using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;

namespace TOReportApplication.Views
{
    /// <summary>
    /// Interaction logic for BlowingMachineView.xaml
    /// </summary>
    public partial class BlowingMachineView : UserControl
    {
        public BlowingMachineView()
        {
                InitializeComponent();
        }

        private void BlowingMachineView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            var dataContexType = this.DataContext.GetType();
            if (dataContexType == typeof(BlowingMachineViewModel))
            {
                ((IBlowingMachineViewModel)this.DataContext).Dispose();
            }

            if (dataContexType.TypeInitializer == typeof(ContinuousBlowingMachineViewModel))
            {
                ((IContinuousBlowingMachineViewModel)this.DataContext).Dispose();
            }
           
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
    }
}
