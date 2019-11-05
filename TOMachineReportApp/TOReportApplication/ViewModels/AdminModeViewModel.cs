using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Logic.interfaces;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class AdminModeViewModel : ViewModelBase, IAdminModeViewModel
    {
        private readonly IMyLogger logger;

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

        private bool isSaveInFileButtonEnabled;
        public bool IsSaveInFileButtonEnabled
        {
            get { return isSaveInFileButtonEnabled;}
            set
            {
                if (value == IsSaveInFileButtonEnabled) return;
                isSaveInFileButtonEnabled = value;
                OnPropertyChanged(nameof(IsSaveInFileButtonEnabled));
            }
        }

        private readonly IAdminModeSearchCriteriaLogic SearchCriteriaLogic;

        public AdminModeViewModel(IUnityContainer container, IApplicationRepository dbConnection, IMyLogger logger) :
            base(container)
        {
            this.logger = logger;
            AdminModeSettingsAndFilterPanelViewModel = new AdminModeSettingsAndFilterPanelViewModel(container);
            AdminModeSettingsAndFilterPanelViewModel.SearchButtonClickAction += OnSearchButtonClick;
            SearchCriteriaLogic = new AdminModeSearchCriteriaLogic(dbConnection);
            ReportModel = new ObservableCollection<object>();
            IsSaveInFileButtonEnabled = false;
        }

       

        public IAdminModeSettingsAndFilterPanelViewModel AdminModeSettingsAndFilterPanelViewModel { get; }

        public Action<object> SearchButtonClickedAction { get; set; }

        private void OnSearchButtonClick()
        {
            SearchButtonClickedAction(AdminModeSettingsAndFilterPanelViewModel.SelectedMachine);
            switch (AdminModeSettingsAndFilterPanelViewModel.SelectedMachine)
            {
                case DataContextEnum.FormViewModel:
                    var formModel = SearchCriteriaLogic.GenerateFormReportModel(
                        AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate,
                        AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);
                    ReportModel = new ObservableCollection<object>(formModel);
                    break;
                case DataContextEnum.BlowingMachineViewModel:
                    var blowingMachineModel = SearchCriteriaLogic.GenerateBlowingMachineReport(
                        AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate,
                        AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);
                    ReportModel = new ObservableCollection<object>(blowingMachineModel);
                    break;
                case DataContextEnum.ContinuousBlowingMachineViewModel:
                    var continuousBlowingMachineModel = SearchCriteriaLogic.GenerateContinuousBlowingMachineReport(
                        AdminModeSettingsAndFilterPanelViewModel.SelectedFromDate,
                        AdminModeSettingsAndFilterPanelViewModel.SelectedToDate);
                    ReportModel = new ObservableCollection<object>(continuousBlowingMachineModel);
                    break;
            }

            if(ReportModel.Count > 0)
                IsSaveInFileButtonEnabled = true;
        }

        public void Dispose()
        {
        }
     }
}