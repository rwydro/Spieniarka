using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class ContinuousBlowingMachineViewModel : ViewModelBase, IBlowingMachineViewModel
    {
       // public ISettingsAndFilterPanelViewModel SettingsAndFilterPanelViewModel { get; }
        private readonly IMyLogger logger;

        public ContinuousBlowingMachineViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger) : base(container)
        {
            //this.logger = logger;
            //SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel(container, repository, logger);
            //SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.BlowingMachineVIewModel;
           // SettingsAndFilterPanelViewModel.GeneratedModelItemsAction += OnGetBlowingMachineReportsModelItems;
        }

        //private void OnGetBlowingMachineReportsModelItems(BlowingMachineReportDto obj)
        //{
            
        //}

        public void Dispose()
        {
            
        }
    }
}
