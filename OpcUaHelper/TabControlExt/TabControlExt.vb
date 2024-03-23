Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Drawing2D
Imports OpcUaHelper.ControlExt.Common
<ToolboxItem(False)>
Public Class TabControlExt


    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.SetStyle(
                      ControlStyles.UserPaint Or
                      ControlStyles.OptimizedDoubleBuffer Or
                      ControlStyles.AllPaintingInWmPaint Or
                      ControlStyles.ResizeRedraw Or
                      ControlStyles.SupportsTransparentBackColor,
                         True)
        Me.UpdateStyles()
        TextColor = Color.White

    End Sub

    Property OperateType As String
    Dim Path As System.Drawing.Drawing2D.GraphicsPath
    Dim linGrBrush As LinearGradientBrush
    Dim mTextColor As Color = DefaultForeColor
    <DefaultValue(GetType(Color), "255,255,255")>
    Property TextColor As Color
        Get
            Return mTextColor
        End Get
        Set(value As Color)
            mTextColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Overloads Sub Invalidate()
        Try
            If Me.InvokeRequired Then
                 MyBase.Invalidate()
            Else
                MyBase.Invalidate()
            End If
           Catch ex As Exception

        End Try
    End Sub
    Dim mBorderColor As Color = DefaultForeColor
    Property BorderColor As Color
        Get
            Return mBorderColor
        End Get
        Set(value As Color)
            mBorderColor = value
            Me.Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
        Try
            MyBase.OnPaintBackground(pevent)
            Dim BackBrush As New SolidBrush(Me.BorderColor)
            pevent.Graphics.FillRectangle(BackBrush, Me.ClientRectangle)
            BackBrush.Dispose()
        Catch ex As Exception
            MicsonControlExt.MessageBoxExt.Show(Nothing, ex.Message & vbCrLf & ex.StackTrace)
        End Try

    End Sub
    Dim mBackroundColor As Color = SystemColors.Control
    <Browsable(True)>
    <DefaultValue(GetType(Color), "240,240,240")>
    Overloads Property BackroundColor As Color
        Get
            Return mBackroundColor
        End Get
        Set(value As Color)
            mBackroundColor = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Try
            'MyBase.OnPaintBackground(e)

            MyBase.OnPaint(e)

            Dim g As Graphics = e.Graphics
            Dim BackBrush As New SolidBrush(Color.FromArgb(255, BackroundColor))
            g.FillRectangle(BackBrush, Me.ClientRectangle)
            BackBrush.Dispose()
            Dim drawFormat As New StringFormat With {.Alignment = StringAlignment.Center,
                                                    .LineAlignment = StringAlignment.Center}
            Dim pen As New System.Drawing.Pen(penColor, 1)
            Dim blackpen As New System.Drawing.Pen(BorderColor, 1)
            Dim textBrush As New SolidBrush(TextColor)
            Dim tab_rect As Rectangle = Me.GetTabRectangle()
            Dim client_region As Region = Nothing
            Dim tabitem_region As Region = Nothing

            If Me.Alignment = TabAlignment.Top OrElse Me.Alignment = TabAlignment.Bottom Then
                client_region = g.Clip.Clone()
                tabitem_region = New Region(tab_rect)
                g.Clip = tabitem_region
            End If
            For i As Integer = 0 To Me.TabCount - 1

                Dim myTabRect As Rectangle = Me.GetTabRect(i)
                Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}

                myTabRect.Inflate(-1, -1)
                Path = DrawRoundRect(myTabRect, 5)
                If i = Me.SelectedIndex Then
                    g.DrawPath(pen, Path)
                Else
                    g.DrawPath(blackpen, Path)
                End If
                myTabRect.Inflate(-1, -1)
                Path = New System.Drawing.Drawing2D.GraphicsPath()
                Path = DrawRoundRect(myTabRect, 5)
                If i = Me.SelectedIndex Then
                    linGrBrush = New LinearGradientBrush(myTabRect, SelectTitleColor, Color.Black, 45)
                Else
                    linGrBrush = New LinearGradientBrush(myTabRect, NormalTileColor, Color.Black, 45)
                End If
                g.FillPath(linGrBrush, Path)
                linGrBrush.Dispose()
                g.DrawString(Me.TabPages(i).Text, Me.Font, textBrush, myTabRect, drawFormat)
            Next
            If tabitem_region IsNot Nothing Then
                g.Clip = client_region
                tabitem_region.Dispose()
            End If
            pen.Dispose()
            blackpen.Dispose()
            textBrush.Dispose()
        Catch ex As Exception
            MicsonControlExt.MessageBoxExt.Show(Nothing, ex.Message & vbCrLf & ex.StackTrace)
        End Try

    End Sub
    Dim mSelectTitleColor As Color = Color.Blue
    Property SelectTitleColor As Color
        Get
            Return mSelectTitleColor
        End Get
        Set(value As Color)
            mSelectTitleColor = value
            Invalidate()
        End Set
    End Property
    Dim mNormalTileColor As Color = SystemColors.Highlight
    Property NormalTileColor As Color
        Get
            Return mNormalTileColor
        End Get
        Set(value As Color)
            mNormalTileColor = value
            Invalidate()
        End Set
    End Property

    Private ReadOnly preNextBtnWidth As Integer = 40
    Private Function GetTabRectangle() As Rectangle
        Dim tabitem_width As Integer = 0
        For i As Integer = 0 To Me.TabCount - 1
            Dim myTabRect As Rectangle = Me.GetTabRect(i)
            tabitem_width += myTabRect.Width
        Next

        If Multiline = False AndAlso Me.Alignment = TabAlignment.Top OrElse Me.Alignment = TabAlignment.Bottom Then
            If tabitem_width > Me.ClientRectangle.Width Then
                tabitem_width = Me.ClientRectangle.Width - Me.preNextBtnWidth - 4
            End If
        End If
        Dim y As Integer = 0
        If Me.Alignment = TabAlignment.Top Then
            y = Me.ClientRectangle.Y + 2
        ElseIf Me.Alignment = TabAlignment.Bottom Then
            y = Me.ClientRectangle.Bottom - 2 - Me.ItemSize.Height
        End If
        Dim Lines As Single = tabitem_width / Me.ClientRectangle.Width
        If Lines - Fix(Lines) > 0 Then
            Lines = Fix(Lines) + 1
        End If
        Return New Rectangle(Me.ClientRectangle.X + 2, y, If(Multiline, Me.ClientRectangle.Width, tabitem_width), Me.ItemSize.Height * (Lines) + 2)
    End Function
    ReadOnly penColor As Color = Color.Black
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}


        Dim g As Graphics = Me.CreateGraphics

        Dim tab_rect As Rectangle = Me.GetTabRectangle()
        Dim client_region As Region = Nothing
        Dim tabitem_region As Region = Nothing

        If Me.Alignment = TabAlignment.Top OrElse Me.Alignment = TabAlignment.Bottom Then
            client_region = g.Clip.Clone()
            tabitem_region = New Region(tab_rect)
            g.Clip = tabitem_region
        End If
        For i As Integer = 0 To TabPages.Count - 1
            Dim myTabRect As Rectangle = Me.GetTabRect(i)
            myTabRect.Inflate(-1, -1)
            Path = DrawRoundRect(myTabRect, 5)
            If myTabRect.Contains(e.Location) Then
                Dim pen As New System.Drawing.Pen(Color.Blue, 1)
                g.DrawPath(pen, Path)
            Else
                Dim pen As New System.Drawing.Pen(Color.Black, 1)
                g.DrawPath(pen, Path)
            End If
        Next
        If tabitem_region IsNot Nothing Then
            g.Clip = client_region
            tabitem_region.Dispose()
        End If
    End Sub



    Public Function DrawRoundRect(rect As Rectangle, Radius As Integer) As GraphicsPath
        If Me.Alignment = TabAlignment.Top Then
            Return ControlExt.Common.DrawChamferRect(rect, Radius, ControlExt.Common.RoundStyle.Top)
        ElseIf Me.Alignment = TabAlignment.Left Then
            Return ControlExt.Common.DrawChamferRect(rect, Radius, ControlExt.Common.RoundStyle.TopLeft)
        ElseIf Me.Alignment = TabAlignment.Right Then
            Return ControlExt.Common.DrawChamferRect(rect, Radius, ControlExt.Common.RoundStyle.TopRight)
        Else
            Return ControlExt.Common.DrawChamferRect(rect, Radius, ControlExt.Common.RoundStyle.Bottom)
        End If


    End Function


End Class
