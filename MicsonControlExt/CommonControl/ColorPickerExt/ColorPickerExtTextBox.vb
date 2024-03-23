
Imports System.ComponentModel
Imports System.ComponentModel.Design

<ToolboxItem(False)>
<Description("颜色文本输入框")>
Public Class ColorPickerExtTextBox
    Inherits TextBox

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Protected Overloads ReadOnly Property DesignMode As Boolean
        Get

            If Me.GetService(GetType(IDesignerHost)) IsNot Nothing OrElse System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Private ReadOnly ColorePickerExt As ColorPickerExt = Nothing
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Public Sub New(ByVal colorePickerExt As ColorPickerExt)
        Me.ColorePickerExt = colorePickerExt
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If Me.DesignMode Then Return
        If Me.[ReadOnly] Then Return

        Select Case e.KeyCode
            Case Keys.D0, Keys.NumPad0, Keys.D1, Keys.NumPad1, Keys.D2, Keys.NumPad2, Keys.D3, Keys.NumPad3, Keys.D4, Keys.NumPad4, Keys.D5, Keys.NumPad5, Keys.D6, Keys.NumPad6, Keys.D7, Keys.NumPad7, Keys.D8, Keys.NumPad8, Keys.D9, Keys.NumPad9, Keys.Oemcomma, Keys.Left, Keys.Right, Keys.Back, Keys.Control, Keys.ControlKey
                e.SuppressKeyPress = False
                Exit Select
            Case Keys.A, Keys.V

                If e.Control Then
                    e.SuppressKeyPress = False
                Else
                    e.SuppressKeyPress = True
                End If

                Exit Select
            Case Keys.C

                If e.Control Then
                    Me.SelectAll()
                    e.SuppressKeyPress = False
                Else
                    e.SuppressKeyPress = True
                End If

                Exit Select
            Case Else
                e.SuppressKeyPress = True
                Exit Select
        End Select
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_RBUTTONDOWN Then Return
        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        Dim rect As Rectangle = Me.ColorePickerExt.GetColorTextBoxRect()
        x = rect.X
        y = rect.Y
        width = rect.Width
        height = rect.Height
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub
End Class
