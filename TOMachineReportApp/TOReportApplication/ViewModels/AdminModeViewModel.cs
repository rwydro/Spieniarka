using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
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
        private IMyLogger logger;

        public Action<object> SearchButtonClickedAction { get; set; }


        private ReadOnlyCollection<object> reportModelToSaveInFile;
        public ReadOnlyCollection<object> ReportModelToSaveInFile
        {
            get => reportModelToSaveInFile;
            set
            {
                if (reportModelToSaveInFile == value) return;
                reportModelToSaveInFile = value;
                OnPropertyChanged(nameof(ReportModelToSaveInFile));
            }
        }

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

        public AdminModeViewModel(IUnityContainer container, IApplicationRepository dbConnection, IMyLogger logger) : base(container)
        {
            this.logger = logger;
            AdminModeSettingsAndFilterPanelViewModel = new AdminModeSettingsAndFilterPanelViewModel(container);
            AdminModeSettingsAndFilterPanelViewModel.SearchButtonClickAction += OnSearchButtonClick;
            SearchCriteriaLogic = new AdminModeSearchCriteriaLogic(dbConnection);
            ReportModel = new ObservableCollection<object>();
        }

        public void SaveInFile()
        {
            var pathToReport = Path.Combine(ConfigurationManager.AppSettings["PathToFormReport"], "Raport_Forma_" + ".xml");
   
            switch (AdminModeSettingsAndFilterPanelViewModel.SelectedMachine)
            {
                case DataContextEnum.FormViewModel:
                    SaveInFileLogic.OnSaveReportInFile<List<object>>(ReportModel.ToList(), pathToReport, logger);
                    break;
                case DataContextEnum.BlowingMachineViewModel:
                    SaveInFileLogic.OnSaveReportInFile<List<BlowingMachineReportModel>>(ReportModelToSaveInFile.ToList(), pathToReport, logger);
                    break;
                case DataContextEnum.ContinuousBlowingMachineViewModel:
                    SaveInFileLogic.OnSaveReportInFile<List<object>>(ReportModelToSaveInFile.ToList(), pathToReport, logger);
                    break;
            }
          
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
