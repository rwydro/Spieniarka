using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public class BlowingMachineViewModel : ViewModelBase, IBlowingMachineViewModel
    {
        private readonly IMyLogger logger;
        private Action _onControlLoaded;

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

        public ObservableCollection<string> ComboBoxValueCollection { get; set; }

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

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate;}
            set
            {
                if (value == selectedDate) return;
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        public DelegateCommand SaveInFileCommand { get; set; }
        public DelegateCommand GenerateShiftReporCommand { get; set; }

        public BlowingMachineViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger)
            : base(container)
        {
            this.logger = logger;
            SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel<BlowingMachineReportModel>(container, repository, logger);
            SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.BlowingMachineVIewModel;
            SettingsAndFilterPanelViewModel.GeneratedModelItemsAction += OnGetBlowingMachineReportsModelItems;
            BlowingMachineReportItems = new ObservableCollection<BlowingMachineReportModel>();
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

        private ObservableCollection<BlowingMachineReportModel> blowingMachineReportItems;
        public ObservableCollection<BlowingMachineReportModel> BlowingMachineReportItems
        {
            get => blowingMachineReportItems;
            set
            {
                if (blowingMachineReportItems == value) return;
                blowingMachineReportItems = value;
                OnPropertyChanged(nameof(BlowingMachineReportItems));
            }
        }

        private ObservableCollection<BlowingMachineReportModel> blowingMachineShiftReportItems;
        public ObservableCollection<BlowingMachineReportModel> BlowingMachineShiftReportItems
        {
            get => blowingMachineShiftReportItems;
            set
            {
                if(blowingMachineShiftReportItems == value) return;
                blowingMachineShiftReportItems = value;
                OnPropertyChanged(nameof(BlowingMachineShiftReportItems));
            }
        }

        public ISettingsAndFilterPanelViewModel<BlowingMachineReportModel> SettingsAndFilterPanelViewModel { get; }


        private void SaveReportInFile()
        {
            try
            {
                var document = SaveBlowingMachineReportInFileLogic.GenerateXml(SelectedShift, BlowingMachineShiftReportItems.ToList());
                SaveInFileAndOpen(CreateMissingFolders(ConfigurationManager.AppSettings["PathToBlowingMachineReport"]),
                    SelectedShift, document);
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

        private void GenerateShiftReport()
        {
            var shiftCalendarManager = new ShiftCalendarManager();
            var shiftInfo = shiftCalendarManager.GetShiftInfo(selectedShift);

            BlowingMachineShiftReportItems = new ObservableCollection<BlowingMachineReportModel>((from item in BlowingMachineReportItems
                where
                    item.DateTimeStop >= new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                        shiftInfo.BeginningShift.Hours, shiftInfo.BeginningShift.Minutes, shiftInfo.BeginningShift.Seconds) &&
                    item.DateTimeStop <= new DateTime(SelectedDate.Year, SelectedDate.Month, shiftInfo.NumberOfShift == ShiftInfoEnum.ThirdShift ? SelectedDate.AddDays(1).Day: SelectedDate.Day,
                        shiftInfo.EndShift.Hours, shiftInfo.EndShift.Minutes, shiftInfo.EndShift.Seconds)
                select item).ToList());
            IsSaveInFileReportButtonEnabled = true;
        }

        private void SaveInFileAndOpen(string path, string shift, XmlDocument document)
        {
            try
            {
                var str = Path.Combine(path, string.Format("{0}_zmiana_{1}.xml", DateTime.Now.Day, shift));
                document.Save(str);
                bool result;
                if (!bool.TryParse(ConfigurationManager.AppSettings["IsOpenReportAfterSaved"], out result))
                    return;
                Process.Start(str);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                MessageBoxHelper.ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                MessageBoxHelper.ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
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

        private void OnGetBlowingMachineReportsModelItems(object sender, EventBaseArgs<BlowingMachineReportModel> e)
        {
            BlowingMachineReportItems.Clear();
            e.ReportModel.Model.ForEach(x => BlowingMachineReportItems.Add(x));
            SelectedDate = e.ReportModel.SelectedDate;
            IsGenerateShiftReportButtonEnabled = true;
        }

        public void Dispose()
        {
            SettingsAndFilterPanelViewModel.GeneratedModelItemsAction -= OnGetBlowingMachineReportsModelItems;
        }
    }
}