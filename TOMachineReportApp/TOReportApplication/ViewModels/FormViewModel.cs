using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly ISettingsAndFilterPanelViewModel settingsAndFilterPanelViewModel;
        public ISettingsAndFilterPanelViewModel SettingsAndFilterPanelViewModel
        {
            get { return this.settingsAndFilterPanelViewModel; }
        }

        public FormViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger) : base(container)
        {
            this.settingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel(container, repository);
            this.SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.FormViewModel;
            this.SettingsAndFilterPanelViewModel.FormReportsModelItemsAction += OnGetFormReportsModelItems;

            GenereteReportCommand = new DelegateCommand(OnGenerateReportForChamber);
            GenereteAllReportCommand = new DelegateCommand(OnGenerateAllReportForChambers);
            IsChamberReportPanelEnabled = false;
        }

        private void OnGenerateAllReportForChambers()
        {
            DetailedReportItems.Clear();
            foreach (var item in ChamberReportItems)
                DetailedReportItems.Add(item);
            
        }

        private void OnGenerateReportForChamber()
        {
            DetailedReportItems.Clear();
            var specificChamberItems = ChamberReportItems.Where(s => s.Chamber == SelectedChamberItem);
            foreach (var formDetailedReportDbModel in specificChamberItems)
                DetailedReportItems.Add(formDetailedReportDbModel);
        }

        private void OnGetFormReportsModelItems(FormReportsDBModel formReportsDbModel)
        {    
            DateReportItems = new ObservableCollection<FormDateReportModel>(GenerateDateReport(formReportsDbModel.DateReportDb));
            ChamberReportItems = new List<FormDetailedReportDBModel>(from li in formReportsDbModel.DetailedReportDb where
                                                                     li.ProductionDate >= DateReportItems.First().TimeFrom &&
                                                                     li.ProductionDate <= DateReportItems.Last().TimeTo
                                                                     select li);
            DetailedReportItems = new ObservableCollection<FormDetailedReportDBModel>(from li in formReportsDbModel.DetailedReportDb where
                                                                                      li.ProductionDate>= DateReportItems.First().TimeFrom &&
                                                                                      li.ProductionDate <= DateReportItems.Last().TimeTo select li);
            SetChambers();
            IsChamberReportPanelEnabled = true;
        }

        private void SetChambers()
        {
            var item = ChamberReportItems.Select(ch => ch.Chamber).Distinct();
            ChamberItems = new ObservableCollection<int>(item);
        }

        private List<FormDateReportModel> GenerateDateReport(List<FormDateReportDBModel> obj)
        {
            int counter = 0;
            int chamber=0;

            var list = new List<FormDateReportModel>();
            var shiftCalendarManager = new ShiftCalendarManager();
            for (int i = 0; i < obj.Count; i++)
            {
                if (chamber == obj[i].Chamber || counter == 0)
                {
                    chamber = obj[i].Chamber;
                    counter++;
                }
                else
                {
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
            }      
            return shiftCalendarManager.RemoveNastedRows(list); 
        }

        private ObservableCollection<FormDetailedReportDBModel> detailedReportItems;

        public ObservableCollection<FormDetailedReportDBModel> DetailedReportItems
        {
            get { return detailedReportItems; }
            set
            {
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

        public void Dispose()
        {
            this.SettingsAndFilterPanelViewModel.FormReportsModelItemsAction -= OnGetFormReportsModelItems;
        }
    }
}
