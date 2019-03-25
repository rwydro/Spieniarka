using System;
using System.Configuration;
using log4net.Config;

namespace SpieniarkaCiagla4._0
{
    class Program
    {
        private static ISpieniarkaCiaglaRepository spieniarkaCiaglaRepository;
        private static ISpieniarkaLogger myLogger;

        private static string LogFilePath => ConfigurationManager.AppSettings["PathToLogFile"];

        static void Main(string[] args)
        {
            Init();
            RunFtpCommand();
            Console.WriteLine("Start Logic");
            var date = spieniarkaCiaglaRepository.GetDateLastRecord();
            spieniarkaCiaglaRepository.InsertMissingData(FilesLogic.GetDataFromFile(LogFilePath), date);
        }

        private static void Init()
        {

            XmlConfigurator.Configure();
            myLogger = new SpieniarkaLogger();
            spieniarkaCiaglaRepository = new SpieniarkaCiaglaRepository(myLogger);
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
