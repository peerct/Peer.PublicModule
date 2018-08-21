'首先选取你要注释的文本块，然后Ctrl-K Ctrl-C 这样对你选择的文本块完成注释．
'而如果你要取消已完成注释的文本块，则Ctrl-K Ctrl-U即可。

'/*   
''                   _ooOoo_
''                  o8888888o
''                  88" . "88
''                  (| -_- |)
''                  O\  =  /O
''               ____/`---'\____
''             .'  \\|     |//  `.
''            /  \\|||  :  |||//  \
''           /  _||||| -:- |||||-  \
''           |   | \\\  -  /// |   |
''           | \_|  ''\---/''  |   |
''           \  .-\__  `-`  ___/-. /
''         ___`. .'  /--.--\  `. . __
''      ."" '<  `.___\_<|>_/___.'  >'"".
''     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
''     \  \ `-.   \_ __\ /__ _/   .-` /  /
''======`-.____`-.___\_____/___.-`____.-'======
''                   `=---='
''^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
''         佛祖保佑       永无BUG
''==============================================================================
''文件
''名称: 
''功能: 
''作者: peer
''日期: 
''修改:
''日期:
''备注:
''==============================================================================
'*/

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Diagnostics
Namespace VBLogToFile
    '简单文本日志类
    Public Class LogManager

        '直接写日志信息
        Public Shared Sub WriteLog(ByVal Msg As String, ByVal MsgType As LogType)
            Dim varAppPath As String = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log"
            If System.IO.Directory.Exists(varAppPath) = False Then
                System.IO.Directory.CreateDirectory(varAppPath)
            End If
            Dim pathStr = System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
            pathStr = System.IO.Path.Combine(pathStr, "syslog")
            pathStr = System.IO.Path.Combine(pathStr, DateTime.Now().Year)
            pathStr = System.IO.Path.Combine(pathStr, DateTime.Now().Month)
            If System.IO.Directory.Exists(pathStr) = False Then
                System.IO.Directory.CreateDirectory(pathStr)
            End If
            Dim strFile As String = System.IO.Path.Combine(pathStr, String.Format("{0}.txt", System.DateTime.Now.ToString("yyyyMMdd")))

            Dim MsgInfo As New StringBuilder
            MsgInfo.AppendFormat("{0} 日志类型:{1}", DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss fff"), MsgType.ToString())
            MsgInfo.AppendLine()
            MsgInfo.AppendLine(Msg)
            MsgInfo.Append("---------------------------------------------------------")

            Dim SW As System.IO.StreamWriter = New System.IO.StreamWriter(strFile, True)
            SW.WriteLine(MsgInfo.ToString())
            MsgInfo.Clear()
            MsgInfo = Nothing
            SW.Flush()
            SW.Close()
        End Sub

        ''' <summary>
        ''' 在异常处理中返回具体位置（使用方法：dim trackRecord as string= LogManager.getLogInfo(new StackFrame(true))
        ''' </summary>
        ''' <param name="stackFrameins"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getLogInfo(ByVal stackFrameins As StackFrame) As String
            Dim st As StackTrace = New StackTrace(stackFrameins)
            Dim sf As StackFrame = st.GetFrame(0)
            Dim logInfo As StringBuilder = New StringBuilder()
            logInfo.AppendLine()
            '得到文件名
            logInfo.Append(sf.GetFileName())
            logInfo.Append("  ")
            ' 得到方法名
            logInfo.Append(sf.GetMethod().Name)
            logInfo.Append("  ")
            '得到行号
            logInfo.Append(sf.GetFileLineNumber())
            logInfo.Append("  ")
            ' 得到列号
            logInfo.Append(sf.GetFileColumnNumber())
            Dim Info As String = logInfo.ToString()
            logInfo.Clear()
            logInfo = Nothing
            Return Info
        End Function

    End Class
    Public Enum LogType
        Trace
        Wrong
        Info
    End Enum
End Namespace
