using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace SpieniarkaCiagla4._0
{
    public interface ISpieniarkaLogger
    {
        ILog logger { get; }
    }

    public class SpieniarkaLogger : ISpieniarkaLogger
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
