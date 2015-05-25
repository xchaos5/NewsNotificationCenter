using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NewsNotificationCenter
{
    public class JianshuTraceListener : TraceListener
    {
        private const string LogFolderName = "Logs";
        private string _fileName;
        private string _sessionID;

        public JianshuTraceListener(string fileName)
        {
            _fileName = fileName;
            _sessionID = Guid.NewGuid().ToString();

            CreateLogDirectoryIfNecessary();
        }

        [Conditional("DEBUG")]
        private void CreateLogDirectoryIfNecessary()
        {
            if (!Directory.Exists(LogFolderName))
            {
                Directory.CreateDirectory(LogFolderName);
            }
        }

        public override void Write(string message)
        {
            File.AppendAllText(LogFolderName + "\\" + _fileName, _sessionID + "\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + message);
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(LogFolderName + "\\" + _fileName, _sessionID + "\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + message + Environment.NewLine);
        }

        public override void WriteLine(string message, string category)
        {
            File.AppendAllText(LogFolderName + "\\" + _fileName, "[" + category + "]\t" + _sessionID + "\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + message + Environment.NewLine);
        }
    }
}
