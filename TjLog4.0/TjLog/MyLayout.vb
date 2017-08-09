Imports System
Imports System.Collections.Generic
Imports System.Text
Imports log4net.Layout.Pattern
Imports log4net.Layout
Imports log4net.Core
Imports System.Reflection
Public Class MyLayout
    Inherits PatternLayout
    Public Sub New()
        Me.AddConverter("property", GetType(TjLog.MyMessagePatternConverter))
    End Sub
End Class
