using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using Castle.Core.Internal;
using FluentNHibernate.Conventions;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;
using Application = System.Windows.Application;

namespace TOReportApplication.ViewModels
{
    public class FormViewModel : ViewModelBase, IFormViewModel
    {
        public DelegateCommand SaveReportInFileCommand { get; set; }
        public DelegateCommand<string> GenerateDetailsReportForChamberCommand { get; set; }

        private string shiftProperty { get; set; }

        public string ShiftProperty
        {
            get { return shiftProperty; }
            set
            {
                shiftProperty = value;
                OnPropertyChanged("ShiftProperty");
            }
        }

        private int selectedChamberItem;
        public int SelectedChamberItem
        {
            get { return selectedChamberItem; }
            set
            {
                selectedChamberItem = value;
                OnPropertyChanged("SelectedChamberItem");
            }
        }

        private bool isChamberReportPanelEnabled { get; set; }

        public bool IsChamberReportPanelEnabled
        {
            get { return isChamberReportPanelEnabled; }
            set
            {
                isChamberReportPanelEnabled = value;
                OnPropertyChanged("IsChamberReportPanelEnabled");
            }
        }

        private bool isSaveInFilePanelEnabled { get; set; }

        public bool IsSaveInFilePanelEnabled
        {
            get { return isSaveInFilePanelEnabled; }
            set
            {
                isSaveInFilePanelEnabled = value;
                OnPropertyChanged("IsSaveInFilePanelEnabled");
            }
        }

        private ObservableCollection<FormDatailedReportModel> detailedReportItems;

        public ObservableCollection<FormDatailedReportModel> DetailedReportItems //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return detailedReportItems; }
            set
            {
                if (detailedReportItems == value) return;
                detailedReportItems = value;
                OnPropertyChanged("DetailedReportItems");
            }
        }

        private ObservableCollection<FormDateReportDBModel> detailedFullVersionReportItems;

        public ObservableCollection<FormDateReportDBModel>
            DetailedFullVersionReportItems //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return detailedFullVersionReportItems; }
            set
            {
                if (detailedFullVersionReportItems == value) return;
                detailedFullVersionReportItems = value;
                OnPropertyChanged("DetailedFullVersionReportItems");
            }
        }

        private ObservableCollection<FormDateReportModel> dateReportItems;

        public ObservableCollection<FormDateReportModel> DateReportItems
        {
            get { return dateReportItems; }
            set
            {
                dateReportItems = value;
                OnPropertyChanged("DateReportItems");
            }
        }

        private string selectedShiftItem;
        public string SelectedShiftItem
        {
            get { return selectedShiftItem; }
            set
            {
                if(selectedShiftItem == value) return;
                selectedShiftItem = value;
                OnPropertyChanged("SelectedShiftItem");
            }
        }

        private ObservableCollection<string> shiftItems;
        public ObservableCollection<string> ShiftItems
        {
            get { return shiftItems; }
            set
            {
                if (shiftItems == value) return;
                shiftItems = value;
                OnPropertyChanged("ShiftItems");
            }
        }

        private FormDateReportDBModel selectedDatailedReportRow { get; set; }

        public FormDateReportDBModel SelectedDatailedReportRow
        {
            get { return selectedDatailedReportRow; }
            set
            {
                if (selectedDatailedReportRow == value) return;
                selectedDatailedReportRow = value;
                OnPropertyChanged("SelectedDatailedReportRow");
            }
        }

        private FormDateReportModel selectedDatedReportRow { get; set; }

        public FormDateReportModel SelectedDatedReportRow
        {
            get { return selectedDatedReportRow; }
            set
            {
                if (selectedDatedReportRow == value) return;      
                try
                {
                    selectedDatedReportRow = value;
                    ActualDatedReportRow = selectedDatedReportRow;
                    if (selectedDatedReportRow != null)
                        OnGenerateDetailsReportForChamber("false");
                    //SetFormDetailedReport();
                }
                catch (Exception e)
                {
                    logger.logger.ErrorFormat("No selected chamber in application: ", e);
                    MessageBoxHelper.ShowMessageBox("Wybierz komore i sprobuj wygenerowac raport ponownie", MessageBoxIcon.Exclamation);
                }

               
                OnPropertyChanged("SelectedDatedReportRow");
            }
        }

        private FormDateReportModel actualDatedReportRow { get; set; }

        public FormDateReportModel ActualDatedReportRow // to nie bedzie bindowane, nie pozwalamy na nulla. Null do widoku jest po to by odczepic na widoku zapamietnaie wybranego wiersza
        {
            get { return actualDatedReportRow; }
            set
            {
                if (actualDatedReportRow == value || value == null) return;
                actualDatedReportRow = value;
                OnPropertyChanged("ActualDatedReportRow");
            }
        }


        private FormDetailedReportTypeEnum detailedReportType { get; set; }
        public FormDetailedReportTypeEnum DetailedReportType
        {
            get { return detailedReportType; }
            set
            {
                if (detailedReportType == value) return;
                detailedReportType = value;
                OnPropertyChanged("DetailedReportType");
            }
        }

        private readonly ISettingsAndFilterPanelViewModel<FormDateReportDBModel> settingsAndFilterPanelViewModel;
        public ISettingsAndFilterPanelViewModel<FormDateReportDBModel> SettingsAndFilterPanelViewModel
        {
            get { return this.settingsAndFilterPanelViewModel; }
        }

        public IFormSetShiftReportDataViewModel ShiftReportDataViewModel { get; }
        private IMyLogger logger;
        private IApplicationRepository applicationRepository;

        public FormViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger, IFormSetShiftReportDataViewModel shiftReportDataViewModel)
            : base(container)
        {
            this.logger = logger;
            this.applicationRepository = repository;
            this.settingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel<FormDateReportDBModel>(container, repository, logger);
            this.ShiftReportDataViewModel = shiftReportDataViewModel;
            this.SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.FormViewModel;
            this.SettingsAndFilterPanelViewModel.GeneratedModelItemsAction += OnGetFormReportsModelItems;
            this.SettingsAndFilterPanelViewModel.IsReportGenerate += OnGenerateReport;
            this.SettingsAndFilterPanelViewModel.SetTimer(TimerActionEnum.Set);
            this.ShiftReportDataViewModel.OnSendMaterialTypeInfo += OnGetMaterialTypeData;
            SaveReportInFileCommand = new DelegateCommand(OnSaveReportInFile);
            GenerateDetailsReportForChamberCommand = new DelegateCommand<string>(OnGenerateDetailsReportForChamber);
            IsChamberReportPanelEnabled = false;
            IsSaveInFilePanelEnabled = false;
            DetailedReportType = FormDetailedReportTypeEnum.Any;
        }

        private void OnGenerateReport(object sender, EventArgs e)
        {
            DetailedReportType = FormDetailedReportTypeEnum.Any;
            IsChamberReportPanelEnabled = false;
        }

        private void OnGenerateDetailsReportForChamber(string isFullReport)
        {
            if (isFullReport == "true")
            {
                DetailedReportType = FormDetailedReportTypeEnum.FullVersionDetailedReport;
                IsChamberReportPanelEnabled = false;
            }
            else
            {
                DetailedReportType = FormDetailedReportTypeEnum.ShortVersionDetailedReport;
                IsChamberReportPanelEnabled = true;
            }

            SetFormDetailedReport();
        }

        private FormReportFileModel GetAdditionalRecord()
        {
            FormDateReportModel item = null;
            switch (SelectedShiftItem)
            {
                case "1":
                    item = DateReportItems.FirstOrDefault(s => s.Shift == "3\\1");
                    if (item != null) item.Shift = "1";
                    break;                   
                case "2":
                    item = DateReportItems.FirstOrDefault(s => s.Shift == "1\\2");
                    if (item != null) item.Shift = "2";
                    break;
                case "3":
                    item = DateReportItems.FirstOrDefault(s => s.Shift == "2\\3");
                    if (item != null) item.Shift = "3";
                    break;
            }
            return item != null
                ? new FormReportFileModel()
                {
                    Silos = item.Silos,
                    AvgDensityOfPearls = item.DetailedReportForChamber.First().AvgDensityOfPearls,
                    Chamber = item.Chamber,
                    NumberOfBlocks = item.NumberOfBlocks,
                    Operator = item.Operator,
                    PzNumber = item.DetailedReportForChamber.First().PzNumber,
                    Shift = item.Shift,
                    SumBlockWeight = item.DetailedReportForChamber.Sum(s => s.Weight),
                    TimeFrom = item.TimeFrom,
                    TimeTo = item.TimeTo,
                }
                : new FormReportFileModel();
        }

        private void OnSaveReportInFile()
        {
            var reportObject = new List<FormReportFileModel>();
             GetAdditionalRecord();
            var item = GetAdditionalRecord();
            if(item.Shift != null)
                reportObject.Add(item);

            foreach (var data in DateReportItems.Where(s=>s.Shift == SelectedShiftItem))
            {
                reportObject.Add(new FormReportFileModel()
                {
                    Silos = data.Silos,
                    AvgDensityOfPearls = data.DetailedReportForChamber.First().AvgDensityOfPearls,
                    Chamber = data.Chamber,
                    NumberOfBlocks = data.NumberOfBlocks,
                    Operator = data.Operator,
                    PzNumber = data.DetailedReportForChamber.First().PzNumber,
                    Shift = data.Shift,
                    SumBlockWeight = data.DetailedReportForChamber.Sum(s => s.Weight),
                    TimeFrom = data.TimeFrom,
                    TimeTo = data.TimeTo,
                });
            }

            var pathToReport = Path.Combine(ConfigurationManager.AppSettings["PathToFormReport"], "Raport_Forma_" + reportObject.First().TimeFrom.Date.ToString("yyyy-MM-dd") + ".xml");
            SaveInFileLogic.OnSaveReportInFile<List<FormReportFileModel>>(reportObject, pathToReport, logger);
            Process.Start(pathToReport);
        }

        private void SetAvailableShifts()
        {
            var items =  DateReportItems.ToList().FindAll(s => s.Shift == "1" || s.Shift == "2" || s.Shift == "3").Select(s => s.Shift).Distinct();
            ShiftItems = new ObservableCollection<string>(items);
        }

        public void OnCommandCellEnded()
        {
            UpdateDataBase(SelectedDatailedReportRow);
        }

        private void OnGetMaterialTypeData(MaterialTypeMenuModel obj)
        {
            if (DetailedReportItems.IsNullOrEmpty())
            {
                MessageBoxHelper.ShowMessageBox("Wybierz komore i sprobuj wygenerowac raport ponownie", MessageBoxIcon.Exclamation);
                return;
            }
            int counter = obj.AssignedNumber;
            foreach (var item in DetailedReportItems)
            {
                item.AvgDensityOfPearls = obj.AvgDensityOfPearls;
                item.Comments = obj.Comments;
                item.Type = obj.SelectedMaterialType;
                item.AssignedNumber = counter;
                item.OrganicDate = obj.OrganicDate;
                item.PzNumber = obj.PzNumber;
                counter++;
                UpdateDataBase(item);
            }
            SetFormDetailedReport();
        }   

        private void UpdateDataBase(FormDatailedReportModel item)
        {
            var query = String.Format(CultureInfo.InvariantCulture,
                "UPDATE public.forma_blok2  Set uwaga = '{0}',gatunek='{1}',getosc_perelek = {2}, nrnadany = {3}, silos = {4}, komora = {5}, data_organiki = '{6}', pz = '{7}' " +
                "where id_blok = {8}", item.Comments, item.Type, item.AvgDensityOfPearls, item.AssignedNumber, item.Silos, item.Chamber, item.OrganicDate.ToString("yyyy-MM-dd"), item.PzNumber, item.Id);
            logger.logger.DebugFormat("Updata data query: {0}", query);
            applicationRepository.UpdateData(query);
        }

        private void OnGetFormReportsModelItems(object sender, EventBaseArgs<FormDateReportDBModel> e)
        {
              Application.Current.Dispatcher.InvokeAsync(() => GetFormReportsModelItems(e.ReportModel));
        }

        private void GetFormReportsModelItems(ReportModel<FormDateReportDBModel> formReportsDbModel)
        {
            DateReportItems = new ObservableCollection<FormDateReportModel>();
            GenerateDateReport(formReportsDbModel.Model).ForEach(DateReportItems.Add);
            SetAvailableShifts();
          
            if (DateReportItems.Count != 0)
            {
                IsChamberReportPanelEnabled = true;
                IsSaveInFilePanelEnabled = true;
            }

            SetFormDetailedReport();
        } 

        private void SetFormDetailedReport()
        {

            if (ActualDatedReportRow != null && SelectedChamberItem != ActualDatedReportRow.Chamber)
            {
                SelectedChamberItem = ActualDatedReportRow.Chamber;
            }

            logger.logger.ErrorFormat("Method SetFormDetailedReport");
            if (ActualDatedReportRow == null) return; // 2 warunek jak przyjdzie timer i nie wybrano zadnej komory zeby sie nie wywalilo

            switch (DetailedReportType)
            {
                case FormDetailedReportTypeEnum.ShortVersionDetailedReport:
                    DetailedReportItems = new ObservableCollection<FormDatailedReportModel>((from li in DateReportItems
                        where li.Chamber == ActualDatedReportRow.Chamber &&
                              li.NumberOfBlocks == ActualDatedReportRow.NumberOfBlocks &&
                              li.Silos == ActualDatedReportRow.Silos &&
                              li.Operator == ActualDatedReportRow.Operator
                        select li).First().DetailedReportForChamber);

                    this.SettingsAndFilterPanelViewModel.SetTimer(TimerActionEnum.Reset);
                    break;
                case FormDetailedReportTypeEnum.FullVersionDetailedReport:
                    DetailedFullVersionReportItems = new ObservableCollection<FormDateReportDBModel>(DateReportItems.First(s =>
                        s.Chamber == ActualDatedReportRow.Chamber &&
                        s.NumberOfBlocks == ActualDatedReportRow.NumberOfBlocks &&
                        s.Silos == ActualDatedReportRow.Silos &&
                        s.Operator == ActualDatedReportRow.Operator).DetailedReportForChamber);
                    SelectedDatedReportRow = null;
                    this.SettingsAndFilterPanelViewModel.SetTimer(TimerActionEnum.Stop);
                    break;
                case FormDetailedReportTypeEnum.Any:
                    DetailedReportItems = new ObservableCollection<FormDatailedReportModel>();
                    DetailedFullVersionReportItems = new ObservableCollection<FormDateReportDBModel>();
                    SelectedDatedReportRow = null;
                    break;
            }

            logger.logger.ErrorFormat("End method SetFormDetailedReport");
        }

        private List<FormDateReportModel> GenerateDateReport(List<FormDateReportDBModel> obj)
        {   
            int counter = 0;
            int chamber = 0;
            bool isLastChamber = false;
            var list = new List<FormDateReportModel>();
            var shiftCalendarManager = new ShiftCalendarManager();

            for (int i = 0; i < obj.Count; i++)
            {
                if ((i + 1) == obj.Count)
                {
                    var dateFrom = obj[i - counter].ProductionDate;
                    var toDate = obj[i].ProductionDate;
                    var shift = shiftCalendarManager.GetShiftAsString(shiftCalendarManager.GetShift(
                        new TimeSpan(dateFrom.Hour, dateFrom.Minute, dateFrom.Second),
                        new TimeSpan(toDate.Hour, toDate.Minute, toDate.Second)));
                    list.Add(new FormDateReportModel()
                    {
                        Shift = shift,
                        TimeFrom = obj[i - counter].ProductionDate,
                        TimeTo = obj[i].ProductionDate,
                        Chamber = obj[i].Chamber,
                        Silos = obj[i].Silos,
                        NumberOfBlocks = counter + 1,
                        Operator = obj[i - counter].Operator,
                        DetailedReportForChamber =  (from it in obj where it.Chamber == obj[i].Chamber && it.Silos == obj[i].Silos &&
                                obj[i - counter].ProductionDate <= it.ProductionDate &&
                                it.ProductionDate <= obj[i].ProductionDate
                                select it).ToList()
                    });
                    ShiftProperty = shift;
                    counter = 0;
                    continue;
                }
                if (chamber != obj[i+1].Chamber && counter != 0 )// ostatni warunek po to zeby wyswietlac nie zakonczone cykle
                {
                    var dateFrom = obj[i - counter].ProductionDate;
                    var toDate = obj[i].ProductionDate;
                    var shift = shiftCalendarManager.GetShiftAsString(shiftCalendarManager.GetShift(
                        new TimeSpan(dateFrom.Hour, dateFrom.Minute, dateFrom.Second),
                        new TimeSpan(toDate.Hour, toDate.Minute, toDate.Second)));
                    list.Add(new FormDateReportModel()
                    {
                        Shift = shift,
                        TimeFrom = obj[i - counter].ProductionDate,
                        TimeTo = obj[i].ProductionDate,
                        Chamber = obj[i].Chamber,
                        Silos = obj[i].Silos,
                        NumberOfBlocks = counter + 1,
                        Operator = obj[i - counter].Operator,
                        DetailedReportForChamber = new List<FormDateReportDBModel>(from it in obj
                            where it.Chamber == obj[i].Chamber && it.Silos == obj[i].Silos &&
                                  obj[i - counter].ProductionDate <= it.ProductionDate &&
                                  it.ProductionDate <= obj[i].ProductionDate
                            select it).ToList()
                    });
                    ShiftProperty = shift;
                    counter = 0;
                    continue;
                }
    
                chamber = obj[i].Chamber;
                counter++;              
            }

            return shiftCalendarManager.RemoveNastedRows(list);
        }

        public void Dispose()
        {
            this.SettingsAndFilterPanelViewModel.GeneratedModelItemsAction -= OnGetFormReportsModelItems;
            IsSaveInFilePanelEnabled = false;
            DetailedReportType = FormDetailedReportTypeEnum.Any;
        }
    }
}
