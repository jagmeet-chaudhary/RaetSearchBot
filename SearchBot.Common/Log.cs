using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Common
{
    public class Log : ILog
    {
        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Info(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }
    }
}
