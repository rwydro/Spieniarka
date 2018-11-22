using System;
using System.Collections.Generic;
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
using TOReportApplication.ViewModels;

namespace TOReportApplication.Views
{
    /// <summary>
    /// Interaction logic for FormView.xaml
    /// </summary>
    public partial class FormView : UserControl
    {
        public FormView()
        {
            InitializeComponent();
        }

        private void FormView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((FormViewModel) this.DataContext).Dispose();
        }
    }
}
