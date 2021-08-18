using System;
using System.Collections.Generic;
using System.Configuration;
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
            var isAdminModeEnabled = ConfigurationManager.AppSettings["IsAdminModeEnabled"];
            adminModeButton.Visibility = (isAdminModeEnabled != null && bool.Parse(isAdminModeEnabled)) ? Visibility.Visible: Visibility.Hidden;
        }


        private void AdminMode_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IAdminModeViewModel>();
        }

        private void BlockHistory_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IBlockHistoryViewModel>();
        }


        private void FormButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IFormViewModel>();
        }

        private void BlowingMachineButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IBlowingMachineViewModel>();
        }


        private void ContinuousBlowingMachineButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = container.Resolve<IContinuousBlowingMachineViewModel>();
        }
    }
}
