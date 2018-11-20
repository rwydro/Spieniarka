using System;
using System.Configuration;
using System.Diagnostics;
using log4net.Config;
using SpieniarkaApplication.DataBase;
using SpieniarkaApplication.Logic;

namespace SpieniarkaApplication
{
    class Program
    {
        private  static ISpieniarkaProbkiRepository spieniarkaProbkiRepository;
        private static ISpieniarkaLogger myLogger;

        private static string BatchFilePath => ConfigurationManager.AppSettings["PathToBatchFile"];
        private static string SummaryFilePath => ConfigurationManager.AppSettings["PathToSummaryFile"];

        static void Main(string[] args)
        {
    
            Init();
            
            RunFtpCommand();
            Console.WriteLine("Start Logic");
            var date = spieniarkaProbkiRepository.GetDateLastRecord(FileType.Batch);
            spieniarkaProbkiRepository.InsertMissingData(FilesLogic.GetDataFromFile(BatchFilePath, FileType.Batch), date, FileType.Batch);
            date = spieniarkaProbkiRepository.GetDateLastRecord(FileType.Summary);
            spieniarkaProbkiRepository.InsertMissingData(FilesLogic.GetDataFromFile(SummaryFilePath, FileType.Summary), date, FileType.Summary);
        }

        private static void Init()
        {

            XmlConfigurator.Configure();
            myLogger = new SpieniarkaLogger();
            spieniarkaProbkiRepository = new SpieniarkaProbkiRepository(myLogger);
        }

        private static void RunFtpCommand()
        {
            var command = ConfigurationManager.AppSettings["CmdCommand"];
            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = System.Diagnostics.Process.Start(processInfo);

            process.OutputDataReceived += (
                object sender, System.Diagnostics.DataReceivedEventArgs e
            ) => Console.WriteLine("stdout>>  " + e.Data + "\r\n");
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (
                object sender, System.Diagnostics.DataReceivedEventArgs e
            ) => Console.WriteLine("stderr>>  " + e.Data + "\r\n");
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("retcode>> " + process.ExitCode.ToString() + "\r\n");
            process.Close();
        }

    }
}
