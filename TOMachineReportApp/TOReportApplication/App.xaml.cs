using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using log4net.Config;
using TOReportApplication.Castle;
using TOReportApplication.Logic;
using Unity;

namespace TOReportApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;
        public Bootstrapper bootstrapper;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
           
            XmlConfigurator.Configure();
         //   MyLogger.log.Error("FirstMessage");
            container = new UnityContainer();
            bootstrapper=new Bootstrapper(container);
            bootstrapper.Install();
            var start = container.Resolve<IShell>();
            start.Run();

        }
    }
}
