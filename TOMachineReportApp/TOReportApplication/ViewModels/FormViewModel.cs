using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Threading;
using Castle.Core.Internal;
using FluentNHibernate.Conventions;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;
using Application = System.Windows.Application;

namespace TOReportApplication.ViewModels
{
    public class FormViewModel : ViewModelBase, IFormViewModel
    {
        public DelegateCommand SaveReportInFileCommand { get; set; }

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

        private ObservableCollection<FormDetailedReportDBModel> detailedReportItems;

        public ObservableCollection<FormDetailedReportDBModel> DetailedReportItems
        {
            get { return detailedReportItems; }
            set
            {
                if (detailedReportItems == value) return;
                detailedReportItems = value;
                OnPropertyChanged("DetailedReportItems");
            }
        }

        private List<FormDetailedReportDBModel> chamberReportItems;

        public List<FormDetailedReportDBModel> ChamberReportItems
        {
            get { return chamberReportItems; }
            set
            {
                chamberReportItems = value;
                OnPropertyChanged("ChamberReportItems");
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

        private FormDetailedReportDBModel selectedDatailedReportRow { get; set; }

        public FormDetailedReportDBModel SelectedDatailedReportRow
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
                if (selectedDatedReportRow == value || value == null) return;
                selectedDatedReportRow = value;
                try
                {
                    ActualDatedReportRow = value;
                    SetFormDetailedReport();
                }
                catch (Exception e)
                {
                    logger.logger.ErrorFormat("No selected chamber in application: ", e);
                    MessageBoxHelper.ShowMessageBox("Wybierz komore i sprobuj wygenerowac raport ponownie", MessageBoxIcon.Exclamation);
                }
                OnPropertyChanged("SelectedDatedReportRow");
            }
        }

        private FormDateReportModel actualdDatedReportRow { get; set; }

        public FormDateReportModel ActualDatedReportRow
        {
            get { return actualdDatedReportRow; }
            set
            {
                if (actualdDatedReportRow == value) return;
                actualdDatedReportRow = value;
                OnPropertyChanged("ActualDatedReportRow");
            }
        }

        private readonly ISettingsAndFilterPanelViewModel settingsAndFilterPanelViewModel;
        public ISettingsAndFilterPanelViewModel SettingsAndFilterPanelViewModel
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
            this.settingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel(container, repository, logger);
            this.ShiftReportDataViewModel = shiftReportDataViewModel;
            this.SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.FormViewModel;
            this.SettingsAndFilterPanelViewModel.FormReportsModelItemsAction += OnGetFormReportsModelItems;
            this.SettingsAndFilterPanelViewModel.SetTimer();
            this.ShiftReportDataViewModel.OnSendMaterialTypeInfo += OnGetMaterialTypeData;
            SaveReportInFileCommand = new DelegateCommand(OnSaveReportInFile);
            IsChamberReportPanelEnabled = false;
        }

        private void OnSaveReportInFile()
        {
            
        }

        private void SetAvailableShifts()
        {
            ShiftItems = new ObservableCollection<string>();
            foreach (var item in DateReportItems)
            {
                if(!string.IsNullOrEmpty(ShiftItems.FirstOrDefault(s => s.Contains(item.Shift))))// tu poprawic
                    ShiftItems.Add(item.Shift);
            }                        
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

        private void UpdateDataBase(FormDetailedReportDBModel item)
        {
            var query = String.Format(CultureInfo.InvariantCulture,
                "UPDATE public.forma_blok2  Set uwaga = '{0}',gatunek='{1}',getosc_perelek = {2}, nrnadany = {3}, silos = {4}, komora = {5}, data_organiki = '{6}', pz = '{7}' " +
                "where id_blok = {8}", item.Comments, item.Type, item.AvgDensityOfPearls, item.AssignedNumber, item.Silos, item.Chamber, item.OrganicDate.ToString("yyyy-MM-dd"), item.PzNumber, item.Id);
            logger.logger.DebugFormat("Updata data query: {0}", query);
            applicationRepository.UpdateData(query);
        }

        private void OnGetFormReportsModelItems(FormReportsDBModel formReportsDbModel)
        {
            Application.Current.Dispatcher.InvokeAsync(() => GetFormReportsModelItems(formReportsDbModel));
        }

        private void GetFormReportsModelItems(FormReportsDBModel formReportsDbModel)
        {
            DateReportItems = DateReportItems = new ObservableCollection<FormDateReportModel>();
            GenerateDateReport(formReportsDbModel.DateReportDb).ForEach(DateReportItems.Add);

            ChamberReportItems = new List<FormDetailedReportDBModel>(from li in formReportsDbModel.DetailedReportDb
                    where li.ProductionDate >= DateReportItems.First().TimeFrom && li.ProductionDate <= DateReportItems.Last().TimeTo
                    select li);
            SetAvailableShifts();
            IsChamberReportPanelEnabled = true;
            SetFormDetailedReport();
        }

        private void SetFormDetailedReport()
        {

            if (SelectedDatedReportRow != null && SelectedChamberItem !=  SelectedDatedReportRow.Chamber)
            {
                SelectedChamberItem = SelectedDatedReportRow.Chamber;
            }

            logger.logger.ErrorFormat("Method SetFormDetailedReport");
            if (ActualDatedReportRow == null ) return; // to jest jak przyjdzie timer i nie wybrano zadnej komory zeby sie nie wywalilo
            DetailedReportItems = new ObservableCollection<FormDetailedReportDBModel>(
                from it in ChamberReportItems
                where it.Chamber == SelectedDatedReportRow.Chamber && it.Silos == SelectedDatedReportRow.Silos &&
                      SelectedDatedReportRow.TimeFrom<= it.ProductionDate && it.ProductionDate <= SelectedDatedReportRow.TimeTo
                select it);
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
                        Operator = obj[i - counter].Operator
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
                        Operator = obj[i - counter].Operator
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
            this.SettingsAndFilterPanelViewModel.FormReportsModelItemsAction -= OnGetFormReportsModelItems;
        }
    }
}
