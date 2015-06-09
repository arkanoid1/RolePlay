using System;
using System.Text;

using Sandbox.ModAPI;

namespace Logger
{
    public class Log
    {
        private System.IO.TextWriter m_writer;
        private StringBuilder m_cache = new StringBuilder();

        public Log(string logFile)
        {
            m_writer = MyAPIGateway.Utilities.WriteFileInLocalStorage(logFile, typeof(Log));
        }
        public void WriteLine(string text)
        {
            if (m_cache.Length > 0)
                m_writer.WriteLine(m_cache);
            m_cache.Clear();
            m_cache.Append(DateTime.Now.ToString("[HH:mm:ss] \t"));
            m_writer.WriteLine(m_cache.Append(text));
            m_writer.Flush();
            m_cache.Clear();
        }
        public void Write(string text)
        {
            m_cache.Append(text);
        }
        internal void Close()
        {
            if (m_cache.Length > 0)
                m_writer.WriteLine(m_cache);
            m_writer.Flush();
            m_writer.Close();
        }
    }
}
