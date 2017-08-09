Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class LogContent
    Public Sub New(ByVal tmessage As String, Optional ByVal tplat_code As String = "", Optional ByVal tinterface_code As String = "", Optional ByVal tinterface_name As String = "", Optional ByVal tKeyInfo As String = "", Optional ByVal tfrom_user As String = "", Optional ByVal tto_user As String = "")
        MessageInfo = tmessage
        KeyInfo = tKeyInfo
        InterfaceCode = tinterface_code
        InterfaceName = tinterface_name
        Plat_Code = tplat_code
        From_User = tfrom_user
        To_User = tto_user
    End Sub
    '日志消息完整内容
    Dim cMessage As String
    Public Property MessageInfo() As String
        Get
            Return cMessage
        End Get
        Set(ByVal value As String)
            cMessage = value
        End Set
    End Property
    '日志消息关键字
    Dim cKeyInfo As String
    Public Property KeyInfo() As String
        Get
            Return cKeyInfo
        End Get
        Set(ByVal value As String)
            cKeyInfo = value
        End Set
    End Property
    '平台模块编码
    Dim cInterfaceCode As String
    Public Property InterfaceCode() As String
        Get
            Return cInterfaceCode
        End Get
        Set(ByVal value As String)
            cInterfaceCode = value
        End Set
    End Property
    '平台模块名称
    Dim cInterfaceName As String
    Public Property InterfaceName() As String
        Get
            Return cInterfaceName
        End Get
        Set(ByVal value As String)
            cInterfaceName = value
        End Set
    End Property
    '平台编码
    Dim cplat_code As String
    Public Property Plat_Code() As String
        Get
            Return cplat_code
        End Get
        Set(ByVal value As String)
            cplat_code = value
        End Set
    End Property

    '日志来源
    Dim cfrom_user As String
    Public Property From_User() As String
        Get
            Return cfrom_user
        End Get
        Set(ByVal value As String)
            cfrom_user = value
        End Set
    End Property
    '日志目的地
    Dim cto_user As String
    Public Property To_User() As String
        Get
            Return cto_user
        End Get
        Set(ByVal value As String)
            cto_user = value
        End Set
    End Property
End Class
