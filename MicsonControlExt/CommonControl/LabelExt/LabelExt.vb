
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D

<ToolboxItem(False)>
<Description("LabelExt控件")>
Public Class LabelExt
    Inherits ControlExt.Control

    Property BorderStyle As System.Windows.Forms.BorderStyle

    Private mscrollThickness As Integer = 10

    <DefaultValue(10)>
    <Description("滚动条厚度")>
    Public Property ScrollThickness As Integer
        Get
            Return Me.mscrollThickness
        End Get
        Set(ByVal value As Integer)
            If Me.mscrollThickness = value OrElse value < 0 Then Return
            Me.mscrollThickness = value
            Me.InitializeRectangle()
            Me.Invalidate()
        End Set
    End Property

    Private mscrollNormalBackColor As Color = Color.FromArgb(68, 128, 128, 128)

    <DefaultValue(GetType(Color), "68, 128, 128, 128")>
    <Description("滑条背景颜色（正常）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollNormalBackColor As Color
        Get
            Return Me.mscrollNormalBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollNormalBackColor = value Then Return
            Me.mscrollNormalBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollSlideThickness As Integer = 10

    <DefaultValue(10)>
    <Description("滑块条厚度")>
    Public Property ScrollSlideThickness As Integer
        Get
            Return Me.mscrollSlideThickness
        End Get
        Set(ByVal value As Integer)
            If Me.mscrollSlideThickness = value OrElse value < 0 Then Return
            Me.mscrollSlideThickness = value
            Me.InitializeRectangle()
            Me.Invalidate()
        End Set
    End Property

    Private mscrollSlideNormalBackColor As Color = Color.FromArgb(120, 64, 64, 64)

    <DefaultValue(GetType(Color), "120, 64, 64, 64")>
    <Description("滑块背景颜色（正常）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollSlideNormalBackColor As Color
        Get
            Return Me.mscrollSlideNormalBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollSlideNormalBackColor = value Then Return
            Me.mscrollSlideNormalBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollSlideEnterBackColor As Color = Color.FromArgb(160, 64, 64, 64)

    <DefaultValue(GetType(Color), "160, 64, 64, 64")>
    <Description("滑块背景颜色（鼠标进入）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollSlideEnterBackColor As Color
        Get
            Return Me.mscrollSlideEnterBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollSlideEnterBackColor = value Then Return
            Me.mscrollSlideEnterBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnShow As Boolean = False

    <DefaultValue(False)>
    <Description("是否显示按钮")>
    Public Property ScrollBtnShow As Boolean
        Get
            Return Me.mscrollBtnShow
        End Get
        Set(ByVal value As Boolean)
            If Me.mscrollBtnShow = value Then Return
            Me.mscrollBtnShow = value
            Me.InitializeRectangle()
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnHeight As Integer = 10

    <DefaultValue(10)>
    <Description("/// 按钮高度")>
    Public Property ScrollBtnHeight As Integer
        Get
            Return Me.mscrollBtnHeight
        End Get
        Set(ByVal value As Integer)
            If Me.mscrollBtnHeight = value OrElse value < 0 Then Return
            Me.mscrollBtnHeight = value
            Me.InitializeRectangle()
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnNormalBackColor As Color = Color.FromArgb(128, 128, 128)

    <DefaultValue(GetType(Color), "128, 128, 128")>
    <Description("按钮背景颜色（正常）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollBtnNormalBackColor As Color
        Get
            Return Me.mscrollBtnNormalBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollBtnNormalBackColor = value Then Return
            Me.mscrollBtnNormalBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnEnterBackColor As Color = Color.FromArgb(128, 128, 128)

    <DefaultValue(GetType(Color), "128, 128, 128")>
    <Description("按钮背景颜色（鼠标进入）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollBtnEnterBackColor As Color
        Get
            Return Me.mscrollBtnEnterBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollBtnEnterBackColor = value Then Return
            Me.mscrollBtnEnterBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnNormalForeColor As Color = Color.FromArgb(64, 64, 64)

    <DefaultValue(GetType(Color), "64, 64, 64")>
    <Description("按钮颜色（正常）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollBtnNormalForeColor As Color
        Get
            Return Me.mscrollBtnNormalForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollBtnNormalForeColor = value Then Return
            Me.mscrollBtnNormalForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mscrollBtnEnterForeColor As Color = Color.FromArgb(255, 255, 255)

    <DefaultValue(GetType(Color), "255, 255, 255")>
    <Description("按钮颜色（鼠标进入）")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ScrollBtnEnterForeColor As Color
        Get
            Return Me.mscrollBtnEnterForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mscrollBtnEnterForeColor = value Then Return
            Me.mscrollBtnEnterForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Dim mText As String = ""
    <Editor(GetType(TextEditorExt), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Overloads Property Text As String
        Get
            Return mText
        End Get
        Set(ByVal value As String)
            If mText = value Then Return
            mText = value
            Me.InitializeRectangle()
            Me.Invalidate()

        End Set
    End Property


    Private text_mrect As RectangleF
    Private text_mstatus As MoveStatus = MoveStatus.Normal
    Private text_reality_mrect As RectangleF
    Private ReadOnly scroll As New ScrollItem()
    Private ReadOnly scroll_slide As New ScrollItem()
    Private ReadOnly scroll_pre As New ScrollItem()
    Private ReadOnly scroll_next As New ScrollItem()
    Private ismovedown As Boolean = False
    Private movedownpoint As Point = Point.Empty

    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.Selectable, True)
        Me.DrawBorder = False
    End Sub
    Dim mBackColor As Color
    Overloads Property BackColor As Color
        Get
            Return Parent?.BackColor
            Return mBackColor
        End Get
        Set(value As Color)
            mBackColor = value
        End Set
    End Property
    Dim mForeColor As Color = Color.White
    <Browsable(True)>
    <DefaultValue(GetType(Color), "255,255,255")>
    Overrides Property ForeColor As Color
        Get
            Return mForeColor 'MyBase.ForeColor
        End Get
        Set(value As Color)
            mForeColor = value
            'MyBase.ForeColor = value
            Invalidate()
        End Set
    End Property
    Dim mOpacity As Byte
    Overrides Property Opacity As Byte
        Get
            If DesignMode Then Return 255
            Return mOpacity
        End Get
        Set(value As Byte)
            mOpacity = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If Opacity = 0 Then Return
        'MyBase.OnPaintBackground(e)
        MyBase.OnParentBackgroundImageChanged(e)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        Dim Regionrect As New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
        Dim scale_mscrollThickness As Integer = CInt((Me.mscrollThickness))
        Dim scale_mscrollSlideThickness As Integer = CInt((Me.mscrollSlideThickness))
        Dim back_sb As New SolidBrush(Me.BackColor)
        g.FillRectangle(back_sb, New RectangleF(Regionrect.X, Regionrect.Y, Regionrect.Width, Regionrect.Height))
        back_sb.Dispose()
        Dim text_sb As New SolidBrush(Me.ForeColor)
        Dim text_sf As StringFormat = ControlExt.Common.GetTextFormat(TextAlign)
        text_sf.Trimming = StringTrimming.Character
        If AutoSize Then
            g.DrawString(Me.Text, Me.Font, text_sb, Regionrect, text_sf)
        Else
            g.DrawString(Me.Text, Me.Font, text_sb, Me.GetDisplayRectangle(), text_sf)
        End If

        text_sf.Dispose()
        text_sb.Dispose()

        If Me.scroll.Rect.Height > Me.scroll_slide.Rect.Height Then
            Dim scroll_normal_back_pen As New Pen(Me.mscrollNormalBackColor, scale_mscrollThickness)
            Dim scroll_slide_back_pen As New Pen(If(Me.scroll_slide.Status = MoveStatus.Normal, Me.mscrollSlideNormalBackColor, Me.mscrollSlideEnterBackColor), scale_mscrollSlideThickness)
            Dim scroll_pre_back_sb As New SolidBrush(If(Me.scroll_pre.Status = MoveStatus.Normal, Me.mscrollBtnNormalBackColor, Me.mscrollBtnEnterBackColor))
            Dim scroll_pre_pen As New Pen(If(Me.scroll_pre.Status = MoveStatus.Normal, Me.mscrollBtnNormalForeColor, Me.mscrollBtnEnterForeColor), scale_mscrollThickness - 2) With {.EndCap = LineCap.Triangle}
            Dim scroll_next_back_sb As New SolidBrush(If(Me.scroll_next.Status = MoveStatus.Normal, Me.mscrollBtnNormalBackColor, Me.mscrollBtnEnterBackColor))
            Dim scroll_next_pen As New Pen(If(Me.scroll_next.Status = MoveStatus.Normal, Me.mscrollBtnNormalForeColor, Me.mscrollBtnEnterForeColor), scale_mscrollThickness - 2) With {.EndCap = LineCap.Triangle}
            Dim scroll_start_point As New Point(CInt(Me.scroll.Rect.X) + CInt((Me.scroll.Rect.Width / 2.0F)), CInt(Me.scroll.Rect.Y))
            Dim scroll_end_point As New Point(CInt(Me.scroll.Rect.X) + CInt((Me.scroll.Rect.Width / 2.0F)), CInt(Me.scroll.Rect.Bottom))
            g.DrawLine(scroll_normal_back_pen, scroll_start_point, scroll_end_point)
            g.FillRectangle(scroll_pre_back_sb, Me.scroll_pre.Rect)
            g.DrawLine(scroll_pre_pen, New PointF(Me.scroll_pre.Rect.X + Me.scroll_pre.Rect.Width / 2.0F, Me.scroll_pre.Rect.Bottom - Me.scroll_pre.Rect.Height / 3.0F), New PointF(Me.scroll_pre.Rect.X + Me.scroll_pre.Rect.Width / 2.0F, Me.scroll_pre.Rect.Bottom - Me.scroll_pre.Rect.Height / 3.0F - 1))
            g.FillRectangle(scroll_next_back_sb, Me.scroll_next.Rect)
            g.DrawLine(scroll_next_pen, New PointF(Me.scroll_next.Rect.X + Me.scroll_next.Rect.Width / 2.0F, Me.scroll_next.Rect.Y + Me.scroll_pre.Rect.Height / 3.0F), New PointF(Me.scroll_next.Rect.X + Me.scroll_next.Rect.Width / 2.0F, Me.scroll_next.Rect.Y + Me.scroll_pre.Rect.Height / 3.0F + 1))
            Dim scroll_slide_start_point As New Point(CInt(Me.scroll_slide.Rect.X) + CInt((Me.scroll_slide.Rect.Width / 2.0F)), CInt(Me.scroll_slide.Rect.Y))
            Dim scroll_slide_end_point As New Point(CInt(Me.scroll_slide.Rect.X) + CInt((Me.scroll_slide.Rect.Width / 2.0F)), CInt(Me.scroll_slide.Rect.Bottom))
            g.DrawLine(scroll_slide_back_pen, scroll_slide_start_point, scroll_slide_end_point)
            scroll_normal_back_pen.Dispose()
            scroll_slide_back_pen.Dispose()
            If scroll_pre_back_sb IsNot Nothing Then scroll_pre_back_sb.Dispose()
            If scroll_pre_pen IsNot Nothing Then scroll_pre_pen.Dispose()
            If scroll_next_back_sb IsNot Nothing Then scroll_next_back_sb.Dispose()
            If scroll_next_pen IsNot Nothing Then scroll_next_pen.Dispose()
        End If

        If DrawBorder Then
            Dim BorderPen As New Pen(BorderColor)
            g.DrawRectangle(BorderPen, New Rectangle(0, 0, Regionrect.Width - 1, Regionrect.Height - 1))
            BorderPen.Dispose()
        End If
    End Sub


    <Description("文本对齐方式")>
    Public Overloads Property TextAlign As ContentAlignment
        Get
            Return MyBase.TextAlign
        End Get
        Set(ByVal value As ContentAlignment)
            If MyBase.TextAlign = value Then Return
            MyBase.TextAlign = value
            Me.InitializeRectangle()
            Me.Invalidate()
        End Set
    End Property

    Dim mAutoSize As Boolean = True
    <Browsable(True)>
    <DefaultValue(True)>
    <Description("否自动调整控件的大小以显示其完整内容。")>
    Public Overrides Property AutoSize As Boolean
        Get
            Return mAutoSize
        End Get
        Set(ByVal value As Boolean)
            If mAutoSize = value Then Return
            mAutoSize = value
            Me.InitializeRectangle()
            Me.Invalidate()

        End Set
    End Property
    Public Overloads Property Size As Size
        Get
            Return MyBase.Size
        End Get
        Set(ByVal value As Size)
            If Me.AutoSize OrElse MyBase.Size = value Then Return
            MyBase.Size = value
            Me.InitializeRectangle()
            Me.Invalidate()

        End Set
    End Property




    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)

        If Me.scroll_pre.Status <> MoveStatus.Normal Then
            Me.scroll_pre.Status = MoveStatus.Normal
        End If

        If Me.scroll_next.Status <> MoveStatus.Normal Then
            Me.scroll_next.Status = MoveStatus.Normal
        End If

        If Me.scroll_slide.Status <> MoveStatus.Normal Then
            Me.scroll_slide.Status = MoveStatus.Normal
        End If

        
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Me.ismovedown = True
        Me.movedownpoint = e.Location
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Dim isreset As Boolean = False

        If Not ismovedown Then

            If Me.scroll.Rect.Contains(e.Location) Then

                If Me.scroll.Status <> MoveStatus.Enter Then
                    Me.scroll.Status = MoveStatus.Enter
                    isreset = True
                    Me.Focus()
                End If
            Else

                If Me.scroll.Status <> MoveStatus.Normal Then
                    Me.scroll.Status = MoveStatus.Normal
                    isreset = True
                    Me.Focus()
                End If
            End If

            If Me.scroll_pre.Rect.Contains(e.Location) Then

                If Me.scroll_pre.Status <> MoveStatus.Enter Then
                    Me.scroll_pre.Status = MoveStatus.Enter
                    isreset = True
                End If
            Else

                If Me.scroll_pre.Status <> MoveStatus.Normal Then
                    Me.scroll_pre.Status = MoveStatus.Normal
                    isreset = True
                End If
            End If

            If Me.scroll_next.Rect.Contains(e.Location) Then

                If Me.scroll_next.Status <> MoveStatus.Enter Then
                    Me.scroll_next.Status = MoveStatus.Enter
                    isreset = True
                End If
            Else

                If Me.scroll_next.Status <> MoveStatus.Normal Then
                    Me.scroll_next.Status = MoveStatus.Normal
                    isreset = True
                End If
            End If

            If Me.scroll_slide.Rect.Contains(e.Location) Then

                If Me.scroll_slide.Status <> MoveStatus.Enter Then
                    Me.scroll_slide.Status = MoveStatus.Enter
                    isreset = True
                End If
            Else

                If Me.scroll_slide.Status <> MoveStatus.Normal Then
                    Me.scroll_slide.Status = MoveStatus.Normal
                    isreset = True
                End If
            End If

            If Me.text_mrect.Contains(e.Location) Then

                If Me.text_mstatus <> MoveStatus.Enter Then
                    Me.text_mstatus = MoveStatus.Enter
                End If
            Else

                If Me.text_mstatus <> MoveStatus.Normal Then
                    Me.text_mstatus = MoveStatus.Normal
                End If
            End If
        End If

        If Me.ismovedown AndAlso Me.scroll.Status = MoveStatus.Enter Then
            Dim offset As Integer = CInt(((e.Location.Y - Me.movedownpoint.Y)))

            If Me.IsResetScroll(offset) Then
                Me.movedownpoint = e.Location
                isreset = True
            End If
        End If

        If isreset Then
            Me.Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If e.Button = System.Windows.Forms.MouseButtons.Left Then

            If Me.scroll_pre.Status = MoveStatus.Enter Then

                If Me.IsResetScroll(-1) Then
                    Me.Invalidate()
                End If
            ElseIf Me.scroll_next.Status = MoveStatus.Enter Then

                If Me.IsResetScroll(1) Then
                    Me.Invalidate()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        If Me.scroll.Status = MoveStatus.Enter OrElse Me.text_mstatus = MoveStatus.Enter Then
            Dim offset As Integer = If(e.Delta > 1, -1, 1)

            If Me.IsResetScroll(offset) Then
                Me.Invalidate()
            End If
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Me.InitializeRectangle()
        Me.Invalidate()

    End Sub

    Private Sub InitializeRectangle()
        Dim g As Graphics = Me.CreateGraphics
        Dim Regionrect As New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
        Dim scale_mscrollThickness As Integer = CInt((Me.mscrollThickness))
        Dim scale_mscrollBtnHeight As Integer = CInt((Me.mscrollBtnHeight))
        Dim text_sf As New StringFormat() With {
            .Trimming = StringTrimming.Character
        }
        Dim text_size As SizeF
        If AutoSize Then
            text_size = g.MeasureString(Me.Text, Me.Font)
        Else
            text_size = g.MeasureString(Me.Text, Me.Font, CInt(Regionrect.Width), text_sf)
        End If
        If text_size.Height > Regionrect.Height Then
            text_size = g.MeasureString(Me.Text, Me.Font, CInt(Regionrect.Width) - scale_mscrollThickness, text_sf)
            Me.text_mrect = New RectangleF(Regionrect.X, Regionrect.Y, Regionrect.Width - scale_mscrollThickness, Regionrect.Height)
        Else
            Me.text_mrect = New RectangleF(Regionrect.X, Regionrect.Y, Regionrect.Width, Regionrect.Height)
        End If

        Me.text_reality_mrect = New RectangleF(text_mrect.Location, text_size)
        If AutoSize Then
            Me.SetBounds(Me.Location.X, Me.Location.Y, CInt(text_reality_mrect.Width + 2), CInt(text_reality_mrect.Height + 2), BoundsSpecified.Size)
        Else
            If text_reality_mrect.Width < Regionrect.Width Then
                text_reality_mrect.Width = Regionrect.Width
            End If
            If text_reality_mrect.Height < Regionrect.Height Then
                text_reality_mrect.Height = Regionrect.Height
            End If
        End If
        text_sf.Dispose()
        If AutoSize Then
            Me.scroll_slide.Rect = RectangleF.Empty
            Me.scroll.Rect = RectangleF.Empty
        Else
            If Me.mscrollBtnShow Then
                Me.scroll_pre.Rect = New RectangleF(Regionrect.Right - scale_mscrollThickness, Regionrect.Top, scale_mscrollThickness, scale_mscrollBtnHeight)
                Me.scroll_next.Rect = New RectangleF(Regionrect.Right - scale_mscrollThickness, Regionrect.Bottom - scale_mscrollBtnHeight, scale_mscrollThickness, scale_mscrollBtnHeight)
            Else
                Me.scroll_pre.Rect = New RectangleF(0, Regionrect.Y, 0, 0)
                Me.scroll_next.Rect = New RectangleF(Regionrect.Right + 1, Regionrect.Bottom + 1, 0, 0) '- scale_mscrollThickness
            End If

            Me.scroll.Rect = New RectangleF(Regionrect.Right - scale_mscrollThickness, Regionrect.Y + Me.scroll_pre.Rect.Height, scale_mscrollThickness, Regionrect.Height - Me.scroll_pre.Rect.Height - Me.scroll_next.Rect.Height)
            Dim slide_h As Single = (Me.text_mrect.Height / Me.text_reality_mrect.Height * Me.scroll.Rect.Height)

            If Me.text_reality_mrect.Height <= Me.text_mrect.Height Then
                slide_h = Me.scroll.Rect.Height
            End If

            Me.scroll_slide.Rect = New RectangleF(Me.scroll.Rect.X, Me.scroll_pre.Rect.Bottom, scale_mscrollThickness, slide_h)

        End If
        g.Dispose()

    End Sub

    Private Function IsResetScroll(ByVal offset As Integer) As Boolean
        Dim y As Single = Me.scroll_slide.Rect.Y + offset
        If y < Me.scroll.Rect.Y Then y = Me.scroll.Rect.Y
        If y > Me.scroll.Rect.Bottom - Me.scroll_slide.Rect.Height Then y = Me.scroll.Rect.Bottom - Me.scroll_slide.Rect.Height
        Dim result As Boolean = Not (Me.scroll_slide.Rect.Y = y)
        Me.scroll_slide.Rect = New RectangleF(Me.scroll_slide.Rect.X, y, Me.scroll_slide.Rect.Width, Me.scroll_slide.Rect.Height)
        Return result
    End Function

    Private Function GetDisplayY() As Integer
        Dim y As Single = 0

        If Me.scroll.Rect.Height > Me.scroll_slide.Rect.Height Then
            y = -(Me.scroll_slide.Rect.Y - Me.scroll_pre.Rect.Bottom) / (Me.scroll.Rect.Height - Me.scroll_slide.Rect.Height) * (Me.text_reality_mrect.Height - Me.text_mrect.Height)
        End If

        Return CInt((Me.text_mrect.Y + y))
    End Function

    Private Function GetDisplayRectangle() As Rectangle
        Return New Rectangle(0, Me.GetDisplayY(), CInt(Me.text_mrect.Width), CInt(Me.text_reality_mrect.Height))
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    <Description("滚动条选项信息")>
    Public Class ScrollItem
        Private mrect As RectangleF = RectangleF.Empty

        Public Property Rect As RectangleF
            Get
                Return Me.mrect
            End Get
            Set(ByVal value As RectangleF)
                Me.mrect = value
            End Set
        End Property

        Private mstatus As MoveStatus = MoveStatus.Normal

        Public Property Status As MoveStatus
            Get
                Return Me.mstatus
            End Get
            Set(ByVal value As MoveStatus)
                Me.mstatus = value
            End Set
        End Property
    End Class

    <Description("鼠标状态")>
    Public Enum MoveStatus
        Normal
        Enter
    End Enum

End Class
