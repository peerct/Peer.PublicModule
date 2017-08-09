using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Diagnostics;
namespace Peer.PublicCsharpModule.CSharpLogToFile
{   
    //简单文本日志类
    public class LogManager
    {
        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    if (System.Web.HttpContext.Current == null)
                        // Windows Forms 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory;
                    else
                        // Web 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory + @"bin\";
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + " " +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }
        /// <summary>
        /// 返回具体位置（使用方法：string trackRecord = LogManager.getLogInfo(new StackFrame(true));）
        /// </summary>
        /// <param name="stackFrame"></param>
        /// <returns></returns>
        public static string getLogInfo(StackFrame stackFrame)
        {
            StackTrace st = new StackTrace(stackFrame);
            StackFrame sf = st.GetFrame(0);
            StringBuilder logInfo = new StringBuilder();
            // 得到文件名
            logInfo.Append(sf.GetFileName());
            logInfo.Append("  ");
            // 得到方法名
            logInfo.Append(sf.GetMethod().Name);
            logInfo.Append("  ");
            // 得到行号
            logInfo.Append(sf.GetFileLineNumber());
            logInfo.Append("  ");
            // 得到列号
            logInfo.Append(sf.GetFileColumnNumber());
            return logInfo.ToString();
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }
}