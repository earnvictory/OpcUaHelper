Imports Opc.Ua
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks


    Public Class OpcUaStatusEventArgs
        Public Property [Error] As Boolean
        Public Property Time As DateTime
        Public Property Text As String

        Public Overrides Function ToString() As String
            Return If([Error], "[Err]", "[OK]" & Time.ToString("  yyyy-MM-dd HH:mm:ss  ") & Text)
        End Function
    End Class

    Public Class OpcNodeAttribute
        Public Property Name As String
        Public Property Type As String
        Public Property StatusCode As StatusCode
        Public Property Value As Object
    End Class

