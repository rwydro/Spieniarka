using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public static class SaveReportInFileLogic
    {

        private static readonly string buttomDocumentMock =
            "<Row ss:Index=\"_OPERATORINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Operator:</Data>\r\n  </Cell>\r\n</Row>\r\n<Row ss:Index=\"_SIGNEDINDEX_\">\r\n  <Cell ss:Index=\"7\">\r\n    <Data ss:Type=\"String\">Podpis:</Data>\r\n  </Cell>\r\n  <Cell>\r\n    <Data ss:Type=\"String\">………………</Data>\r\n  </Cell>\r\n</Row>";

        private static readonly string rowMock =
            "<Row>\r\n    <Cell ss:StyleID=\"s23\">\r\n      <Data ss:Type=\"Number\">_NUMBER_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_STARTDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s28\" >\r\n      <Data ss:Type=\"String\">_STOPDATE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s27\">\r\n      <Data ss:Type=\"String\">_SILOS_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_DENSITYMEAN_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_TYPE_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_WEIGHTSET_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s21\" >\r\n      <Data ss:Type=\"String\">_MATERIAL_</Data>\r\n    </Cell>\r\n    <Cell ss:StyleID=\"s23\" >\r\n      <Data ss:Type=\"String\">_LOTNUMBER_</Data>\r\n    </Cell>\r\n</Row>";

        private static readonly string rowMaterialTableMock =
            "<Row>\r\n  <Cell ss:Index=\"2\" ss:StyleID=\"s27\">\r\n    <Data ss:Type=\"String\">_ROWMATERIAL_</Data>\r\n  </Cell>\r\n  <Cell ss:StyleID=\"s27\">\r\n    <Data ss:Type=\"Number\">_RWNUM_</Data>\r\n  </Cell>\r\n</Row>";

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

        public static XmlDocument GenerateXml(string shift, List<BlowingMachineReportModel> item)
        {
            var document = new XmlDocument();
            document.Load("Mock.xml");
            document.InnerXml = document.InnerXml.Replace("_OPERATOR_", item.First().Operator)
                .Replace("_DATE_", DateTime.Now.ToString("g")).Replace("_ZMIANA_", shift)
                .Replace("_MATERIALTABLEINDEX_", (item.Count + 8).ToString());


            document.InnerXml = document.InnerXml.Replace("_MYROW_", GeneratMainTable(item))
                .Replace("_BUTTOM_", GenerateButtomFields(item.Count))
                .Replace("_TYPETABLE_", GenerateTypeTable(item.Count));
            return document;
        }

        private static string GenerateTypeTable(int count)
        {
            var sumDict = GetSum(typeDictionary);
            var type = new StringBuilder();
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

        private static string GeneratMainTable(List<BlowingMachineReportModel> item)
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
    

