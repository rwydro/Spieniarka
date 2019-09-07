using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{

    public class SettingsAndFilterPanelViewModel<T> : ViewModelBase, ISettingsAndFilterPanelViewModel<T> where T : ReportModelBase, new()
    {
        private readonly IApplicationRepository dbConnection;
        private readonly IMyLogger logger;
        private MyTimer timer = new MyTimer();

        private DateTime currentDateTime;

        public DateTime CurrentDateTime
        {
            get { return currentDateTime; }
            set
            {
                if (value.Equals(currentDateTime))
                {
                    return;
                }
                currentDateTime = value;
                OnPropertyChanged("CurrentDateTime");

                GenerateReportCommand.RaiseCanExecuteChanged();
            }
        }

        public event EventHandler<EventBaseArgs<T>> GeneratedModelItemsAction;
        protected virtual void OnGeneratedModelItemsAction(EventBaseArgs<T> e)
        {
            EventHandler<EventBaseArgs<T>> handler = GeneratedModelItemsAction;
            if (handler != null)
            {
                handler(this, e );
            }
        }

        public event EventHandler IsReportGenerate;
        protected virtual void OnReportGenerated()
        {
            EventHandler handler = IsReportGenerate;
            if (handler != null)
            {
                handler(this,EventArgs.Empty);
            }
        }

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
            GenerateReportCommand = new DelegateCommand(OnGenereteDateReport);
            IsSaveInFileReportButtonEnabled = false;
            SelectedDate = DateTime.Now.Date;
            CurrentDateTime = DateTime.Now;
            timer.Elapsed += OnTimerElapsed;
            logger.logger.Debug($"SettingsAndFilterPanelViewModel with DataContext {DataContextEnum}");
        }

        private void OnGenereteDateReport()
        {
            OnReportGenerated();
            GenereteDateReport();
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            logger.logger.DebugFormat("On timer Elapsed");
            GenereteDateReport();
        }

        public void SetTimer(TimerActionEnum action)
        {
            this.timer.SetTimerAction(action);
        }
     
        private void GenereteDateReport()
        {
            if (DataContextEnum == DataContextEnum.BlowingMachineVIewModel)
                GenereteBlowingMachineReport();
            if (DataContextEnum == DataContextEnum.FormViewModel)
            {
                GenereteDateFormReport();
                //this.timer.SetTimerAction(TimerActionEnum.Reset);
            }
            IsSaveInFileReportButtonEnabled = true;
        }


        private string GenerateBlowingMachineQuery(string dbTableName, string dbColName)
        {
                return string.Format(
                    "SELECT * FROM public.{0} where {1} > '{2}' and {3} < '{4}' order by {1}",
                    dbTableName, dbColName,
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,7,0,0),
                    dbColName,
                    new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.AddDays(1).Day,7,0,0));
        }

        private void GenereteBlowingMachineReport()
        {
            var data = dbConnection.GetDataFromDB(typeof(T) == typeof(BlowingMachineReportModel) ? GenerateBlowingMachineQuery("spieniarka_probki_summary", "data_koniec") :
                    GenerateBlowingMachineQuery("spieniarka_ciagla_probki","data_czas"));
          
            var model = GenerateModelLogic<T>.GenerateReportModel(data, typeof(T) == typeof(BlowingMachineReportModel) ? ModelDictionaries.BlowingMachineDbColumnNameToModelPropertyNameDictionary :
                ModelDictionaries.ContinuousBlowingMachineDbColumnNameToModelPropertyNameDictionary);
            OnGeneratedModelItemsAction(new EventBaseArgs<T>(){ReportModel = new ReportModel<T>()
            {
                Model = model as List<T>,
                SelectedDate = SelectedDate
            }
         });
            CurrentDateTime = DateTime.Now;
        }

        private void GenereteDateFormReport()
        {
            try
            {
                var query = string.Format(
                    "SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'",
                    SelectedDate.AddHours(4), SelectedDate.AddDays(1).AddHours(10));
                var data = dbConnection.GetDataFromDB(query);
                var dateReportDbModelList = GenerateModelLogic<FormDateReportDBModel>.GenerateReportModel(data, ModelDictionaries.FormDetailedReportDbModelPropertyNameDictionary);

                if (dateReportDbModelList == null)
                {
                    logger.logger.ErrorFormat("Pusta kolekcja: dateReportDbModelList{0} detailedReportDbModelList{1}", dateReportDbModelList.Count);
                }

                OnGeneratedModelItemsAction(new EventBaseArgs<T>()
                {
                    ReportModel = new ReportModel<T>()
                    {
                        Model = dateReportDbModelList as List<T>
                    }
                });

                CurrentDateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                logger.logger.ErrorFormat("Failed during generating FormReport:  {0}", ex);
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