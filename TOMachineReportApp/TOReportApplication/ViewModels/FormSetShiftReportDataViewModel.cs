using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Prism.Commands;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace TOReportApplication.ViewModels
{
    public class FormSetShiftReportDataViewModel : ViewModelBase, IFormSetShiftReportDataViewModel
    {
        private ObservableCollection<string> materialType { get; set; }

        public ObservableCollection<string> MaterialType
        {
            get { return materialType; }
            set
            {
                if(materialType == value) return;
                materialType = value;
                OnPropertyChanged("Type");
            }
        }

        private string avgDensityOfPearls { get; set; }

        public string AvgDensityOfPearls
        {
            get { return avgDensityOfPearls; }
            set
            {
                if (avgDensityOfPearls == value) return;
                avgDensityOfPearls = value;
                OnPropertyChanged("AvgDensityOfPearls");
            }
        }

        private string comments { get; set; }

        public string Comments
        {
            get { return comments; }
            set
            {
                if (comments == value) return;
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        private string numberOfBlock { get; set; }

        public string NumberOfBlock
        {
            get { return numberOfBlock; }
            set
            {
                if (numberOfBlock == value) return;
                numberOfBlock = value;
                OnPropertyChanged("NumberOfBlock");
            }
        }

        private int assignedNumber { get; set; }

        public int AssignedNumber
        {
            get { return assignedNumber; }
            set
            {
                if (assignedNumber == value) return;
                assignedNumber = value;
                OnPropertyChanged("AssignedNumber");
            }
        }

        private string selectedMaterialType { get; set; }

        public string SelectedMaterialType
        {
            get { return selectedMaterialType; }
            set
            {
                if(selectedMaterialType == value)return;
                selectedMaterialType = value;
                OnPropertyChanged("SelectedMaterialType");
            }
        }

        public DelegateCommand SaveCommand { get; set; }
        public FormSetShiftReportDataViewModel(IUnityContainer container) : base(container)
        {
            SetMaterialType();
            SaveCommand = new DelegateCommand(OnSave);
        }

        private void OnSave()
        {
            double density;
            Double.TryParse(this.AvgDensityOfPearls, out density);
            OnSendMaterialTypeInfo(new MaterialTypeMenuModel()
            {
                AvgDensityOfPearls = density,
                Comments = this.Comments,
                NumberOfBlock = this.NumberOfBlock,
                SelectedMaterialType = this.SelectedMaterialType,
                AssignedNumber = this.AssignedNumber
            });
        }

        private void SetMaterialType()
        {
            MaterialType = new ObservableCollection<string>();
            var items = LoadJson();
            items.ForEach(x=> MaterialType.Add(x));
        }

        public List<string> LoadJson()
        {
            using (StreamReader r = new StreamReader("gatunek.json"))
            {
               string json = r.ReadToEnd();
               var items = JsonConvert.DeserializeObject<MaterialTypeModel>(json);
               return items.Type.Select(s => s.Name).ToList();
            }
        }

        public Action<MaterialTypeMenuModel> OnSendMaterialTypeInfo { get; set; }
    }
}
