using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    }
}
