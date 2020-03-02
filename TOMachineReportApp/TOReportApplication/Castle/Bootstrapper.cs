using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.ViewModels;
using TOReportApplication.ViewModels.interfaces;
using TOReportApplication.Views;
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
            container.RegisterType<IBlowingMachineViewModel, BlowingMachineViewModel>();
            container.RegisterType<IContinuousBlowingMachineViewModel, ContinuousBlowingMachineViewModel>();
            container.RegisterType<IFormSetShiftReportDataViewModel, FormSetShiftReportDataViewModel>();
            container.RegisterType<IApplicationRepository,ApplicationRepository>();
            container.RegisterType<IBlockHistoryGetDataLogic, BlockHistoryGetDataLogic>();
            container.RegisterType<IBlockHistoryViewModel,BlockHistoryViewModel>();
            container.RegisterType<IMyLogger, MyLogger>();
            container.RegisterType<IAdminModeViewModel, AdminModeViewModel>();
        }

    }
}