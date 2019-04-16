using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class ContinuousBlowingMachineViewModel : ViewModelBase, IContinuousBlowingMachineViewModel
    {
        private readonly IMyLogger logger;
        public ISettingsAndFilterPanelViewModel<ContinuousBlowingMachineReportModel> SettingsAndFilterPanelViewModel { get; }


        private ObservableCollection<ContinuousBlowingMachineReportModel> blowingMachineReportItems;
        public ObservableCollection<ContinuousBlowingMachineReportModel> BlowingMachineReportItems
        {
            get => blowingMachineReportItems;
            set
            {
                if (blowingMachineReportItems == value) return;
                blowingMachineReportItems = value;
                OnPropertyChanged(nameof(BlowingMachineReportItems));
            }
        }

        private ObservableCollection<ContinuousBlowingMachineShiftReportModel> blowingMachineShiftReportItems;
        public ObservableCollection<ContinuousBlowingMachineShiftReportModel> BlowingMachineShiftReportItems
        {
            get => blowingMachineShiftReportItems;
            set
            {
                if (blowingMachineShiftReportItems == value) return;
                blowingMachineShiftReportItems = value;
                OnPropertyChanged(nameof(BlowingMachineShiftReportItems));
            }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (value == selectedDate) return;
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        private bool isGenerateShiftReportButtonEnabled { get; set; }

        public bool IsGenerateShiftReportButtonEnabled
        {
            get => isGenerateShiftReportButtonEnabled;
            set
            {
                if (isGenerateShiftReportButtonEnabled == value)
                    return;
                isGenerateShiftReportButtonEnabled = value;
                OnPropertyChanged("IsGenerateShiftReportButtonEnabled");
            }
        }

        private bool isSaveReportInFileButtonEnabled { get; set; }

        public bool IsSaveInFileReportButtonEnabled
        {
            get => isSaveReportInFileButtonEnabled;
            set
            {
                if (isSaveReportInFileButtonEnabled == value)
                    return;
                isSaveReportInFileButtonEnabled = value;
                OnPropertyChanged("IsSaveInFileReportButtonEnabled");
            }
        }

        private string selectedShift;
        public string SelectedShift
        {
            get => selectedShift;
            set
            {
                if (selectedShift == value)
                    return;
                selectedShift = value;
                IsSaveInFileReportButtonEnabled = false;
                OnPropertyChanged("SelectedShift");
            }
        }

        public ObservableCollection<string> ComboBoxValueCollection { get; set; }
        public DelegateCommand SaveInFileCommand { get; set; }
        public DelegateCommand GenerateShiftReporCommand { get; set; }

        public ContinuousBlowingMachineViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger) : base(container)
        {
            this.logger = logger;
            SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel<ContinuousBlowingMachineReportModel>(container, repository, logger);
            SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.BlowingMachineVIewModel;
            SettingsAndFilterPanelViewModel.GeneratedModelItemsAction += OnGetBlowingMachineReportsModelItems;
            BlowingMachineReportItems = new ObservableCollection<ContinuousBlowingMachineReportModel>();
            ComboBoxValueCollection = new ObservableCollection<string>
            {
                "1",
                "2",
                "3"
            };
            SelectedShift = ComboBoxValueCollection.First();
            SaveInFileCommand = new DelegateCommand(SaveReportInFile);
            GenerateShiftReporCommand = new DelegateCommand(GenerateShiftReport);
            IsSaveInFileReportButtonEnabled = false;
            IsGenerateShiftReportButtonEnabled = false;
        }

        private void GenerateShiftReport()
        {
            var shiftCalendarManager = new ShiftCalendarManager();
            var shiftInfo = shiftCalendarManager.GetShiftInfo(selectedShift);
            var shiftReport =
                ContinuousBlowingMachineReportLogic.GenerateContinuousBlowingMachineFileReportModel(
                    BlowingMachineReportItems.ToArray(), shiftInfo,SelectedDate);

            BlowingMachineShiftReportItems = new ObservableCollection<ContinuousBlowingMachineShiftReportModel>(shiftReport);

            IsSaveInFileReportButtonEnabled = true;
        }

        private string CreateMissingFolders(string path)
        {
            try
            {
                path = Path.Combine(path, DateTime.Now.Year.ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture));
                if (!Directory.Exists(Path.Combine(path, DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture))))
                    Directory.CreateDirectory(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                logger.logger.Error("Failed during creating directory", ex);
                MessageBoxHelper.ShowMessageBox("Błąd podczas tworzenia katalogów sprawdź uprawnienia", MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Failed during creating directory", ex);
                MessageBoxHelper.ShowMessageBox("Błąd podczas tworzenia katalogów sprawdź uprawnienia", MessageBoxIcon.Exclamation);
            }

            return path;
        }

        private void SaveReportInFile()
        {
            try
            {
                var document = BlowingMachineXmlGenerator.GenerateXml(SelectedShift, blowingMachineShiftReportItems.ToList());
                SaveInFileLogic.SaveInFileAndOpen(CreateMissingFolders(ConfigurationManager.AppSettings["PathToBlowingMachineReport"]),
                    SelectedShift, document, logger);
            }
            catch (InvalidOperationException ex)
            {
                logger.logger.Error("Pusta kolekcja", ex);
                MessageBoxHelper.ShowMessageBox("Brak wartości dla wybranej daty", MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                MessageBoxHelper.ShowMessageBox("Błąd podczas zapisu pliku spróbuj ponownie", MessageBoxIcon.Exclamation);
            }
        }
      
        private void OnGetBlowingMachineReportsModelItems(object sender, EventBaseArgs<ContinuousBlowingMachineReportModel> e)
        {
            BlowingMachineReportItems.Clear();
            e.ReportModel.Model.ForEach(x => BlowingMachineReportItems.Add(x));
            SelectedDate = e.ReportModel.SelectedDate;
            IsGenerateShiftReportButtonEnabled = true;
        }


        public void Dispose()
        {
            
        }
    }
}
