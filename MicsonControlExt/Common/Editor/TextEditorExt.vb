Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Public Class TextEditorExt
    Inherits System.Drawing.Design.UITypeEditor
    Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object

        Dim edSvc As IWindowsFormsEditorService = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
        If (edSvc IsNot Nothing) Then
            Dim InputTextBox As New TextBox With {.Multiline = True, .Width = 400, .Height = 200}
            InputTextBox.Text = value
            edSvc.DropDownControl(InputTextBox)
            Return InputTextBox.Text
        End If
        Return value
    End Function


End Class
