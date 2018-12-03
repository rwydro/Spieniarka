using System;
using System.Threading.Tasks;
using System.Windows;
using log4net.Config;
using TOReportApplication.Castle;
using Unity;

namespace TOReportApplication
{

    public static class AsyncHelper
    {
        public static Action<string, Exception> OnUnhandledException;
        public static void InAsyncSafe(this Task task)
        {

            task.ContinueWith(
                t => OnUnhandledException("Async", t.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    public partial class App : Application
    {
        private IUnityContainer container;
        public Bootstrapper bootstrapper;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            XmlConfigurator.Configure();
            container = new UnityContainer();
            bootstrapper=new Bootstrapper(container);
            bootstrapper.Install();
            var start = container.Resolve<IShell>();
            start.Run();

        }
    }
}
