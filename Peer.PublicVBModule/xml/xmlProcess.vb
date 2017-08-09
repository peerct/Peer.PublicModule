Imports System.IO
Imports System.Xml
Namespace VBXml
    Public Class xmlProcess

        ''' <summary>
        ''' 转换xml串为dataset
        ''' </summary>
        ''' <param name="xmlStr">xml字符串</param>
        ''' <param name="ds">内存数据集</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConvertXmlToDataset(ByVal xmlStr As String, ByRef ds As DataSet) As Boolean
            Dim StrStream As StringReader = Nothing
            Dim Xmlrdr As XmlTextReader = Nothing
            Try
                If String.IsNullOrEmpty(xmlStr) = False Then
                    '读取字符串中的信息
                    StrStream = New StringReader(xmlStr)
                    '获取StrStream中的数据
                    Xmlrdr = New XmlTextReader(StrStream)
                    'ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr, XmlReadMode.InferSchema)
                    If IsNothing(Xmlrdr) = False Then
                        Xmlrdr.Close()
                        StrStream.Close()
                        StrStream.Dispose()
                    End If
                    Return True
                End If
            Catch ex As Exception
                If IsNothing(Xmlrdr) = False Then
                    Xmlrdr.Close()
                    StrStream.Close()
                    StrStream.Dispose()
                End If
            End Try
            Return False
        End Function

        ''' <summary>
        ''' 读取xml文件到dataset
        ''' </summary>
        ''' <param name="xmlFilePath">xml文件路径</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReadXmlFileToDataSet(ByVal xmlFilePath As String) As DataSet
            Try
                Dim myds As DataSet = New DataSet
                myds.ReadXml(xmlFilePath)
                Return myds
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

    End Class

End Namespace
