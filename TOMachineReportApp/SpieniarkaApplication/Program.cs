using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using Npgsql;
using SpieniarkaApplication.DataBase;
using SpieniarkaApplication.Logic;

namespace SpieniarkaApplication
{
    class Program
    {
        private static FilesLogic filesLogic;
        private  static ISpieniarkaProbkiRepository spieniarkaProbkiRepository;
        private static ISpieniarkaLogger myLogger;

        static void Main(string[] args)
        {
            // var iqweqw = Convert.ToDouble(item.ItemArray[6], CultureInfo.CreateSpecificCulture("en-US"));
    
            Init();
            
            RunFtpCommand();
            var date = spieniarkaProbkiRepository.GetDateLastRecord();
            spieniarkaProbkiRepository.InsertMissingData(filesLogic.GetDataFromFile(), date);
        }

        private static void Init()
        {
            XmlConfigurator.Configure();
            filesLogic = new FilesLogic();
            myLogger = new SpieniarkaLogger();
            spieniarkaProbkiRepository = new SpieniarkaProbkiRepository(myLogger);
        }

        private static void RunFtpCommand()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = ConfigurationManager.AppSettings["CmdCommand"];
            process.StartInfo = startInfo;
            Console.WriteLine("Start read file");
            process.Start();
            Console.WriteLine("End read file");

            process.WaitForExit();
        }
    }
}
