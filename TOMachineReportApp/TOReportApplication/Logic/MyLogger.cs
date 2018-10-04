using System.Reflection;
using Castle.Core.Logging;
using log4net;

namespace TOReportApplication.Logic
{
    public interface IMyLogger
    {
        ILog logger { get; }
    }

    public class MyLogger:IMyLogger
    {
        public ILog logger
        {
            get
            {
                return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}
