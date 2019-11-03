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


        private ReadOnlyCollection<object> reportModelToSaveInFile;

        private readonly IAdminModeSearchCriteriaLogic SearchCriteriaLogic;

        public AdminModeViewModel(IUnityContainer container, IApplicationRepository dbConnection, IMyLogger logger) :
            base(container)
        {
            this.logger = logger;
            AdminModeSettingsAndFilterPanelViewModel = new AdminModeSettingsAndFilterPanelViewModel(container);
            AdminModeSettingsAndFilterPanelViewModel.SearchButtonClickAction += OnSearchButtonClick;
            SearchCriteriaLogic = new AdminModeSearchCriteriaLogic(dbConnection);
            ReportModel = new ObservableCollection<object>();
        }

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

        public Action<object> SearchButtonClickedAction { get; set; }

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

        public void SaveInFile()
        {
            var saveFileDialog1 = new SaveFileDialog {CreatePrompt = false, Filter = "Xml|*.xml", OverwritePrompt = true};

            switch (AdminModeSettingsAndFilterPanelViewModel.SelectedMachine)
            {
                case DataContextEnum.FormViewModel:
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var fileModel = new List<FormDateReportDBModel>();
                        foreach (var el in ReportModelToSaveInFile)
                            if (el is FormDateReportDBModel)
                                fileModel.Add(el as FormDateReportDBModel);
                        SaveInFileLogic.OnSaveReportInFile<List<FormDateReportDBModel>>(fileModel,
                            saveFileDialog1.FileName, logger);
                    }

                    break;
                case DataContextEnum.BlowingMachineViewModel:
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var fileModel = new List<BlowingMachineReportModel>();
                        foreach (var el in ReportModelToSaveInFile)
                            if (el is BlowingMachineReportModel)
                                fileModel.Add(el as BlowingMachineReportModel);
                        SaveInFileLogic.OnSaveReportInFile<List<BlowingMachineReportModel>>(fileModel,
                            saveFileDialog1.FileName, logger);
                    }

                    break;
                case DataContextEnum.ContinuousBlowingMachineViewModel:
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var fileModel = new List<ContinuousBlowingMachineReportModel>();
                        foreach (var el in ReportModelToSaveInFile)
                            if (el is ContinuousBlowingMachineReportModel)
                                fileModel.Add(el as ContinuousBlowingMachineReportModel);
                        SaveInFileLogic.OnSaveReportInFile<List<ContinuousBlowingMachineReportModel>>(fileModel,
                            saveFileDialog1.FileName, logger);
                    }

                    break;
            }
        }

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
        }

        public void Dispose()
        {
        }
     }
}