Imports System.ComponentModel
Imports System.Drawing.Drawing2D

<ToolboxItem(False)>
Public Class ToolStripProfessionalRendererExt
    Inherits ToolStripProfessionalRenderer
    Private ReadOnly _color As Color = Color.White

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal color As Color)
        MyBase.New()
        _color = color
    End Sub

    Public Shared Function GetRoundedRectPath(ByVal rect As Rectangle, ByVal radius As Integer) As GraphicsPath
        Dim diameter As Integer = radius
        Dim arcRect As New Rectangle(rect.Location, New Size(diameter, diameter))
        Dim path As New GraphicsPath()
        path.AddArc(arcRect, 180, 90)
        arcRect.X = rect.Right - diameter
        path.AddArc(arcRect, 270, 90)
        arcRect.Y = rect.Bottom - diameter
        path.AddArc(arcRect, 0, 90)
        arcRect.X = rect.Left
        path.AddArc(arcRect, 90, 90)
        path.CloseFigure()
        Return path
    End Function
    Public Shared Sub SafelyDrawLinearGradient(ByVal rectangle As Rectangle, ByVal startColor As Color, ByVal endColor As Color, ByVal mode As LinearGradientMode, ByVal graphics As Graphics)
        If rectangle.Width > 0 AndAlso rectangle.Height > 0 Then

            Using brush As LinearGradientBrush = New LinearGradientBrush(rectangle, startColor, endColor, mode)
                graphics.FillRectangle(brush, rectangle)
            End Using
        End If
    End Sub
    '渲染背景 包括menustrip背景 toolstripDropDown背景

    Protected Overrides Sub OnRenderToolStripBackground(ByVal e As ToolStripRenderEventArgs)
        Dim toolStrip As ToolStrip = e.ToolStrip
        Dim g As Graphics = ControlExt.Common.Graphics_SetSmoothHighQuality(e.Graphics)

        Dim bounds As Rectangle = e.AffectedBounds
        Dim lgbrush As New LinearGradientBrush(New Point(0, 0), New Point(0, toolStrip.Height), Color.FromArgb(255, _color), Color.FromArgb(150, _color))

        If TypeOf toolStrip Is MenuStrip OrElse TypeOf toolStrip Is ToolStrip OrElse TypeOf toolStrip Is ToolStripDropDown Then

            If TypeOf toolStrip Is ToolStripDropDown Then
                Dim diameter As Integer = 10
                Dim path As New GraphicsPath()
                Dim rect As New Rectangle(Point.Empty, toolStrip.Size)
                Dim arcRect As New Rectangle(rect.Location, New Size(diameter, diameter))
                path.AddLine(0, 0, 10, 0)
                arcRect.X = rect.Right - diameter
                path.AddArc(arcRect, 270, 90)
                arcRect.Y = rect.Bottom - diameter
                path.AddArc(arcRect, 0, 90)
                arcRect.X = rect.Left
                path.AddArc(arcRect, 90, 90)
                path.CloseFigure()
                toolStrip.Region = New Region(path)
                g.FillPath(lgbrush, path)
            Else
                Dim mode As LinearGradientMode = If(toolStrip.Orientation = Orientation.Horizontal, LinearGradientMode.Vertical, LinearGradientMode.Horizontal)
                lgbrush = New LinearGradientBrush(toolStrip.ClientRectangle, Color.FromArgb(255, _color), Color.FromArgb(150, _color), mode)
                g.FillRectangle(lgbrush, Rectangle.Inflate(toolStrip.ClientRectangle, -1, -1))
            End If
        Else
            MyBase.OnRenderToolStripBackground(e)
        End If

        For Each item As Object In e.ToolStrip.Items
            If TypeOf (item) Is ToolStripDropDownItem AndAlso TypeOf (item) Is ToolStripMenuItemExt Then
                CType(item, ToolStripDropDownItem).Visible = CType(item, ToolStripMenuItemExt).Visible
                CType(item, ToolStripDropDownItem).Enabled = CType(item, ToolStripMenuItemExt).Enabled
            ElseIf TypeOf (item) Is ToolStripDropDownButtonExt Then
                CType(item, ToolStripDropDownItem).Visible = CType(item, ToolStripDropDownButtonExt).Visible
                CType(item, ToolStripDropDownItem).Enabled = CType(item, ToolStripDropDownButtonExt).Enabled
            End If
        Next



    End Sub

    Protected Overrides Sub OnRenderToolStripBorder(ByVal e As ToolStripRenderEventArgs)
        MyBase.OnRenderToolStripBorder(e)
        ControlPaint.DrawFocusRectangle(e.Graphics, e.AffectedBounds, SystemColors.ControlDarkDark, SystemColors.ControlDarkDark)
    End Sub

    Protected Overrides Sub OnRenderArrow(ByVal e As ToolStripArrowRenderEventArgs)
        e.ArrowColor = Color.Blue
        MyBase.OnRenderArrow(e)
    End Sub
    '渲染项 不调用基类同名方法
    Protected Overrides Sub OnRenderMenuItemBackground(ByVal e As ToolStripItemRenderEventArgs)
        Dim g As Graphics = ControlExt.Common.Graphics_SetSmoothHighQuality(e.Graphics)
        Dim item As ToolStripItem = e.Item
        Dim toolstrip As ToolStrip = e.ToolStrip

        If TypeOf toolstrip Is MenuStrip OrElse TypeOf toolstrip Is ToolStrip OrElse TypeOf toolstrip Is ToolStripDropDown Then

            If TypeOf toolstrip Is ToolStripDropDown Then

                Dim lgbrush As New LinearGradientBrush(New Point(0, 0), New Point(item.Width, 0), Color.FromArgb(255, _color), Color.FromArgb(255, Color.White))

                If item.Selected Then
                    Dim gp As GraphicsPath = GetRoundedRectPath(New Rectangle(0, 0, item.Width, item.Height), 10)
                    g.FillPath(lgbrush, gp)
                End If
            Else
                Dim lgbrush As New LinearGradientBrush(New Point(0, 0), New Point(0, item.Height), Color.FromArgb(255, _color), Color.FromArgb(0, Color.White))
                Dim brush As New SolidBrush(Color.FromArgb(255, Color.White))
                Dim ItemRec As New Rectangle(New Point(0, 0), item.Size)
                If e.Item.Selected Then
                    Dim gp As GraphicsPath = GetRoundedRectPath(ItemRec, 5)
                    g.FillPath(lgbrush, gp)
                End If

                If item.Pressed Then
                    g.FillRectangle(Brushes.White, New Rectangle(Point.Empty, item.Size))
                End If
            End If
        Else
            MyBase.OnRenderMenuItemBackground(e)
        End If

    End Sub
    Dim mSeparatorColor As Color = SystemColors.ControlDarkDark
    Property SeparatorColor As Color
        Get
            Return mSeparatorColor
        End Get
        Set(value As Color)
            mSeparatorColor = value
        End Set
    End Property
    Protected Overrides Sub OnRenderSeparator(ByVal e As ToolStripSeparatorRenderEventArgs)
        Dim g As Graphics = e.Graphics

        If e.Vertical Then
            Dim lgbrush As New SolidBrush(Color.White) 'SystemColors.ControlDarkDark
            g.FillRectangle(lgbrush, New Rectangle(e.Item.Width / 2, 4, 1, e.Item.Height - 8))
        Else
            Dim lgbrush As New LinearGradientBrush(New Point(0, 0), New Point(e.Item.Width, 0), SeparatorColor, Color.FromArgb(255, _color))
            g.FillRectangle(lgbrush, New Rectangle(3, e.Item.Height / 2, e.Item.Width, 1))
        End If
    End Sub

    Protected Overrides Sub OnRenderImageMargin(ByVal e As ToolStripRenderEventArgs)
    End Sub

    Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)

        MyBase.OnRenderItemText(e)


    End Sub

    Private Sub ToolStripProfessionalRendererExt_RenderToolStripBorder(sender As Object, e As ToolStripRenderEventArgs) Handles Me.RenderToolStripBorder

    End Sub
End Class
