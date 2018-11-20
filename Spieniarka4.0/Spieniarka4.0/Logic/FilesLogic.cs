using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpieniarkaApplication.Logic
{
    public class FilesLogic
    {
        private string batchFilePath;

        public FilesLogic()
        {
            batchFilePath = ConfigurationManager.AppSettings["PathToBatchFile"];
        }
        public string [] GetDataFromFile()
        {
            return File.ReadAllLines(batchFilePath);
        }
    }
}
