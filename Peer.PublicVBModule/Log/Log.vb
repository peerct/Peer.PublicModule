Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Diagnostics
Namespace VBLogToFile
    '简单文本日志类
    Public Class LogManager
        Private Shared logPathB As String = String.Empty
        '保存日志的文件夹
        Public Shared Property LogPath As String
            Get
                If logPathB = String.Empty Then

                    If IsNothing(System.Web.HttpContext.Current) = True Then
                        ' Windows Forms 应用
                        logPathB = AppDomain.CurrentDomain.BaseDirectory
                    Else
                        ' Web 应用
                        logPathB = AppDomain.CurrentDomain.BaseDirectory + "bin\"
                    End If
                End If
                Return logPathB
            End Get
            Set(value As String)
                logPathB = value
            End Set
        End Property
        Private Shared logFielPrefixB As String = String.Empty
        '日志文件前缀
        Public Shared Property LogFielPrefix As String
            Get
                Return logFielPrefixB
            End Get
            Set(value As String)
                logFielPrefixB = value
            End Set
        End Property
        '写日志
        Public Shared Sub WriteLog(ByVal logFile As String, ByVal msg As String)
            Try
                Dim sw As System.IO.StreamWriter = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + " " +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    )
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg)
                sw.Close()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub
        Public Shared Sub WriteLog(ByVal logFile As LogType, ByVal msg As String)
            WriteLog(logFile.ToString(), msg)
        End Sub
        ''' <summary>
        ''' 在异常处理中返回具体位置（使用方法：dim trackRecord as string= LogManager.getLogInfo(new StackFrame(true))）
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
    Public Enum LogType
        Trace
        Warning
        ErrorInfo
        SQL
    End Enum
End Namespace
