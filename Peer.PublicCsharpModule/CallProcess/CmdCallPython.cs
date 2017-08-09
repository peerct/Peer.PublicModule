using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peer.PublicCsharpModule.CallProcess
{
   public class CmdCallPython
    {
       /// <summary>
        /// 调用示例：StartPythonScrapy(@"scrapy runspider  D:/QQPCmgr/Desktop/nfzmxw/scrapy_cs.py");
       /// </summary>
       /// <param name="CmdStr">完整cmd命令</param>
       public static void StartPythonScrapy(string CmdStr)
       {
           try
           {
               System.Diagnostics.Process p = new System.Diagnostics.Process();
               p.StartInfo.FileName = "cmd.exe";
               p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
               p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
               p.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
               p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
               p.StartInfo.CreateNoWindow = true;//不显示程序窗口
               p.Start();//启动程序
               //向cmd窗口发送输入信息
               p.StandardInput.WriteLine(CmdStr + "&exit");
               p.StandardInput.AutoFlush = true;
               //等待程序执行完退出进程
               p.WaitForExit();
               p.Close();
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.ToString());
           }
           finally
           {

           }
       }
    }
}
