using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;


namespace TOReportApplication.ViewModels
{
    public class MainViewModel
    {
      private DelegateCommand<string> SwitchViewCommand { get; set; }

        public MainViewModel()
        {
            SwitchViewCommand = new DelegateCommand<string>(SwitchView);
        }

        private void SwitchView(string obj)
        {
           
        }
    }
}
