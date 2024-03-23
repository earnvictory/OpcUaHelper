Imports System.ComponentModel

<Description("展开属性选型去除描述")>
Public Class EmptyConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
        If destinationType = GetType(String) Then Return CObj(String.Empty)
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class
