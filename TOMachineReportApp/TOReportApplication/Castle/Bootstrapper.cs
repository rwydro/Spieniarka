using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.Castle
{
    public class Bootstrapper 
    {
        private IUnityContainer container;

        public Bootstrapper(IUnityContainer container)
        {
            this.container = container;
        }

        public void Install()
        {

            container.RegisterType<IShell, Shell>();
            container.RegisterType<IFormViewModel, FormViewModel>();
            container.RegisterType<ISettingsAndFilterPanelViewModel, SettingsAndFilterPanelViewModel>();
            container.RegisterType<IBlowingMachineViewModel, BlowingMachineViewModel>();
            container.RegisterType<IApplicationRepository,ApplicationRepository>();
            container.RegisterType<IMyLogger, MyLogger>();
        }

    }
}