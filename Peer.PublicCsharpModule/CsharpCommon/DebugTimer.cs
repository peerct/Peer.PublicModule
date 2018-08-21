using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Peer.PublicCsharpModule.CsharpCommon
{
   public class DebugTimer
    {
       Stopwatch watchIns = null;
       public string DebugStart()
       {
           watchIns = new Stopwatch();
           watchIns.Start();
           return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
       }
       public string DebugStop()
       {
           string timeStr = "";
           if (watchIns != null && watchIns.IsRunning)
           {
               watchIns.Stop();
               //获取当前实例测量得出的总运行时间（以毫秒为单位）
               timeStr = watchIns.ElapsedMilliseconds.ToString();
               watchIns.Reset();
               watchIns = null;
           }
           return timeStr;
       }

    }
}
