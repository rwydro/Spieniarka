using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public static class BlowingMachineXmlGenerator
    {

        private static readonly string buttomDocumentMock =
            "<Row ss:Index=\"_OPERATORINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Operator:</Data>\r\n  </Cell>\r\n</Row>\r\n<Row ss:Index=\"_SIGNEDINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Podpis:</Data>\r\n  </Cell>\r\n  <Cell>\r\n    <Data ss:Type=\"String\">………………</Data>\r\n  </Cell>\r\n</Row>";

        private static readonly string columnMock =
            "<Row ss:Index=\"4\" ss:AutoFitHeight=\"0\" ss:Height=\"30.75\">\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Lp.</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Godzina rozpoczęcia</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Godzina zakończenia</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Silos/\r\nkomora</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Gęstość zadana</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Gęstość średnia</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Surowiec</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Ilość spieniona</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Produkt</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">PZ</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s16\" />\r\n      </Row>";

        private static readonly string rowMock =
            "<Row>\r\n    <Cell ss:StyleID=\"s23\">\r\n      <Data ss:Type=\"Number\">_NUMBER_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_STARTDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s28\" >\r\n      <Data ss:Type=\"String\">_STOPDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s27\">\r\n      <Data ss:Type=\"String\">_SILOS_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYMEAN_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_TYPE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_WEIGHTSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_MATERIAL_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_LOTNUMBER_</Data>\r\n    </Cell>\r\n</Row>";

        private static readonly string typeTableHeader =
            "<Row ss:Index=\"_MATERIALTABLEINDEX_\">\r\n\t\t<Cell ss:Index=\"2\" ss:StyleID=\"s27\">\r\n\t\t\t<Data ss:Type=\"String\">BILANS SUROWCA:</Data>\r\n\t\t</Cell>\r\n\t\t<Cell ss:StyleID=\"s27\" />\r\n\t</Row>";

        private static readonly string rowMaterialTableMock =
            "<Row>\r\n  <Cell ss:Index=\"2\" ss:StyleID=\"s27\">\r\n    <Data ss:Type=\"String\">_ROWMATERIAL_</Data>\r\n  </Cell>\r\n  <Cell ss:StyleID=\"s27\">\r\n    <Data ss:Type=\"Number\">_RWNUM_</Data>\r\n  </Cell>\r\n</Row>";

        private static readonly string rowContinuousMock =
            "<Row>\r\n    <Cell ss:StyleID=\"s23\">\r\n      <Data ss:Type=\"Number\">_NUMBER_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_STARTDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s28\" >\r\n      <Data ss:Type=\"String\">_ENDDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s27\">\r\n      <Data ss:Type=\"String\">_CHAMBER_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s27\">\r\n      <Data ss:Type=\"String\">_NRPZ_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_SETDENSITY_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_AVGSETDENTISY_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_TYPE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_MATERIAL_</Data>\r\n    </Cell>\r\n</Row>";

        private static readonly string columnContinuousMock =
            "<Row ss:Index=\"4\" ss:AutoFitHeight=\"0\" ss:Height=\"30.75\">\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Lp.</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Data rozpoczęcia</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Data zakończenia</Data>\r\n        </Cell>\r\n              <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Komora</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Numez PZ</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s27\">\r\n          <Data ss:Type=\"String\">Gęstość zadana</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Średnia gęstość</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Gatunek</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s19\">\r\n          <Data ss:Type=\"String\">Materiał</Data>\r\n        </Cell>\r\n        <Cell ss:StyleID=\"s16\" />\r\n      </Row>";


        

        private static List<KeyValuePair<string, double>> typeDictionary = new List<KeyValuePair<string, double>>();

        private static List<KeyValuePair<string, double>> GetSum(List<KeyValuePair<string, double>> dict)
        {
            if (dict == null || !dict.Any())
                return null;
            return dict
                .GroupBy(x => x.Key)
                .Select(g => new KeyValuePair<string, double>(g.Key, g.Sum(x => x.Value)))
                .ToList();
        }


        public static XmlDocument GenerateXml<T>(string shift, List<T> item) where T : BlowingMachineReportModelBase
        {
            var document = new XmlDocument();
            document.Load("Mock.xml");
            document.InnerXml = document.InnerXml.Replace("_OPERATOR_", item.First().Operator)
                .Replace("_DATE_", DateTime.Now.ToString("g")).Replace("_ZMIANA_", shift)
                .Replace("_COLUMNMOCK_", typeof(T) == typeof(BlowingMachineReportModel) ? columnMock: columnContinuousMock);

            if (typeof(T) == typeof(BlowingMachineReportModel))
            {
                document.InnerXml = document.InnerXml
                    .Replace("_HEADER_", "RAPORT ZMIANOWY SPIENIANIA - I STOPIEŃ")
                    .Replace("_MYROW_", GenerateMainTable(item as List<BlowingMachineReportModel>))                   
                    .Replace("_BUTTOM_", GenerateButtomFields(item.Count))
                    .Replace("_TYPETABLE_", GenerateTypeTable(item.Count))
                    .Replace("_MATERIALTABLEINDEX_", (item.Count + 8).ToString());
            }
            else
            {
                document.InnerXml = document.InnerXml
                    .Replace("_HEADER_", "RAPORT ZMIANOWY SPIENIANIA - II/III STOPIEŃ")
                    .Replace("_MYROW_", GenerateContinuousMainTable(item as List<ContinuousBlowingMachineShiftReportModel>))
                    .Replace("_BUTTOM_", GenerateButtomFields(item.Count));
            }
         
            return document;
        }

        private static string GenerateTypeTable(int count)
        {
            var sumDict = GetSum(typeDictionary);
            var type = new StringBuilder();
            type.Append(typeTableHeader);
            foreach (var item in sumDict)
            {
                type.Append(rowMaterialTableMock).Replace("_ROWMATERIAL_", item.Key)
                    .Replace("_RWNUM_", item.Value.ToString());
            }
          return type.ToString();
        }

        private static string GenerateButtomFields(int count)
        {
            var buttom = new StringBuilder();
            buttom.Append(buttomDocumentMock).Replace("_OPERATORINDEX_", (count + 6).ToString())
                .Replace("_SIGNEDINDEX_", (count + 7).ToString());
            return buttom.ToString();
        }

        private static string GenerateContinuousMainTable(List<ContinuousBlowingMachineShiftReportModel> item)
        {
            var counter = 0;
            var stringBuilder = new StringBuilder();
            var rowMaterialDictionary = new List<KeyValuePair<string, double>>();
            foreach (var machineReportItem in item)
            {
                ++counter;
                stringBuilder.Append(rowContinuousMock).Replace("_NUMBER_", counter.ToString().Trim())
                    .Replace("_STARTDATE_", machineReportItem.StartDate.ToString().Trim())
                    .Replace("_ENDDATE_", machineReportItem.EndDate.ToString().Trim())
                    .Replace("_CHAMBER_", machineReportItem.Chamber.ToString().Trim())
                    .Replace("_NRPZ_", machineReportItem.PzNumber.ToString().Trim())
                    .Replace("_SETDENSITY_", machineReportItem.DensitySet.ToString().Trim())
                    .Replace("_AVGSETDENTISY_", machineReportItem.AvgDensityOfMeassurement.Trim())
                    .Replace("_TYPE_", machineReportItem.Type.Trim())
                    .Replace("_MATERIAL_", machineReportItem.Material.Trim());
            }

            typeDictionary = rowMaterialDictionary;
            return stringBuilder.ToString();
        }

        private static string GenerateMainTable(List<BlowingMachineReportModel> item)
        {
            var counter = 0;
            var stringBuilder = new StringBuilder();
            var rowMaterialDictionary = new List<KeyValuePair<string, double>>();
            foreach (var machineReportItem in item)
            {
                ++counter;
                stringBuilder.Append(rowMock).Replace("_NUMBER_", counter.ToString())
                    .Replace("_STARTDATE_", machineReportItem.DateTimeStart.ToString())
                    .Replace("_STOPDATE_", machineReportItem.DateTimeStop.ToString())
                    .Replace("_DENSITYSET_", machineReportItem.DensitySet.ToString())
                    .Replace("_DENSITYMEAN_", machineReportItem.DensityMean.ToString())
                    .Replace("_WEIGHTSET_", machineReportItem.WeightSet.ToString())
                    .Replace("_SILOS_", machineReportItem.Silos0)
                    .Replace("_TYPE_", machineReportItem.Type)
                    .Replace("_MATERIAL_", machineReportItem.Material)
                    .Replace("_LOTNUMBER_", machineReportItem.LotNumber);
                    rowMaterialDictionary.Add(new KeyValuePair<string, double>(machineReportItem.Type,
                    machineReportItem.WeightSet));
            }

            typeDictionary = rowMaterialDictionary;
            return stringBuilder.ToString();
        }
    }
}
    

