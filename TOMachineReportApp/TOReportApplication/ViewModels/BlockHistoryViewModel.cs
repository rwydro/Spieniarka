using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class BlockHistoryViewModel: ViewModelBase, IBlockHistoryViewModel
    {
        public ISettingsAndFilterPanelViewModel<FormDateReportDBModel> SettingsAndFilterPanelViewModel { get; }

        private IBlockHistoryGetDataLogic dataLogic;

        private ObservableCollection<FormDatailedReportModel> formReportItems;

        public ObservableCollection<FormDatailedReportModel> FormReportItems //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return formReportItems; }
            set
            {
                if (formReportItems == value) return;
                formReportItems = value;
                OnPropertyChanged("FormReportItems");
            }
        }


        private ObservableCollection<FormDatailedReportModel> selectedFormBlockHistory;

        public ObservableCollection<FormDatailedReportModel> SelectedFormBlockHistory //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return selectedFormBlockHistory; }
            set
            {
                if (selectedFormBlockHistory == value) return;
                selectedFormBlockHistory = value;
                OnPropertyChanged("SelectedFormBlockHistory");
            }
        }

        private ObservableCollection<BlowingMachineReportModel> selectedBlowingMachineBlockHistory;

        public ObservableCollection<BlowingMachineReportModel> SelectedBlowingMachineBlockHistory //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return selectedBlowingMachineBlockHistory; }
            set
            {
                if (selectedBlowingMachineBlockHistory == value) return;
                selectedBlowingMachineBlockHistory = value;
                OnPropertyChanged("SelectedBlowingMachineBlockHistory");
            }
        }


        private ObservableCollection<ContinuousBlowingMachineReportModel> selectedContinuousBlowingMachineBlockHistory;

        public ObservableCollection<ContinuousBlowingMachineReportModel> SelectedContinuousBlowingMachineBlockHistory //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return selectedContinuousBlowingMachineBlockHistory; }
            set
            {
                if (selectedContinuousBlowingMachineBlockHistory == value) return;
                selectedContinuousBlowingMachineBlockHistory = value;
                OnPropertyChanged("SelectedContinuousBlowingMachineBlockHistory");
            }
        }

        private FormDateReportDBModel formItemSelected;

        public FormDateReportDBModel FormItemSelected //todo moze by tu zrobil jakies stata ktory by trzyaml obecny stan widoku(na jakim raorcie jestes co wybrales itp)
        {
            get { return formItemSelected; }
            set
            {
                if (formItemSelected == value)
                {
                    if (formItemSelected != null)
                    {
                        GetBlockHistory();
                    }

                    return;
                }
                formItemSelected = value;
                OnPropertyChanged("FormItemSelected");
                if (formItemSelected != null)
                {
                    GetBlockHistory();
                }
            }
        }

        private string pz;
        public string Pz 
        {
            get { return pz; }
            set
            {
                if (pz == value) return;
                pz = value;
                OnPropertyChanged("Pz");
            }
        }


        private string blockNumber;
        public string BlockNumber
        {
            get { return blockNumber; }
            set
            {
                if (blockNumber == value) return;
                blockNumber = value;
                OnPropertyChanged("BlockNumber");
            }
        }

        private async void GetBlockHistory()
        {
            var blowingMachineReportModels = await dataLogic.GetBlowingMachineData(FormItemSelected.ProductionDate, FormItemSelected.Chamber, FormItemSelected.PzNumber);
            SelectedFormBlockHistory = new ObservableCollection<FormDatailedReportModel>(new List<FormDatailedReportModel>(){ FormItemSelected });
            if (blowingMachineReportModels.Count > 0)
            {
                SelectedBlowingMachineBlockHistory = new ObservableCollection<BlowingMachineReportModel>(new List<BlowingMachineReportModel>() { blowingMachineReportModels[0] });

            }

            var continuousBlowingMachineReportModels = await dataLogic.GetContinuousBlowingMachineData(FormItemSelected.ProductionDate, FormItemSelected.Chamber, FormItemSelected.PzNumber);
            if (continuousBlowingMachineReportModels.Count > 0)
            {
                SelectedContinuousBlowingMachineBlockHistory = new ObservableCollection<ContinuousBlowingMachineReportModel>(new List<ContinuousBlowingMachineReportModel>() { continuousBlowingMachineReportModels[0] });
            }
        
    }

        public BlockHistoryViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger, IBlockHistoryGetDataLogic dataLogic) : base(container)
        {
            SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel<FormDateReportDBModel>(container, repository, logger);
            this.SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.BlockHistoryViewModel;
            this.SettingsAndFilterPanelViewModel.GeneratedModelItemsAction += OnGetFormBlocks;
            this.dataLogic = dataLogic;
            FormItemSelected = null;
            Pz = "";
            BlockNumber = "";
        }

        private void OnGetFormBlocks(object sender, EventBaseArgs<FormDateReportDBModel> result)
        {
            IEnumerable<FormDatailedReportModel> data = result.ReportModel.Model;
            if (Pz != "")
            {
                data = data.Where(el => el.PzNumber.ToString() == (Pz));
            }
            if (BlockNumber != "")
            {
                data = data.Where(el => el.AssignedNumber.ToString() == BlockNumber);
            }

            FormReportItems = new ObservableCollection<FormDatailedReportModel>(data);
        }

        public void Dispose()
        {
      
        }
    }
}
