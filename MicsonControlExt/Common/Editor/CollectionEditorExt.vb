Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Reflection

Public Class CollectionEditorExt
    Inherits CollectionEditor

    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub
    Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function
    Protected Overrides Function CreateCollectionForm() As CollectionForm
        Dim frm As CollectionForm = MyBase.CreateCollectionForm()
        Dim fileinfo As FieldInfo = frm.[GetType]().GetField("propertyBrowser", BindingFlags.NonPublic Or BindingFlags.Instance)

        If fileinfo IsNot Nothing Then
            Dim PropertyGrid As PropertyGrid = CType(fileinfo.GetValue(frm), System.Windows.Forms.PropertyGrid)
            PropertyGrid.HelpVisible = True
            PropertyGrid.PropertySort = PropertySort.Alphabetical
        End If
        frm.Width = 700
        frm.Height = 600
        Return frm
    End Function

End Class
