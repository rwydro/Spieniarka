using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Logic.interfaces;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class AdminModeViewModel: ViewModelBase, IAdminModeViewModel
    {

        public Action<object> SearchButtonClickedAction { get; set; }

        private ObservableCollection<object> reportModel;
        public ObservableCollection<object> ReportModel
        {
            get => reportModel;
            set
            {
                if (reportModel == value) return;
                reportModel = value;
                OnPropertyChanged(nameof(ReportModel));
            }
        }

        public IAdminModeSettingsAndFilterPanelViewModel AdminModeSettingsAndFilterPanelViewModel { get; }

        private IAdminModeSearchCriteriaLogic SearchCriteriaLogic;

        public AdminModeViewModel(IUnityContainer container, IApplicationRepository dbConnection) : base(container)
        {
            AdminModeSettingsAndFilterPanelViewModel = new AdminModeSettingsAndFilterPanelViewModel(container);
            AdminModeSettingsAndFilterPanelViewModel.SearchButtonClickAction += OnSearchButtonClick;
            SearchCriteriaLogic = new AdminModeSearchCriteriaLogic(dbConnection);
            ReportModel = new ObservableCollection<object>();
        }

        private void OnSearchButtonClick()
        {

            SearchButtonClickedAction(AdminModeSettingsAndFilterPanelViewModel.SelectedMachine);
            switch (AdminModeSettingsAndFilterPanelViewModel.SelectedMachine)
            {
                case DataContextEnum.FormViewModel:      
                    var formModel = SearchCriteriaLogic.GenerateFormReportModel(AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate, AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);  
                    ReportModel = new ObservableCollection<object>(formModel);
                    break;
                case DataContextEnum.BlowingMachineViewModel:
                    var blowingMachineModel = SearchCriteriaLogic.GenerateBlowingMachineReport(AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate, AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);
                    ReportModel = new ObservableCollection<object>(blowingMachineModel);
                    break;
                case DataContextEnum.ContinuousBlowingMachineViewModel:
                    var continuousBlowingMachineModel = SearchCriteriaLogic.GenerateContinuousBlowingMachineReport(AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate, AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);
                    ReportModel = new ObservableCollection<object>(continuousBlowingMachineModel);
                    break;
            }
        }

        public void Dispose()
        {

        }
    }
}
