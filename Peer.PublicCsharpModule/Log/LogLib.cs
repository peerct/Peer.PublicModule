using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Peer.PublicCsharpModule.Log
{
   public class LogLib
    {
        #region 删除指定目录下几天之前的文件
        public static void DeleteFolorAFile(int Day)
        {
            var pathStr = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            pathStr = Path.Combine(pathStr, "syslog");
            if (Directory.Exists(pathStr) == false)
            {
                Directory.CreateDirectory(pathStr);
            }
            //参数为非负数，则默认删一周前的日志
            if (Day >= 0)
            {
                Day = -7;
            }
            DirectoryInfo di = new DirectoryInfo(@pathStr);
            //获取子文件夹列表
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                if (dir.CreationTime < DateTime.Today.AddDays(Day))
                {
                    DeleteFolder(@dir.FullName);
                }
            }
            DeleteLogFiles(pathStr, Day);
        }
        private static void DeleteLogFiles(string FolorStr, int Day)
        {
            try
            {
                if (Directory.Exists(FolorStr) == true)
                {
                    DirectoryInfo dir = new DirectoryInfo(@FolorStr);
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        if (Day < 0)
                        {
                            if (fi.CreationTime < DateTime.Today.AddDays(Day))
                                fi.Delete();
                        }
                        else
                        {
                            fi.Delete();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 用递归方法删除文件夹目录及文件
        /// </summary>
        /// <param name="dir">带文件夹名的路径</param> 
        private static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }
        #endregion


        //直接写日志信息
        public static void WriteLog(string Msg, LogType MsgType)
        {
            var pathStr = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            pathStr = Path.Combine(pathStr, "syslog");
            pathStr = Path.Combine(pathStr, DateTime.Now.Year.ToString());
            pathStr = Path.Combine(pathStr, DateTime.Now.Month.ToString());
            if (Directory.Exists(pathStr) == false)
            {
                Directory.CreateDirectory(pathStr);
            }
            string strFile = Path.Combine(pathStr, String.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd")));

            StringBuilder MsgInfo = new StringBuilder();
            MsgInfo.AppendFormat("{0} 日志类型:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), MsgType.ToString());
            MsgInfo.AppendLine();
            MsgInfo.AppendLine(Msg);
            MsgInfo.Append("---------------------------------------------------------");

            StreamWriter SW = new StreamWriter(strFile, true);
            SW.WriteLine(MsgInfo.ToString());
            MsgInfo.Clear();
            MsgInfo = null;
            SW.Flush();
            SW.Close();
        }

        /// <summary>
        /// 获取异常详细信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="moduleInfo">具体异常发生模块名：功能模块名、类名、方法名等</param>
        /// <param name="parasInfo">参数信息</param>
        /// <returns></returns>
        public static string GetExceptionInfo(Exception ex, string moduleInfo, string parasInfo="")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("【异常模块】：" + moduleInfo);
            if (!parasInfo.Equals(""))
            {
                sb.AppendLine("【参数信息】：" + parasInfo);
            }
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
                sb.AppendLine("【异常方法】：" + ex.TargetSite);
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine("【内部异常信息】：" + ex.InnerException.Message);
                sb.AppendLine("【内部异常源】：" + ex.InnerException.Source);
                sb.AppendLine("【内部异常堆栈】：" + ex.InnerException.StackTrace);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
        /// <summary>
        /// 调用说明：getLogInfo(new StackFrame(true)
        /// </summary>
        /// <param name="StackFrameIns"></param>
        /// <returns></returns>
        public static string getLogInfo(StackFrame StackFrameIns)
        {
            StackTrace st = new StackTrace(StackFrameIns);
            StackFrame sf   = st.GetFrame(0);
            StringBuilder logInfo = new StringBuilder();
            logInfo.AppendLine();
            //得到文件名
            logInfo.Append(sf.GetFileName());
            logInfo.Append("  ");
            //得到方法名
            logInfo.Append(sf.GetMethod().Name);
            logInfo.Append("  ");
            //得到行号
            logInfo.Append(sf.GetFileLineNumber());
            logInfo.Append("  ");
            //得到列号
            logInfo.Append(sf.GetFileColumnNumber());
            string Info  = logInfo.ToString();
            logInfo.Clear();
            logInfo = null;
            return Info;
        }
    }
    public enum LogType
    {
        Trace=0,
        Wrong=1,
        Info
    }
}
