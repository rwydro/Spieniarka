using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public static class SaveInFileLogic
    {
        public static void OnSaveReportInFile<T>(object objectToSave, string filePath, IMyLogger log)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                MessageBox.Show("Nie można zapisac pliku. Nieprawidłowa ścieżka", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                log.logger.WarnFormat("Zła ściezka: {0}", filePath);
                return;
            }
            using (FileStream fs = File.Create(filePath))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    s.Serialize(fs, objectToSave);
                }
                catch (IOException e)
                {
                    MessageBox.Show("Nie można zapisac pliku.", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    log.logger.Error(e);
                }
                   
            }
        }

        public static void SaveInFileAndOpen(string path, string shift, string blowingMachineType, XmlDocument document,IMyLogger logger)
        {
            try
            {
                var str = Path.Combine(path, string.Format("Spieniarka_{0}_{1}_zmiana_{2}.xml", blowingMachineType, DateTime.Now.Day, shift));
                document.Save(str);
                bool result;
                if (!bool.TryParse(ConfigurationManager.AppSettings["IsOpenReportAfterSaved"], out result))
                    return;
                Process.Start(str);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                MessageBoxHelper.ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                logger.logger.Error("Błąd zapisu pliku", ex);
                MessageBoxHelper.ShowMessageBox("Nie można zapisać pliku. Sprawdź czy nie jest on otwarty", MessageBoxIcon.Exclamation);
            }
        }
    }
}
