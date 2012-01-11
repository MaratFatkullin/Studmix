using System;
using AI_.Studmix.ApplicationServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace AI_.Studmix.Infrastructure
{
    public class Logger : ILogger
    {
        private readonly LogWriter _logWriter;

        public Logger()
        {
            _logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        }

        public void Write(string message, string category, int priority)
        {
            //_logWriter.Write(message, category, priority,1,TraceEventType.Critical);
        }

        public void Error(Exception exception)
        {
            //throw new NotImplementedException();
        }
    }
}