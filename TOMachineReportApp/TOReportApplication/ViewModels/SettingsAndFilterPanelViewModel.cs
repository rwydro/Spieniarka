using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private MyTimer timer = new MyTimer();

       

        public Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }

        public Action<BlowingMachineReportDto> BlowingMachineReportsModelItemsAction { get; set; }

        private DataContextEnum dataContextEnum { get; set; }

        public DataContextEnum DataContextEnum
        {
            get => dataContextEnum;
            set
            {
                if(dataContextEnum == value) return;
                if(value == DataContextEnum.BlowingMachineVIewModel)
                    timer.Dispose();
                dataContextEnum = value;
                OnPropertyChanged("DataContextEnum");
            }
        }

        public DelegateCommand GenerateReportCommand { get; set; }

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

        public SettingsAndFilterPanelViewModel(IUnityContainer unityContainer, IApplicationRepository dbConnection,
            IMyLogger logger)
            : base(unityContainer)
        {
            this.dbConnection = dbConnection;
            this.logger = logger;
            GenerateReportCommand = new DelegateCommand(() => OnGenereteDateReportAsync().InAsyncSafe());
            IsSaveInFileReportButtonEnabled = false;
            SelectedDate = DateTime.Now.Date;
            timer.Elapsed += OnTimerElapsed;
            logger.logger.Debug($"SettingsAndFilterPanelViewModel with DataContext {DataContextEnum}");
        }

        private async void OnTimerElapsed(object sender, EventArgs e)
        {
            await OnGenereteDateReportAsync();
        }

        public void SetTimer()
        {
            this.timer.SetTimer();
        }
     
        private async Task OnGenereteDateReportAsync()
        {
            if (DataContextEnum == DataContextEnum.BlowingMachineVIewModel)
                await GenereteBlowingMachineReport();
            if (DataContextEnum == DataContextEnum.FormViewModel)
            {
                await GenereteDateFormReport();
                this.timer.ResetTimer();
            }
            IsSaveInFileReportButtonEnabled = true;
        }


        private string GenerateBlowingMachineQuery()
        {
                return string.Format(
                    "SELECT * FROM public.spieniarka_probki_summary where data_koniec > '{0}' and data_koniec < '{1}'",
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,7,0,0),
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.AddDays(1).Day,7,0,0));
        }   

        private async Task GenereteBlowingMachineReport()
        {
            var data = dbConnection.GetFormDateReportItems(GenerateBlowingMachineQuery());
            var model = GenerateModelLogic<BlowingMachineReportModel>.GenerateBlowingMachineReportModel(data,ModelDictionaries.BlowingMachineDbColumnNameToModelPropertyNameDictionary);
            BlowingMachineReportsModelItemsAction(new BlowingMachineReportDto {Model = model,SelectedDate = SelectedDate});
        }

        private async Task GenereteDateFormReport()
        {
            try
            {
                var query = string.Format(
                    "SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'",
                    SelectedDate.AddHours(4), SelectedDate.AddDays(1).AddHours(10));
                var data = dbConnection.GetFormDateReportItems(query);
                //var dateReportDbModelList = GenerateModelLogic<FormDateReportDBModel>.GenerateBlowingMachineReportModel(data, ModelDictionaries.FormDetailedReportDbModelPropertyNameDictionary);
                //var detailedReportDbModelList = GenerateModelLogic<FormDetailedReportDBModel>.GenerateBlowingMachineReportModel(data, ModelDictionaries.FormDetailedReportDbModelPropertyNameDictionary);
                var dateReportDbModelList = GenerateModelLogic<FormDateReportDBModel>.GeneratFormDateReportModel(data);
                var detailedReportDbModelList = GenerateModelLogic<FormDetailedReportDBModel>.GeneratFormDetailedReportModel(data);
                if (detailedReportDbModelList == null || detailedReportDbModelList == null)
                {
                    logger.logger.ErrorFormat("Pusta kolekcja: dateReportDbModelList{0} detailedReportDbModelList{1}", dateReportDbModelList.Count, detailedReportDbModelList.Count);
                }
                FormReportsModelItemsAction(new FormReportsDBModel
                {
                    DateReportDb = dateReportDbModelList,
                    DetailedReportDb = detailedReportDbModelList
                });
            }
            catch (Exception ex)
            {
                logger.logger.ErrorFormat("Failed during generating FormReport:  {0}", ex);
                throw;
            }
        }
    }
    public class DateExpiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime orderDate = (DateTime)value;

            return new ValidationResult(orderDate < DateTime.Now, "Please, enter date before Now()");
        }
    }
}