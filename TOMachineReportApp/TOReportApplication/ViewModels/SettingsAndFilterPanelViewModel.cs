using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public enum DataContextEnum
    {
        FormViewModel,
        BlowingMachineVIewModel
    }

    public class SettingsAndFilterPanelViewModel : ViewModelBase, ISettingsAndFilterPanelViewModel
    {
        private readonly IApplicationRepository dbConnection;
        private readonly IMyLogger logger;

        private string selectedShift;
        private readonly ShiftCalendarManager shiftCalendar;

        public SettingsAndFilterPanelViewModel(IUnityContainer unityContainer, IApplicationRepository dbConnection,
            IMyLogger logger)
            : base(unityContainer)
        {
            this.dbConnection = dbConnection;
            this.logger = logger;
            shiftCalendar = new ShiftCalendarManager();
            GenerateReportCommand = new DelegateCommand(async () => await OnGenereteDateReport());
            SaveInFileCommand = new DelegateCommand(OnSaveInFile);
            IsSaveInFileReportButtonEnabled = false;
            SelectedDate = DateTime.Now.Date;
            ComboBoxValueCollection = new ObservableCollection<string>
            {
                "1",
                "2",
                "3"
            };

            SelectedShift = ComboBoxValueCollection.First();
            logger.logger.Debug($"SettingsAndFilterPanelViewModel with DataContext {DataContextEnum}");
        }

        public DelegateCommand GenerateReportCommand { get; set; }

        public DelegateCommand SaveInFileCommand { get; set; }

        public bool IsSaveInFileVisible => DataContextEnum == DataContextEnum.BlowingMachineVIewModel;

        public ObservableCollection<string> ComboBoxValueCollection { get; set; }

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

        private DateTime selectedDate { get; set; }

        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                if (selectedDate == value)
                    return;
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
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

        private DataTable BlowingMachineData { get; set; }


        public DataContextEnum DataContextEnum { get; set; }

        public Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }

        public Action<BlowingMachineReportDto> BlowingMachineReportsModelItemsAction { get; set; }

        public Action<string> SaveBlowingMachineReportsInFileAction { get; set; }

        private void OnSaveInFile()
        {
            SaveBlowingMachineReportsInFileAction(SelectedShift);
        }

        private async Task OnGenereteDateReport()
        {
            if (DataContextEnum == DataContextEnum.BlowingMachineVIewModel)
                await GenereteBlowingMachineReport();
            if (DataContextEnum == DataContextEnum.FormViewModel)
                await GenereteDateFormReport();
            IsSaveInFileReportButtonEnabled = true;
        }

        private string GenerateBlowingMachineQuery()
        {
            var shiftInfo = shiftCalendar.GetShiftInfo(SelectedShift);
            if (SelectedShift != "3")
                return string.Format(
                    "SELECT * FROM public.spieniarka_probki_summary where data_koniec > '{0}' and data_koniec < '{1}'",
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                        shiftInfo.BeginningShift.Hours,
                        shiftInfo.BeginningShift.Minutes, shiftInfo.BeginningShift.Seconds),
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, shiftInfo.EndShift.Hours,
                        shiftInfo.EndShift.Minutes, shiftInfo.EndShift.Seconds));

            return string.Format(
                "SELECT * FROM public.spieniarka_probki_summary where data_koniec > '{0}' and data_koniec < '{1}'",
                new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, shiftInfo.BeginningShift.Hours,
                    shiftInfo.BeginningShift.Minutes, shiftInfo.BeginningShift.Seconds),
                new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.AddDays(1).Day,
                    shiftInfo.EndShift.Hours,
                    shiftInfo.EndShift.Minutes, shiftInfo.EndShift.Seconds));
        }

        private async Task GenereteBlowingMachineReport()
        {
            var data = dbConnection.GetFormDateReportItems(GenerateBlowingMachineQuery());
            var model = GenerateModelLogic.GenerateBlowingMachineReportModel(data);
            BlowingMachineReportsModelItemsAction(new BlowingMachineReportDto {Model = model});
        }

        private async Task GenereteDateFormReport()
        {
            try
            {
                var query = string.Format(
                    "SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'",
                    SelectedDate.AddHours(4), SelectedDate.AddDays(1).AddHours(10));
                var data = dbConnection.GetFormDateReportItems(query);
                var dateReportDbModelList = GenerateModelLogic.GeneratFormDateReportModel(data);
                var detailedReportDbModelList = GenerateModelLogic.GeneratFormDetailedReportModel(data);
                FormReportsModelItemsAction(new FormReportsDBModel
                {
                    DateReportDb = dateReportDbModelList,
                    DetailedReportDb = detailedReportDbModelList
                });
            }
            catch (Exception ex)
            {
                logger.logger.Error("Pusta kolekcja", ex);
                throw;
            }
        }
    }
}