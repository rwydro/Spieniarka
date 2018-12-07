using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace TOReportApplication.ViewModels
{
    public class BlowingMachineSetShiftReportDataViewModel : ViewModelBase, IBlowingMachineSetShiftReportDataViewModel
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

        public BlowingMachineSetShiftReportDataViewModel(IUnityContainer container) : base(container)
        {
            SetMaterialType();
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
    }
}
