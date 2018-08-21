
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
Imports System.Text
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Web.Services.Description
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports System.Reflection
Imports System.CodeDom
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler

Namespace VBReflection
    ''' <summary>
    ''' 此类是通过反射机制加载外部DLL的辅助类;主要完成对与.net类库的动态调用
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ReflectHelper
        '互斥访问控制
        Shared lockobj As Object = New Object
        '一个组织的程序集反射信息(键为【组织编码】)
        Private Shared OrgServiceinfoDic As Dictionary(Of String, AssemblyInfo)
        '配置文件字典信息
        Private Shared ReflectSetdic As Dictionary(Of String, Dictionary(Of String, ReflectSetNode))
        Sub New()
            ReflectSetdic = New Dictionary(Of String, Dictionary(Of String, ReflectSetNode))
            OrgServiceinfoDic = New Dictionary(Of String, AssemblyInfo)
        End Sub
        ''' <summary>
        ''' 获得内存配置结构
        ''' </summary>
        ''' <param name="OrgCode">组织的唯一标识</param>
        ''' <returns>内存配置结构</returns>
        ''' <remarks>不存在时先创建再返回</remarks>
        Public Function GetReflectSetDic(ByVal OrgCode As String) As Dictionary(Of String, ReflectSetNode)
                If ReflectSetdic.ContainsKey(OrgCode) = False Then
                    Dim dicsetIns As New Dictionary(Of String, ReflectSetNode)
                    ReflectSetdic.Add(OrgCode, dicsetIns)
                End If
            Return ReflectSetdic(OrgCode)
        End Function
        ''' <summary>
        ''' 获得组织的程序集信息
        ''' </summary>
        ''' <param name="OrgCode">组织的唯一标识</param>
        ''' <returns>程序集内存结构</returns>
        ''' <remarks>不存在时先创建再返回</remarks>
        Public Function GetOrgService(ByVal OrgCode As String) As AssemblyInfo
            If OrgServiceinfoDic.ContainsKey(OrgCode) = False Then
                Dim AssemblyInfoIns As New AssemblyInfo()
                OrgServiceinfoDic.Add(OrgCode, AssemblyInfoIns)
            End If
            Return OrgServiceinfoDic(OrgCode)
        End Function
        ''' <summary>
        ''' 对xml的输入参数进行处理
        ''' </summary>
        ''' <param name="Paras">参数名和参数值组成的xml串</param>
        ''' <param name="ParaInfo">具体方法的参数信息（包含参数名、参数类型）</param>
        ''' <param name="ParasObj">格式化处理后的参数值</param>
        ''' <remarks></remarks>
        Private Sub ParaAnalytic(ByVal Paras As String, ByVal ParaInfo As ParameterInfo(), ByRef ParasObj As Object())
            Dim dic As New Dictionary(Of String, String)
            Dim str As String = "<?xml version='1.0' encoding='utf-8' ?><Root>" & Paras & "</Root>"
            Dim xmldoc As XmlDocument = New XmlDocument
            xmldoc.LoadXml(str)
            Dim nodelist0 As XmlNodeList = xmldoc.SelectSingleNode("Root").ChildNodes
            Dim i As Integer = -1
            For Each keyvalue As ParameterInfo In ParaInfo
                i = i + 1
                For Each xn0 As XmlNode In nodelist0
                    If xn0.NodeType <> XmlNodeType.Comment Then
                        Dim xe0 As XmlElement = CType(xn0, XmlElement)
                      If xe0.Name = keyvalue.Name Then
                            Select Case keyvalue.ParameterType.Name
                                Case "String"
                                    ParasObj(i) = Convert.ToString(Trim(xe0.InnerText))
                                Case "Integer"
                                    ParasObj(i) = Convert.ToInt32(Trim(xe0.InnerText))
                                Case "Single"
                                    ParasObj(i) = Convert.ToSingle(Trim(xe0.InnerText))
                                Case "Double"
                                    ParasObj(i) = Convert.ToDouble(Trim(xe0.InnerText))
                                Case "Boolean"
                                    ParasObj(i) = Convert.ToBoolean(Trim(xe0.InnerText))
                                Case "DateTime"
                                    ParasObj(i) = Convert.ToDateTime(Trim(xe0.InnerText))
                            End Select
                            Exit For
                        End If
                    End If
                Next
            Next
        End Sub
        ''' <summary>
        ''' 执行DLL中的具体业务逻辑
        ''' </summary>
        ''' <param name="OrgCode">组织唯一标识符</param>
        ''' <param name="CurDllPath">DLL路径</param>
        ''' <param name="CurNameSpace">命名空间名称</param>
        ''' <param name="CurClassName">类名</param>
        ''' <param name="CurMethodName">方法名</param>
        ''' <param name="Paras">传入的参数信息（XML结构）</param>
        ''' <returns>返回object对象</returns>
        ''' <remarks></remarks>
        Public Function ExcuteDll(ByVal OrgCode As String, ByVal CurDllPath As String, ByVal CurNameSpace As String, ByVal CurClassName As String, ByVal CurMethodName As String, ByVal Paras As String) As Object
            Dim Result As Object = Nothing
            Dim AssemblyInfoIns As AssemblyInfo
            Dim key As String
            '互斥临界区
            SyncLock lockobj
                AssemblyInfoIns = GetOrgService(OrgCode)
                key = CurNameSpace + "." + CurClassName
                If AssemblyInfoIns.dic.ContainsKey(key) = False Then
                    Dim filedata As Byte() = File.ReadAllBytes(CurDllPath)
                    AssemblyInfoIns.VBAssembly = Assembly.Load(filedata)
                    Dim Net_ClassInfoIns As New ClassInfo
                    '由命名空间和类名获取到类这个类型
                    Net_ClassInfoIns.VBType = AssemblyInfoIns.VBAssembly.GetType(key)
                    '创建上面类的对象实例
                    Net_ClassInfoIns.VBInstance = Activator.CreateInstance(Net_ClassInfoIns.VBType)
                    '写入此类的内存结构
                    AssemblyInfoIns.dic.Add(key, Net_ClassInfoIns)
                End If
                If AssemblyInfoIns.dic(key).VBMethodInfo.ContainsKey(CurMethodName) = False Then
                    '添加方法名和参数集合
                    AssemblyInfoIns.dic(key).VBMethodInfo.Add(CurMethodName, AssemblyInfoIns.dic(key).VBType.GetMethod(CurMethodName))
                    AssemblyInfoIns.dic(key).VBParasInfo.Add(CurMethodName, AssemblyInfoIns.dic(key).VBMethodInfo(CurMethodName).GetParameters())
                End If
            End SyncLock
            If IsNothing(AssemblyInfoIns.dic(key).VBMethodInfo(CurMethodName)) = False Then
                Try
                    If IsNothing(AssemblyInfoIns.dic(key).VBParasInfo(CurMethodName)) = False AndAlso AssemblyInfoIns.dic(key).VBParasInfo(CurMethodName).Count > 0 Then
                        '处理参数
                        Dim ParasObj As Object() = Nothing
                        If Paras IsNot Nothing Then
                            ReDim ParasObj(AssemblyInfoIns.dic(key).VBParasInfo(CurMethodName).Count - 1)
                            ParaAnalytic(Paras, AssemblyInfoIns.dic(key).VBParasInfo(CurMethodName), ParasObj)
                        End If
                        '执行方法
                        Result = AssemblyInfoIns.dic(key).VBMethodInfo(CurMethodName).Invoke(AssemblyInfoIns.dic(key).VBInstance, ParasObj)
                    Else
                        Result = AssemblyInfoIns.dic(key).VBMethodInfo(CurMethodName).Invoke(AssemblyInfoIns.dic(key).VBInstance, Nothing)
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return Result
        End Function
        ''' <summary>
        ''' 添加DLL中的具体业务逻辑
        ''' </summary>
        ''' <param name="OrgCode">组织唯一标识符</param>
        ''' <param name="CurDllPath">DLL路径</param>
        ''' <param name="CurNameSpace">命名空间名称</param>
        ''' <param name="CurClassName">类名</param>
        ''' <param name="CurMethodName">方法名</param>
        ''' <remarks></remarks>
        Public Sub AddDllToMemory(ByVal OrgCode As String, ByVal CurDllPath As String, ByVal CurNameSpace As String, ByVal CurClassName As String, ByVal CurMethodName As String)
            Dim AssemblyInfoIns As AssemblyInfo = GetOrgService(OrgCode)
            Dim key As String = CurNameSpace + "." + CurClassName
            Try
                If AssemblyInfoIns.dic.ContainsKey(key) = False Then
                    Dim filedata As Byte() = File.ReadAllBytes(CurDllPath)
                    AssemblyInfoIns.VBAssembly = Assembly.Load(filedata)
                    Dim Net_ClassInfoIns As New ClassInfo
                    Net_ClassInfoIns.VBType = AssemblyInfoIns.VBAssembly.GetType(key)
                    '创建上面类的对象实例
                    Net_ClassInfoIns.VBInstance = Activator.CreateInstance(Net_ClassInfoIns.VBType)
                    '写入此类的内存结构
                    AssemblyInfoIns.dic.Add(key, Net_ClassInfoIns)
                End If
                If AssemblyInfoIns.dic(key).VBMethodInfo.ContainsKey(CurMethodName) = False Then
                    '添加方法名和参数集合
                    AssemblyInfoIns.dic(key).VBMethodInfo.Add(CurMethodName, AssemblyInfoIns.dic(key).VBType.GetMethod(CurMethodName))
                    AssemblyInfoIns.dic(key).VBParasInfo.Add(CurMethodName, AssemblyInfoIns.dic(key).VBMethodInfo(CurMethodName).GetParameters())
                End If
            Catch ex As Exception
                Throw ex
            End Try
                
        End Sub

        ''' <summary>
        ''' xml反序列化
        ''' </summary>
        ''' <param name="PathName"></param>
        ''' <remarks></remarks>
        Public Shared Function DeXmlSerialization(ByVal PathName As String) As Dictionary(Of String, AssemblyInfo)
            Dim Serializer As New XmlSerializer(GetType(Dictionary(Of String, AssemblyInfo)))
            '从文件反序列化
            Using fs As New IO.FileStream(PathName, IO.FileMode.Open)
                Dim dic As Dictionary(Of String, AssemblyInfo) = DirectCast(Serializer.Deserialize(fs), Dictionary(Of String, AssemblyInfo))
                Return dic
            End Using
        End Function
        ''' <summary>
        ''' xml序列化当前内存结构
        ''' </summary>
        ''' <param name="PathName"></param>
        ''' <remarks></remarks>
        Public Shared Sub XmlSerialization(ByVal PathName As String, ByVal dic As Dictionary(Of String, AssemblyInfo))
            Dim Serializer As New XmlSerializer(GetType(Dictionary(Of String, AssemblyInfo)))
            '序列化至文件
            Using fs As New IO.FileStream(PathName, IO.FileMode.Create)
                Serializer.Serialize(fs, dic)
            End Using
        End Sub
        '类的内存销毁
        Public Sub MemoryClear()
            ReflectSetdic.Clear()
            OrgServiceinfoDic.Clear()
        End Sub
       
    End Class

    '程序集内存结构
    Public Class AssemblyInfo
        Sub New()
            dic = New Dictionary(Of String, ClassInfo)
        End Sub
        '存放一个具体类的信息(键为【命名空间.类名】 必须保证唯一)
        Public dic As Dictionary(Of String, ClassInfo)
        '此变量只是一个临时性质的变量；可以把它看作临时变量；真正的每个DLL的信息都存放在dic中
        Public VBAssembly As Assembly
    End Class
    '类的内存结构
    Public Class ClassInfo
        Sub New()
            VBMethodInfo = New Dictionary(Of String, MethodInfo)
            VBParasInfo = New Dictionary(Of String, ParameterInfo())
        End Sub
        '存放一个具体方法的参数信息（键为【方法名】）
        Public VBParasInfo As Dictionary(Of String, ParameterInfo())
        '存放一个具体方法信息（键为【方法名】）
        Public VBMethodInfo As Dictionary(Of String, MethodInfo)
        Public VBInstance As Object
        Public VBType As Type
    End Class

   
End Namespace

