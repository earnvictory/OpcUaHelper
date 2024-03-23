
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Text.RegularExpressions

<ToolboxItem(False)>
<Description("颜色面板美化控件")>
<DefaultProperty("ColorValue")>
<DefaultEvent("BottomBarConfirmClick")>
<Designer(GetType(ColorPickerExtDesigner))>
<TypeConverter(GetType(EmptyConverter))>
Public Class ColorPickerExt
    Inherits Control

    Public Delegate Sub ColorValueChangedEventHandler(ByVal sender As Object, ByVal e As ColorValueChangedEventArgs)


    <Description("颜色值更改事件")>
    Public Event ColorValueChanged As ColorValueChangedEventHandler


    Public Delegate Sub HtmlColorItemClickEventHandler(ByVal sender As Object, ByVal e As HtmlColorItemClickEventArgs)


    <Description("html颜色面板选项单击事件")>
    Public Event HtmlColorItemClick As HtmlColorItemClickEventHandler


    Public Delegate Sub ColorItemClickEventHandler(ByVal sender As Object, ByVal e As ColorItemClickEventArgs)

    <Description("主题颜色面板选项单击事件")>
    Public Event ThemeColorItemClick As ColorItemClickEventHandler




    <Description("标准颜色面板选项单击事件")>
    Public Event StandardColorItemClick As ColorItemClickEventHandler



    <Description("自定义颜色面板选项单击事件")>
    Public Event CustomColorItemClick As ColorItemClickEventHandler


    Public Delegate Sub BottomBarIiemClickEventHandler(ByVal sender As Object, ByVal e As BottomBarIiemClickEventArgs)

    <Description("自定义颜色单击事件")>
    Public Event BottomBarCustomClick As BottomBarIiemClickEventHandler




    <Description("清除单击事件")>
    Public Event BottomBarClearClick As BottomBarIiemClickEventHandler




    <Description("确认单击事件")>
    Public Event BottomBarConfirmClick As BottomBarIiemClickEventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event BackgroundImageChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event BackgroundImageLayoutChanged As EventHandler

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event TabIndexChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event TabStopChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event DockChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event TextChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event FontChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event ForeColorChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event RightToLeftChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event ImeModeChanged As EventHandler


    Private mcolorReadOnly As Boolean = False

    <DefaultValue(False)>
    <Description("颜色面板是否只读")>
    Public Property ColorReadOnly As Boolean
        Get
            Return Me.mcolorReadOnly
        End Get
        Set(ByVal value As Boolean)
            If Me.mcolorReadOnly = value Then Return
            Me.mcolorReadOnly = value
            Me.Invalidate()
        End Set
    End Property

    Private mcolorInput As Boolean = True

    <DefaultValue(True)>
    <Description("是否允许颜色输入框输入")>
    Public Property ColorInput As Boolean
        Get
            Return Me.mcolorInput
        End Get
        Set(ByVal value As Boolean)
            If Me.mcolorInput = value Then Return
            Me.mcolorInput = value
            Me.colorTextBox.Enabled = value
        End Set
    End Property

    Private mcolorType As McolorTypes = McolorTypes.[Default]

    <DefaultValue(McolorTypes.[Default])>
    <Description("颜色面板选中类型")>
    Public Property ColorType As McolorTypes
        Get
            Return Me.mcolorType
        End Get
        Set(ByVal value As McolorTypes)
            If Me.mcolorType = value Then Return
            Me.mcolorType = value
            Me.Invalidate()
        End Set
    End Property

    Private mtopBarBtnForeColor As Color = Color.FromArgb(158, 158, 158)

    <DefaultValue(GetType(Color), "158, 158, 158")>
    <Description("顶部按钮字体颜色(正常)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property TopBarBtnForeColor As Color
        Get
            Return Me.mtopBarBtnForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mtopBarBtnForeColor = value Then Return
            Me.mtopBarBtnForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mtopBarBtnForeSelectColor As Color = Color.FromArgb(153, 204, 204)

    <DefaultValue(GetType(Color), "153, 204, 204")>
    <Description("顶部按钮字体颜色(选中)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property TopBarBtnForeSelectColor As Color
        Get
            Return Me.mtopBarBtnForeSelectColor
        End Get
        Set(ByVal value As Color)
            If Me.mtopBarBtnForeSelectColor = value Then Return
            Me.mtopBarBtnForeSelectColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mthemeTitleForeColor As Color = Color.FromArgb(153, 204, 204)

    <DefaultValue(GetType(Color), "153, 204, 204")>
    <Description("主题颜色标题字体颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property ThemeTitleForeColor As Color
        Get
            Return Me.mthemeTitleForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mthemeTitleForeColor = value Then Return
            Me.mthemeTitleForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mstandardTitleForeColor As Color = Color.FromArgb(153, 204, 204)

    <DefaultValue(GetType(Color), "153, 204, 204")>
    <Description("标准颜色标题字体颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property StandardTitleForeColor As Color
        Get
            Return Me.mstandardTitleForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mstandardTitleForeColor = value Then Return
            Me.mstandardTitleForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mcustomTitleForeColor As Color = Color.FromArgb(153, 204, 204)

    <DefaultValue(GetType(Color), "153, 204, 204")>
    <Description("自定义颜色标题字体颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property CustomTitleForeColor As Color
        Get
            Return Me.mcustomTitleForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mcustomTitleForeColor = value Then Return
            Me.mcustomTitleForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mcustomSelectLineColor As Color = Color.FromArgb(107, 142, 35)

    <DefaultValue(GetType(Color), "107, 142, 35")>
    <Description("自定义颜色选中颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property CustomSelectLineColor As Color
        Get
            Return Me.mcustomSelectLineColor
        End Get
        Set(ByVal value As Color)
            If Me.mcustomSelectLineColor = value Then Return
            Me.mcustomSelectLineColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mcurrentTextForeColor As Color = Color.FromArgb(105, 105, 105)

    <DefaultValue(GetType(Color), "105, 105, 105")>
    <Description("当前颜色字体颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property CurrentTextForeColor As Color
        Get
            Return Me.mcurrentTextForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mcurrentTextForeColor = value Then Return
            Me.mcurrentTextForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mbottomBarBtnBackColor As Color = Color.FromArgb(153, 204, 204)

    <DefaultValue(GetType(Color), "153, 204, 204")>
    <Description("底部按钮背景颜色(正常)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnBackColor As Color
        Get
            Return Me.mbottomBarBtnBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mbottomBarBtnBackColor = value Then Return
            Me.mbottomBarBtnBackColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mbottomBarBtnForeColor As Color = Color.FromArgb(255, 255, 255)

    <DefaultValue(GetType(Color), "255,255,255")>
    <Description("底部按钮字体颜色(正常)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnForeColor As Color
        Get
            Return Me.mbottomBarBtnForeColor
        End Get
        Set(ByVal value As Color)
            If Me.mbottomBarBtnForeColor = value Then Return
            Me.mbottomBarBtnForeColor = value
            Me.Invalidate()
        End Set
    End Property

    Private mbottomBarBtnBackDisabledColor As Color = Color.FromArgb(170, 192, 192, 192)

    <DefaultValue(GetType(Color), "170, 192, 192, 192")>
    <Description("底部按钮背景颜色(禁用)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnBackDisabledColor As Color
        Get
            Return Me.mbottomBarBtnBackDisabledColor
        End Get
        Set(ByVal value As Color)
            If Me.mbottomBarBtnBackDisabledColor = value Then Return
            Me.mbottomBarBtnBackDisabledColor = value
            Me.Invalidate()
        End Set
    End Property

    Private bottomBarBtnForeDisabledColor_ As Color = Color.FromArgb(170, 255, 255, 255)

    <DefaultValue(GetType(Color), "170, 255, 255, 255")>
    <Description("底部按钮字体颜色(禁用)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnForeDisabledColor As Color
        Get
            Return Me.bottomBarBtnForeDisabledColor_
        End Get
        Set(ByVal value As Color)
            If Me.bottomBarBtnForeDisabledColor_ = value Then Return
            Me.bottomBarBtnForeDisabledColor_ = value
            Me.Invalidate()
        End Set
    End Property

    Private bottomBarBtnBackEnterColor_ As Color = Color.FromArgb(200, 153, 204, 204)

    <DefaultValue(GetType(Color), "200, 153, 204, 204")>
    <Description("底部按钮背景颜色(鼠标进入)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnBackEnterColor As Color
        Get
            Return Me.bottomBarBtnBackEnterColor_
        End Get
        Set(ByVal value As Color)
            If Me.bottomBarBtnBackEnterColor_ = value Then Return
            Me.bottomBarBtnBackEnterColor_ = value
            Me.Invalidate()
        End Set
    End Property

    Private bottomBarBtnForeEnterColor_ As Color = Color.FromArgb(200, 255, 255, 255)

    <DefaultValue(GetType(Color), "200,255,255,255")>
    <Description("底部按钮字体颜色(鼠标进入)")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BottomBarBtnForeEnterColor As Color
        Get
            Return Me.bottomBarBtnForeEnterColor_
        End Get
        Set(ByVal value As Color)
            If Me.bottomBarBtnForeEnterColor_ = value Then Return
            Me.bottomBarBtnForeEnterColor_ = value
            Me.Invalidate()
        End Set
    End Property

    Private colorValue_ As Color = Color.Empty

    <DefaultValue(GetType(Color), "Empty")>
    <Description("颜色")>
    <Editor(GetType(ColorEditorExt), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property ColorValue As Color
        Get
            Return Me.colorValue_
        End Get
        Set(ByVal value As Color)
            If Me.colorValue_ = value Then Return

            Dim arg As New ColorValueChangedEventArgs() With {
                .OldColorValue = Me.colorValue_,
                .NewColorValue = value
            }
            Me.colorValue_ = value
            currentValue = value
            colorTextBox.Text = String.Format("{0},{1},{2},{3}", value.A, value.R, value.G, value.B)
            Me.OnColorValueChanged(arg)
        End Set
    End Property

    Protected Overloads Property Enabled As Boolean
        Get
            If DesignMode Then Return True
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)
            MyBase.Enabled = value
            Me.Invalidate()
        End Set
    End Property

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

    <DefaultValue(GetType(Color), "255, 255, 255")>
    Public Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            MyBase.BackColor = value
        End Set
    End Property

    <DefaultValue(GetType(Padding), "5,5,5,5")>
    <Description("控件默认内边距")>
    Protected Overrides ReadOnly Property DefaultPadding As Padding
        Get
            Return New Padding(5, 5, 5, 5)
        End Get
    End Property

    <DefaultValue(GetType(Size), "465, 285")>
    <Description("控件默认大小")>
    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(465, 330)
        End Get
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Protected Overrides ReadOnly Property DefaultImeMode As ImeMode
        Get
            Return System.Windows.Forms.ImeMode.Disable
        End Get
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property BackgroundImage As Image
        Get
            Return MyBase.BackgroundImage
        End Get
        Set(ByVal value As Image)
            MyBase.BackgroundImage = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property BackgroundImageLayout As ImageLayout
        Get
            Return MyBase.BackgroundImageLayout
        End Get
        Set(ByVal value As ImageLayout)
            MyBase.BackgroundImageLayout = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property TabIndex As Integer
        Get
            Return 0
        End Get
        Set(ByVal value As Integer)
            MyBase.TabIndex = 0
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property TabStop As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
            MyBase.TabStop = False
        End Set
    End Property

    <DefaultValue(DockStyle.None)>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property Dock As DockStyle
        Get
            Return DockStyle.None
        End Get
        Set(ByVal value As DockStyle)
            MyBase.Dock = DockStyle.None
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MinimumSize = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property MaximumSize As Size
        Get
            Return MyBase.MaximumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MaximumSize = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(ByVal value As Color)
            MyBase.ForeColor = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property RightToLeft As RightToLeft
        Get
            Return MyBase.RightToLeft
        End Get
        Set(ByVal value As RightToLeft)
            MyBase.RightToLeft = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property ImeMode As ImeMode
        Get
            Return MyBase.ImeMode
        End Get
        Set(ByVal value As ImeMode)
            MyBase.ImeMode = value
        End Set
    End Property


    Private ReadOnly border_slide_back_color As Color
    Private gradual_color As Color
    'Private defaultColorValue As Color = Color.Empty
    Private currentValue As Color = Color.Empty
    Private ReadOnly gradual_bmp As Bitmap
    Private ReadOnly gradual_bar_bmp As Bitmap
    Private colorMoveStatus As ColorMoveStatuss = ColorMoveStatuss.Normal
    Private ReadOnly ColorObject As ColorClass
    Private custom_select_row_index As Integer = 0
    Private custom_select_cel_index As Integer = 0
    Private Shared ReadOnly text_left_sf As New StringFormat() With {
        .Alignment = StringAlignment.Near,
        .LineAlignment = StringAlignment.Center,
        .FormatFlags = StringFormatFlags.NoClip
    }
    Private Shared ReadOnly text_center_sf As New StringFormat() With {
        .Alignment = StringAlignment.Center,
        .LineAlignment = StringAlignment.Center,
        .FormatFlags = StringFormatFlags.NoClip
    }
    Private Shared ReadOnly text_right_sf As New StringFormat() With {
        .Alignment = StringAlignment.Far,
        .LineAlignment = StringAlignment.Center,
        .FormatFlags = StringFormatFlags.NoClip
    }
    Private ReadOnly colorTextWidth As Integer = 100
    Private ReadOnly colorTextBox As ColorPickerExtTextBox = Nothing
    Private ReadOnly SolidBrushManageObject As SolidBrushManage

    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.ContainerControl, True)
        Me.SuspendLayout()
        Me.BackColor = Color.FromArgb(255, 255, 255)
        Me.border_slide_back_color = Color.FromArgb(200, 255, 255, 255)
        Me.ColorObject = New ColorClass(Me)
        Me.InitializeControlRectangle()
        Me.SolidBrushManageObject = New SolidBrushManage(Me)
        Me.gradual_bmp = New Bitmap(Me.ColorObject.GradualRect.Width, Me.ColorObject.GradualRect.Height)
        Me.gradual_bar_bmp = New Bitmap(Me.ColorObject.GradualBarRect.Width, Me.ColorObject.GradualBarRect.Height)
        Me.Update_GradualBar_Image()
        Me.colorTextBox = New ColorPickerExtTextBox(Me) With {
            .BackColor = Me.BackColor,
            .ForeColor = Me.ForeColor,
            .ImeMode = ImeMode.Off,
            .TextAlign = HorizontalAlignment.Left,
            .TabStop = False,
            .Cursor = Cursors.[Default],
            .BorderStyle = BorderStyle.None
        }
        AddHandler Me.colorTextBox.LostFocus, AddressOf Me.ColorTextBox_LostFocus
        AddHandler Me.colorTextBox.TextChanged, AddressOf Me.ColorTextBox_TextChanged
        Me.Controls.Add(Me.colorTextBox)
        Me.ResumeLayout()
        Me.UpdateLocationSize()
        Me.InitializeColor()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
        MyBase.OnPaint(e)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        Me.SolidBrushManageObject.Common_sb.Color = Me.BackColor
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, g.ClipBounds)
        Dim top_defaultcolorbtn_fore_color As Color = If((Me.mcolorType = McolorTypes.[Default]), Me.mtopBarBtnForeSelectColor, Me.mtopBarBtnForeColor)
        Me.SolidBrushManageObject.Common_sb.Color = top_defaultcolorbtn_fore_color
        g.DrawString(Me.ColorObject.DefaultColorBtn.Text, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.DefaultColorBtn.Rect, text_center_sf)

        If Me.mcolorType = McolorTypes.[Default] Then
            g.DrawLines(Me.SolidBrushManageObject.Border_pen, New Point() {New Point(Me.ColorObject.ColorRect.X, Me.ColorObject.DefaultColorBtn.Rect.Bottom), New Point(Me.ColorObject.DefaultColorBtn.Rect.Left, Me.ColorObject.DefaultColorBtn.Rect.Bottom), New Point(Me.ColorObject.DefaultColorBtn.Rect.Left, Me.ColorObject.DefaultColorBtn.Rect.Top), New Point(Me.ColorObject.DefaultColorBtn.Rect.Right, Me.ColorObject.DefaultColorBtn.Rect.Top), New Point(Me.ColorObject.DefaultColorBtn.Rect.Right, Me.ColorObject.DefaultColorBtn.Rect.Bottom), New Point(Me.ColorObject.ColorRect.Right, Me.ColorObject.DefaultColorBtn.Rect.Bottom)})
        End If

        Dim top_htmlcolorbtn_fore_color As Color = If((Me.mcolorType = McolorTypes.Html), Me.mtopBarBtnForeSelectColor, Me.mtopBarBtnForeColor)
        Me.SolidBrushManageObject.Common_sb.Color = top_htmlcolorbtn_fore_color
        g.DrawString(Me.ColorObject.HtmlColorBtn.Text, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.HtmlColorBtn.Rect, text_center_sf)

        If Me.mcolorType = McolorTypes.Html Then
            g.DrawLines(Me.SolidBrushManageObject.Border_pen, New Point() {New Point(Me.ColorObject.ColorRect.X, Me.ColorObject.HtmlColorBtn.Rect.Bottom), New Point(Me.ColorObject.HtmlColorBtn.Rect.Left, Me.ColorObject.HtmlColorBtn.Rect.Bottom), New Point(Me.ColorObject.HtmlColorBtn.Rect.Left, Me.ColorObject.HtmlColorBtn.Rect.Top), New Point(Me.ColorObject.HtmlColorBtn.Rect.Right, Me.ColorObject.HtmlColorBtn.Rect.Top), New Point(Me.ColorObject.HtmlColorBtn.Rect.Right, Me.ColorObject.HtmlColorBtn.Rect.Bottom), New Point(Me.ColorObject.ColorRect.Right, Me.ColorObject.HtmlColorBtn.Rect.Bottom)})
        End If

        If Me.mcolorType = McolorTypes.[Default] Then
            Me.SolidBrushManageObject.Common_sb.Color = Me.mthemeTitleForeColor
            g.DrawString("Theme Colors", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ThemeTitleRect, text_left_sf)
            Dim theme_colors_item_enter As ColorItemClass = Nothing

            For i As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(0) - 1

                For j As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(1) - 1
                    Me.SolidBrushManageObject.Common_sb.Color = ColorManage.ThemeColors(i, j)
                    g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ThemeColorsItem(i, j).Rect)

                    If Me.ColorObject.ThemeColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter Then
                        theme_colors_item_enter = Me.ColorObject.ThemeColorsItem(i, j)
                    End If
                Next
            Next

            If theme_colors_item_enter IsNot Nothing Then
                Dim rect As New Rectangle(theme_colors_item_enter.Rect.X - 1, theme_colors_item_enter.Rect.Y - 1, theme_colors_item_enter.Rect.Width + 1, theme_colors_item_enter.Rect.Height + 1)
                g.DrawRectangle(Me.SolidBrushManageObject.Border_ts_pen, rect)
            End If

            Me.SolidBrushManageObject.Common_sb.Color = Me.mstandardTitleForeColor
            g.DrawString("Standard Colors", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.StandardTitleRect, text_left_sf)

            For i As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(0) - 1

                For j As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(1) - 1
                    Me.SolidBrushManageObject.Common_sb.Color = ColorManage.StandardColors(i, j)
                    g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.StandardColorsItem(i, j).Rect)

                    If Me.ColorObject.StandardColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter Then
                        Dim rect As New Rectangle(Me.ColorObject.StandardColorsItem(i, j).Rect.X - 1, Me.ColorObject.StandardColorsItem(i, j).Rect.Y - 1, Me.ColorObject.StandardColorsItem(i, j).Rect.Width + 1, Me.ColorObject.StandardColorsItem(i, j).Rect.Height + 1)
                        g.DrawRectangle(Me.SolidBrushManageObject.Border_ts_pen, rect)
                    End If
                Next
            Next
        ElseIf Me.mcolorType = McolorTypes.Html Then

            For i As Integer = 0 To Me.ColorObject.HtmlColorsItem.Count - 1

                For j As Integer = 0 To Me.ColorObject.HtmlColorsItem(i).ColorsRects.Count - 1
                    Me.SolidBrushManageObject.Common_sb.Color = ColorManage.HtmlColors(i).Colors(j)
                    g.FillPolygon(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs)
                Next
            Next
        End If

        Me.SolidBrushManageObject.Common_sb.Color = Me.mcustomTitleForeColor
        g.DrawString("Custom Color", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CustomTitleRect, text_left_sf)

        For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

            For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1
                ' g.DrawImage(back_image, Me.ColorObject.CustomColorsItem(i, j).Rect, 0, 0, Me.ColorObject.CustomColorsItem(i, j).Rect.Width, Me.ColorObject.CustomColorsItem(i, j).Rect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)
                Me.SolidBrushManageObject.Common_sb.Color = ColorManage.CustomColors(i, j)
                g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CustomColorsItem(i, j).Rect)
                Dim rect As New Rectangle(Me.ColorObject.CustomColorsItem(i, j).Rect.X, Me.ColorObject.CustomColorsItem(i, j).Rect.Y, Me.ColorObject.CustomColorsItem(i, j).Rect.Width - 1, Me.ColorObject.CustomColorsItem(i, j).Rect.Height - 1)
                g.DrawRectangle(If((Me.ColorObject.CustomColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter), Me.SolidBrushManageObject.Border_ts_pen, Me.SolidBrushManageObject.Border_pen), rect)

                If Me.custom_select_row_index = i AndAlso Me.custom_select_cel_index = j Then
                    Me.SolidBrushManageObject.Common_pen.Color = Me.mcustomSelectLineColor
                    Dim w As Single = Me.SolidBrushManageObject.Common_pen.Width
                    Me.SolidBrushManageObject.Common_pen.Width = 2
                    g.DrawLine(Me.SolidBrushManageObject.Common_pen, New Point(Me.ColorObject.CustomColorsItem(i, j).Rect.Left, Me.ColorObject.CustomColorsItem(i, j).Rect.Bottom + 3), New Point(Me.ColorObject.CustomColorsItem(i, j).Rect.Right, Me.ColorObject.CustomColorsItem(i, j).Rect.Bottom + 3))
                    Me.SolidBrushManageObject.Common_pen.Width = w
                End If
            Next
        Next

        Dim gradual_border_rect As New Rectangle(Me.ColorObject.GradualRect.X - 1, Me.ColorObject.GradualRect.Y - 1, Me.ColorObject.GradualRect.Width + 1, Me.ColorObject.GradualRect.Height + 1)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, gradual_border_rect)
        g.DrawImage(Me.gradual_bmp, Me.ColorObject.GradualRect)

        If Me.ColorObject.GradualSelectPoint <> Point.Empty Then
            Dim point_rect_in As New Rectangle(Me.ColorObject.GradualRect.X + Me.ColorObject.GradualSelectPoint.X - 2, Me.ColorObject.GradualRect.Y + Me.ColorObject.GradualSelectPoint.Y - 2, 4, 4)
            Dim point_rect_out As New Rectangle(Me.ColorObject.GradualRect.X + Me.ColorObject.GradualSelectPoint.X - 3, Me.ColorObject.GradualRect.Y + Me.ColorObject.GradualSelectPoint.Y - 3, 6, 6)
            Me.SolidBrushManageObject.Common_pen.Color = Color.Black
            g.DrawEllipse(Me.SolidBrushManageObject.Common_pen, point_rect_in)
            Me.SolidBrushManageObject.Common_pen.Color = Color.White
            g.DrawEllipse(Me.SolidBrushManageObject.Common_pen, point_rect_out)
        End If

        g.DrawImage(Me.gradual_bar_bmp, Me.ColorObject.GradualBarRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.GradualBarRect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.border_slide_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.GradualBarSlideRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_slide_pen, Me.ColorObject.GradualBarSlideRect)
        ' g.DrawImage(back_image, Me.ColorObject.CurrentValue_A_Rect, 0, 0, Me.ColorObject.CurrentValue_A_Rect.Width, Me.ColorObject.CurrentValue_A_Rect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)
        Me.SolidBrushManageObject.Argb_lgb.LinearColors = New Color() {Color.Transparent, Color.FromArgb(Byte.MaxValue, Me.currentValue)}
        g.FillRectangle(Me.SolidBrushManageObject.Argb_lgb, Me.ColorObject.CurrentValue_A_Rect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.CurrentValue_A_Rect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.border_slide_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CurrentValue_A_SlideRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_slide_pen, Me.ColorObject.CurrentValue_A_SlideRect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim a_rect As New Rectangle(Me.ColorObject.CurrentValue_A_Rect.Right + Me.ColorObject.CurrentValue_A_SlideRect.Width, Me.ColorObject.CurrentValue_A_Rect.Y, 20, Me.ColorObject.CurrentValue_A_Rect.Height)
        g.DrawString("A", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, a_rect, text_right_sf)
        'g.DrawImage(back_image, Me.ColorObject.CurrentValue_R_Rect, 0, 0, Me.ColorObject.CurrentValue_R_Rect.Width, Me.ColorObject.CurrentValue_R_Rect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)
        Me.SolidBrushManageObject.Argb_lgb.LinearColors = New Color() {Color.Transparent, Color.Red}
        g.FillRectangle(Me.SolidBrushManageObject.Argb_lgb, Me.ColorObject.CurrentValue_R_Rect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.CurrentValue_R_Rect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.border_slide_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CurrentValue_R_SlideRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_slide_pen, Me.ColorObject.CurrentValue_R_SlideRect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim r_rect As New Rectangle(Me.ColorObject.CurrentValue_R_Rect.Right + Me.ColorObject.CurrentValue_R_SlideRect.Width, Me.ColorObject.CurrentValue_R_Rect.Y, 20, Me.ColorObject.CurrentValue_R_Rect.Height)
        g.DrawString("R", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, r_rect, text_right_sf)
        ' g.DrawImage(back_image, Me.ColorObject.CurrentValue_G_Rect, 0, 0, Me.ColorObject.CurrentValue_G_Rect.Width, Me.ColorObject.CurrentValue_G_Rect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)
        Me.SolidBrushManageObject.Argb_lgb.LinearColors = New Color() {Color.Transparent, Color.Green}
        g.FillRectangle(Me.SolidBrushManageObject.Argb_lgb, Me.ColorObject.CurrentValue_G_Rect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.CurrentValue_G_Rect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.border_slide_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CurrentValue_G_SlideRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_slide_pen, Me.ColorObject.CurrentValue_G_SlideRect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim g_rect As New Rectangle(Me.ColorObject.CurrentValue_G_Rect.Right + Me.ColorObject.CurrentValue_G_SlideRect.Width, Me.ColorObject.CurrentValue_G_Rect.Y, 20, Me.ColorObject.CurrentValue_G_Rect.Height)
        g.DrawString("G", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, g_rect, text_right_sf)
        ' g.DrawImage(back_image, Me.ColorObject.CurrentValue_B_Rect, 0, 0, Me.ColorObject.CurrentValue_B_Rect.Width, Me.ColorObject.CurrentValue_B_Rect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)
        Me.SolidBrushManageObject.Argb_lgb.LinearColors = New Color() {Color.Transparent, Color.Blue}
        g.FillRectangle(Me.SolidBrushManageObject.Argb_lgb, Me.ColorObject.CurrentValue_B_Rect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.CurrentValue_B_Rect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.border_slide_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CurrentValue_B_SlideRect)
        g.DrawRectangle(Me.SolidBrushManageObject.Border_slide_pen, Me.ColorObject.CurrentValue_B_SlideRect)
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim b_rect As New Rectangle(Me.ColorObject.CurrentValue_B_Rect.Right + Me.ColorObject.CurrentValue_B_SlideRect.Width, Me.ColorObject.CurrentValue_B_Rect.Y, 20, Me.ColorObject.CurrentValue_B_Rect.Height)
        g.DrawString("B", Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, b_rect, text_right_sf)
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim newcolor_str As String = "Current:"
        g.DrawString(newcolor_str, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CurrentColorTextRect, text_left_sf)
        Dim colortext_border_pen As New Pen(Color.FromArgb(192, 192, 192), 1)
        g.DrawRectangle(colortext_border_pen, New Rectangle(Me.colorTextBox.Location.X - 3, Me.colorTextBox.Location.Y - 4, Me.colorTextBox.Size.Width + 4, Me.colorTextBox.Height + 5))
        colortext_border_pen.Dispose()
        ' g.DrawImage(back_image, Me.ColorObject.CurrentColorRect, 0, 0, Me.ColorObject.CurrentColorRect.Width, Me.ColorObject.CurrentColorRect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)

        If Me.currentValue <> Color.Empty Then
            Me.SolidBrushManageObject.Common_sb.Color = Color.FromArgb(Byte.MaxValue, Me.currentValue)
            g.FillRectangle(Me.SolidBrushManageObject.Common_sb, New Rectangle(Me.ColorObject.CurrentColorRect.X, Me.ColorObject.CurrentColorRect.Y, Me.ColorObject.CurrentColorRect.Width / 2, Me.ColorObject.CurrentColorRect.Height))
            Me.SolidBrushManageObject.Common_sb.Color = Me.currentValue
            g.FillRectangle(Me.SolidBrushManageObject.Common_sb, New Rectangle(Me.ColorObject.CurrentColorRect.X + Me.ColorObject.CurrentColorRect.Width / 2, Me.ColorObject.CurrentColorRect.Y, Me.ColorObject.CurrentColorRect.Width / 2, Me.ColorObject.CurrentColorRect.Height))
        End If

        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.CurrentColorRect)
        g.DrawLine(Me.SolidBrushManageObject.Border_pen, New Point(Me.ColorObject.CurrentColorRect.X + Me.ColorObject.CurrentColorRect.Width / 2, Me.ColorObject.CurrentColorRect.Y), New Point(Me.ColorObject.CurrentColorRect.X + Me.ColorObject.CurrentColorRect.Width / 2, Me.ColorObject.CurrentColorRect.Bottom))
        Me.SolidBrushManageObject.Common_sb.Color = Me.mcurrentTextForeColor
        Dim oldcolor_str As String = If(Me.colorValue_ = Color.Empty, "Origion:", String.Format("Origion: {0},{1},{2},{3}", Me.colorValue_.A, Me.colorValue_.R, Me.colorValue_.G, Me.colorValue_.B))
        g.DrawString(oldcolor_str, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.OriginalColorTextRect, text_left_sf)
        ' g.DrawImage(back_image, Me.ColorObject.OriginalColorRect, 0, 0, Me.ColorObject.OriginalColorRect.Width, Me.ColorObject.OriginalColorRect.Height, GraphicsUnit.Pixel, Me.SolidBrushManageObject.back_image_ia)

        If Me.colorValue_ <> Color.Empty Then
            Me.SolidBrushManageObject.Common_sb.Color = Color.FromArgb(Byte.MaxValue, Me.colorValue_)
            g.FillRectangle(Me.SolidBrushManageObject.Common_sb, New Rectangle(Me.ColorObject.OriginalColorRect.X, Me.ColorObject.OriginalColorRect.Y, Me.ColorObject.OriginalColorRect.Width / 2, Me.ColorObject.OriginalColorRect.Height))
            Me.SolidBrushManageObject.Common_sb.Color = Me.colorValue_
            g.FillRectangle(Me.SolidBrushManageObject.Common_sb, New Rectangle(Me.ColorObject.OriginalColorRect.X + Me.ColorObject.OriginalColorRect.Width / 2, Me.ColorObject.OriginalColorRect.Y, Me.ColorObject.OriginalColorRect.Width / 2, Me.ColorObject.OriginalColorRect.Height))
        End If

        g.DrawRectangle(Me.SolidBrushManageObject.Border_pen, Me.ColorObject.OriginalColorRect)
        g.DrawLine(Me.SolidBrushManageObject.Border_pen, New Point(Me.ColorObject.OriginalColorRect.X + Me.ColorObject.OriginalColorRect.Width / 2, Me.ColorObject.OriginalColorRect.Y), New Point(Me.ColorObject.OriginalColorRect.X + Me.ColorObject.OriginalColorRect.Width / 2, Me.ColorObject.OriginalColorRect.Bottom))
        Dim bottom_custom_back_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.mbottomBarBtnBackDisabledColor, (If(Me.ColorObject.CustomBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnBackEnterColor_, Me.mbottomBarBtnBackColor)))
        Dim bottom_custom_fore_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.bottomBarBtnForeDisabledColor_, (If(Me.ColorObject.CustomBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnForeEnterColor_, Me.mbottomBarBtnForeColor)))
        Me.SolidBrushManageObject.Common_sb.Color = bottom_custom_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CustomBtn.Rect)
        Me.SolidBrushManageObject.Common_sb.Color = bottom_custom_fore_color
        g.DrawString(Me.ColorObject.CustomBtn.Text, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.CustomBtn.Rect, text_center_sf)
        Dim bottom_clear_back_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.mbottomBarBtnBackDisabledColor, (If(Me.ColorObject.ClearBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnBackEnterColor_, Me.mbottomBarBtnBackColor)))
        Dim bottom_clear_fore_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.bottomBarBtnForeDisabledColor_, (If(Me.ColorObject.ClearBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnForeEnterColor_, Me.mbottomBarBtnForeColor)))
        Me.SolidBrushManageObject.Common_sb.Color = bottom_clear_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ClearBtn.Rect)
        Me.SolidBrushManageObject.Common_sb.Color = bottom_clear_fore_color
        g.DrawString(Me.ColorObject.ClearBtn.Text, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ClearBtn.Rect, text_center_sf)
        Dim bottom_confirm_back_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.mbottomBarBtnBackDisabledColor, (If(Me.ColorObject.ConfirmBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnBackEnterColor_, Me.mbottomBarBtnBackColor)))
        Dim bottom_confirm_fore_color As Color = If((Me.mcolorReadOnly OrElse Not Me.Enabled), Me.bottomBarBtnForeDisabledColor_, (If(Me.ColorObject.ConfirmBtn.MoveStatus = ColorItemMoveStatuss.Enter, Me.bottomBarBtnForeEnterColor_, Me.mbottomBarBtnForeColor)))
        Me.SolidBrushManageObject.Common_sb.Color = bottom_confirm_back_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ConfirmBtn.Rect)
        Me.SolidBrushManageObject.Common_sb.Color = bottom_confirm_fore_color
        g.DrawString(Me.ColorObject.ConfirmBtn.Text, Me.SolidBrushManageObject.Text_font, Me.SolidBrushManageObject.Common_sb, Me.ColorObject.ConfirmBtn.Rect, text_center_sf)
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        MyBase.OnLostFocus(e)
        If Me.DesignMode Then Return
        If Me.SolidBrushManageObject IsNot Nothing Then Me.SolidBrushManageObject.ReleaseSolidBrushs()
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If Me.DesignMode Then Return
        If Me.mcolorReadOnly Then Return

        If e.Button = System.Windows.Forms.MouseButtons.Left Then
            Me.Focus()
            Dim point As Point = Me.PointToClient(Control.MousePosition)

            If Me.ColorObject.DefaultColorBtn.Rect.Contains(point) AndAlso Me.mcolorType <> McolorTypes.[Default] Then
                Me.mcolorType = McolorTypes.[Default]
                Me.Invalidate()
            ElseIf Me.ColorObject.HtmlColorBtn.Rect.Contains(point) AndAlso Me.mcolorType <> McolorTypes.Html Then
                Me.mcolorType = McolorTypes.Html
                Me.Invalidate()
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.ThemeDown Then

                If Me.ColorObject.ThemeRect.Contains(point) Then

                    For i As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(1) - 1

                            If Me.ColorObject.ThemeColorsItem(i, j).Rect.Contains(point) Then

                                If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                                    Me.ColorObject.CurrentValue_A_SlideValue = 255
                                    Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
                                End If

                                Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, ColorManage.ThemeColors(Me.ColorObject.ThemeColorsItem(i, j).ColorRowIndex, Me.ColorObject.ThemeColorsItem(i, j).ColorColIndex))
                                Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
                                Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
                                Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
                                Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
                                Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
                                Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
                                Me.UpdateColorText()
                                Me.gradual_color = ColorManage.ThemeColors(Me.ColorObject.ThemeColorsItem(i, j).ColorRowIndex, Me.ColorObject.ThemeColorsItem(i, j).ColorColIndex)
                                Me.ColorObject.GradualSelectPoint = Point.Empty
                                Me.Update_Gradual_Image()
                                Me.OnThemeColorItemClick(New ColorItemClickEventArgs() With {
                                    .Item = Me.ColorObject.ThemeColorsItem(i, j)
                                })
                                Me.Invalidate()
                            End If
                        Next
                    Next
                End If
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.StandardDown Then

                If Me.ColorObject.StandardRect.Contains(point) Then

                    For i As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(1) - 1

                            If Me.ColorObject.StandardColorsItem(i, j).Rect.Contains(point) Then

                                If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                                    Me.ColorObject.CurrentValue_A_SlideValue = 255
                                    Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
                                End If

                                Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, ColorManage.StandardColors(Me.ColorObject.StandardColorsItem(i, j).ColorRowIndex, Me.ColorObject.StandardColorsItem(i, j).ColorColIndex))
                                Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
                                Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
                                Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
                                Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
                                Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
                                Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
                                Me.UpdateColorText()
                                Me.gradual_color = ColorManage.StandardColors(Me.ColorObject.StandardColorsItem(i, j).ColorRowIndex, Me.ColorObject.StandardColorsItem(i, j).ColorColIndex)
                                Me.ColorObject.GradualSelectPoint = Point.Empty
                                Me.Update_Gradual_Image()
                                Me.OnStandardColorItemClick(New ColorItemClickEventArgs() With {
                                    .Item = Me.ColorObject.StandardColorsItem(i, j)
                                })
                                Me.Invalidate()
                            End If
                        Next
                    Next
                End If
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.HtmlDown Then
                Dim html_gp As New GraphicsPath()
                Dim html_r As New Region()

                For i As Integer = 0 To Me.ColorObject.HtmlColorsItem.Count - 1

                    For j As Integer = 0 To Me.ColorObject.HtmlColorsItem(i).ColorsRects.Count - 1
                        'Dim isselect As Boolean
                        html_gp.Reset()
                        html_gp.AddPolygon(Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs)
                        html_r.MakeEmpty()
                        html_r.Union(html_gp)

                        If html_r.IsVisible(point) Then

                            If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                                Me.ColorObject.CurrentValue_A_SlideValue = 255
                                Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
                            End If

                            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, ColorManage.HtmlColors(i).Colors(j))
                            Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
                            Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
                            Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
                            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
                            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
                            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
                            Me.UpdateColorText()
                            Me.gradual_color = ColorManage.HtmlColors(i).Colors(j)
                            Me.ColorObject.GradualSelectPoint = Point.Empty
                            Me.Update_Gradual_Image()
                            Me.OnHtmlColorItemClick(New HtmlColorItemClickEventArgs() With {
                                .Item = Me.ColorObject.HtmlColorsItem(i).ColorsRects(j)
                            })
                            Me.Invalidate()
                            'isselect = True
                            Exit For
                        End If

                        'If isselect Then
                        '    Exit For
                        'End If
                    Next
                Next

                html_gp.Dispose()
                html_r.Dispose()
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.CustomDown Then

                If Me.ColorObject.CustomRect.Contains(point) Then

                    For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1

                            If Me.ColorObject.CustomColorsItem(i, j).Rect.Contains(point) Then
                                Me.custom_select_row_index = i
                                Me.custom_select_cel_index = j

                                If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                                    Me.ColorObject.CurrentValue_A_SlideValue = 255
                                    Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
                                End If

                                Me.currentValue = ColorManage.CustomColors(Me.ColorObject.CustomColorsItem(i, j).ColorRowIndex, Me.ColorObject.CustomColorsItem(i, j).ColorColIndex)
                                Me.ColorObject.CurrentValue_A_SlideValue = Me.currentValue.A
                                Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
                                Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
                                Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
                                Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
                                Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
                                Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
                                Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
                                Me.UpdateColorText()
                                Me.gradual_color = ColorManage.CustomColors(Me.ColorObject.CustomColorsItem(i, j).ColorRowIndex, Me.ColorObject.CustomColorsItem(i, j).ColorColIndex)
                                Me.ColorObject.GradualSelectPoint = Point.Empty
                                Me.Update_Gradual_Image()
                                Me.OnCustomColorItemClick(New ColorItemClickEventArgs() With {
                                    .Item = Me.ColorObject.CustomColorsItem(i, j)
                                })
                                Me.Invalidate()
                            End If
                        Next
                    Next
                End If
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.CustomDown Then

                If Me.ColorObject.CustomBtn.Rect.Contains(point) Then
                    Me.OnCustomClick(New BottomBarIiemClickEventArgs() With {
                        .Item = Me.ColorObject.CustomBtn
                    })
                End If
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.ClearDown Then

                If Me.ColorObject.ClearBtn.Rect.Contains(point) Then
                    Me.OnClearClick(New BottomBarIiemClickEventArgs() With {
                        .Item = Me.ColorObject.ClearBtn
                    })
                End If
            End If

            If Me.colorMoveStatus = ColorMoveStatuss.ConfirmDown Then

                If Me.ColorObject.ConfirmBtn.Rect.Contains(point) Then
                    Me.OnConfirmClick(New BottomBarIiemClickEventArgs() With {
                        .Item = Me.ColorObject.ConfirmBtn
                    })
                End If
            End If
        End If
        UpdateColorText()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If Me.DesignMode Then Return
        If Me.mcolorReadOnly Then Return

        If Me.ColorObject.ColorRect.Contains(e.Location) Then

            If Me.mcolorType = McolorTypes.[Default] Then

                If Me.ColorObject.ThemeRect.Contains(e.Location) Then
                    Me.colorMoveStatus = ColorMoveStatuss.ThemeDown
                ElseIf Me.ColorObject.StandardRect.Contains(e.Location) Then
                    Me.colorMoveStatus = ColorMoveStatuss.StandardDown
                End If
            ElseIf Me.mcolorType = McolorTypes.Html Then
                Me.colorMoveStatus = ColorMoveStatuss.HtmlDown
            End If
        ElseIf Me.ColorObject.CustomRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.CustomDown

            For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

                For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1

                    If Me.ColorObject.CustomColorsItem(i, j).Rect.Contains(e.Location) Then
                        Me.custom_select_row_index = i
                        Me.custom_select_cel_index = j
                    End If
                Next
            Next
        ElseIf Me.ColorObject.GradualRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.GradualDown
            Me.Calculate_GradualSelectPoint_Value(e.Location)

            If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                Me.ColorObject.CurrentValue_A_SlideValue = 255
                Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
            End If

            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, Me.gradual_bmp.GetPixel(Me.ColorObject.GradualSelectPoint.X, Me.ColorObject.GradualSelectPoint.Y))
            Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
            Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
            Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.Invalidate()
        ElseIf Me.ColorObject.GradualBarRect.Contains(e.Location) OrElse Me.ColorObject.GradualBarSlideRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.GradualBarDown
            Me.Calculate_GradualBar_Value(e.Location)
            Dim color As Color = Me.gradual_bar_bmp.GetPixel(Me.ColorObject.GradualBarRect.Width / 2, Me.ColorObject.GradualBarSlideValue)

            If Me.ColorObject.CurrentValue_A_SlideValue = 0 Then
                Me.ColorObject.CurrentValue_A_SlideValue = 255
                Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
            End If

            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, color)
            Me.gradual_color = color
            Me.ColorObject.GradualSelectPoint = Point.Empty
            Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
            Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
            Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
            Me.Update_GradualBar_Rect(Me.ColorObject.GradualBarSlideValue)
            Me.Update_Gradual_Image()
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.ColorObject.CurrentValue_A_Rect.Contains(e.Location) OrElse Me.ColorObject.CurrentValue_A_SlideRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.ADown
            Me.Calculate_A_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, Me.currentValue)
            Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
            Me.Invalidate()
        ElseIf Me.ColorObject.CurrentValue_R_Rect.Contains(e.Location) OrElse Me.ColorObject.CurrentValue_R_SlideRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.RDown
            Me.Calculate_R_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.ColorObject.CurrentValue_R_SlideValue, Me.currentValue.G, Me.currentValue.B)
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.Invalidate()
        ElseIf Me.ColorObject.CurrentValue_G_Rect.Contains(e.Location) OrElse Me.ColorObject.CurrentValue_G_SlideRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.GDown
            Me.Calculate_G_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.currentValue.R, Me.ColorObject.CurrentValue_G_SlideValue, Me.currentValue.B)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.Invalidate()
        ElseIf Me.ColorObject.CurrentValue_B_Rect.Contains(e.Location) OrElse Me.ColorObject.CurrentValue_B_SlideRect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.BDown
            Me.Calculate_B_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.currentValue.R, Me.currentValue.G, Me.ColorObject.CurrentValue_B_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.Invalidate()
        ElseIf Me.ColorObject.CustomBtn.Rect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.CustomDown
        ElseIf Me.ColorObject.ClearBtn.Rect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.ClearDown
        ElseIf Me.ColorObject.ConfirmBtn.Rect.Contains(e.Location) Then
            Me.colorMoveStatus = ColorMoveStatuss.ConfirmDown
        Else
            Me.colorMoveStatus = ColorMoveStatuss.Normal
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        MyBase.OnMouseDown(e)
        If Me.mcolorReadOnly Then Return
        Me.colorMoveStatus = ColorMoveStatuss.Normal
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        MyBase.OnMouseDown(e)
        If Me.mcolorReadOnly Then Return

        If Me.colorMoveStatus = ColorMoveStatuss.Normal Then
            Dim isenter As Boolean = False

            If Me.ColorObject.DefaultColorBtn.Rect.Contains(e.Location) OrElse Me.ColorObject.HtmlColorBtn.Rect.Contains(e.Location) Then
                isenter = True
            End If

            If Me.mcolorType = McolorTypes.[Default] Then

                If Me.ColorObject.ThemeRect.Contains(e.Location) Then

                    For i As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(1) - 1

                            If Me.ColorObject.ThemeColorsItem(i, j).Rect.Contains(e.Location) Then
                                Me.ColorObject.ThemeColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter
                            Else
                                Me.ColorObject.ThemeColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                            End If
                        Next
                    Next
                Else

                    For i As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(1) - 1
                            Me.ColorObject.ThemeColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                        Next
                    Next
                End If

                If Me.ColorObject.StandardRect.Contains(e.Location) Then

                    For i As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(1) - 1

                            If Me.ColorObject.StandardColorsItem(i, j).Rect.Contains(e.Location) Then
                                Me.ColorObject.StandardColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter
                            Else
                                Me.ColorObject.StandardColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                            End If
                        Next
                    Next
                Else

                    For i As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(0) - 1

                        For j As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(1) - 1
                            Me.ColorObject.StandardColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                        Next
                    Next
                End If
            End If

            If Me.ColorObject.CustomRect.Contains(e.Location) Then

                For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

                    For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1

                        If Me.ColorObject.CustomColorsItem(i, j).Rect.Contains(e.Location) Then
                            Me.ColorObject.CustomColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Enter
                        Else
                            Me.ColorObject.CustomColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                        End If
                    Next
                Next
            Else

                For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

                    For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1
                        Me.ColorObject.CustomColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
                    Next
                Next
            End If

            If Me.ColorObject.CustomBtn.Rect.Contains(e.Location) Then
                Me.ColorObject.CustomBtn.MoveStatus = ColorItemMoveStatuss.Enter
                isenter = True
            Else
                Me.ColorObject.CustomBtn.MoveStatus = ColorItemMoveStatuss.Normal
            End If

            If Me.ColorObject.ClearBtn.Rect.Contains(e.Location) Then
                Me.ColorObject.ClearBtn.MoveStatus = ColorItemMoveStatuss.Enter
                isenter = True
            Else
                Me.ColorObject.ClearBtn.MoveStatus = ColorItemMoveStatuss.Normal
            End If

            If Me.ColorObject.ConfirmBtn.Rect.Contains(e.Location) Then
                Me.ColorObject.ConfirmBtn.MoveStatus = ColorItemMoveStatuss.Enter
                isenter = True
            Else
                Me.ColorObject.ConfirmBtn.MoveStatus = ColorItemMoveStatuss.Normal
            End If

            Me.Cursor = If(isenter, Cursors.Hand, Cursors.[Default])
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.GradualDown Then
            Me.Calculate_GradualSelectPoint_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, Me.gradual_bmp.GetPixel(Me.ColorObject.GradualSelectPoint.X, Me.ColorObject.GradualSelectPoint.Y))
            Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
            Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
            Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.GradualBarDown Then
            Me.Calculate_GradualBar_Value(e.Location)
            Dim color As Color = Me.gradual_bar_bmp.GetPixel(Me.ColorObject.GradualBarRect.Width / 2, Me.ColorObject.GradualBarSlideValue)
            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, color)
            Me.gradual_color = color
            Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
            Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
            Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
            Me.Update_GradualBar_Rect(Me.ColorObject.GradualBarSlideValue)
            Me.Update_Gradual_Image()
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.ADown Then
            Me.Calculate_A_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.ColorObject.CurrentValue_A_SlideValue, Me.currentValue)
            Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.RDown Then
            Me.Calculate_R_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.ColorObject.CurrentValue_R_SlideValue, Me.currentValue.G, Me.currentValue.B)
            Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.GDown Then
            Me.Calculate_G_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.currentValue.R, Me.ColorObject.CurrentValue_G_SlideValue, Me.currentValue.B)
            Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        ElseIf Me.colorMoveStatus = ColorMoveStatuss.BDown Then
            Me.Calculate_B_Value(e.Location)
            Me.currentValue = Color.FromArgb(Me.currentValue.A, Me.currentValue.R, Me.currentValue.G, Me.ColorObject.CurrentValue_B_SlideValue)
            Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
            Me.UpdateColorText()
            Me.Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Me.InitializeControlRectangle()
        Me.Invalidate()
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        Dim scale_width As Integer = CInt((Me.DefaultSize.Width))
        Dim scale_height As Integer = CInt((Me.DefaultSize.Height))
        MyBase.SetBoundsCore(x, y, scale_width, scale_height, specified)
        Me.Invalidate()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Me.SolidBrushManageObject IsNot Nothing Then Me.SolidBrushManageObject.ReleaseSolidBrushs()
            If Me.gradual_bmp IsNot Nothing Then Me.gradual_bmp.Dispose()
            If Me.gradual_bar_bmp IsNot Nothing Then Me.gradual_bar_bmp.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Protected Overridable Sub OnColorValueChanged(ByVal e As ColorValueChangedEventArgs)
        RaiseEvent ColorValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnHtmlColorItemClick(ByVal e As HtmlColorItemClickEventArgs)
        RaiseEvent HtmlColorItemClick(Me, e)
    End Sub

    Protected Overridable Sub OnThemeColorItemClick(ByVal e As ColorItemClickEventArgs)
        RaiseEvent ThemeColorItemClick(Me, e)
    End Sub

    Protected Overridable Sub OnStandardColorItemClick(ByVal e As ColorItemClickEventArgs)
        RaiseEvent StandardColorItemClick(Me, e)
    End Sub

    Protected Overridable Sub OnCustomColorItemClick(ByVal e As ColorItemClickEventArgs)
        RaiseEvent CustomColorItemClick(Me, e)
    End Sub

    Protected Overridable Sub OnCustomClick(ByVal e As BottomBarIiemClickEventArgs)
        ColorManage.CustomColors(Me.custom_select_row_index, Me.custom_select_cel_index) = Me.currentValue
        Dim row As Integer = ColorManage.CustomColors.GetLength(0)
        Dim cel As Integer = ColorManage.CustomColors.GetLength(1)

        If Me.custom_select_cel_index < cel - 1 Then
            Me.custom_select_cel_index += 1
        ElseIf Me.custom_select_row_index = 0 Then
            Me.custom_select_row_index = 1
            Me.custom_select_cel_index = 0
        ElseIf Me.custom_select_row_index = 1 Then
            Me.custom_select_row_index = 0
            Me.custom_select_cel_index = 0
        End If

        Me.Invalidate()
        RaiseEvent BottomBarCustomClick(Me, e)
    End Sub

    Protected Overridable Sub OnClearClick(ByVal e As BottomBarIiemClickEventArgs)
        ' Me.colorValue_ = Color.Empty
        Me.ColorValue = Color.Empty
        RaiseEvent BottomBarClearClick(Me, e)
    End Sub

    Protected Overridable Sub OnConfirmClick(ByVal e As BottomBarIiemClickEventArgs)
        'Me.colorValue_ = Me.currentValue
        Me.ColorValue = Me.currentValue
        Me.colorTextBox.Text=""
        RaiseEvent BottomBarConfirmClick(Me, e)
    End Sub

    Private Sub ColorTextBox_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim color As Color? = ValidColor(Me.colorTextBox.Text)

        If Not color.HasValue Then
            ' Me.UpdateColorText()
            ColorValue = color
        End If
    End Sub

    Private Sub ColorTextBox_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim color As Color? = ValidColor(Me.colorTextBox.Text)

        If color.HasValue Then
            Me.UpdateColorInputValue(color.Value)
        End If
    End Sub

    Private Sub InitializeControlRectangle()
        Dim color_width As Integer = CInt((203))
        Dim color_height As Integer = CInt((212))
        Dim top_btn_width As Integer = CInt((70))
        Dim top_btn_height As Integer = CInt((20))
        Dim theme_title_height As Integer = CInt((30))
        Dim theme_item_width As Integer = CInt((14))
        Dim theme_item_height As Integer = CInt((14))
        Dim standard_title_height As Integer = CInt((30))
        Dim standard_item_width As Integer = CInt((14))
        Dim standard_item_height As Integer = CInt((14))
        Dim html_item_side As Integer = CInt((8))
        Dim custom_title_height As Integer = CInt((30))
        Dim custom_item_width As Integer = CInt((14))
        Dim custom_item_height As Integer = CInt((14))
        Dim gradual_width As Integer = CInt((200))
        Dim gradual_height As Integer = CInt((135))
        Dim gradualbar_width As Integer = CInt((30))
        Dim gradualbar_height As Integer = CInt((220))
        Dim gradualbar_slide_height As Integer = CInt((6))
        Dim argb_width As Integer = CInt((180))
        Dim argb_height As Integer = CInt((12))
        Dim argb_slide_width As Integer = CInt((6))
        Dim current_text_height As Integer = CInt((20))
        Dim bottom_btn_width As Integer = CInt((50))
        Dim bottom_btn_height As Integer = CInt((30))
        Me.ColorObject.ColorRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ClientRectangle.Y + Me.Padding.Top, color_width, color_height)
        Dim scale_colorRect_left_padding As Integer = CInt((10))
        Me.ColorObject.DefaultColorBtn.Rect = New Rectangle(Me.ColorObject.ColorRect.Left + scale_colorRect_left_padding, Me.ClientRectangle.Y + Me.Padding.Top, top_btn_width, top_btn_height)
        Me.ColorObject.HtmlColorBtn.Rect = New Rectangle(Me.ColorObject.DefaultColorBtn.Rect.Right, Me.ClientRectangle.Y + Me.Padding.Top, top_btn_width, top_btn_height)
        Dim theme_color_rect_width As Integer = theme_item_width * Me.ColorObject.ThemeColorsItem.GetLength(0) + (theme_item_width / 2 * (Me.ColorObject.ThemeColorsItem.GetLength(0) - 1))
        Dim theme_color_rect_height As Integer = theme_item_height * Me.ColorObject.ThemeColorsItem.GetLength(1) + theme_item_height / 2
        Me.ColorObject.ThemeTitleRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ClientRectangle.Y + Me.Padding.Top + top_btn_height, theme_color_rect_width, theme_title_height)
        Me.ColorObject.ThemeRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ColorObject.ThemeTitleRect.Bottom, theme_color_rect_width, theme_color_rect_height)

        For i As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(0) - 1

            For j As Integer = 0 To Me.ColorObject.ThemeColorsItem.GetLength(1) - 1
                Dim x As Integer = Me.ColorObject.ThemeRect.X + (theme_item_width / 2 + theme_item_width) * i
                Dim y As Integer = Me.ColorObject.ThemeTitleRect.Bottom + theme_item_height * j + (If(j > 0, theme_item_height / 2, 0))
                Me.ColorObject.ThemeColorsItem(i, j).Rect = New Rectangle(x, y, theme_item_width, theme_item_height)
                Me.ColorObject.ThemeColorsItem(i, j).ColorRowIndex = i
                Me.ColorObject.ThemeColorsItem(i, j).ColorColIndex = j
                Me.ColorObject.ThemeColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
            Next
        Next

        Dim tandard_color_rect_width As Integer = standard_item_width * Me.ColorObject.StandardColorsItem.GetLength(1) + (standard_item_width / 2 * (Me.ColorObject.StandardColorsItem.GetLength(1) - 1))
        Dim tandard_color_rect_height As Integer = standard_item_height * Me.ColorObject.StandardColorsItem.GetLength(0) + (standard_item_height / 2 * (Me.ColorObject.StandardColorsItem.GetLength(0) - 1))
        Me.ColorObject.StandardTitleRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ColorObject.ThemeRect.Bottom, tandard_color_rect_width, standard_title_height)
        Me.ColorObject.StandardRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ColorObject.StandardTitleRect.Bottom, tandard_color_rect_width, tandard_color_rect_height)

        For i As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(0) - 1

            For j As Integer = 0 To Me.ColorObject.StandardColorsItem.GetLength(1) - 1
                Dim x As Integer = Me.ColorObject.ThemeRect.X + (theme_item_width / 2 + theme_item_width) * j
                Dim y As Integer = Me.ColorObject.StandardTitleRect.Bottom + theme_item_height * i + (If(i > 0, theme_item_height / 2, 0))
                Me.ColorObject.StandardColorsItem(i, j).Rect = New Rectangle(x, y, theme_item_width, theme_item_height)
                Me.ColorObject.StandardColorsItem(i, j).ColorRowIndex = i
                Me.ColorObject.StandardColorsItem(i, j).ColorColIndex = j
                Me.ColorObject.StandardColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
            Next
        Next

        Dim custom_color_rect_width As Integer = custom_item_width * Me.ColorObject.CustomColorsItem.GetLength(1) + (custom_item_width / 2 * (Me.ColorObject.CustomColorsItem.GetLength(1) - 1))
        Dim custom_color_rect_height As Integer = custom_item_height * Me.ColorObject.CustomColorsItem.GetLength(0) + (custom_item_height / 2 * (Me.ColorObject.CustomColorsItem.GetLength(0) - 1))
        Me.ColorObject.CustomTitleRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ColorObject.StandardRect.Bottom, tandard_color_rect_width, custom_title_height)
        Me.ColorObject.CustomRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left, Me.ColorObject.CustomTitleRect.Bottom, tandard_color_rect_width, tandard_color_rect_height)

        For i As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(0) - 1

            For j As Integer = 0 To Me.ColorObject.CustomColorsItem.GetLength(1) - 1
                Dim x As Integer = Me.ColorObject.ThemeRect.X + (theme_item_width / 2 + theme_item_width) * j
                Dim y As Integer = Me.ColorObject.CustomTitleRect.Bottom + theme_item_height * i + (If(i > 0, theme_item_height / 2, 0))
                Me.ColorObject.CustomColorsItem(i, j).Rect = New Rectangle(x, y, theme_item_width, theme_item_height)
                Me.ColorObject.CustomColorsItem(i, j).ColorRowIndex = i
                Me.ColorObject.CustomColorsItem(i, j).ColorColIndex = j
                Me.ColorObject.CustomColorsItem(i, j).MoveStatus = ColorItemMoveStatuss.Normal
            Next
        Next

        Dim html_w As Double = html_item_side * Math.Cos(2 * Math.PI / 360 * 30)
        Dim html_h As Double = html_item_side * Math.Sin(2 * Math.PI / 360 * 30)
        Dim html_top As Double = CInt((10))
        Dim html_left As Double = (Me.ColorObject.ColorRect.Width - (Me.ColorObject.HtmlColorsItem(Me.ColorObject.HtmlColorsItem.Count / 2).ColorsRects.Count * html_w * 2)) / 2

        For i As Integer = 0 To Me.ColorObject.HtmlColorsItem.Count - 1
            Dim num As Integer = Math.Abs(Me.ColorObject.HtmlColorsItem.Count / 2 - i)

            For j As Integer = 0 To Me.ColorObject.HtmlColorsItem(i).ColorsRects.Count - 1
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(0) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2 + html_w)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side))))
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(1) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2 + html_w + html_w)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side) + html_h)))
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(2) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2 + html_w + html_w)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side) + html_h + html_item_side)))
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(3) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2 + html_w)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side) + html_h + html_item_side + html_h)))
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(4) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side) + html_h + html_item_side)))
                Me.ColorObject.HtmlColorsItem(i).ColorsRects(j).pointfs(5) = New PointF(CSng((Me.ClientRectangle.Left + html_left + num * html_w + j * html_w * 2)), CSng((Me.ColorObject.DefaultColorBtn.Rect.Bottom + html_top + i * (html_h + html_item_side) + html_h)))
            Next
        Next

        Dim scale_padding As Integer = CInt((5))
        Me.ColorObject.GradualRect = New Rectangle(Me.ColorObject.ThemeRect.Right + scale_padding * 2, Me.ClientRectangle.Y + Me.Padding.Top + scale_padding, gradual_width, gradual_height)
        Me.ColorObject.GradualBarRect = New Rectangle(Me.ColorObject.GradualRect.Right + scale_padding * 2, Me.ClientRectangle.Y + Me.Padding.Top + scale_padding, gradualbar_width, gradualbar_height)
        Me.ColorObject.GradualBarSlideRect = New Rectangle(Me.ColorObject.GradualBarRect.X, Me.ColorObject.GradualBarRect.Y + gradualbar_slide_height / 2, Me.ColorObject.GradualBarRect.Width, gradualbar_slide_height)
        Me.ColorObject.CurrentValue_A_Rect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.GradualRect.Bottom + scale_padding * 2, argb_width, argb_height)
        Me.ColorObject.CurrentValue_A_SlideRect = New Rectangle(Me.ColorObject.CurrentValue_A_Rect.Right - argb_slide_width / 2, Me.ColorObject.CurrentValue_A_Rect.Y, argb_slide_width, Me.ColorObject.CurrentValue_A_Rect.Height)
        Me.ColorObject.CurrentValue_R_Rect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.CurrentValue_A_Rect.Bottom + scale_padding * 2, argb_width, argb_height)
        Me.ColorObject.CurrentValue_R_SlideRect = New Rectangle(Me.ColorObject.CurrentValue_R_Rect.Right - argb_slide_width / 2, Me.ColorObject.CurrentValue_R_Rect.Y, argb_slide_width, Me.ColorObject.CurrentValue_R_Rect.Height)
        Me.ColorObject.CurrentValue_G_Rect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.CurrentValue_R_Rect.Bottom + scale_padding * 2, argb_width, argb_height)
        Me.ColorObject.CurrentValue_G_SlideRect = New Rectangle(Me.ColorObject.CurrentValue_G_Rect.Right - argb_slide_width / 2, Me.ColorObject.CurrentValue_G_Rect.Y, argb_slide_width, Me.ColorObject.CurrentValue_G_Rect.Height)
        Me.ColorObject.CurrentValue_B_Rect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.CurrentValue_G_Rect.Bottom + scale_padding * 2, argb_width, argb_height)
        Me.ColorObject.CurrentValue_B_SlideRect = New Rectangle(Me.ColorObject.CurrentValue_B_Rect.Right - argb_slide_width / 2, Me.ColorObject.CurrentValue_B_Rect.Y, argb_slide_width, Me.ColorObject.CurrentValue_B_Rect.Height)
        Me.ColorObject.CurrentColorRect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.CurrentValue_B_SlideRect.Bottom + scale_padding * 2, current_text_height * 2, current_text_height)
        Me.ColorObject.CurrentColorTextRect = New Rectangle(Me.ColorObject.CurrentColorRect.Right, Me.ColorObject.CurrentValue_B_SlideRect.Bottom + scale_padding * 2, tandard_color_rect_width, current_text_height)
        Me.ColorObject.OriginalColorRect = New Rectangle(Me.ColorObject.GradualRect.X, Me.ColorObject.CurrentColorRect.Bottom + scale_padding, current_text_height * 2, current_text_height)
        Me.ColorObject.OriginalColorTextRect = New Rectangle(Me.ColorObject.CurrentColorRect.Right, Me.ColorObject.CurrentColorRect.Bottom + scale_padding, tandard_color_rect_width, current_text_height)
        Me.ColorObject.ConfirmBtn.Rect = New Rectangle(Me.ClientRectangle.Right - bottom_btn_width - scale_padding, Me.ClientRectangle.Bottom - bottom_btn_height - scale_padding, bottom_btn_width, bottom_btn_height)
        Me.ColorObject.ClearBtn.Rect = New Rectangle(Me.ColorObject.ConfirmBtn.Rect.X - scale_padding - bottom_btn_width, Me.ClientRectangle.Bottom - bottom_btn_height - scale_padding, bottom_btn_width, bottom_btn_height)
        Me.ColorObject.CustomBtn.Rect = New Rectangle(Me.ColorObject.ClearBtn.Rect.X - scale_padding - scale_padding * 4 - bottom_btn_width, Me.ClientRectangle.Bottom - bottom_btn_height - scale_padding, bottom_btn_width + scale_padding * 4, bottom_btn_height)
    End Sub

    Private Sub Calculate_GradualSelectPoint_Value(ByVal point As Point)
        Dim x As Integer = point.X - Me.ColorObject.GradualRect.X
        If x < 0 Then x = 0
        If x > Me.ColorObject.GradualRect.Width - 1 Then x = Me.ColorObject.GradualRect.Width - 1
        Dim y As Integer = point.Y - Me.ColorObject.GradualRect.Y
        If y < 0 Then y = 0
        If y > Me.ColorObject.GradualRect.Height - 1 Then y = Me.ColorObject.GradualRect.Height - 1
        Me.ColorObject.GradualSelectPoint = New Point(x, y)
    End Sub

    Private Sub Calculate_GradualBar_Value(ByVal point As Point)
        Dim sum As Integer = Me.ColorObject.GradualBarRect.Height - 1
        Dim s As Integer = CInt((sum * (point.Y - Me.ColorObject.GradualBarRect.Y) / CSng(Me.ColorObject.GradualBarRect.Height)))
        If s < 0 Then s = 0
        If s > sum Then s = sum
        Me.ColorObject.GradualBarSlideValue = s
    End Sub

    Private Sub Calculate_A_Value(ByVal point As Point)
        Dim a As Integer = CInt((255 * (point.X - Me.ColorObject.CurrentValue_A_Rect.X) / CSng(Me.ColorObject.CurrentValue_A_Rect.Width)))
        If a < Byte.MinValue Then a = Byte.MinValue
        If a > Byte.MaxValue Then a = Byte.MaxValue
        Me.ColorObject.CurrentValue_A_SlideValue = a
    End Sub

    Private Sub Calculate_R_Value(ByVal point As Point)
        Dim r As Integer = CInt((255 * (point.X - Me.ColorObject.CurrentValue_R_Rect.X) / CSng(Me.ColorObject.CurrentValue_R_Rect.Width)))
        If r < Byte.MinValue Then r = Byte.MinValue
        If r > Byte.MaxValue Then r = Byte.MaxValue
        Me.ColorObject.CurrentValue_R_SlideValue = r
    End Sub

    Private Sub Calculate_G_Value(ByVal point As Point)
        Dim g As Integer = CInt((255 * (point.X - Me.ColorObject.CurrentValue_G_Rect.X) / CSng(Me.ColorObject.CurrentValue_G_Rect.Width)))
        If g < Byte.MinValue Then g = Byte.MinValue
        If g > Byte.MaxValue Then g = Byte.MaxValue
        Me.ColorObject.CurrentValue_G_SlideValue = g
    End Sub

    Private Sub Calculate_B_Value(ByVal point As Point)
        Dim b As Integer = CInt((255 * (point.X - Me.ColorObject.CurrentValue_B_Rect.X) / CSng(Me.ColorObject.CurrentValue_B_Rect.Width)))
        If b < Byte.MinValue Then b = Byte.MinValue
        If b > Byte.MaxValue Then b = Byte.MaxValue
        Me.ColorObject.CurrentValue_B_SlideValue = b
    End Sub

    Private Sub Update_GradualBar_Rect(ByVal s As Integer)
        Dim sum As Single = Me.ColorObject.GradualBarRect.Height - 1
        Me.ColorObject.GradualBarSlideRect.Y = CInt((Me.ColorObject.GradualBarRect.Y + (Me.ColorObject.GradualBarRect.Height * s / sum) - Me.ColorObject.GradualBarSlideRect.Height / 2))
    End Sub

    Private Sub Update_A_Rect(ByVal a As Integer)
        Me.ColorObject.CurrentValue_A_SlideRect.X = CInt((Me.ColorObject.CurrentValue_A_Rect.X + (Me.ColorObject.CurrentValue_A_Rect.Width * a / 255.0F) - Me.ColorObject.CurrentValue_A_SlideRect.Width / 2))
    End Sub

    Private Sub Update_R_Rect(ByVal r As Integer)
        Me.ColorObject.CurrentValue_R_SlideRect.X = CInt((Me.ColorObject.CurrentValue_R_Rect.X + (Me.ColorObject.CurrentValue_R_Rect.Width * r / 255.0F) - Me.ColorObject.CurrentValue_R_SlideRect.Width / 2))
    End Sub

    Private Sub Update_G_Rect(ByVal g As Integer)
        Me.ColorObject.CurrentValue_G_SlideRect.X = CInt((Me.ColorObject.CurrentValue_G_Rect.X + (Me.ColorObject.CurrentValue_G_Rect.Width * g / 255.0F) - Me.ColorObject.CurrentValue_G_SlideRect.Width / 2))
    End Sub

    Private Sub Update_B_Rect(ByVal b As Integer)
        Me.ColorObject.CurrentValue_B_SlideRect.X = CInt((Me.ColorObject.CurrentValue_B_Rect.X + (Me.ColorObject.CurrentValue_B_Rect.Width * b / 255.0F) - Me.ColorObject.CurrentValue_B_SlideRect.Width / 2))
    End Sub

    Private Sub Update_Gradual_Image()
        Dim bmp_rect As New Rectangle(0, 0, Me.gradual_bmp.Width, Me.gradual_bmp.Height)
        Dim g As Graphics = Graphics.FromImage(Me.gradual_bmp)
        Me.SolidBrushManageObject.Common_sb.Color = Me.gradual_color
        g.FillRectangle(Me.SolidBrushManageObject.Common_sb, bmp_rect)
        g.FillRectangle(Me.SolidBrushManageObject.Gradual_h_lgb, bmp_rect)
        g.FillRectangle(Me.SolidBrushManageObject.Gradual_v_lgb, bmp_rect)
        g.Dispose()
    End Sub

    Private Sub Update_GradualBar_Image()
        Dim barbmp_rect As New Rectangle(0, 0, Me.gradual_bar_bmp.Width, Me.gradual_bar_bmp.Height)
        Dim g As Graphics = Graphics.FromImage(Me.gradual_bar_bmp)
        Dim gradualbar_lgb As New LinearGradientBrush(Me.ColorObject.GradualBarRect, Color.Transparent, Color.Transparent, 90) With {
            .InterpolationColors = New ColorBlend() With {
                .Colors = ColorManage.GradualBarcolors,
                .Positions = ColorManage.GradualBarInterval
            }
        }
        g.FillRectangle(gradualbar_lgb, barbmp_rect)
        gradualbar_lgb.Dispose()
        g.Dispose()
    End Sub

    Private Sub UpdateLocationSize()
        Me.colorTextBox.Location = New Point(0, 0)
    End Sub

    Friend Function GetColorTextBoxRect() As Rectangle
        Return New Rectangle(Me.ColorObject.CurrentColorTextRect.X + CInt((60)), Me.ColorObject.CurrentColorTextRect.Y + CInt((3)), CInt((colorTextWidth)), Me.ColorObject.CurrentColorTextRect.Height)
    End Function

    Private Sub UpdateColorText()
        Me.colorTextBox.Text = String.Format("{0},{1},{2},{3}", Me.ColorObject.CurrentValue_A_SlideValue, Me.ColorObject.CurrentValue_R_SlideValue, Me.ColorObject.CurrentValue_G_SlideValue, Me.ColorObject.CurrentValue_B_SlideValue)
    End Sub

    Private Sub UpdateColorInputValue(ByVal color As Color)
        Me.ColorObject.CurrentValue_A_SlideValue = color.A
        Me.ColorObject.CurrentValue_R_SlideValue = color.R
        Me.ColorObject.CurrentValue_G_SlideValue = color.G
        Me.ColorObject.CurrentValue_B_SlideValue = color.B
        Me.Update_R_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
        Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
        Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
        Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
        Me.gradual_color = color
        Me.ColorObject.GradualSelectPoint = Point.Empty
        Me.Update_Gradual_Image()
        Me.Invalidate()
    End Sub

    Public Sub InitializeColor()
        Me.currentValue = Me.colorValue_
        ' Me.defaultColorValue = Me.colorValue_
        Me.ColorObject.CurrentValue_A_SlideValue = Me.currentValue.A
        Me.ColorObject.CurrentValue_R_SlideValue = Me.currentValue.R
        Me.ColorObject.CurrentValue_G_SlideValue = Me.currentValue.G
        Me.ColorObject.CurrentValue_B_SlideValue = Me.currentValue.B
        Me.gradual_color = Color.FromArgb(Byte.MaxValue, Me.currentValue)
        Me.ColorObject.GradualSelectPoint = Point.Empty
        Me.Update_Gradual_Image()
        Me.Update_A_Rect(Me.ColorObject.CurrentValue_A_SlideValue)
        Me.Update_R_Rect(Me.ColorObject.CurrentValue_R_SlideValue)
        Me.Update_G_Rect(Me.ColorObject.CurrentValue_G_SlideValue)
        Me.Update_B_Rect(Me.ColorObject.CurrentValue_B_SlideValue)
        Me.UpdateColorText()
    End Sub

    Public Shared Function ValidColor(ByVal mcolor As String) As Color?
        mcolor = mcolor.Replace(" ", "")
        Dim color_tmp As Color? = Nothing
        Dim argb_reg As String = "^((2[0-4][0-9]|25[0-5]|[01]?[0-9][0-9]?),){2}((2[0-4][0-9]|25[0-5]|[01]?[0-9][0-9]?),)?(2[0-4][0-9]|25[0-5]|[01]?[0-9][0-9]?)$"
        Dim h16_reg As String = "^#([0-9a-fA-F]{6}[0-9a-fA-F]{8}|[0-9a-fA-F]{3}|[0-9a-fA-F]{4})$"

        If Regex.IsMatch(mcolor, argb_reg) Then
            Dim color_arr As String() = mcolor.Split(","c)

            If color_arr.Length = 3 Then
                color_tmp = Color.FromArgb(255, Integer.Parse(color_arr(0)), Integer.Parse(color_arr(1)), Integer.Parse(color_arr(2)))
            ElseIf color_arr.Length = 4 Then
                color_tmp = Color.FromArgb(Integer.Parse(color_arr(0)), Integer.Parse(color_arr(1)), Integer.Parse(color_arr(2)), Integer.Parse(color_arr(3)))
            End If
        ElseIf Regex.IsMatch(mcolor, h16_reg) Then
            color_tmp = ColorTranslator.FromHtml(mcolor)
        End If

        Return color_tmp
    End Function

    Public Sub UpdateDateValueNotInvalidate(ByVal color As Color)
        If Me.colorValue_ = color Then Return
        Dim arg As New ColorValueChangedEventArgs() With {
            .OldColorValue = Me.colorValue_,
            .NewColorValue = color
        }
        Me.colorValue_ = color

        Me.OnColorValueChanged(arg)
    End Sub

    <Description("顶部选项")>
    Public Class TopBarItemClass
        Public parent As ColorPickerExt
        Public ower As ColorClass

        Public Sub New(ByVal parent As ColorPickerExt, ByVal ower As ColorClass)
            Me.parent = parent
            Me.ower = ower
        End Sub

        Public Rect As Rectangle
        Public Text As String
    End Class

    <Description("底部选项")>
    Public Class BottomBarItemClass
        Public parent As ColorPickerExt
        Public ower As ColorClass

        Public Sub New(ByVal parent As ColorPickerExt, ByVal ower As ColorClass)
            Me.parent = parent
            Me.ower = ower
        End Sub

        Public Rect As Rectangle
        Public Text As String
        Private moveStatus_ As ColorItemMoveStatuss = ColorItemMoveStatuss.Normal

        <DefaultValue(ColorItemMoveStatuss.Normal)>
        <Description("底部选项鼠标状态")>
        Public Property MoveStatus As ColorItemMoveStatuss
            Get
                Return Me.moveStatus_
            End Get
            Set(ByVal value As ColorItemMoveStatuss)
                If Me.moveStatus_ = value Then Return
                Me.moveStatus_ = value
                Me.parent？.Invalidate()
            End Set
        End Property
    End Class

    <Description("颜色面板")>
    Public Class ColorClass
        Public parent As ColorPickerExt

        Public Sub New(ByVal parent As ColorPickerExt)
            Me.parent = parent
            Me.DefaultColorBtn = New TopBarItemClass(parent, Me) With {
                .Text = "Default"
            }
            Me.HtmlColorBtn = New TopBarItemClass(parent, Me) With {
                .Text = "Html"
            }
            Me.ThemeColorsItem = New ColorItemClass(ColorManage.ThemeColors.GetLength(0) - 1, ColorManage.ThemeColors.GetLength(1) - 1) {}

            For i As Integer = 0 To ColorManage.ThemeColors.GetLength(0) - 1

                For j As Integer = 0 To ColorManage.ThemeColors.GetLength(1) - 1
                    Me.ThemeColorsItem(i, j) = New ColorItemClass(parent, Me)
                Next
            Next

            Me.StandardColorsItem = New ColorItemClass(ColorManage.StandardColors.GetLength(0) - 1, ColorManage.StandardColors.GetLength(1) - 1) {}

            For i As Integer = 0 To ColorManage.StandardColors.GetLength(0) - 1

                For j As Integer = 0 To ColorManage.StandardColors.GetLength(1) - 1
                    Me.StandardColorsItem(i, j) = New ColorItemClass(parent, Me)
                Next
            Next

            Me.HtmlColorsItem = New List(Of HtmlColorsRectItem)()

            For i As Integer = 0 To ColorManage.HtmlColors.Count - 1
                Dim RectItem As New HtmlColorsRectItem()

                For j As Integer = 0 To ColorManage.HtmlColors(i).Colors.Count - 1
                    RectItem.ColorsRects.Add(New HtmlColorsRectPointItem() With {
                        .pointfs = New PointF(5) {}
                    })
                Next

                HtmlColorsItem.Add(RectItem)
            Next

            Me.CustomColorsItem = New ColorItemClass(ColorManage.CustomColors.GetLength(0) - 1, ColorManage.CustomColors.GetLength(1) - 1) {}

            For i As Integer = 0 To ColorManage.CustomColors.GetLength(0) - 1

                For j As Integer = 0 To ColorManage.CustomColors.GetLength(1) - 1
                    Me.CustomColorsItem(i, j) = New ColorItemClass(parent, Me)
                Next
            Next

            Me.CustomBtn = New BottomBarItemClass(parent, Me) With {
                .Text = "Custom",
                .MoveStatus = ColorItemMoveStatuss.Normal
            }
            Me.ClearBtn = New BottomBarItemClass(parent, Me) With {
                .Text = "Clear",
                .MoveStatus = ColorItemMoveStatuss.Normal
            }
            Me.ConfirmBtn = New BottomBarItemClass(parent, Me) With {
                .Text = "OK",
                .MoveStatus = ColorItemMoveStatuss.Normal
            }
        End Sub

        Public DefaultColorBtn As TopBarItemClass
        Public HtmlColorBtn As TopBarItemClass
        Public ColorRect As Rectangle
        Public ThemeTitleRect As Rectangle
        Public ThemeRect As Rectangle
        Public ThemeColorsItem As ColorItemClass(,)
        Public StandardTitleRect As Rectangle
        Public StandardRect As Rectangle
        Public StandardColorsItem As ColorItemClass(,)
        Public HtmlColorsItem As List(Of HtmlColorsRectItem)
        Public CustomTitleRect As Rectangle
        Public CustomRect As Rectangle
        Public CustomColorsItem As ColorItemClass(,)
        Public CurrentColorTextRect As Rectangle
        Public CurrentColorRect As Rectangle
        Public OriginalColorTextRect As Rectangle
        Public OriginalColorRect As Rectangle
        Public GradualRect As Rectangle
        Public GradualSelectPoint As Point = Point.Empty
        Public GradualBarRect As Rectangle
        Public GradualBarSlideRect As Rectangle
        Public GradualBarSlideValue As Integer
        Public CurrentValue_A_Rect As Rectangle
        Public CurrentValue_A_SlideRect As Rectangle
        Public CurrentValue_A_SlideValue As Integer
        Public CurrentValue_R_Rect As Rectangle
        Public CurrentValue_R_SlideRect As Rectangle
        Public CurrentValue_R_SlideValue As Integer
        Public CurrentValue_G_Rect As Rectangle
        Public CurrentValue_G_SlideRect As Rectangle
        Public CurrentValue_G_SlideValue As Integer
        Public CurrentValue_B_Rect As Rectangle
        Public CurrentValue_B_SlideRect As Rectangle
        Public CurrentValue_B_SlideValue As Integer
        Public CustomBtn As BottomBarItemClass
        Public ClearBtn As BottomBarItemClass
        Public ConfirmBtn As BottomBarItemClass
    End Class

    <Description("颜色面板选项")>
    Public Class ColorItemClass
        Public parent As ColorPickerExt
        Public ower As ColorClass

        Public Sub New(ByVal parent As ColorPickerExt, ByVal ower As ColorClass)
            Me.parent = parent
            Me.ower = ower
        End Sub

        Public Rect As Rectangle
        Public ColorRowIndex As Integer
        Public ColorColIndex As Integer
        Private moveStatus_ As ColorItemMoveStatuss = ColorItemMoveStatuss.Normal

        <DefaultValue(ColorItemMoveStatuss.Normal)>
        <Description("选项鼠标状态")>
        Public Property MoveStatus As ColorItemMoveStatuss
            Get
                Return Me.moveStatus_
            End Get
            Set(ByVal value As ColorItemMoveStatuss)
                If Me.moveStatus_ = value Then Return
                Me.moveStatus_ = value
                Me.parent？.Invalidate()
            End Set
        End Property
    End Class

    <Description("html颜色集合")>
    Public Class HtmlColorsItem
        Public Colors As New List(Of Color)()
    End Class

    <Description("html颜色选项集合")>
    Public Class HtmlColorsRectItem
        Public ColorsRects As New List(Of HtmlColorsRectPointItem)()
    End Class

    <Description("html颜色选项")>
    Public Class HtmlColorsRectPointItem
        Public pointfs As PointF()
    End Class

    <Description("画笔管理")>
    Public Class SolidBrushManage
        Private ReadOnly ower As ColorPickerExt

        Public Sub New(ByVal ower As ColorPickerExt)
            Me.ower = ower
        End Sub

        Private ReadOnly Property Gradual_rect As Rectangle
            Get
                Return New Rectangle(0, 0, Me.ower.ColorObject.GradualRect.Width - 1, Me.ower.ColorObject.GradualRect.Height - 1)
            End Get
        End Property

        Private ReadOnly Property Argb_lgb_rect As Rectangle
            Get
                Return New Rectangle(Me.ower.ColorObject.CurrentValue_A_Rect.X - 1, 0, Me.ower.ColorObject.CurrentValue_A_Rect.Width, Me.ower.ColorObject.CurrentValue_A_Rect.Height)
            End Get
        End Property

        Private _back_image_ia As ImageAttributes = Nothing

        Public ReadOnly Property Back_image_ia As ImageAttributes
            Get

                If Me._back_image_ia Is Nothing Then
                    Me._back_image_ia = New ImageAttributes()
                    Me._back_image_ia.SetWrapMode(WrapMode.Tile)
                End If

                Return Me._back_image_ia
            End Get
        End Property

        Private _gradual_h_lgb As LinearGradientBrush = Nothing

        Public ReadOnly Property Gradual_h_lgb As LinearGradientBrush
            Get
                If Me._gradual_h_lgb Is Nothing Then Me._gradual_h_lgb = New LinearGradientBrush(Me.Gradual_rect, Color.White, Color.Transparent, 0F)
                Return Me._gradual_h_lgb
            End Get
        End Property

        Private _gradual_v_lgb As LinearGradientBrush = Nothing

        Public ReadOnly Property Gradual_v_lgb As LinearGradientBrush
            Get
                If Me._gradual_v_lgb Is Nothing Then Me._gradual_v_lgb = New LinearGradientBrush(Me.Gradual_rect, Color.Transparent, Color.Black, 90.0F)
                Return Me._gradual_v_lgb
            End Get
        End Property

        Private _argb_lgb As LinearGradientBrush = Nothing

        Public ReadOnly Property Argb_lgb As LinearGradientBrush
            Get
                If Me._argb_lgb Is Nothing Then Me._argb_lgb = New LinearGradientBrush(Me.Argb_lgb_rect, Color.Transparent, Color.Yellow, 0F)
                Return Me._argb_lgb
            End Get
        End Property

        Private _border_pen As Pen = Nothing

        Public ReadOnly Property Border_pen As Pen
            Get
                If Me._border_pen Is Nothing Then Me._border_pen = New Pen(Color.FromArgb(100, 102, 102, 102), 1)
                Return Me._border_pen
            End Get
        End Property

        Private _border_ts_pen As Pen = Nothing

        Public ReadOnly Property Border_ts_pen As Pen
            Get
                If Me._border_ts_pen Is Nothing Then Me._border_ts_pen = New Pen(Color.FromArgb(255, 193, 7), 1)
                Return Me._border_ts_pen
            End Get
        End Property

        Private _border_slide_pen As Pen = Nothing

        Public ReadOnly Property Border_slide_pen As Pen
            Get
                If Me._border_slide_pen Is Nothing Then Me._border_slide_pen = New Pen(Color.FromArgb(200, 105, 105, 105), 1)
                Return Me._border_slide_pen
            End Get
        End Property

        Private _common_sb As SolidBrush = Nothing

        Public ReadOnly Property Common_sb As SolidBrush
            Get
                If Me._common_sb Is Nothing Then Me._common_sb = New SolidBrush(Color.White)
                Return Me._common_sb
            End Get
        End Property

        Private _common_pen As Pen = Nothing

        Public ReadOnly Property Common_pen As Pen
            Get
                If Me._common_pen Is Nothing Then Me._common_pen = New Pen(Color.White, 1)
                Return Me._common_pen
            End Get
        End Property

        Private _text_font As Font = Nothing

        Public ReadOnly Property Text_font As Font
            Get
                If Me._text_font Is Nothing Then Me._text_font = New Font("微软雅黑", 9)
                Return Me._text_font
            End Get
        End Property

        Public Sub ReleaseSolidBrushs()
            If Me._back_image_ia IsNot Nothing Then
                Me._back_image_ia.Dispose()
                Me._back_image_ia = Nothing
            End If

            If Me.Gradual_h_lgb IsNot Nothing Then
                Me._gradual_h_lgb.Dispose()
                Me._gradual_h_lgb = Nothing
            End If

            If Me.Gradual_v_lgb IsNot Nothing Then
                Me._gradual_v_lgb.Dispose()
                Me._gradual_v_lgb = Nothing
            End If

            If Me.Argb_lgb IsNot Nothing Then
                Me._argb_lgb.Dispose()
                Me._argb_lgb = Nothing
            End If

            If Me.Border_pen IsNot Nothing Then
                Me._border_pen.Dispose()
                Me._border_pen = Nothing
            End If

            If Me.Border_ts_pen IsNot Nothing Then
                Me._border_ts_pen.Dispose()
                Me._border_ts_pen = Nothing
            End If

            If Me.Border_slide_pen IsNot Nothing Then
                Me._border_slide_pen.Dispose()
                Me._border_slide_pen = Nothing
            End If

            If Me.Common_sb IsNot Nothing Then
                Me._common_sb.Dispose()
                Me._common_sb = Nothing
            End If

            If Me.Common_pen IsNot Nothing Then
                Me._common_pen.Dispose()
                Me._common_pen = Nothing
            End If

            If Me.Text_font IsNot Nothing Then
                Me._text_font.Dispose()
                Me._text_font = Nothing
            End If
        End Sub
    End Class

    <Description("颜色表管理")>
    Class ColorManage
        Public Shared ReadOnly ThemeColors As Color(,) = New Color(9, 5) {
        {Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242), Color.FromArgb(216, 216, 216), Color.FromArgb(191, 191, 191), Color.FromArgb(165, 165, 165), Color.FromArgb(127, 127, 127)},
        {Color.FromArgb(0, 0, 0), Color.FromArgb(127, 127, 127), Color.FromArgb(89, 89, 89), Color.FromArgb(63, 63, 63), Color.FromArgb(38, 38, 38), Color.FromArgb(12, 12, 12)},
        {Color.FromArgb(238, 236, 225), Color.FromArgb(221, 217, 195), Color.FromArgb(196, 189, 151), Color.FromArgb(147, 137, 83), Color.FromArgb(73, 68, 41), Color.FromArgb(29, 27, 16)},
        {Color.FromArgb(31, 73, 125), Color.FromArgb(198, 217, 240), Color.FromArgb(141, 179, 226), Color.FromArgb(84, 141, 212), Color.FromArgb(23, 54, 93), Color.FromArgb(15, 36, 62)},
        {Color.FromArgb(79, 129, 189), Color.FromArgb(219, 229, 241), Color.FromArgb(184, 204, 228), Color.FromArgb(149, 179, 215), Color.FromArgb(54, 96, 146), Color.FromArgb(36, 64, 97)},
        {Color.FromArgb(192, 80, 77), Color.FromArgb(242, 220, 219), Color.FromArgb(229, 185, 183), Color.FromArgb(217, 150, 148), Color.FromArgb(149, 55, 52), Color.FromArgb(99, 36, 35)},
        {Color.FromArgb(155, 187, 89), Color.FromArgb(235, 241, 221), Color.FromArgb(215, 227, 188), Color.FromArgb(195, 214, 155), Color.FromArgb(118, 146, 60), Color.FromArgb(79, 97, 40)},
        {Color.FromArgb(128, 100, 162), Color.FromArgb(229, 224, 236), Color.FromArgb(204, 193, 217), Color.FromArgb(178, 162, 199), Color.FromArgb(95, 73, 122), Color.FromArgb(63, 49, 81)},
        {Color.FromArgb(75, 172, 198), Color.FromArgb(219, 238, 243), Color.FromArgb(183, 221, 232), Color.FromArgb(146, 205, 220), Color.FromArgb(49, 133, 155), Color.FromArgb(32, 88, 103)},
        {Color.FromArgb(247, 150, 70), Color.FromArgb(253, 234, 218), Color.FromArgb(251, 213, 181), Color.FromArgb(250, 192, 143), Color.FromArgb(227, 108, 9), Color.FromArgb(151, 72, 6)}}
        Public Shared ReadOnly StandardColors As Color(,) = New Color(1, 9) {
        {Color.FromArgb(244, 67, 54), Color.FromArgb(233, 30, 99), Color.FromArgb(160, 115, 232), Color.FromArgb(156, 39, 176), Color.FromArgb(103, 58, 183), Color.FromArgb(63, 81, 181), Color.FromArgb(33, 150, 243), Color.FromArgb(33, 150, 243), Color.FromArgb(0, 188, 212), Color.FromArgb(158, 158, 158)},
        {Color.FromArgb(1, 255, 255), Color.FromArgb(0, 150, 136), Color.FromArgb(76, 175, 80), Color.FromArgb(139, 195, 74), Color.FromArgb(205, 220, 57), Color.FromArgb(255, 235, 59), Color.FromArgb(255, 193, 7), Color.FromArgb(255, 152, 0), Color.FromArgb(255, 87, 34), Color.FromArgb(121, 85, 72)}}
        Public Shared ReadOnly CustomColors As Color(,) = New Color(1, 9) {
        {Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0)},
        {Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0)}}
        Public Shared ReadOnly GradualBarcolors As Color() = New Color(6) {Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0)}
        Public Shared ReadOnly GradualBarInterval As Single() = New Single(6) {0.0F, 0.17F, 0.33F, 0.5F, 0.67F, 0.83F, 1.0F}
        Public Shared ReadOnly HtmlColors As New List(Of HtmlColorsItem)() From {
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#003366"),
                    ColorTranslator.FromHtml("#336699"),
                    ColorTranslator.FromHtml("#3366cc"),
                    ColorTranslator.FromHtml("#003399"),
                    ColorTranslator.FromHtml("#000099"),
                    ColorTranslator.FromHtml("#0000cc"),
                    ColorTranslator.FromHtml("#000066")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#006666"),
                    ColorTranslator.FromHtml("#006699"),
                    ColorTranslator.FromHtml("#0099cc"),
                    ColorTranslator.FromHtml("#0066cc"),
                    ColorTranslator.FromHtml("#0033cc"),
                    ColorTranslator.FromHtml("#0000ff"),
                    ColorTranslator.FromHtml("#3333ff"),
                    ColorTranslator.FromHtml("#333399")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#669999"),
                    ColorTranslator.FromHtml("#009999"),
                    ColorTranslator.FromHtml("#33cccc"),
                    ColorTranslator.FromHtml("#00ccff"),
                    ColorTranslator.FromHtml("#0099ff"),
                    ColorTranslator.FromHtml("#0066ff"),
                    ColorTranslator.FromHtml("#3366ff"),
                    ColorTranslator.FromHtml("#3333cc"),
                    ColorTranslator.FromHtml("#666699")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#339966"),
                    ColorTranslator.FromHtml("#00cc99"),
                    ColorTranslator.FromHtml("#00ffcc"),
                    ColorTranslator.FromHtml("#00ffff"),
                    ColorTranslator.FromHtml("#33ccff"),
                    ColorTranslator.FromHtml("#3399ff"),
                    ColorTranslator.FromHtml("#6699ff"),
                    ColorTranslator.FromHtml("#6666ff"),
                    ColorTranslator.FromHtml("#6600ff"),
                    ColorTranslator.FromHtml("#6600cc")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#339933"),
                    ColorTranslator.FromHtml("#00cc66"),
                    ColorTranslator.FromHtml("#00ff99"),
                    ColorTranslator.FromHtml("#66ffcc"),
                    ColorTranslator.FromHtml("#66ffff"),
                    ColorTranslator.FromHtml("#66ccff"),
                    ColorTranslator.FromHtml("#99ccff"),
                    ColorTranslator.FromHtml("#9999ff"),
                    ColorTranslator.FromHtml("#9966ff"),
                    ColorTranslator.FromHtml("#9933ff"),
                    ColorTranslator.FromHtml("#9900ff")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#006600"),
                    ColorTranslator.FromHtml("#00cc00"),
                    ColorTranslator.FromHtml("#00ff00"),
                    ColorTranslator.FromHtml("#66ff99"),
                    ColorTranslator.FromHtml("#99ffcc"),
                    ColorTranslator.FromHtml("#ccffff"),
                    ColorTranslator.FromHtml("#ccccff"),
                    ColorTranslator.FromHtml("#cc99ff"),
                    ColorTranslator.FromHtml("#cc66ff"),
                    ColorTranslator.FromHtml("#cc33ff"),
                    ColorTranslator.FromHtml("#cc00ff"),
                    ColorTranslator.FromHtml("#9900cc")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#003300"),
                    ColorTranslator.FromHtml("#009933"),
                    ColorTranslator.FromHtml("#33cc33"),
                    ColorTranslator.FromHtml("#66ff66"),
                    ColorTranslator.FromHtml("#99ff99"),
                    ColorTranslator.FromHtml("#ccffcc"),
                    ColorTranslator.FromHtml("#ffffff"),
                    ColorTranslator.FromHtml("#ffccff"),
                    ColorTranslator.FromHtml("#ff99ff"),
                    ColorTranslator.FromHtml("#ff66ff"),
                    ColorTranslator.FromHtml("#ff00ff"),
                    ColorTranslator.FromHtml("#cc00cc"),
                    ColorTranslator.FromHtml("#660066")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#333300"),
                    ColorTranslator.FromHtml("#009900"),
                    ColorTranslator.FromHtml("#66ff33"),
                    ColorTranslator.FromHtml("#99ff66"),
                    ColorTranslator.FromHtml("#ccff99"),
                    ColorTranslator.FromHtml("#ffffcc"),
                    ColorTranslator.FromHtml("#ffcccc"),
                    ColorTranslator.FromHtml("#ff99cc"),
                    ColorTranslator.FromHtml("#ff66cc"),
                    ColorTranslator.FromHtml("#ff33cc"),
                    ColorTranslator.FromHtml("#cc0099"),
                    ColorTranslator.FromHtml("#993399")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#336600"),
                    ColorTranslator.FromHtml("#669900"),
                    ColorTranslator.FromHtml("#99ff33"),
                    ColorTranslator.FromHtml("#ccff66"),
                    ColorTranslator.FromHtml("#ffff99"),
                    ColorTranslator.FromHtml("#ffcc99"),
                    ColorTranslator.FromHtml("#ff9999"),
                    ColorTranslator.FromHtml("#ff6699"),
                    ColorTranslator.FromHtml("#ff3399"),
                    ColorTranslator.FromHtml("#cc3399"),
                    ColorTranslator.FromHtml("#990099")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#666633"),
                    ColorTranslator.FromHtml("#99cc00"),
                    ColorTranslator.FromHtml("#ccff33"),
                    ColorTranslator.FromHtml("#ffff66"),
                    ColorTranslator.FromHtml("#ffcc66"),
                    ColorTranslator.FromHtml("#ff9966"),
                    ColorTranslator.FromHtml("#ff6666"),
                    ColorTranslator.FromHtml("#ff0066"),
                    ColorTranslator.FromHtml("#d60094"),
                    ColorTranslator.FromHtml("#993366")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#a58800"),
                    ColorTranslator.FromHtml("#cccc00"),
                    ColorTranslator.FromHtml("#ffff00"),
                    ColorTranslator.FromHtml("#ffcc00"),
                    ColorTranslator.FromHtml("#ff9933"),
                    ColorTranslator.FromHtml("#ff6600"),
                    ColorTranslator.FromHtml("#ff0033"),
                    ColorTranslator.FromHtml("#cc0066"),
                    ColorTranslator.FromHtml("#660033")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#996633"),
                    ColorTranslator.FromHtml("#cc9900"),
                    ColorTranslator.FromHtml("#ff9900"),
                    ColorTranslator.FromHtml("#cc6600"),
                    ColorTranslator.FromHtml("#ff3300"),
                    ColorTranslator.FromHtml("#ff0000"),
                    ColorTranslator.FromHtml("#cc0000"),
                    ColorTranslator.FromHtml("#990033")
                }
            },
            New HtmlColorsItem() With {
                .Colors = New List(Of Color)() From {
                    ColorTranslator.FromHtml("#663300"),
                    ColorTranslator.FromHtml("#996600"),
                    ColorTranslator.FromHtml("#cc3300"),
                    ColorTranslator.FromHtml("#993300"),
                    ColorTranslator.FromHtml("#990000"),
                    ColorTranslator.FromHtml("#800000"),
                    ColorTranslator.FromHtml("#993333")
                }
            }
        }
    End Class

    <Description("颜色值更改事件参数")>
    Public Class ColorValueChangedEventArgs
        Inherits EventArgs

        <Description("更改前颜色")>
        Public Property OldColorValue As Color
        <Description("更改后颜色")>
        Public Property NewColorValue As Color
    End Class

    <Description("html选项单击事件参数")>
    Public Class HtmlColorItemClickEventArgs
        Inherits EventArgs

        <Description("html颜色面板选项")>
        Public Property Item As HtmlColorsRectPointItem
    End Class

    <Description("颜色选项单击事件参数")>
    Public Class ColorItemClickEventArgs
        Inherits EventArgs

        <Description("颜色面板选项")>
        Public Property Item As ColorItemClass
    End Class

    <Description("底部选项单击事件参数")>
    Public Class BottomBarIiemClickEventArgs
        Inherits EventArgs

        <Description("底部选项")>
        Public Property Item As BottomBarItemClass
    End Class

    <Description("鼠标在选项上状态")>
    Public Enum ColorItemMoveStatuss
        Normal
        Enter
    End Enum

    <Description("鼠标状态")>
    Public Enum ColorMoveStatuss
        Normal
        HtmlDown
        ThemeDown
        StandardDown
        GradualDown
        GradualBarDown
        ADown
        RDown
        GDown
        BDown
        CustomDown
        ClearDown
        ConfirmDown
    End Enum

    <Description("颜色面板选中类型")>
    Public Enum McolorTypes
        [Default]
        Html
    End Enum
End Class
