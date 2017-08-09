Imports log4net
Imports MySql.Data
Imports System.Text
Imports System.Diagnostics
Public Class Log
    '数据库日志对象
    Private Shared DBLog As log4net.ILog = log4net.LogManager.GetLogger("DBLog.Logging")
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">日志消息完整信息</param>
    ''' <param name="plat_code">平台编码</param>
    ''' <param name="interface_code">平台模块编码</param>
    ''' <param name="interface_name">平台模块名称</param>
    ''' <param name="KeyInfo">日志消息关键词</param>
    ''' <param name="from_user">日志消息来源标识</param>
    ''' <param name="to_user">日志消息目标标识</param>
    ''' <remarks></remarks>
    Public Shared Sub InfoLog(ByVal message As String, Optional ByVal plat_code As String = "", Optional ByVal interface_code As String = "", Optional ByVal interface_name As String = "", Optional ByVal KeyInfo As String = "", Optional ByVal from_user As String = "", Optional ByVal to_user As String = "")
        If DBLog.IsInfoEnabled Then
            DBLog.Info(New TjLog.LogContent(message, plat_code, interface_code, interface_name, KeyInfo, from_user, to_user))
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">日志消息完整信息</param>
    ''' <param name="plat_code">平台编码</param>
    ''' <param name="interface_code">平台模块编码</param>
    ''' <param name="interface_name">平台模块名称</param>
    ''' <param name="KeyInfo">日志消息关键词</param>
    ''' <param name="from_user">日志消息来源标识</param>
    ''' <param name="to_user">日志消息目标标识</param>
    ''' <remarks></remarks>
    Public Shared Sub ErrorLog(ByVal message As String, Optional ByVal plat_code As String = "", Optional ByVal interface_code As String = "", Optional ByVal interface_name As String = "", Optional ByVal KeyInfo As String = "", Optional ByVal from_user As String = "", Optional ByVal to_user As String = "")
        If DBLog.IsErrorEnabled Then
            DBLog.Error(New TjLog.LogContent(message, plat_code, interface_code, interface_name, KeyInfo, from_user, to_user))
        End If
    End Sub
 ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">日志消息完整信息</param>
    ''' <param name="plat_code">平台编码</param>
    ''' <param name="interface_code">平台模块编码</param>
    ''' <param name="interface_name">平台模块名称</param>
    ''' <param name="KeyInfo">日志消息关键词</param>
    ''' <param name="from_user">日志消息来源标识</param>
    ''' <param name="to_user">日志消息目标标识</param>
    ''' <remarks></remarks>
    Public Shared Sub WarnLog(ByVal message As String, Optional ByVal plat_code As String = "", Optional ByVal interface_code As String = "", Optional ByVal interface_name As String = "", Optional ByVal KeyInfo As String = "", Optional ByVal from_user As String = "", Optional ByVal to_user As String = "")
        If DBLog.IsWarnEnabled Then
            DBLog.Warn(New TjLog.LogContent(message, plat_code, interface_code, interface_name, KeyInfo, from_user, to_user))
        End If
    End Sub
     ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">日志消息完整信息</param>
    ''' <param name="plat_code">平台编码</param>
    ''' <param name="interface_code">平台模块编码</param>
    ''' <param name="interface_name">平台模块名称</param>
    ''' <param name="KeyInfo">日志消息关键词</param>
    ''' <param name="from_user">日志消息来源标识</param>
    ''' <param name="to_user">日志消息目标标识</param>
    ''' <remarks></remarks>
    Public Shared Sub FatalLog(ByVal message As String, Optional ByVal plat_code As String = "", Optional ByVal interface_code As String = "", Optional ByVal interface_name As String = "", Optional ByVal KeyInfo As String = "", Optional ByVal from_user As String = "", Optional ByVal to_user As String = "")
        If DBLog.IsFatalEnabled Then
            DBLog.Fatal(New TjLog.LogContent(message, plat_code, interface_code, interface_name, KeyInfo, from_user, to_user))
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="message">日志消息完整信息</param>
    ''' <param name="plat_code">平台编码</param>
    ''' <param name="interface_code">平台模块编码</param>
    ''' <param name="interface_name">平台模块名称</param>
    ''' <param name="KeyInfo">日志消息关键词</param>
    ''' <param name="from_user">日志消息来源标识</param>
    ''' <param name="to_user">日志消息目标标识</param>
    ''' <remarks></remarks>
    Public Shared Sub DebugLog(ByVal message As String, Optional ByVal plat_code As String = "", Optional ByVal interface_code As String = "", Optional ByVal interface_name As String = "", Optional ByVal KeyInfo As String = "", Optional ByVal from_user As String = "", Optional ByVal to_user As String = "")
        If DBLog.IsDebugEnabled Then
            DBLog.Debug(New TjLog.LogContent(message, plat_code, interface_code, interface_name, KeyInfo, from_user, to_user))
        End If
    End Sub

    '文件日志对象
    Public Shared FileLog As log4net.ILog = log4net.LogManager.GetLogger("FileLog.Logging")

    ''' <summary>
    ''' 在异常处理中返回具体位置（使用方法：dim trackRecord as string= Log.getLogInfo(new StackFrame(true))）
    ''' </summary>
    ''' <param name="stackFrameins"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getLogInfo(ByVal stackFrameins As StackFrame) As String

        Dim st As StackTrace = New StackTrace(stackFrameins)
        Dim sf As StackFrame = st.GetFrame(0)
        Dim logInfo As StringBuilder = New StringBuilder()
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
        Return logInfo.ToString()
    End Function
End Class
