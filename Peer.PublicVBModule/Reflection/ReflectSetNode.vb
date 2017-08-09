Namespace VBReflection
    ''' <summary>
    ''' 反射类配置信息类
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Class ReflectSetNode
        '关键字
        Public Property CurKeyinfo As String
        '组织编码
        Public Property CurOrgCode As String
        '命名空间
        Public Property CurNameSpace As String
        '类名
        Public Property CurClassName As String
        'dll路径名
        Public Property CurDllPath As String
        '方法名
        Public Property CurMethodName As String
    End Class
End Namespace

