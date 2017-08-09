
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization
Namespace VBXml
    Public Class XmlSerialize

        ''' <summary>
        ''' 序列化 对象到字符串
        ''' </summary>
        ''' <typeparam name="T">泛型类型</typeparam>
        ''' <param name="obj">泛型对象</param>
        ''' <returns>序列化后的字符串</returns>
        ''' <remarks></remarks>
        Shared Function SerializeObjToStr(Of T)(ByVal obj As T) As String
            Try
                Dim Serializer As New XmlSerializer(GetType(T))
                Dim ns As New XmlSerializerNamespaces
                Dim stream As New MemoryStream
                Dim writer As New StreamWriter(stream, Encoding.UTF8)
                Serializer.Serialize(writer, obj, ns)
                stream.Position = 0
                Dim buf(stream.Length) As Byte
                stream.Read(buf, 0, buf.Length)
                stream.Close()
                Return Encoding.UTF8.GetString(buf)
            Catch ex As Exception
                Throw New Exception("序列化失败,原因:" & ex.Message)
            End Try
        End Function
        ''' <summary>
        ''' 序列化对象到文件
        ''' </summary>
        ''' <typeparam name="T">泛型类型</typeparam>
        ''' <param name="obj">泛型对象</param>
        ''' <param name="PathName">文件全路径</param>
        ''' <remarks></remarks>
        Shared Sub SerializeObjToFile(Of T)(ByVal obj As T, ByVal PathName As String)
            Try
                Dim Serializer As New XmlSerializer(GetType(T))
                '序列化至文件
                Using fs As New IO.FileStream(PathName, IO.FileMode.Create)
                    Serializer.Serialize(fs, obj)
                End Using
            Catch ex As Exception
                Throw New Exception("序列化失败,原因:" & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 反序列化 字符串到对象
        ''' </summary>
        ''' <typeparam name="T">泛型类型</typeparam>
        ''' <param name="str">要转换为对象的字符串</param>
        ''' <returns>反序列化出来的对象</returns>
        ''' <remarks></remarks>
        Shared Function DesrializeStrToObj(Of T)(ByVal str As String) As T
            Try
                Dim Serializer As New XmlSerializer(GetType(T))
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(str)
                Dim stream As New MemoryStream(buffer)
                Dim obj As T = DirectCast(Serializer.Deserialize(stream), T)
                stream.Close()
                Return obj
            Catch ex As Exception
                Throw New Exception("反序列化失败,原因:" & ex.Message)
            End Try
        End Function
        ''' <summary>
        ''' 反序列化 字符串到对象
        ''' </summary>
        ''' <typeparam name="T">泛型类型</typeparam>
        ''' <param name="pathName">文件路径</param>
        ''' <returns>反序列化出来的对象</returns>
        ''' <remarks></remarks>
        Shared Function DesrializefileToObj(Of T)(ByVal pathName As String) As T
            Try
                Dim Serializer As New XmlSerializer(GetType(T))
                '从文件反序列化
                Using fs As New IO.FileStream(pathName, IO.FileMode.Open)
                    Dim obj As T = DirectCast(Serializer.Deserialize(fs), T)
                    Return obj
                End Using
            Catch ex As Exception
                Throw New Exception("反序列化失败,原因:" & ex.Message)
            End Try
        End Function

    End Class
End Namespace
