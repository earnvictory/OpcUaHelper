Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Windows.Forms.Design


<Description("绘制虚线边框")>
    Public Class DottedLineBorderExtDesigner
        Inherits ControlDesigner

        Protected Overrides Sub OnPaintAdornments(ByVal pe As PaintEventArgs)
            MyBase.OnPaintAdornments(pe)
            Me.DrawBorder(pe.Graphics)
        End Sub

    Private Sub DrawBorder(ByVal graphics As Graphics)
        Dim control As Control = Me.Control
        Dim clientRectangle As Rectangle = control.ClientRectangle
        Dim pen As New Pen(If(CDbl(control.BackColor.GetBrightness()) >= 0.5, ControlPaint.Dark(control.BackColor), ControlPaint.Light(control.BackColor))) With {.DashStyle = DashStyle.Dash}

        clientRectangle.Width -= 1
        clientRectangle.Height -= 1
        graphics.DrawRectangle(pen, clientRectangle)
        pen.Dispose()
    End Sub
    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            Return SelectionRules.Moveable Or SelectionRules.BottomSizeable Or SelectionRules.TopSizeable
        End Get
    End Property
End Class

