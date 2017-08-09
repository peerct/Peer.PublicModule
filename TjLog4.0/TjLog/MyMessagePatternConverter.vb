Imports System
Imports System.Collections.Generic
Imports System.Text
Imports log4net.Layout.Pattern
Imports log4net.Layout
Imports log4net.Core
Imports System.Reflection

Public Class MyMessagePatternConverter
    Inherits PatternLayoutConverter

    Protected Overloads Overrides Sub Convert(ByVal writer As System.IO.TextWriter, ByVal loggingEvent As log4net.Core.LoggingEvent)
        If IsNothing(Me.Option) = False Then
            WriteObject(writer, loggingEvent.Repository, LookupProperty(Me.Option, loggingEvent))
        Else
            WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties())
        End If
    End Sub
    Private Function LookupProperty(ByVal Cproperty As String, ByVal loggingEvent As log4net.Core.LoggingEvent) As Object

        Dim CpropertyValue As Object = String.Empty
        Dim CPropertyInfo As PropertyInfo = loggingEvent.MessageObject.GetType().GetProperty(Cproperty)
        If IsNothing(CPropertyInfo) = False Then
            CpropertyValue = CPropertyInfo.GetValue(loggingEvent.MessageObject, Nothing)
        End If
        Return CpropertyValue
    End Function
End Class
