
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

<Description("选取颜色(可选透明度)")>
<System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
Public Class ColorEditorExt
    Inherits System.Drawing.Design.UITypeEditor


    Public Sub New()
    End Sub
    Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.DropDown
    End Function



    Dim edSvc As IWindowsFormsEditorService
    Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object

        edSvc = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
        If (edSvc IsNot Nothing) Then
            Using ColorEditControl As New ColorPickerExt
                AddHandler ColorEditControl.BottomBarConfirmClick, AddressOf CloseDropDown
                ColorEditControl.ColorValue = value
                edSvc.DropDownControl(ColorEditControl)

                Dim ReturnColor As Object = ColorEditControl.ColorValue
                Return ReturnColor 'ColorEditControl.ColorValue
            End Using
        End If
        Return value
    End Function
    Private Sub CloseDropDown()

        edSvc?.CloseDropDown()

    End Sub


    Public Overloads Overrides Sub PaintValue(ByVal e As System.Drawing.Design.PaintValueEventArgs)
        If Not (TypeOf e.Value Is Color) Then Return
        Dim color As Color = CType(e.Value, Color)
        Dim pen As  New Pen(Color.Gray)
        Dim solidBrush1 As New SolidBrush(If((color = Color.Empty OrElse color = Color.Transparent), Color.Empty, Color.FromArgb(color.R, color.G, color.B)))
        Dim solidBrush2 As New SolidBrush(color)
        e.Graphics.FillRectangle(solidBrush1, New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width / 2, e.Bounds.Height))
        If Not (color = Color.Empty OrElse color = Color.Transparent) Then e.Graphics.DrawLine(pen, CSng(e.Bounds.Width / 2 + 1), CSng(e.Bounds.Y), CSng(e.Bounds.Width / 2 + 1), CSng(e.Bounds.Bottom - 2))
        e.Graphics.FillRectangle(solidBrush2, New Rectangle(e.Bounds.Width / 2, e.Bounds.Y, e.Bounds.Width / 2, e.Bounds.Height))
        solidBrush1.Dispose()
        solidBrush2.Dispose()
        pen.Dispose()
    End Sub

    Public Overloads Overrides Function GetPaintValueSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function


End Class

