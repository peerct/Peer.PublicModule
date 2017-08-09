Imports System
Namespace VBMessage
    Public Class msgTemplate
        ''' <summary>
        ''' 平台编码（用于反射调用时查找的目录名）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Plat_Code As String
        ''' <summary>
        ''' 消息类别关键词（用于反射调用时查找DLL时的主键）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MsgType As String
        ''' <summary>
        ''' 消息发送目的地
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToUserName As String
        ''' <summary>
        ''' 消息来源
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromUserName As String
        ''' <summary>
        ''' 消息创建时间
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CreateTime As Long
        ''' <summary>
        ''' 消息内容(一般是把内容定义为 Dim lst As List(Of Object) = New List(Of Object)；在接收后解析时在进行object到list(of object)的转换）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property msgContent As Object
        ''' <summary>
        ''' 消息guid
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MsgId As String
        ''' <summary>
        ''' 构造函数默认赋值消息id
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            MsgId = Guid.NewGuid().ToString()
        End Sub
    End Class

End Namespace
