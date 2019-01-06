using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using Castle.Core.Internal;
using FluentNHibernate.Conventions;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class FormViewModel: ViewModelBase, IFormViewModel
    {      
        public DelegateCommand GenereteReportCommand { get; set; }
        public DelegateCommand GenereteAllReportCommand { get; set; }

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
                if(detailedReportItems == value) return;
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

        private ObservableCollection<int> chamberItems;

        public ObservableCollection<int> ChamberItems
        {
            get { return chamberItems; }
            set
            {
                chamberItems = value;
                OnPropertyChanged("ChamberItems");
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
                if (selectedDatedReportRow == value) return;
                selectedDatedReportRow = value;
                try
                {
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
            GenereteReportCommand = new DelegateCommand(OnGenerateReportForChamber);
            GenereteAllReportCommand = new DelegateCommand(OnGenerateAllReportForChambers);
            IsChamberReportPanelEnabled = false;
            
        }


        public void OnCommandCellEnded()
        {
            UpdateDataBase(SelectedDatailedReportRow);
        }

        private void OnGetMaterialTypeData(MaterialTypeMenuModel obj)
        {
            int counter = obj.AssignedNumber;
            foreach (var item in DetailedReportItems)
            {
                item.AvgDensityOfPearls = obj.AvgDensityOfPearls;
                item.Comments = obj.Comments;
                item.Type = obj.SelectedMaterialType;
                item.AssignedNumber = counter;
                counter++;
                UpdateDataBase(item);
            }

            OnGenerateReportForChamber();
       }

        private void UpdateDataBase(FormDetailedReportDBModel item)
        {
            var query = String.Format(CultureInfo.InvariantCulture,
                "UPDATE public.forma_blok2  Set uwaga = '{0}',gatunek='{1}',getosc_perelek = {2}, nrnadany = {3} " +
                "where id_blok = {4}",item.Comments,item.Type,item.AvgDensityOfPearls,item.AssignedNumber,item.Id);
            logger.logger.DebugFormat("Updata data query: {0}", query);
            applicationRepository.UpdateData(query);
        }

        private void OnGenerateAllReportForChambers()
        {
            DetailedReportItems.Clear();
            foreach (var item in ChamberReportItems)
                DetailedReportItems.Add(item);
            
        }
      
        private void OnGenerateReportForChamber()
        {
            try
            {
                if (DetailedReportItems.IsNullOrEmpty())
                {
                    SetFormDetailedReport();
                }
                DetailedReportItems.Clear();
                var specificChamberItems = ChamberReportItems.Where(s => s.Chamber == SelectedChamberItem);
                foreach (var formDetailedReportDbModel in specificChamberItems)
                    DetailedReportItems.Add(formDetailedReportDbModel);
            }
            catch (Exception e)
            {
                logger.logger.ErrorFormat("No selected chamber in application: ", e);
                MessageBoxHelper.ShowMessageBox("Wybierz komore i sprobuj wygenerowac raport ponownie", MessageBoxIcon.Exclamation);
            }
           
        }

        private void OnGetFormReportsModelItems(FormReportsDBModel formReportsDbModel)
        {
            
            DateReportItems = new ObservableCollection<FormDateReportModel>(GenerateDateReport(formReportsDbModel.DateReportDb));
            ChamberReportItems = new List<FormDetailedReportDBModel>(from li in formReportsDbModel.DetailedReportDb
                                                                     where li.ProductionDate >= DateReportItems.First().TimeFrom && li.ProductionDate <= DateReportItems.Last().TimeTo
                                                                     select li);
            SetChambers();
            IsChamberReportPanelEnabled = true;
            UpdateChamberReport();
        }

        private void UpdateChamberReport()
        {
            if (SelectedChamberItem != 0)
                SetFormDetailedReport();
        }


        private void SetChambers()
        {
            var item = ChamberReportItems.Select(ch => ch.Chamber).Distinct();
            ChamberItems = new ObservableCollection<int>(item);
        }

        private void SetFormDetailedReport()
        {
            if (SelectedDatedReportRow != null)
            {
                SelectedChamberItem = SelectedDatedReportRow.Chamber;
            }
            
            DetailedReportItems = new ObservableCollection<FormDetailedReportDBModel>(from it in ChamberReportItems where it.Chamber == SelectedChamberItem select it);
            SelectedChamberItem = DetailedReportItems.FirstOrDefault().Chamber;
        }
       
        private List<FormDateReportModel> GenerateDateReport(List<FormDateReportDBModel> obj)
        {
            int counter = 0;
            int chamber=0;
            bool isLastChamber = false;
            var list = new List<FormDateReportModel>();
            var shiftCalendarManager = new ShiftCalendarManager();
            for (int i = 0; i < obj.Count; i++)
            {
                if ((chamber == obj[i].Chamber || counter == 0) && obj.Count-1 != i)// ostatni warunek po to zeby wyswietlac nie zakonczone cykle
                {
                    chamber = obj[i].Chamber;
                    counter++;
                    continue;
                }
               
                    var dateFrom = obj[i - counter].ProductionDate;
                    var toDate = obj[i - 1].ProductionDate;
                    var shift = shiftCalendarManager.GetShiftAsString(shiftCalendarManager.GetShift(
                        new TimeSpan(dateFrom.Hour, dateFrom.Minute, dateFrom.Second),
                        new TimeSpan(toDate.Hour, toDate.Minute, toDate.Second)));
                    list.Add(new FormDateReportModel()
                    {
                        Shift = shift,
                        TimeFrom = dateFrom,
                        TimeTo = toDate,
                        Chamber = obj[i-1].Chamber,
                        Silos = obj[i-1].Silos,
                        NumberOfBlocks = counter,
                        Operator = obj[i-counter].Operator
                    });
                    ShiftProperty = shift;
                    counter = 0;
                    counter++;
                    chamber = obj[i].Chamber;
                
            }      
            return shiftCalendarManager.RemoveNastedRows(list); 
        }

        public void Dispose()
        {
            this.SettingsAndFilterPanelViewModel.FormReportsModelItemsAction -= OnGetFormReportsModelItems;
        }
    }
}
