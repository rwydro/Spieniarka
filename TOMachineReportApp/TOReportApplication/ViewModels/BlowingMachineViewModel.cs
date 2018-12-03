using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using TOReportApplication.DataBase;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels.interfaces;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class BlowingMachineViewModel : ViewModelBase, IBlowingMachineViewModel
    {
        private readonly IMyLogger logger;
        private Action _onControlLoaded;
        private ObservableCollection<BlowingMachineReportModel> blowingMachineReportItems;

        private readonly string buttomDocumentMock =
            "<Row ss:Index=\"_OPERATORINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Operator:</Data>\r\n  </Cell>\r\n </Row>\r\n<Row ss:Index=\"_SIGNEDINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Podpis:</Data>\r\n  </Cell>\r\n  <Cell>\r\n    <Data ss:Type=\"String\">………………</Data>\r\n  </Cell>\r\n</Row>\r\n<Row ss:Index=\"_MATERIALTABLEINDEX_\">\r\n  <Cell ss:Index=\"2\" ss:StyleID=\"s21\">\r\n    <Data ss:Type=\"String\">BILANS SUROWCA:</Data>\r\n  </Cell>\r\n  <Cell ss:StyleID=\"s21\" />\r\n</Row>\r\n<Row>\r\n  <Cell ss:Index=\"2\" ss:StyleID=\"s21\">\r\n    <Data ss:Type=\"String\">FS0816</Data>\r\n  </Cell>\r\n  <Cell ss:StyleID=\"s21\" ss:Formula=\"=SUMIF(R[-11]C[4]:R[-8]C[4],RC[-1],R[-11]C[5]:R[-8]C[5])\">\r\n    <Data ss:Type=\"Number\">0</Data>\r\n  </Cell>\r\n</Row>";

        private readonly string rowMock =
            "<Row>\r\n    <Cell ss:StyleID=\"s23\">\r\n      <Data ss:Type=\"Number\">_NUMBER_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_STARTDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s28\" >\r\n      <Data ss:Type=\"String\">_STOPDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\">\r\n      <Data ss:Type=\"String\">_SILOS_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYMEAN_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_TYPE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_WEIGHTSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_MATERIAL_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_LOTNUMBER_</Data>\r\n    </Cell>\r\n</Row>";

        private readonly string rowMaterialTableMock =
            "<Row>\r\n  <Cell ss:Index=\"2\" ss:StyleID=\"s21\">\r\n    <Data ss:Type=\"String\">_ROWMATERIAL_</Data>\r\n  </Cell>\r\n  <Cell ss:StyleID=\"s21\" ss:Formula=\"=SUMIF(R[-11]C[4]:R[-8]C[4],RC[-1],R[-11]C[5]:R[-8]C[5])\">\r\n    <Data ss:Type=\"Number\">_RWNUM_</Data>\r\n  </Cell>\r\n</Row>";

        public BlowingMachineViewModel(IUnityContainer container, IApplicationRepository repository, IMyLogger logger)
            : base(container)
        {
            this.logger = logger;
            SettingsAndFilterPanelViewModel = new SettingsAndFilterPanelViewModel(container, repository, logger);
            SettingsAndFilterPanelViewModel.DataContextEnum = DataContextEnum.BlowingMachineVIewModel;
            SettingsAndFilterPanelViewModel.BlowingMachineReportsModelItemsAction +=
                OnGetBlowingMachineReportsModelItems;
            SettingsAndFilterPanelViewModel.SaveBlowingMachineReportsInFileAction += SaveReportInFile;
            BlowingMachineReportItems = new ObservableCollection<BlowingMachineReportModel>();
        }

        public ObservableCollection<BlowingMachineReportModel> BlowingMachineReportItems
        {
            get => blowingMachineReportItems;
            set
            {
                blowingMachineReportItems = value;
                OnPropertyChanged(nameof(BlowingMachineReportItems));
            }
        }

        public ISettingsAndFilterPanelViewModel SettingsAndFilterPanelViewModel { get; }

        private void SaveReportInFile(string shift)
        {
            try
            {
               // var document = new XmlDocument();
               // document.Load("Mock.xml");
               // document.InnerXml = document.InnerXml.Replace("_OPERATOR_", BlowingMachineReportItems.First().Operator)
               //     .Replace("_DATE_", DateTime.Now.ToString("g")).Replace("_ZMIANA_", shift);
               // var rowMaterialDictionary = new List<KeyValuePair<string, double>>();
               // var stringBuilder = new StringBuilder();
               // var stringBuilderRawMaterial = new StringBuilder();
               // var counter = 0;
               // double materialCounter = 0;
               // var materialType = "";

               // foreach (var machineReportItem in BlowingMachineReportItems)
               // {
               //     if (String.IsNullOrEmpty(materialType))
               //         materialType = machineReportItem.Type;
               //     ++counter;
               //     stringBuilder.Append(rowMock).Replace("_NUMBER_", counter.ToString())
               //         .Replace("_STARTDATE_", machineReportItem.DateTimeStart.ToString())
               //         .Replace("_STOPDATE_", machineReportItem.DateTimeStop.ToString())
               //         .Replace("_DENSITYSET_", machineReportItem.DensitySet.ToString())
               //         .Replace("_DENSITYMEAN_", machineReportItem.DensityMean.ToString())
               //         .Replace("_WEIGHTSET_", machineReportItem.WeightSet.ToString())
               //         .Replace("_SILOS_", machineReportItem.Silos0)
               //         .Replace("_TYPE_", machineReportItem.Type)
               //         .Replace("_MATERIAL_", machineReportItem.Material)
               //         .Replace("_LOTNUMBER_", machineReportItem.LotNumber);
               //     materialCounter = materialCounter + machineReportItem.WeightSet;
               //     rowMaterialDictionary.Add(new KeyValuePair<string, double>(machineReportItem.Type,
               //         machineReportItem.WeightSet));
               // }

               //// var sum = GetSum(rowMaterialDictionary);
               // var buttom = new StringBuilder();

               // buttom.Append(buttomDocumentMock).Replace("_OPERATORINDEX_", (counter + 6).ToString())
               //     .Replace("_SIGNEDINDEX_", (counter + 7).ToString())
               //     .Replace("_MATERIALTABLEINDEX_", (counter + 9).ToString()).ToString();

               // document.InnerXml = document.InnerXml.Replace("_MYROW_", stringBuilder.ToString())
               //     .Replace("_BUTTOM_", buttom.ToString());
                 var document = SaveReportInFileLogic.GenerateXml(shift, BlowingMachineReportItems.ToList());
                SaveInFileAndOpen(CreateMissingFolders(ConfigurationManager.AppSettings["PathToBlowingMachineReport"]),
                    shift, document);
            }
            catch (InvalidOperationException ex)
            {
                logger.logger.Error("Pusta kolekcja", ex);
                ShowMessageBox("Brak wartości dla wybranej daty", MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                ShowMessageBox("Błąd podczas zapisu pliku spróbuj ponownie", MessageBoxIcon.Exclamation);
            }
        }

      

        private void SaveInFileAndOpen(string path, string shift, XmlDocument document)
        {
            try
            {
                var str = Path.Combine(path, string.Format("{0}_zmiana_{1}.xml", DateTime.Now.Day, shift));
                document.Save(str);
                bool result;
                if (!bool.TryParse(ConfigurationManager.AppSettings["IsOpenReportAfterSaved"], out result))
                    return;
                Process.Start(str);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
        }

        private string CreateMissingFolders(string path)
        {
            try
            {
                path = Path.Combine(path, DateTime.Now.Year.ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture));
                if (!Directory.Exists(Path.Combine(path, DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture))))
                    Directory.CreateDirectory(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                logger.logger.Error("Failed during creating directory", ex);
                ShowMessageBox("Błąd podczas tworzenia katalogów sprawdź uprawnienia", MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Failed during creating directory", ex);
                ShowMessageBox("Błąd podczas tworzenia katalogów sprawdź uprawnienia", MessageBoxIcon.Exclamation);
            }

            return path;
        }

        private void OnGetBlowingMachineReportsModelItems(BlowingMachineReportDto obj)
        {
            BlowingMachineReportItems.Clear();
            obj.Model.ForEach(x => BlowingMachineReportItems.Add(x));
        }

        public void Dispose()
        {
            SettingsAndFilterPanelViewModel.SaveBlowingMachineReportsInFileAction -= SaveReportInFile;
            SettingsAndFilterPanelViewModel.BlowingMachineReportsModelItemsAction -=
                OnGetBlowingMachineReportsModelItems;
        }

        private void ShowMessageBox(string info, MessageBoxIcon icon)
        {
            MessageBox.Show(info, "Error", MessageBoxButtons.OK,
                icon);
        }
    }
}