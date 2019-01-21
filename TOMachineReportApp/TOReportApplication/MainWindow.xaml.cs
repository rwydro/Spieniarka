using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private IUnityContainer container;
        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
        }


        private void FormButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IFormViewModel>();
        }

        private void BlowingMachineButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IBlowingMachineViewModel>();
        }
    }
}
