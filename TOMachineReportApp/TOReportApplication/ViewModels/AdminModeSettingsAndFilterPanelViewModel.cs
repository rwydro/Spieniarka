using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Logic.interfaces;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class AdminModeSettingsAndFilterPanelViewModel : ViewModelBase, IAdminModeSettingsAndFilterPanelViewModel
    {

        private ObservableCollection<DataContextEnum> machinesList { get; set; }

        public ObservableCollection<DataContextEnum> MachinesList
        {
            get => machinesList;
            set
            {
                if(value == machinesList) return;
                machinesList = value;
                OnPropertyChanged("MachinesList");
            }
        }

        private DateTime selectedToDate { get; set; }

        public DateTime SelectedToDate
        {
            get => selectedToDate;
            set
            {
                if (selectedToDate == value)
                    return;
                selectedToDate = value;
                OnPropertyChanged("SelectedToDate");
            }
        }

        private DateTime selectedFromDate { get; set; }

        public DateTime SelectedFromDate
        {
            get => selectedFromDate;
            set
            {
                if (selectedFromDate == value)
                    return;
                selectedFromDate = value;
                OnPropertyChanged("SelectedFromDate");
            }
        }

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
            }
        }

        private object selectedMachine { get; set; }

        public object SelectedMachine
        {
            get => selectedMachine;
            set
            {
                if (selectedMachine == value)
                    return;

                switch (value)
                {
                    case "Forma":
                        selectedMachine = DataContextEnum.FormViewModel;
                        break;
                    case "Spieniarka":
                        selectedMachine = DataContextEnum.BlowingMachineViewModel;
                        break;
                    default:
                        selectedMachine = DataContextEnum.ContinuousBlowingMachineViewModel;
                        break;
                }
                OnPropertyChanged("SelectedMachine");
            }
        }

        public Action SearchButtonClickAction { get; set; }

        public DelegateCommand SearchButtonClickCommand { get; set; }

        public AdminModeSettingsAndFilterPanelViewModel(IUnityContainer container) : base(container)
        {
            SelectedFromDate = DateTime.Now.Date.AddDays(-1);
            SelectedToDate = DateTime.Now.Date;
            CurrentDateTime = DateTime.Now;
            MachinesList = new ObservableCollection<DataContextEnum>
            {
                DataContextEnum.FormViewModel,
                DataContextEnum.BlowingMachineViewModel,
                DataContextEnum.ContinuousBlowingMachineViewModel,
            };
            SearchButtonClickCommand = new DelegateCommand(OnSearchButtonClick);
        }

        public void OnSearchButtonClick()
        {
            SearchButtonClickAction();
        }
    }
}
