using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Prism.Commands;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using Unity;

namespace TOReportApplication.ViewModels
{
    public interface IFormTopPanelViewModel
    {
       Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }
    }

    public class FormTopPanelViewModel:ViewModelBase, IFormTopPanelViewModel
    {
        public DelegateCommand GenerateReportCommand { get; set; }
        public DelegateCommand GenerateShiftReportCommand { get; set; }
        public Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }

        private readonly IForm2Repository dbConnection;
        private DateTime selectedDate { get; set; }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if(selectedDate == value ) return;
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        public FormTopPanelViewModel(IUnityContainer unityContainer, IForm2Repository dbConnection) : base(unityContainer)
        {
            this.dbConnection = dbConnection;
            GenerateReportCommand = new DelegateCommand(OnGenereteDateReport);
            GenerateShiftReportCommand = new DelegateCommand(OnGenereteShiftReport);
            
        }



        private void OnGenereteShiftReport()
        {
            
        }

        private void OnGenereteDateReport()
        {
            var dateReportDto = dbConnection.GetFormDateReportItems(String.Format("SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'", SelectedDate.AddHours(4), SelectedDate.AddDays(1).AddHours(10)));
            var dateReportModel = GenerateModelLogic.GeneratFormDateReportModel(dateReportDto);
            var detailedReportDto =
                dbConnection.GetFormDateReportItems(String.Format("SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'", SelectedDate.AddHours(4), SelectedDate.AddDays(1).AddHours(10)));
            var detailedReportModel = GenerateModelLogic.GeneratFormDetailedReportModel(detailedReportDto);
            FormReportsModelItemsAction(new FormReportsDBModel(){DateReportDb = dateReportModel, DetailedReportDb = detailedReportModel });
        }

      
    }
}
