using UnityEngine;

namespace PNKZ
{
    internal class LogQueue
    {
        public string LogText { get; private set; }
        public LogType LogType { get; private set; }

        public LogQueue(string logTextParam, LogType logTypeParam)
        {
            LogText = logTextParam;
            LogType = logTypeParam;
        }
    }
}