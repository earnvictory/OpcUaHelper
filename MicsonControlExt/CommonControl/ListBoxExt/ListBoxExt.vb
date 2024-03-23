Imports System.CodeDom
Imports System.ComponentModel
Imports System.ComponentModel.Design

Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Linq.Expressions
Imports System.Net.Sockets
Imports System.Text.RegularExpressions


<ToolboxItem(True)>
<Description("ListBoxExt控件")>
<DefaultProperty("Items")>
<DefaultEvent("ItemClick")>
Public Class ListBoxExt
    Inherits Control


    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property SelectedItems As Item()
        Get


            Return Array.FindAll(Of Item)(Items.GetArray, Function(x) x.Selected)


        End Get
    End Property


    Public Delegate Sub ItemClickEventHandler(ByVal sender As Object, ByVal e As ItemClickEventArgs)


    <Description("选项单击事件")>
    Public Event ItemClick As ItemClickEventHandler

    Public Delegate Sub ItemSelectedChangedEventHandler(ByVal sender As Object, ByVal e As ItemSelectedChangedEventArgs)


    <Description("选项选中状态更改事件")>
    Public Event ItemSelectedChanged As ItemSelectedChangedEventHandler


    Public Delegate Sub SelectedChangedEventHandler(ByVal sender As Object, ByVal e As SelectedIndexChangedEventArgs)


    <Description("选中选项更改事件")>
    Public Event SelectedIndexChanged As SelectedChangedEventHandler


    Public Delegate Sub DrawItemEventHandler(ByVal sender As Object, ByVal e As DrawItemEventArgs)


    <Description("选项自定义绘制事件")>
    Public Event DrawItem As DrawItemEventHandler



    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event MarginChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event PaddingChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event TextChanged As EventHandler


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event RightToLeftChanged As EventHandler

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Event ImeModeChanged As EventHandler

    Private mactivateColor As Color = Color.Gray

    <DefaultValue(GetType(Color), "Gray")>
    <Description("控件激活的虚线框颜色")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ActivateColor As Color
        Get
            Return mactivateColor
        End Get
        Set(ByVal value As Color)
            If mactivateColor = value Then Return
            mactivateColor = value
            Invalidate()
        End Set
    End Property


    Private mdrawType As DrawTypes = DrawTypes.[Default]

    <DefaultValue(DrawTypes.[Default])>
    <Description("选项绘制方式")>
    Public Property DrawType As DrawTypes
        Get
            Return mdrawType
        End Get
        Set(ByVal value As DrawTypes)
            If mdrawType = value Then Return
            mdrawType = value
            Invalidate()
        End Set
    End Property

    Private mborderWidth As Integer = 1

    <DefaultValue(1)>
    <Description("边框宽度")>
    Public Property BorderWidth As Integer
        Get
            Return mborderWidth
        End Get
        Set(ByVal value As Integer)
            If mborderWidth = value OrElse value < 0 Then Return
            mborderWidth = value
            Me.InitializeRectangle()
            Invalidate()
        End Set
    End Property

    Private mborderColor As Color = Color.FromArgb(192, 192, 192)

    <DefaultValue(GetType(Color), "192, 192, 192")>
    <Description(" 边框颜色")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property BorderColor As Color
        Get
            Return mborderColor
        End Get
        Set(ByVal value As Color)
            If Me.BorderColor = value Then Return
            mborderColor = value
            Invalidate()
        End Set
    End Property

    Private mmultiple As Boolean = False

    <DefaultValue(False)>
    <Description("是否多想选中")>
    Public Property Multiple As Boolean
        Get
            Return mmultiple
        End Get
        Set(ByVal value As Boolean)
            If Me.Multiple = value Then Return
            mmultiple = value

            If Me.Multiple = False Then
                Dim readly As Boolean = False

                For Each item As Item In Me.Items

                    If readly Then
                        item.Selected = False
                    Else

                        If item.Selected Then
                            readly = True
                        End If
                    End If
                Next
            End If
        End Set
    End Property

    Private mVerticalscroll As ScrollClass

    <Description("滚动条")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property VerticalScroll As ScrollClass
        Get
            If mVerticalscroll Is Nothing Then mVerticalscroll = New ScrollClass(Me)
            Return mVerticalscroll
        End Get
    End Property

    Private mHorizontalscroll As ScrollClass

    <Description("滚动条")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property Horizontalscroll As ScrollClass
        Get
            If mHorizontalscroll Is Nothing Then mHorizontalscroll = New ScrollClass(Me)
            Return mHorizontalscroll
        End Get
    End Property


    Private mitemCollection As ItemCollection

    <Description("选项集合")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property Items As ItemCollection
        Get
            If mitemCollection Is Nothing Then mitemCollection = New ItemCollection(Me)
            Return mitemCollection
        End Get
    End Property

    Private mitemBHeight As Integer = 24

    <DefaultValue(24)>
    <Description("选项高度")>
    Public Property ItemHeight As Integer
        Get
            If mitemBHeight < (Me.Font.Height + 6) Then
                mitemBHeight = (Me.Font.Height + 6)
                Me.InitializeRectangle()
                Invalidate()
            End If
            Return mitemBHeight
        End Get
        Set(ByVal value As Integer)
            If mitemBHeight = value OrElse value < 0 Then Return
            mitemBHeight = value
            Me.InitializeRectangle()
            Invalidate()
        End Set
    End Property

    Private mitemBWidth As Integer
    <Browsable(False)>
    <DefaultValue(24)>
    <Description("选项高度")>
    Public Property ItemWidth As Integer
        Get

            Return mitemBWidth
        End Get
        Set(ByVal value As Integer)
            If mitemBWidth = value OrElse value < 0 Then Return
            mitemBWidth = value
            Me.InitializeRectangle()
            Invalidate()
        End Set
    End Property



    Private mitemBorderStyle As ItemBorderStyles = ItemBorderStyles.Line

    <DefaultValue(ItemBorderStyles.Line)>
    <Description("选项边框风格")>
    Public Property ItemBorderStyle As ItemBorderStyles
        Get
            Return mitemBorderStyle
        End Get
        Set(ByVal value As ItemBorderStyles)
            If mitemBorderStyle = value Then Return
            mitemBorderStyle = value
            Invalidate()
        End Set
    End Property

    Private mitemBorderColor As Color = Color.FromArgb(224, 224, 224)

    <DefaultValue(GetType(Color), "224, 224, 224")>
    <Description(" 选项边框颜色")>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property ItemBorderColor As Color
        Get
            Return mitemBorderColor
        End Get
        Set(ByVal value As Color)
            If mitemBorderColor = value Then Return
            mitemBorderColor = value
            Invalidate()
        End Set
    End Property

    Private mitemImageShow As Boolean = False

    <DefaultValue(False)>
    <Description("是否显示选项图片")>
    Public Property ItemImageShow As Boolean
        Get
            Return mitemImageShow
        End Get
        Set(ByVal value As Boolean)
            If mitemImageShow = value Then Return
            mitemImageShow = value
            Invalidate()
        End Set
    End Property

    Private mitemImageSize As New Size(22, 22)

    <DefaultValue(GetType(Size), "22,22")>
    <Description("选项图片Size")>
    Public Property ItemImageSize As Size
        Get
            Return mitemImageSize
        End Get
        Set(ByVal value As Size)
            If mitemImageSize = value Then Return
            mitemImageSize = value
            Invalidate()
        End Set
    End Property

    Private mitemImageList As ImageList

    <Description("选项图片List")>
    <RefreshProperties(RefreshProperties.Repaint)>
    Public Property ItemImageList As ImageList
        Get
            Return mitemImageList
        End Get
        Set(ByVal value As ImageList)
            If value Is Nothing OrElse mitemImageList.Equals(value) Then Return
            Dim eventHandler1 As New EventHandler(AddressOf Me.ImageListRecreateHandle)
            Dim eventHandler2 As New EventHandler(AddressOf Me.DetachImageList)

            If Me.ItemImageList IsNot Nothing Then
                RemoveHandler Me.ItemImageList.RecreateHandle, eventHandler1
                RemoveHandler Me.ItemImageList.Disposed, eventHandler2
            End If



            mitemImageList = value
            mitemImageIndex.ImageList = value

            If value IsNot Nothing Then
                AddHandler value.RecreateHandle, eventHandler1
                AddHandler value.Disposed, eventHandler2
            Else
                mitemImageIndex.Index = -1
                mitemImageIndex.Key = String.Empty
            End If

            Invalidate()
        End Set
    End Property

    Private mitemImage As Image = Nothing

    <Description("选项图片")>
    Public Property ItemImage As Image
        Get

            If mitemImage IsNot Nothing Then
                Return mitemImage
            Else

                If Me.ItemImageList IsNot Nothing Then
                    Dim index As Integer = mitemImageIndex.ActualIndex
                    If index >= Me.ItemImageList.Images.Count Then Return Nothing
                    If index >= 0 Then Return Me.ItemImageList.Images(index)
                End If

                Return Nothing
            End If
        End Get
        Set(ByVal value As Image)
            If value Is Nothing OrElse mitemImage.Equals(value) Then Return
            mitemImage = value
            mitemImageIndex.Index = -1
            mitemImageIndex.Key = String.Empty
            Invalidate()
        End Set
    End Property

    Private ReadOnly mitemImageIndex As New Indexer

    <Description("选项图片Index")>
    <DefaultValue(GetType(Integer), "-1")>
    <Localizable(True)>
    <RefreshProperties(RefreshProperties.Repaint)>
    <TypeConverter(GetType(ImageIndexConverter))>
    Public Property ItemImageIndex As Integer
        Get
            If mitemImageIndex.Index <> -1 AndAlso Me.ItemImageList IsNot Nothing AndAlso mitemImageIndex.Index >= Me.ItemImageList.Images.Count Then Return Me.ItemImageList.Images.Count - 1
            Return mitemImageIndex.Index
        End Get
        Set(ByVal value As Integer)
            If mitemImageIndex.Index = value OrElse value < -1 Then Return
            mitemImageIndex.Index = value
            Invalidate()
        End Set
    End Property

    <Description("选项图片Key")>
    <DefaultValue("")>
    <Localizable(True)>
    <RefreshProperties(RefreshProperties.Repaint)>
    <TypeConverter(GetType(ImageKeyConverter))>
    Public Property ItemImageKey As String
        Get
            Return mitemImageIndex.Key
        End Get
        Set(ByVal value As String)
            If mitemImageIndex.Key = value Then Return
            mitemImageIndex.Key = value
            Invalidate()
        End Set
    End Property

    Private mnormalBackColor As Color = Color.FromArgb(255, 255, 255)

    <DefaultValue(GetType(Color), "255, 255, 255")>
    <Description(" 选项背景颜色（正常）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property NormalBackColor As Color
        Get
            Return mnormalBackColor
        End Get
        Set(ByVal value As Color)
            If mnormalBackColor = value Then Return
            mnormalBackColor = value
            Invalidate()
        End Set
    End Property

    Private mnormalTextColor As Color = Color.FromArgb(0, 0, 0)

    <DefaultValue(GetType(Color), "0, 0, 0")>'128,128,128
    <Description(" 选项文本颜色（正常）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property NormalTextColor As Color
        Get
            Return mnormalTextColor
        End Get
        Set(ByVal value As Color)
            If mnormalTextColor = value Then Return
            mnormalTextColor = value
            Invalidate()
        End Set
    End Property

    Private menterBackColor As Color = Color.FromArgb(79, 129, 189) '(189, 208, 188)

    <DefaultValue(GetType(Color), "79, 129, 189")>
    <Description(" 选项背景颜色（鼠标进入）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property EnterBackColor As Color
        Get
            Return menterBackColor
        End Get
        Set(ByVal value As Color)
            If menterBackColor = value Then Return
            menterBackColor = value
            Invalidate()
        End Set
    End Property

    Private menterTextColor As Color = Color.FromArgb(255, 255, 255)

    <DefaultValue(GetType(Color), "255, 255, 255")>
    <Description(" 选项文本颜色（鼠标进入）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property EnterTextColor As Color
        Get
            Return menterTextColor
        End Get
        Set(ByVal value As Color)
            If menterTextColor = value Then Return
            menterTextColor = value
            Invalidate()
        End Set
    End Property

    Private mselectedBackColor As Color = Color.FromArgb(176, 197, 175)

    <DefaultValue(GetType(Color), "176, 197, 175")>
    <Description(" 选项背景颜色（选中）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property SelectedBackColor As Color
        Get
            Return mselectedBackColor
        End Get
        Set(ByVal value As Color)
            If mselectedBackColor = value Then Return
            mselectedBackColor = value
            Invalidate()
        End Set
    End Property

    Private mselectedTextColor As Color = Color.FromArgb(255, 255, 255)

    <DefaultValue(GetType(Color), "255, 255, 255")>
    <Description(" 选项文本颜色（选中）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property SelectedTextColor As Color
        Get
            Return mselectedTextColor
        End Get
        Set(ByVal value As Color)
            If mselectedTextColor = value Then Return
            mselectedTextColor = value
            Invalidate()
        End Set
    End Property

    Private mdisableBackColor As Color = Color.FromArgb(234, 234, 234)

    <DefaultValue(GetType(Color), "234, 234, 234")>
    <Description(" 选项背景颜色（禁止）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property DisableBackColor As Color
        Get
            Return mdisableBackColor
        End Get
        Set(ByVal value As Color)
            If mdisableBackColor = value Then Return
            mdisableBackColor = value
            Invalidate()
        End Set
    End Property

    Private mdisableTextColor As Color = Color.FromArgb(192, 192, 192)

    <DefaultValue(GetType(Color), "192, 192, 192")>
    <Description("选项文本颜色（禁止）")>
    <NotifyParentProperty(True)>
    <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
    Public Property DisableTextColor As Color
        Get
            Return mdisableTextColor
        End Get
        Set(ByVal value As Color)
            If Me.DisableTextColor = value Then Return
            mdisableTextColor = value
            Invalidate()
        End Set
    End Property


    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(100, 200)
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
    Public Overrides Property AutoSize As Boolean
        Get
            Return MyBase.AutoSize
        End Get
        Set(ByVal value As Boolean)
            MyBase.AutoSize = False
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property Margin As Padding
        Get
            Return MyBase.Margin
        End Get
        Set(ByVal value As Padding)
            MyBase.Margin = value
        End Set
    End Property

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads Property Padding As Padding
        Get
            Return MyBase.Padding
        End Get
        Set(ByVal value As Padding)
            MyBase.Padding = value
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

    Private activatedState As Boolean = False
    Private activatedStateIndex As Integer = -1
    Dim mMainRect As Rectangle = Rectangle.Empty
    Property MainRect As Rectangle
        Get
            Return mMainRect
        End Get
        Set(value As Rectangle)
            mMainRect = value
        End Set
    End Property
    Dim mMainRealityRect As Rectangle = Rectangle.Empty
    Property MainRealityRect As Rectangle
        Get
            Return mMainRealityRect
        End Get
        Set(value As Rectangle)
            mMainRealityRect = value
        End Set
    End Property

    Private ismovedown As Boolean = False
    Private movedownpoint As Point = Point.Empty
    Private ReadOnly mousedowninfo As New MouseDownClass()

    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.UpdateStyles()
        Me.AutoSize = False
        Me.InitializeRectangle()
        BackColor = Color.White

    End Sub


    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If DesignMode = False Then

            End If

        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Dim mOpacity As Byte = 255
    Overridable Property Opacity As Byte
        Get
            If DesignMode Then Return 255

            Return mOpacity
        End Get
        Set(value As Byte)
            mOpacity = value
            Invalidate()
        End Set
    End Property
    Property FirstDisplayedScrollingRowIndex As Integer
    Property DisplayedRowCount As Integer
    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        MyBase.OnPaint(e)
        Try
            Dim g As Graphics = ControlExt.Common.Graphics_SetSmoothHighQuality(e.Graphics)
            g.Clear(BackColor)
            Dim scale_itemHeight As Integer = CInt((Me.ItemHeight))
            Dim scale_itemImageSize As New Size(CInt((Me.ItemImageSize.Width)), CInt((Me.ItemImageSize.Height)))
            Dim Verticalscale_scrollThickness As Integer = CInt((Me.VerticalScroll.Thickness))
            Dim Horizontalscale_scrollThickness As Integer = CInt((Me.Horizontalscroll.Thickness))

            If Me.BorderWidth > 0 Then
                Dim border_pen As New Pen(Me.BorderColor, Me.BorderWidth) With {.Alignment = PenAlignment.Inset}
                Dim border As Integer = If(Me.BorderWidth = 1, -1, 0)
                g.DrawRectangle(border_pen, New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, CInt((Me.ClientRectangle.Width + border)), Me.ClientRectangle.Height + border))
                border_pen.Dispose()
            End If

            Dim back_sb As New SolidBrush(Me.BackColor)
            g.FillRectangle(back_sb, Me.MainRect)
            back_sb.Dispose()
            Dim client_region As Region = Nothing
            Dim main_region As Region = Nothing

            If Me.BorderWidth > 0 Then
                client_region = g.Clip.Clone()
                main_region = New Region(Me.MainRect)
                g.Clip = main_region
            End If

            Dim itemborder_pen As Pen = Nothing
            Dim itemborder_lgb As LinearGradientBrush = Nothing

            If Me.ItemBorderStyle = ItemBorderStyles.Line Then
                itemborder_pen = New Pen(Me.ItemBorderColor, 1)
            ElseIf Me.ItemBorderStyle = ItemBorderStyles.GradualLine Then
                itemborder_lgb = New LinearGradientBrush(New PointF(Me.MainRect.X, Me.MainRect.Y), New PointF(Me.MainRect.Right, Me.MainRect.Y), Color.Transparent, Color.Transparent)
                Dim itemborder_cb As New ColorBlend() With {
                  .Colors = New Color() {Color.Transparent, Me.ItemBorderColor, Me.ItemBorderColor, Color.Transparent},
                .Positions = New Single() {0.0F, 0.23F, 0.7F, 1.0F}}
                itemborder_lgb.InterpolationColors = itemborder_cb
                itemborder_pen = New Pen(itemborder_lgb, 1)
            End If

            Dim item_normal_back_sb As LinearGradientBrush = Nothing
            Dim item_enter_back_sb As LinearGradientBrush = Nothing
            Dim item_selected_back_sb As LinearGradientBrush = Nothing
            Dim item_disable_back_sb As LinearGradientBrush = Nothing

            Dim item_normal_text_sb As New SolidBrush(Me.NormalTextColor)
            Dim item_enter_text_sb As New SolidBrush(Me.EnterTextColor)
            Dim item_selected_text_sb As New SolidBrush(Me.SelectedTextColor)
            Dim item_disable_text_sb As New SolidBrush(Me.DisableTextColor)

            Dim MaxItemWidth As Integer = 0
            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Litem.Rect.Bottom >= Me.MainRect.Y AndAlso Litem?.Rect.Y <= Me.MainRect.Bottom Then
                    Dim tSize As SizeF = g.MeasureString(Litem?.Text, Me.Font)
                    If tSize.Width > MaxItemWidth Then
                        MaxItemWidth = tSize.Width + 1
                    End If
                End If
            Next

            ItemWidth = If(MaxItemWidth < MainRect.Width, MainRect.Width, MaxItemWidth)

            Dim mDisplayedRowCount As Integer
            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Litem Is Nothing Then
                    Invalidate()
                    Exit For
                End If
                If Litem Is Nothing OrElse Litem?.Text Is Nothing Then Continue For
                If Litem?.Rect.Equals(RectangleF.Empty) Then Continue For
                If Not (Litem?.Rect.Bottom >= Me.MainRect.Y AndAlso Litem?.Rect.Y <= Me.MainRect.Bottom) Then Continue For

                item_normal_back_sb = New LinearGradientBrush(Litem.Rect, Me.NormalBackColor, NormalBackColor, 90)
                item_enter_back_sb = New LinearGradientBrush(Litem.Rect, Me.EnterBackColor, EnterBackColor, 90)
                item_selected_back_sb = New LinearGradientBrush(Litem.Rect, Me.SelectedBackColor, SelectedBackColor, 90)
                item_disable_back_sb = New LinearGradientBrush(Litem.Rect, Me.DisableBackColor, DisableBackColor, 90)
                If Litem?.Rect.Bottom >= Me.MainRect.Y AndAlso Litem?.Rect.Y <= Me.MainRect.Bottom Then
                    If (Litem?.Rect.Y >= Me.MainRect.Y AndAlso Litem?.Rect.Bottom <= Me.MainRect.Y + ItemHeight + BorderWidth) OrElse (Litem?.Rect.Bottom > Me.MainRect.Y AndAlso Litem?.Rect.Bottom <= Me.MainRect.Y + ItemHeight + BorderWidth) Then
                        FirstDisplayedScrollingRowIndex = i
                    End If
                    mDisplayedRowCount += 1

                    Dim commom_back_sb As LinearGradientBrush
                    Dim commom_text_sb As SolidBrush
                    Dim custom_back_sb As Boolean = False
                    Dim custom_text_sb As Boolean = False
                    Dim commom_image As Image = Nothing

                    If Me.ItemImageShow Then
                        commom_image = If(Litem?.Image, ItemImage)
                    End If

                    If Litem?.Enabled = False Then

                        If Litem?.DisableBackColor = Color.Empty Then
                            commom_back_sb = item_disable_back_sb
                        Else
                            custom_back_sb = True
                            commom_back_sb = New LinearGradientBrush(Litem.Rect, Litem.DisableBackColor, Litem.DisableBackColor, 90)
                        End If

                        If Litem?.DisableTextColor = Color.Empty Then
                            commom_text_sb = item_disable_text_sb
                        Else
                            custom_text_sb = True
                            commom_text_sb = New SolidBrush(Litem?.DisableTextColor)
                        End If
                    Else

                        If Litem?.Selected = True Then

                            If Litem?.SelectedBackColor = Color.Empty Then
                                commom_back_sb = item_selected_back_sb
                            Else
                                custom_back_sb = True
                                commom_back_sb = New LinearGradientBrush(Litem.Rect, Litem.SelectedBackColor, Litem.SelectedBackColor, 90) 'New SolidBrush(Litem?.SelectedBackColor)
                            End If

                            If Litem?.SelectedTextColor = Color.Empty Then
                                commom_text_sb = item_selected_text_sb
                            Else
                                custom_text_sb = True
                                commom_text_sb = New SolidBrush(Litem?.SelectedTextColor)
                            End If
                        Else

                            If Litem?.MouseStatus = ItemMouseStatuss.Enter Then

                                If Litem?.EnterBackColor = Color.Empty Then
                                    commom_back_sb = item_enter_back_sb
                                Else
                                    custom_back_sb = True
                                    commom_back_sb = New LinearGradientBrush(Litem.Rect, Litem.EnterBackColor, Litem.EnterBackColor, 90) 'New SolidBrush(Litem?.EnterBackColor)
                                End If

                                If Litem?.EnterTextColor = Color.Empty Then
                                    commom_text_sb = item_enter_text_sb
                                Else
                                    custom_text_sb = True
                                    commom_text_sb = New SolidBrush(Litem?.EnterTextColor)
                                End If
                            Else

                                If Litem?.NormalBackColor = Color.Empty Then
                                    commom_back_sb = item_normal_back_sb
                                Else
                                    custom_back_sb = True
                                    commom_back_sb = New LinearGradientBrush(Litem.Rect, Litem.NormalBackColor, Litem.NormalBackColor, 90) ' New SolidBrush(Litem?.NormalBackColor)
                                End If

                                If Litem?.NormalTextColor = Color.Empty Then
                                    commom_text_sb = item_normal_text_sb
                                Else
                                    custom_text_sb = True
                                    commom_text_sb = New SolidBrush(Litem?.NormalTextColor)
                                End If
                            End If
                        End If
                    End If

                    If Me.DrawType = DrawTypes.[Default] Then
                        Me.PaintItem(Litem, g, commom_image, itemborder_pen, commom_back_sb, commom_text_sb, scale_itemImageSize, MainRealityRect.X)
                    Else
                        Me.OnDrawItem(New DrawItemEventArgs() With {
                                .Graphics = g,
                                .Image = commom_image,
                                .Item = Litem,
                                .BackBrush = commom_back_sb,
                                .TextBrush = commom_text_sb,
                                .XoffSet = MainRealityRect.X,
                                .BorderPen = itemborder_pen
                            })
                    End If

                    If custom_back_sb AndAlso commom_back_sb IsNot Nothing Then commom_back_sb.Dispose()
                    If custom_text_sb AndAlso commom_text_sb IsNot Nothing Then commom_text_sb.Dispose()
                End If
            Next



            If itemborder_pen IsNot Nothing Then itemborder_pen.Dispose()
            If itemborder_lgb IsNot Nothing Then itemborder_lgb.Dispose()
            If item_normal_back_sb IsNot Nothing Then item_normal_back_sb.Dispose()
            If item_enter_back_sb IsNot Nothing Then item_enter_back_sb.Dispose()
            If item_selected_back_sb IsNot Nothing Then item_selected_back_sb.Dispose()
            If item_disable_back_sb IsNot Nothing Then item_disable_back_sb.Dispose()
            If item_normal_text_sb IsNot Nothing Then item_normal_text_sb.Dispose()
            If item_enter_text_sb IsNot Nothing Then item_enter_text_sb.Dispose()
            If item_selected_text_sb IsNot Nothing Then item_selected_text_sb.Dispose()
            If item_disable_text_sb IsNot Nothing Then item_disable_text_sb.Dispose()

            If Me.MainRealityRect.Height > Me.MainRect.Height Then
                Dim bar_back_sb As SolidBrush
                Dim slide_back_pen As Pen

                If Me.Enabled Then
                    bar_back_sb = New SolidBrush(Me.VerticalScroll.BarNormalBackColor)
                    slide_back_pen = New Pen(If(Me.VerticalScroll.SlideStatus = ScrollSlideMoveStatus.Normal, Me.VerticalScroll.SlideNormalBackColor, Me.VerticalScroll.SlideEnterBackColor), Verticalscale_scrollThickness)
                Else
                    bar_back_sb = New SolidBrush(Me.VerticalScroll.BarDisableBackColor)
                    slide_back_pen = New Pen(Me.VerticalScroll.SlideDisableBackColor, Verticalscale_scrollThickness)
                End If

                g.FillRectangle(bar_back_sb, Me.VerticalScroll.Rect)
                Dim sp_start As New PointF(Me.VerticalScroll.SlideRect.X + Verticalscale_scrollThickness / 2, Me.VerticalScroll.SlideRect.Y)
                Dim sp_end As New PointF(Me.VerticalScroll.SlideRect.X + Verticalscale_scrollThickness / 2, Me.VerticalScroll.SlideRect.Bottom)
                g.DrawLine(slide_back_pen, sp_start, sp_end)
                If bar_back_sb IsNot Nothing Then bar_back_sb.Dispose()
                If slide_back_pen IsNot Nothing Then slide_back_pen.Dispose()
            End If

            If Me.MainRealityRect.Width > Me.MainRect.Width Then
                Dim bar_back_sb As SolidBrush
                Dim slide_back_pen As Pen

                If Me.Enabled Then
                    bar_back_sb = New SolidBrush(Me.Horizontalscroll.BarNormalBackColor)
                    slide_back_pen = New Pen(If(Me.Horizontalscroll.SlideStatus = ScrollSlideMoveStatus.Normal, Me.Horizontalscroll.SlideNormalBackColor, Me.Horizontalscroll.SlideEnterBackColor), Horizontalscale_scrollThickness)
                Else
                    bar_back_sb = New SolidBrush(Me.Horizontalscroll.BarDisableBackColor)
                    slide_back_pen = New Pen(Me.Horizontalscroll.SlideDisableBackColor, Horizontalscale_scrollThickness)
                End If

                g.FillRectangle(bar_back_sb, Me.Horizontalscroll.Rect)
                Dim sp_start As New PointF(Me.Horizontalscroll.SlideRect.X, Me.Horizontalscroll.SlideRect.Y + Horizontalscale_scrollThickness / 2)
                Dim sp_end As New PointF(Me.Horizontalscroll.SlideRect.Right, Me.Horizontalscroll.SlideRect.Y + Horizontalscale_scrollThickness / 2)
                g.DrawLine(slide_back_pen, sp_start, sp_end)
                If bar_back_sb IsNot Nothing Then bar_back_sb.Dispose()
                If slide_back_pen IsNot Nothing Then slide_back_pen.Dispose()
            End If

            If main_region IsNot Nothing Then
                g.Clip = client_region
                main_region.Dispose()
            End If
        Catch ex As Exception
            Invalidate()
        End Try




    End Sub

    Protected Overrides Sub OnEnter(ByVal e As EventArgs)
        Me.activatedState = True
        Me.activatedStateIndex = 0
        Invalidate()
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As EventArgs)
        Me.activatedState = False
        Me.activatedStateIndex = -1
        Invalidate()
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        Me.activatedState = True
        Me.activatedStateIndex = 0
        Invalidate()
        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        Me.activatedState = False
        Me.activatedStateIndex = -1
        Invalidate()
        MyBase.OnLostFocus(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        If Me.DesignMode Then Return

        Me.ismovedown = False
        If Me.ResetItemsMouseStatus() Then
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If Me.DesignMode Then Return

        If Not Me.Focused Then
            Me.Focus()
        End If

        Me.ismovedown = True
        Me.movedownpoint = e.Location

        If MainRealityRect.Height > MainRect.Height AndAlso Me.VerticalScroll.Rect.Contains(e.Location) Then

            If Me.VerticalScroll.SlideRect.Contains(e.Location) Then
                Me.mousedowninfo.Type = MouseDownTypes.Scroll
                Me.mousedowninfo.Sender = Me.VerticalScroll
            Else
                Me.mousedowninfo.Type = MouseDownTypes.None
                Me.mousedowninfo.Sender = Nothing
            End If
        ElseIf MainRealityRect.Width > MainRect.Width AndAlso Me.Horizontalscroll.Rect.Contains(e.Location) Then

            If Me.Horizontalscroll.SlideRect.Contains(e.Location) Then
                Me.mousedowninfo.Type = MouseDownTypes.Scroll
                Me.mousedowninfo.Sender = Me.Horizontalscroll
            Else
                Me.mousedowninfo.Type = MouseDownTypes.None
                Me.mousedowninfo.Sender = Nothing
            End If

        ElseIf Me.MainRect.Contains(e.Location) Then
            Dim item As Item = Me.FindMouseDownItem(e.Location)

            If item IsNot Nothing Then
                Me.mousedowninfo.Type = MouseDownTypes.MainItem
                Me.mousedowninfo.Sender = item
            Else
                Me.mousedowninfo.Type = MouseDownTypes.None
                Me.mousedowninfo.Sender = Nothing
            End If
        Else
            Me.mousedowninfo.Type = MouseDownTypes.None
            Me.mousedowninfo.Sender = Nothing
        End If
        If e.Button <> MouseButtons.Left Then Return
        If Me.mousedowninfo.Type = MouseDownTypes.MainItem Then
            Me.UpdateItemSelectedStatusForDown(CType(Me.mousedowninfo.Sender, Item), e.Location)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        If Me.DesignMode Then Return
        Me.ismovedown = False
        Me.movedownpoint = Point.Empty
        Me.mousedowninfo.Type = MouseDownTypes.None
        Me.mousedowninfo.Sender = Nothing

    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Me.DesignMode Then Return

        If Me.ismovedown Then

            If Me.mousedowninfo.Type = MouseDownTypes.Scroll Then

                If Me.mousedowninfo.Type = MouseDownTypes.Scroll AndAlso CType(Me.mousedowninfo.Sender, ScrollClass).Equals(Me.VerticalScroll) Then
                    Dim yoffset As Integer = CInt(((e.Location.Y - Me.movedownpoint.Y)))

                    Me.movedownpoint = e.Location
                    Me.MouseMoveWheel(yoffset)

                End If
                If Me.mousedowninfo.Type = MouseDownTypes.Scroll AndAlso CType(Me.mousedowninfo.Sender, ScrollClass).Equals(Me.Horizontalscroll) Then

                    Dim xoffset As Integer = CInt(((e.Location.X - Me.movedownpoint.X)))
                    Me.movedownpoint = e.Location

                    MouseMoveX(xoffset)
                End If
            End If
        Else
            Me.UpdateItemsMouseStatus(e.Location)
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If Me.DesignMode Then Return

        If e.Button = MouseButtons.Left Then

            If Me.mousedowninfo.Type = MouseDownTypes.MainItem Then
                Me.UpdateItemSelectedStatusForDown(CType(Me.mousedowninfo.Sender, Item), e.Location)
                Me.OnItemClick(New ItemClickEventArgs() With {
                            .Item = CType(Me.mousedowninfo.Sender, Item)
                        })
            End If

        End If

    End Sub

    Protected Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        If Me.DesignMode Then
            Return MyBase.ProcessDialogKey(keyData)
        End If

        If Me.activatedState Then

            If keyData = Keys.Up Then
                Dim tmp_index As Integer = Me.activatedStateIndex - 1

                For i As Integer = tmp_index To -1
                    Dim index As Integer = If((i > -1), i, Me.Items.Count - 1)

                    If Me.Items(index).Enabled Then
                        Me.activatedStateIndex = index
                        Invalidate()
                        Exit For
                    ElseIf index = Me.Items.Count - 1 Then

                        For j As Integer = index To Me.activatedStateIndex + 1

                            If Me.Items(j).Enabled Then
                                Me.activatedStateIndex = j
                                Invalidate()
                                Exit For
                            End If
                        Next
                    End If
                Next

                Return False
            ElseIf keyData = Keys.Down Then
                Dim tmp_index As Integer = Me.activatedStateIndex + 1

                For i As Integer = tmp_index To Me.Items.Count
                    Dim index As Integer = If((i < Me.Items.Count), i, 0)

                    If Me.Items(index).Enabled Then
                        Me.activatedStateIndex = index
                        Invalidate()
                        Exit For
                    ElseIf index = 0 Then

                        For j As Integer = index To Me.activatedStateIndex - 1

                            If Me.Items(j).Enabled Then
                                Me.activatedStateIndex = j
                                Invalidate()
                                Exit For
                            End If
                        Next
                    End If
                Next

                Return False
            ElseIf keyData = Keys.Enter Then

                If Me.activatedStateIndex > -1 AndAlso Me.activatedStateIndex < Me.Items.Count Then
                    Dim point As New Point(CInt((Me.Items(Me.activatedStateIndex).Rect.X + 1)), CInt((Me.Items(Me.activatedStateIndex).Rect.Y + 1)))
                    Me.UpdateItemSelectedStatusForDown(Me.Items(Me.activatedStateIndex), point)
                    Me.OnItemClick(New ItemClickEventArgs() With {
                            .Item = Me.Items(Me.activatedStateIndex)
                        })
                    Return False
                End If
            End If
        End If

        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If Control.ModifierKeys = Keys.Control AndAlso e.KeyCode = Keys.A Then
            For Each item As Item In Items
                item.UpdateItemStatus(True)
            Next
            Invalidate()
        ElseIf e.KeyCode = Keys.Down Then
            If MainRealityRect.Bottom <= MainRect.Bottom Then Return
            VerticalScrollSetFirstShowItem(FirstDisplayedScrollingRowIndex + 1)
        ElseIf e.KeyCode = Keys.Up Then
            If FirstDisplayedScrollingRowIndex = 0 Then Return
            VerticalScrollSetFirstShowItem(FirstDisplayedScrollingRowIndex - 1)
        ElseIf e.KeyCode = Keys.Left Then
            MouseMoveX(-ItemHeight)
        ElseIf e.KeyCode = Keys.Right Then
            MouseMoveX(ItemHeight)
        End If

    End Sub

    Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
        MyBase.OnMouseWheel(e)
        If Me.DesignMode Then Return
        Dim offset As Integer = If(e.Delta > 1, -1, 1)
        If offset > 0 Then
            If MainRealityRect.Bottom <= MainRect.Bottom Then Return
            VerticalScrollSetFirstShowItem(FirstDisplayedScrollingRowIndex + 1)
        ElseIf offset < 0 Then
            If FirstDisplayedScrollingRowIndex = 0 Then Return
            VerticalScrollSetFirstShowItem(FirstDisplayedScrollingRowIndex - 1)
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Me.InitializeMainRectangle()
        Dim VerticalScrollThickness As Integer = CInt((Me.VerticalScroll.Thickness))
        Me.VerticalScroll.Rect = New Rectangle(CInt(Me.ClientRectangle.Right) - Me.BorderWidth - VerticalScrollThickness, Me.ClientRectangle.Top + Me.BorderWidth, VerticalScrollThickness, Me.ClientRectangle.Height - Me.BorderWidth * 2 - If(MainRealityRect.Width > MainRect.Width, Horizontalscroll.Thickness, 0))

        Dim HorizontalscrollThickness As Integer = CInt((Me.Horizontalscroll.Thickness))
        Me.Horizontalscroll.Rect = New Rectangle(CInt(Me.ClientRectangle.Left) + Me.BorderWidth, Me.ClientRectangle.Bottom - Me.BorderWidth - HorizontalscrollThickness, Me.ClientRectangle.Width - Me.BorderWidth * 2 - If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0), HorizontalscrollThickness)


        Me.InitializeMainRealityRectangle(True)

    End Sub

    Protected Overridable Sub OnItemClick(ByVal e As ItemClickEventArgs)
        RaiseEvent ItemClick(Me, e)
    End Sub
    Dim mSelectedItem As Item
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property SelectedItem As Item
        Get
            Return mSelectedItem
        End Get
        Set(value As Item)
            mSelectedItem = value
            If value IsNot Nothing AndAlso Items.Count > 0 Then
                mSelectedItem.UpdateItemStatus(True)
            Else
                ClearSelectItems()
            End If

        End Set
    End Property
    Dim mSelectedIndex As Integer = -1
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property SelectedIndex As Integer
        Get
            Return mSelectedIndex
        End Get
        Set(value As Integer)
            Dim index As Integer = Me.GetSelectedIndex()
            If index = value Then Return
            mSelectedIndex = value
            If value > -1 AndAlso Items.Count > 0 Then
                ClearSelectItems()
                Items(value).UpdateItemStatus(True)
            Else
                ClearSelectItems()
            End If
        End Set
    End Property

    Protected Overridable Sub OnItemSelectedChanged(ByVal e As ItemSelectedChangedEventArgs)
        If e.Item.Selected Then
            mSelectedItem = e.Item
        End If
        RaiseEvent ItemSelectedChanged(Me, e)
    End Sub

    Protected Overridable Sub OnSelectedIndexChanged(ByVal e As SelectedIndexChangedEventArgs)
        If e.Item.Selected Then
            mSelectedIndex = e.Index
        End If
        RaiseEvent SelectedIndexChanged(Me, e)
    End Sub

    Protected Overridable Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        RaiseEvent DrawItem(Me, e)

    End Sub

    Public Sub InitializeRectangle()
        Me.InitializeMainRectangle()
        Me.InitializeMainRealityRectangle()
        Me.InitializeScrollRectangle()
        Me.UpdateItemsRect()
    End Sub

    Private Sub InitializeMainRectangle()
        Me.MainRect = New Rectangle(Me.ClientRectangle.X + Me.BorderWidth, Me.ClientRectangle.Top + Me.BorderWidth, Me.ClientRectangle.Width - Me.BorderWidth * 2, Me.ClientRectangle.Height - Me.BorderWidth * 2)
        Me.MainRealityRect = New Rectangle(Me.MainRect.X, Me.MainRect.Y, ItemWidth, 0)
    End Sub
    Protected Friend Sub InitializeMainRealityRectangle(Optional Update As Boolean = False)
        Dim scale_itemHeight As Integer = CInt((Me.ItemHeight))
        Me.MainRealityRect = If(Update, New Rectangle(Me.MainRealityRect.X, Me.MainRealityRect.Y, ItemWidth, 0), New Rectangle(Me.MainRect.X, Me.MainRect.Y, ItemWidth, 0))
        '  New Rectangle(Me.MainRect.X, Me.MainRect.Y, ItemWidth, 0)

        Dim h As Integer = 0

        For i As Integer = 0 To Me.Items.Count - 1
            Dim Litem As Item = Items(i)
            If MainRealityRect.Width > MainRect.Width Then
                Litem.Rect = New RectangleF(Me.MainRealityRect.Left + BorderWidth, Me.MainRect.Top + h + BorderWidth, Me.MainRealityRect.Width - VerticalScroll.Thickness, scale_itemHeight)
            Else
                Litem.Rect = New RectangleF(Me.MainRealityRect.Left + BorderWidth, Me.MainRect.Top + h + BorderWidth, Me.MainRealityRect.Width, scale_itemHeight)
            End If
            Litem.Rect.Inflate(-1, -1)

            h += scale_itemHeight
        Next
        Me.MainRealityRect = New Rectangle(Me.MainRealityRect.X, Me.MainRealityRect.Y, Me.MainRealityRect.Width, h)

        Dim y As Integer = Me.MainRealityRect.Y

        If Me.MainRealityRect.Bottom < Me.MainRect.Bottom Then
            y += (Me.MainRect.Bottom - Me.MainRealityRect.Bottom)
        End If

        If y > Me.MainRect.Y Then
            y = Me.MainRect.Y
        End If

        Me.MainRealityRect = New Rectangle(Me.MainRealityRect.X, y, Me.MainRealityRect.Width, Me.MainRealityRect.Height)

        Me.UpdateScrollSlideRectLocation()
        Invalidate()
    End Sub

    Private Sub InitializeScrollRectangle()
        Dim VerticalScrollThickness As Integer = CInt((Me.VerticalScroll.Thickness))
        Me.VerticalScroll.Rect = New RectangleF((Me.MainRect.Right - VerticalScrollThickness), Me.MainRect.Top, VerticalScrollThickness, Me.MainRect.Height - If(MainRealityRect.Width > MainRect.Width, Horizontalscroll.Thickness, 0))
        Dim vbi As Single = CSng(Me.MainRect.Height - If(MainRealityRect.Width > MainRect.Width, Horizontalscroll.Thickness, 0)) / CSng(Me.MainRealityRect.Height)

        If vbi > 1 Then
            vbi = 1
        End If

        Dim slide_height As Single = Me.VerticalScroll.Rect.Height * vbi

        If slide_height < Me.VerticalScroll.SlideMinHeight Then
            slide_height = Me.VerticalScroll.SlideMinHeight
        End If

        Me.VerticalScroll.SlideRect = New RectangleF(Me.VerticalScroll.Rect.X, Me.VerticalScroll.Rect.Y, VerticalScrollThickness, slide_height)


        Dim HorizontalscrollThickness As Integer = CInt((Me.Horizontalscroll.Thickness))
        Me.Horizontalscroll.Rect = New Rectangle(CInt(Me.MainRect.Left), Me.MainRect.Bottom - HorizontalscrollThickness, Me.MainRect.Width - If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0), VerticalScrollThickness)

        Dim hbi As Single = CSng(Me.MainRect.Width - If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0)) / CSng(Me.MainRealityRect.Width)

        If hbi > 1 Then
            hbi = 1
        End If

        Dim slide_Width As Single = Me.Horizontalscroll.Rect.Width * hbi
        If slide_Width < Me.Horizontalscroll.SlideMinHeight Then
            slide_Width = Me.Horizontalscroll.SlideMinHeight
        End If

        Me.Horizontalscroll.SlideRect = New RectangleF(Me.Horizontalscroll.Rect.X, Me.Horizontalscroll.Rect.Y, slide_Width, VerticalScrollThickness)
    End Sub

    Private Sub UpdateItemsMouseStatus(ByVal mousePoint As Point)
        Dim result As Boolean = False
        Dim isInMainRect As Boolean = Me.MainRect.Contains(mousePoint) AndAlso Not Me.VerticalScroll.Rect.Contains(mousePoint) AndAlso Not Me.Horizontalscroll.Rect.Contains(mousePoint)

        For Each item As Item In Me.Items

            If item.Enabled Then

                If isInMainRect AndAlso item.Rect.Contains(mousePoint) Then

                    If item.MouseStatus = ItemMouseStatuss.Normal Then
                        item.MouseStatus = ItemMouseStatuss.Enter
                        result = True
                    End If
                Else

                    If item.MouseStatus = ItemMouseStatuss.Enter Then
                        item.MouseStatus = ItemMouseStatuss.Normal
                        result = True
                    End If
                End If
            End If
        Next

        If result Then
            Invalidate()
        End If
    End Sub

    Private Function ResetItemsMouseStatus() As Boolean
        Dim result As Boolean = False

        For Each item As Item In Me.Items

            If item.MouseStatus <> ItemMouseStatuss.Normal Then
                item.MouseStatus = ItemMouseStatuss.Normal
                result = True
            End If
        Next

        Return result
    End Function

    Private Function FindMouseDownItem(ByVal mousePoint As Point) As Item
        For Each item As Item In Me.Items

            If item.Enabled Then

                If item.Rect.Contains(mousePoint) Then
                    Return item
                End If
            End If
        Next

        Return Nothing
    End Function


    Private Sub UpdateItemSelectedStatusForDown(ByVal down_item As Item, ByVal mousePoint As Point)
        If Me.Multiple AndAlso SelectedItems.Count > 0 AndAlso (Control.ModifierKeys = Keys.Shift OrElse Control.ModifierKeys = Keys.Control) Then
            Dim FirstIndex As Integer = Me.Items.IndexOf(SelectedItems(0))
            Dim item As Item = Me.FindMouseDownItem(mousePoint)
            Dim CurrentItemIndex As Integer = Me.Items.IndexOf(item)
            Dim MinIndex As Integer = Math.Min(FirstIndex, CurrentItemIndex)
            Dim MaxIndex As Integer = Math.Max(FirstIndex, CurrentItemIndex)

            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Control.ModifierKeys = Keys.Control AndAlso Litem.Enabled AndAlso down_item.Equals(Litem) AndAlso Litem.Rect.Contains(mousePoint) Then
                    Litem.UpdateItemStatus(Not Litem.Selected)
                    Invalidate()
                    Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                            .Item = Litem, .Index = i
                        })
                    Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                    .Item = Litem, .Index = i
                                })
                    Exit For
                ElseIf Control.ModifierKeys = Keys.Shift AndAlso Litem.Enabled Then
                    Dim SelectedChanged As Boolean
                    If i >= MinIndex AndAlso i <= MaxIndex Then
                        SelectedChanged = If(Not (Items(i)?.Selected), False) ' If(Items(i).Selected, False, True)
                        Items(i).UpdateItemStatus(True)
                    Else
                        SelectedChanged = If(Items(i)?.Selected, False)
                        Items(i).UpdateItemStatus(False)
                    End If
                    Invalidate()
                    If SelectedChanged Then
                        Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                          .Item = Litem, .Index = i
                      })
                        Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                        .Item = Litem, .Index = i
                                    })
                    End If

                End If
            Next
        Else

            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Litem.Enabled Then
                    If down_item.Equals(Litem) AndAlso Litem.Rect.Contains(mousePoint) Then

                        If Litem.Selected = False Then
                            '  Dim index As Integer = Me.GetSelectedIndex()
                            Litem.UpdateItemStatus(True)
                            Invalidate()
                            Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                                    .Item = Litem, .Index = i
                                })
                            Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                        .Item = Litem, .Index = i
                                    })
                            'If index > -1 AndAlso index <> i Then

                            '    For j As Integer = 0 To Me.Items.Count - 1

                            '        If i <> j Then
                            '            Me.Items(j).UpdateItemStatus(False)
                            '        End If
                            '    Next

                            '    Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                            '            .Item = Litem, .Index = i
                            '        })
                            'End If

                        End If
                    Else
                        If Items(i).Selected Then
                            Items(i).UpdateItemStatus(False)
                            Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                      .Item = Items(i), .Index = i
                                  })
                        End If
                    End If
                End If

            Next
        End If
    End Sub
    Private Sub ClearSelectItems()
        For i As Integer = 0 To Me.Items.Count - 1
            Dim Litem As Item = Items(i)
            Dim index As Integer = Me.GetSelectedIndex()
            Litem.UpdateItemStatus(False)

            Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                          .Item = Litem, .Index = i
                      })
            Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                              .Item = Litem, .Index = i
                          })
            If index > -1 AndAlso index <> i Then

                For j As Integer = 0 To Me.Items.Count - 1

                    If i <> j Then
                        Me.Items(j).UpdateItemStatus(False)
                    End If
                Next
            End If
        Next
        Me.Invalidate()
    End Sub
    Private Sub UpdateItemSelectedStatus(ByVal down_item As Item, ByVal isselected As Boolean)
        If Me.Multiple Then

            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Litem.Enabled AndAlso down_item.Equals(Litem) Then
                    Litem.UpdateItemStatus(isselected)
                    Invalidate()
                    Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                            .Item = Litem, .Index = i
                        })
                    Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                    .Item = Litem, .Index = i
                                })
                    Exit For
                End If
            Next
        Else

            For i As Integer = 0 To Me.Items.Count - 1
                Dim Litem As Item = Items(i)
                If Litem.Enabled AndAlso down_item.Equals(Litem) Then

                    If Litem.Selected = False Then
                        Dim index As Integer = Me.GetSelectedIndex()
                        Litem.UpdateItemStatus(True)
                        Invalidate()
                        Me.OnItemSelectedChanged(New ItemSelectedChangedEventArgs() With {
                                .Item = Litem, .Index = i
                            })
                        Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                    .Item = Litem, .Index = i
                                })
                        If index > -1 AndAlso index <> i Then

                            For j As Integer = 0 To Me.Items.Count - 1

                                If i <> j Then
                                    Me.Items(j).UpdateItemStatus(False)
                                End If
                            Next

                            Me.OnSelectedIndexChanged(New SelectedIndexChangedEventArgs() With {
                                    .Item = Litem, .Index = i
                                })
                        End If

                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    Private Function GetSelectedIndex() As Integer
        For i As Integer = 0 To Me.Items.Count - 1

            If Items(i).Selected Then
                Return i
            End If
        Next

        Return -1
    End Function


    Private Sub UpdateScrollSlideRectLocation()
        Dim slide_height As Single = Me.VerticalScroll.Rect.Height * (CSng(Me.MainRect.Height - If(MainRealityRect.Width > MainRect.Width, Horizontalscroll.Thickness, 0)) / CSng(Me.MainRealityRect.Height))

        If slide_height < Me.VerticalScroll.SlideMinHeight Then
            slide_height = Me.VerticalScroll.SlideMinHeight
        End If

        Dim h As Single = Me.MainRect.Y - Me.MainRealityRect.Y

        If Me.MainRealityRect.Y < 0 Then
            h = Me.MainRect.Y + Math.Abs(Me.MainRealityRect.Y)
        End If

        Dim slide_y As Single = Me.VerticalScroll.Rect.Y + (Me.VerticalScroll.Rect.Height - slide_height) * h / CSng((Me.MainRealityRect.Height - Me.MainRect.Height + If(MainRealityRect.Width > MainRect.Width, Horizontalscroll.Thickness, 0)))
        slide_y += mMainRealityRect.Y
        Me.VerticalScroll.SlideRect = New RectangleF(Me.VerticalScroll.Rect.X, slide_y, Me.VerticalScroll.SlideRect.Width, slide_height)



        Dim slide_Width As Single = Me.Horizontalscroll.Rect.Width * (CSng(Me.MainRect.Width - If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0)) / CSng(Me.MainRealityRect.Width))

        If slide_Width < Me.Horizontalscroll.SlideMinHeight Then
            slide_Width = Me.Horizontalscroll.SlideMinHeight
        End If

        Dim w As Single = Me.MainRect.X - Me.MainRealityRect.X

        If Me.MainRealityRect.X < 0 Then
            w = Me.MainRect.X + Math.Abs(Me.MainRealityRect.X)
        End If

        Dim slide_x As Single = Me.Horizontalscroll.Rect.X + (Me.Horizontalscroll.Rect.Width - slide_height) * w / CSng((Me.MainRealityRect.Width - Me.MainRect.Width + If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0)))
        slide_x += mMainRealityRect.X
        Me.Horizontalscroll.SlideRect = New RectangleF(slide_x, Me.Horizontalscroll.Rect.Y, slide_Width, Horizontalscroll.Thickness)
    End Sub

    Private Sub UpdateItemsRect()

        Dim scale_itemHeight As Integer = CInt((Me.ItemHeight))
        Dim h As Single = 0
        For i As Integer = 0 To Me.Items.Count - 1
            Dim Litem As Item = Items(i)
            Litem.Rect = New RectangleF(Me.MainRealityRect.Left + BorderWidth, Me.MainRealityRect.Top + h + BorderWidth, Me.MainRealityRect.Width - If(MainRealityRect.Height > MainRect.Height, VerticalScroll.Thickness, 0), scale_itemHeight)
            Litem.Rect.Inflate(-1, -1)

            h += scale_itemHeight
        Next
    End Sub
    Private Sub MouseMoveX(ByVal offset As Integer)
        Dim X As Single = Me.Horizontalscroll.SlideRect.X
        X += offset

        If X < Me.Horizontalscroll.Rect.X Then
            X = Me.Horizontalscroll.Rect.X
        End If

        If X > Me.Horizontalscroll.Rect.Right - Me.Horizontalscroll.SlideRect.Width Then
            X = Me.Horizontalscroll.Rect.Right - Me.Horizontalscroll.SlideRect.Width
        End If
        If Me.Horizontalscroll.Rect.Width = Me.Horizontalscroll.SlideRect.Width Then Return

        Me.Horizontalscroll.SlideRect = New RectangleF(X, Me.Horizontalscroll.SlideRect.Location.Y, Me.Horizontalscroll.SlideRect.Width, Me.Horizontalscroll.SlideRect.Height)
        Dim bi As Single = CSng(Me.Horizontalscroll.SlideRect.X - Me.Horizontalscroll.Rect.X) / CSng(Me.Horizontalscroll.Rect.Width - Me.Horizontalscroll.SlideRect.Width)
        Dim scroll_h As Single = If(Me.MainRealityRect.Width > Me.MainRect.Width, Me.MainRealityRect.Width - Me.MainRect.Width + If(Me.MainRealityRect.Height > Me.MainRect.Height, VerticalScroll.Thickness, 0), 0)
        mMainRealityRect.X = CInt((Me.MainRect.X - scroll_h * bi))

        If MainRect.Right - mMainRealityRect.Right > VerticalScroll.Thickness Then
            mMainRealityRect.X = MainRect.Right + VerticalScroll.Thickness + mMainRealityRect.Width
        End If

        Me.UpdateItemsRect()
        Invalidate()
    End Sub
    Public Sub VerticalScrollSetFirstShowItem(index As Integer)
        If MainRealityRect.Height <= MainRect.Height Then Return
        Dim y As Single = ItemHeight * index * (MainRect.Height / mMainRealityRect.Height) + ItemHeight
        If y < Me.VerticalScroll.Rect.Y Then
            y = Me.VerticalScroll.Rect.Y
        End If

        If y > Me.VerticalScroll.Rect.Bottom - Me.VerticalScroll.SlideRect.Height Then
            y = Me.VerticalScroll.Rect.Bottom - Me.VerticalScroll.SlideRect.Height
        End If
        If Me.VerticalScroll.Rect.Height = Me.VerticalScroll.SlideRect.Height Then Return
        Me.VerticalScroll.SlideRect = New RectangleF(Me.VerticalScroll.SlideRect.Location.X, y, Me.VerticalScroll.SlideRect.Width, Me.VerticalScroll.SlideRect.Height)
        mMainRealityRect.Y = -(ItemHeight * index)
        If mMainRealityRect.Y < (MainRect.Height - mMainRealityRect.Height) Then
            mMainRealityRect.Y = MainRect.Height - mMainRealityRect.Height
        End If
        Me.UpdateItemsRect()
        Invalidate()
    End Sub
    Private Sub MouseMoveWheel(ByVal offset As Integer)
        If mMainRealityRect.Height = 0 Then Return
        Dim y As Single = Me.VerticalScroll.SlideRect.Y
        y += offset

        If y < Me.VerticalScroll.Rect.Y Then
            y = Me.VerticalScroll.Rect.Y
        End If

        If y > Me.VerticalScroll.Rect.Bottom - Me.VerticalScroll.SlideRect.Height Then
            y = Me.VerticalScroll.Rect.Bottom - Me.VerticalScroll.SlideRect.Height
        End If
        If Me.VerticalScroll.Rect.Height = Me.VerticalScroll.SlideRect.Height Then Return
        Me.VerticalScroll.SlideRect = New RectangleF(Me.VerticalScroll.SlideRect.Location.X, y, Me.VerticalScroll.SlideRect.Width, Me.VerticalScroll.SlideRect.Height)
        Dim bi As Single = CSng(Me.VerticalScroll.SlideRect.Y - Me.VerticalScroll.Rect.Y) / CSng(Me.VerticalScroll.Rect.Height - Me.VerticalScroll.SlideRect.Height) '
        Dim scroll_h As Single = If(Me.MainRealityRect.Height > Me.MainRect.Height, Me.MainRealityRect.Height - Me.MainRect.Height + If(Me.MainRealityRect.Width > Me.MainRect.Width, Horizontalscroll.Thickness, 0), 0)
        mMainRealityRect.Y = CInt((Me.MainRect.Y - scroll_h * bi))
        If mMainRealityRect.Height > MainRect.Height AndAlso MainRect.Bottom - mMainRealityRect.Bottom > Horizontalscroll.Thickness Then
            mMainRealityRect.Y = MainRect.Bottom + Horizontalscroll.Thickness + mMainRealityRect.Height
        End If

        Me.UpdateItemsRect()
        Invalidate()
    End Sub

    Protected Overridable Sub PaintItem(ByVal item As Item, ByVal g As Graphics, ByVal image As Image, ByVal itemborder_pen As Pen, ByVal commom_back_sb As LinearGradientBrush, ByVal commom_text_sb As SolidBrush, ByVal scale_itemImageSize As Size, Xoffset As Integer)
        If item.Data IsNot Nothing AndAlso TypeOf （item.Data) Is DataRow Then
            Dim row As DataRow = item.Data
            If row.Table Is Nothing OrElse row.RowState = DataRowState.Deleted Then
                Items.Remove(item)
                Invalidate()
                Return
            End If
        End If

        If item.Rect.Equals(RectangleF.Empty) Then Return
        g.FillRectangle(commom_back_sb, item.Rect)
        Dim image_padding As Integer = 2

        If Me.ItemImageShow AndAlso image IsNot Nothing Then
            Dim image_rect As New Rectangle(CInt((item.Rect.X + image_padding)), CInt((item.Rect.Y + (item.Rect.Height - image.Height) / 2)), image.Width, image.Height)
            g.DrawImage(image, image_rect)
        End If

        Dim text_size As SizeF = g.MeasureString(item.Text, Me.Font)

        Dim text_x As Single = (If(Me.ItemImageShow, (scale_itemImageSize.Width + image_padding * 2), 0)) + Xoffset

        Dim text_rect As New RectangleF(text_x, item.Rect.Y, text_size.Width, item.Rect.Height)

        Dim tsf As StringFormat = ControlExt.Common.GetTextFormat(ContentAlignment.MiddleLeft)

        tsf.FormatFlags = StringFormatFlags.NoWrap
        Dim txt As String = Regex.Replace(item.Text, "[\r\n]", " ", RegexOptions.IgnoreCase)

        g.DrawString(txt, Me.Font, commom_text_sb, text_rect, tsf)

        If Me.ItemBorderStyle <> ItemBorderStyles.None Then
            g.DrawLine(itemborder_pen, item.Rect.X, item.Rect.Bottom - 1, item.Rect.Right, item.Rect.Bottom - 1)
        End If
    End Sub

    Private Sub ImageListRecreateHandle(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.IsHandleCreated Then Return
        Invalidate()
    End Sub

    Private Sub DetachImageList(ByVal sender As Object, ByVal e As EventArgs)
        Me.ItemImageList = CType(Nothing, ImageList)
    End Sub
    '<Description("选中项集合")>
    'Public Class SelectedObjectCollection
    '    Implements IList, ICollection, IEnumerable

    '    Private ReadOnly owner As ListBoxExt = Nothing
    '    Private ReadOnly Property ItemList As New List(Of Item)
    '    Public Sub New(ByVal owner As ListBoxExt)
    '        Me.owner = owner
    '    End Sub
    '    Public Function GetArray() As Array
    '        Return ItemList.ToArray
    '    End Function
    '    Public Function GetEnumerator() As IEnumerator
    '        Return ItemList.GetEnumerator()
    '    End Function

    '    Public Sub CopyTo(ByVal array As Array, ByVal index As Integer)
    '        ItemList.CopyTo(array, index)
    '    End Sub

    '    Public ReadOnly Property Count As Integer
    '        Get
    '            Return Me.ItemList.Count
    '        End Get
    '    End Property

    '    Public Shared ReadOnly Property IsSynchronized As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property

    '    Public ReadOnly Property SyncRoot As Object
    '        Get
    '            Return CObj(Me)
    '        End Get
    '    End Property
    '    Public Sub AddRange(ByVal items As ItemCollection)
    '        For Each value As Object In items
    '            Me.ItemList.Add(New Item(owner) With {.Data = value})
    '        Next

    '    End Sub
    '    Public Sub AddRange(ByVal items As Object())
    '        For Each value As Object In items
    '            Me.ItemList.Add(New Item(owner) With {.Data = value})
    '        Next

    '    End Sub
    '    Public Sub AddRange(ByVal items As Item())
    '        For Each value As Item In items
    '            value.owner = owner
    '            Me.ItemList.Add(value)
    '        Next

    '    End Sub
    '    Public Function Add(ByVal value As Object) As Integer
    '        If Not (TypeOf value Is Item) Then
    '            Return Add(New Item(owner) With {.Data = value})
    '        Else
    '            Return Add(CType(value, Item))
    '        End If
    '    End Function

    '    Public Function Add(ByVal item As Item) As Integer
    '        If item Is Nothing Then
    '            Throw New ArgumentNullException(NameOf(item))
    '        End If

    '        item.owner = owner
    '        If item.Selected = False Then item.UpdateItemStatus(True)
    '        Me.ItemList.Add(item)

    '        Return Me.Count - 1
    '    End Function

    '    Public Sub Clear()
    '        For Each item As Item In Me.ItemList
    '            item.UpdateItemStatus(False)
    '        Next
    '        Me.ItemList.Clear()
    '    End Sub

    '    Public Function Contains(ByVal value As Item) As Boolean
    '        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
    '        Return Me.IndexOf(value) <> -1
    '    End Function

    '    Public Function Contains(ByVal value As Object) As Boolean
    '        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
    '        If TypeOf value Is Item Then
    '            Return Me.Contains(CType(value, Item))
    '        Else
    '            Return Me.Contains(New Item() With {.Data = value})
    '        End If
    '        Return False
    '    End Function

    '    Public Function IndexOf(ByVal Value As Object) As Integer
    '        If Value Is Nothing Then Throw New ArgumentNullException(NameOf(Value))
    '        If TypeOf Value Is Item Then
    '            Return Me.ItemList.IndexOf(Value)
    '        Else
    '            Return ItemList.IndexOf(New ListBoxExt.Item() With {.Data = Value})
    '        End If
    '        Return -1
    '    End Function


    '    Public Sub Insert(ByVal index As Integer, ByVal value As Item)
    '        value.owner = owner
    '        value.UpdateItemStatus(True)
    '        ItemList.RemoveAt(index)
    '        ItemList.Insert(index, value)
    '        Me.owner.Invalidate()
    '    End Sub

    '    Public Shared ReadOnly Property IsFixedSize As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property

    '    Public Shared ReadOnly Property IsReadOnly As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property



    '    Public Sub Remove(ByVal item As Item)

    '        If ItemList.Contains(item) Then
    '            ItemList.Remove(item)
    '            item.UpdateItemStatus(False)
    '        End If

    '        Me.owner.Invalidate()
    '    End Sub

    '    Public Sub RemoveAt(ByVal index As Integer)
    '        If ItemList.Count > index Then
    '            Dim Item As Item = ItemList(index)
    '            ItemList.Remove(Item)
    '            Item.UpdateItemStatus(False)
    '            Me.owner.Invalidate()
    '        Else
    '            Throw New ArgumentException("Index Not exist")
    '        End If

    '    End Sub

    '    Default Public Property Item(index As Integer) As Item
    '        Get
    '            If ItemList.Count <= index Then
    '                Throw New ArgumentException("Index Not exist")
    '            End If
    '            Return CType(Me.ItemList(index), Item)
    '        End Get
    '        Set(value As Item)
    '            Me.ItemList(index) = value
    '        End Set
    '    End Property



    '    Private Function IList_Add(value As Object) As Integer Implements IList.Add
    '        Return Add(value)
    '    End Function

    '    Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
    '        Return Contains(value)
    '    End Function

    '    Private Sub IList_Clear() Implements IList.Clear
    '        Clear()
    '    End Sub

    '    Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
    '        Return IndexOf(value)
    '    End Function

    '    Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
    '        Insert(index, value)
    '    End Sub

    '    Private Sub IList_Remove(value As Object) Implements IList.Remove
    '        Remove(value)
    '    End Sub

    '    Private Sub IList_RemoveAt(index As Integer) Implements IList.RemoveAt
    '        RemoveAt(index)
    '    End Sub

    '    Private Sub ICollection_CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
    '        CopyTo(array, index)
    '    End Sub

    '    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
    '        Return GetEnumerator()
    '    End Function




    '    Private Property IList_Item(index As Integer) As Object Implements IList.Item
    '        Get
    '            Return Item(index)
    '        End Get
    '        Set(value As Object)
    '            Item(index) = value
    '        End Set
    '    End Property

    '    Private ReadOnly Property IList_IsReadOnly As Boolean Implements IList.IsReadOnly
    '        Get
    '            Return IsReadOnly
    '        End Get
    '    End Property

    '    Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
    '        Get
    '            Return IsFixedSize
    '        End Get
    '    End Property

    '    Private ReadOnly Property ICollection_Count As Integer Implements ICollection.Count
    '        Get
    '            Return Count
    '        End Get
    '    End Property

    '    Private ReadOnly Property ICollection_SyncRoot As Object Implements ICollection.SyncRoot
    '        Get
    '            Return SyncRoot
    '        End Get
    '    End Property

    '    Private ReadOnly Property ICollection_IsSynchronized As Boolean Implements ICollection.IsSynchronized
    '        Get
    '            Return IsSynchronized
    '        End Get
    '    End Property


    'End Class

    <Description("选项集合")>
    Public NotInheritable Class ItemCollection
        Implements IList, ICollection, IEnumerable

        Private ReadOnly owner As ListBoxExt = Nothing
        Private ReadOnly Property ItemList As New List(Of Item)

        Public Sub New(ByVal owner As ListBoxExt)
            Me.owner = owner
        End Sub
        Public Function GetArray() As Array

            Return ItemList.ToArray
        End Function


        Public Function GetEnumerator() As IEnumerator
            Return ItemList.ToArray.GetEnumerator()
        End Function

        Public Sub CopyTo(ByVal array As Array, ByVal index As Integer)
            For i As Integer = 0 To Me.Count - 1
                array.SetValue(Me.ItemList(i), i + index)
            Next
        End Sub

        Public ReadOnly Property Count As Integer
            Get
                Return Me.ItemList.Count
            End Get
        End Property

        Public Shared ReadOnly Property IsSynchronized As Boolean
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property SyncRoot As Object
            Get
                Return CObj(Me)
            End Get
        End Property

        Public Sub AddRange(ByVal items As Array)
            owner?.SuspendLayout()
            For Each value As Object In items
                If TypeOf (value) Is Item Then
                    CType(value, Item).owner = owner
                    ItemList.Add(value)
                Else
                    ItemList.Add(New Item(owner) With {.Data = value})
                End If
            Next
            owner?.ResumeLayout()
            Me.owner.InitializeMainRealityRectangle(True)

        End Sub


        Public Function Add(ByVal value As Object) As Integer
            If TypeOf value Is Item Then
                Dim it As Item = value
                it.owner = owner
                Return Add(it)
            Else
                Return Add(New Item(owner) With {.Data = value})
            End If
        End Function

        Private Function Add(ByVal item As Item) As Integer
            If item Is Nothing Then
                Throw New ArgumentNullException(NameOf(item))
            End If

            item.owner = owner
            Me.ItemList.Add(item)
            Me.owner.InitializeMainRealityRectangle(True)

            Return Me.Count - 1
        End Function

        Public Sub Clear()
            Me.ItemList.Clear()
            Me.owner.InitializeMainRealityRectangle(True)
        End Sub

        Public Function Contains(ByVal value As Item) As Boolean
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            Return Me.IndexOf(value) <> -1
        End Function

        Public Function Contains(ByVal value As Object) As Boolean
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            If TypeOf value Is Item Then
                Dim it As Item = value
                it.owner = owner
                Return Me.Contains(it)
            Else
                Return Me.Contains(New Item(owner) With {.Data = value})
            End If
            Return False
        End Function

        Public Function IndexOf(ByVal Value As Object) As Integer
            If Value Is Nothing Then Throw New ArgumentNullException(NameOf(Value))
            If TypeOf Value Is Item Then
                Dim it As Item = Value
                it.owner = owner
                Return Me.ItemList.IndexOf(it)
            Else
                Return ItemList.IndexOf(New Item(owner) With {.Data = Value})
            End If
            Return -1
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As Object)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            If Not (TypeOf value Is Item) Then
                Insert(index, New Item(owner) With {.Data = value})
            Else
                Dim it As Item = value
                it.owner = owner
                Insert(index, it)
            End If

        End Sub
        Public Sub Insert(ByVal index As Integer, ByVal value As Item)
            CType(value, Item).owner = owner
            If ItemList.Count > index Then
                ItemList.Insert(index, value)
            Else
                ItemList.Add(value)
            End If
            Me.owner.InitializeMainRealityRectangle(True)

        End Sub
        Public Sub Replace(ByVal value1 As Object, ByVal value2 As Object)
            Replace(New Item(owner) With {.Data = value1}, value2)
        End Sub
        Public Sub Replace(ByVal value1 As Item, ByVal value2 As Object)
            Dim index As Integer = IndexOf(value1)
            Replace(index, value2)
        End Sub

        Public Sub Replace(ByVal index As Integer, ByVal value As Object)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            If Not (TypeOf value Is Item) Then
                Replace(index, New Item(owner) With {.Data = value})
            Else
                Dim it As Item = value
                it.owner = owner
                Replace(index, value)
            End If

        End Sub
        Public Sub Replace(ByVal index As Integer, ByVal value As Item)
            CType(value, Item).owner = owner
            If ItemList.Count > index Then
                ItemList.RemoveAt(index)
                ItemList.Insert(index, value)
            Else
                ItemList.Add(value)
            End If
            Me.owner.InitializeMainRealityRectangle(True)

        End Sub
        Public Shared ReadOnly Property IsFixedSize As Boolean
            Get
                Return False
            End Get
        End Property

        Public Shared ReadOnly Property IsReadOnly As Boolean
            Get
                Return False
            End Get
        End Property

        Public Sub Remove(ByVal value As Object)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            If Not (TypeOf value Is Item) Then
                Dim Item As Item = ItemList.Find(Function(x) x.Data.Equals(value))
                Me.Remove(Item)
            Else
                Me.Remove(CType(value, Item))
            End If
        End Sub

        Public Sub Remove(ByVal item As Item)
            Dim _item As Item = ItemList.Find(Function(x) x.Equals(item))
            If _item IsNot Nothing Then
                If _item.Data IsNot Nothing OrElse TypeOf (_item.Data) Is DataRow Then
                    If TypeOf (_item.Data) Is DataRow Then
                        Dim row As DataRow = item.Data
                        If row.Table IsNot Nothing AndAlso row.RowState <> DataRowState.Deleted Then
                            row.Delete()
                        End If
                    End If
                End If
                ItemList.Remove(_item)
            End If

            Me.owner.InitializeMainRealityRectangle(True)

        End Sub

        Public Sub RemoveAt(ByVal index As Integer)
            If ItemList.Count > index Then
                Dim _item As Item = ItemList(index)
                If TypeOf (_item.Data) Is DataRow Then
                    Dim row As DataRow = _item.Data
                    If row.Table IsNot Nothing AndAlso row.RowState <> DataRowState.Deleted Then
                        row.Delete()
                    End If
                End If
                Me.ItemList.RemoveAt(index)
                Me.owner.InitializeMainRealityRectangle(True)

            Else
                Throw New ArgumentException("Index Not exist")
            End If

        End Sub

        Default Public Property Items(index As Integer) As Item
            Get
                If ItemList.Count <= index Then
                    'Throw New ArgumentException("Index Not exist")
                    Return Nothing
                End If
                Return CType(Me.ItemList(index), Item)
            End Get
            Set(value As Item)
                Me.ItemList(index) = value
            End Set
        End Property



        Private Function IList_Add(value As Object) As Integer Implements IList.Add
            Return Add(value)
        End Function

        Private Function IList_Contains(value As Object) As Boolean Implements IList.Contains
            Return Contains(value)
        End Function

        Private Sub IList_Clear() Implements IList.Clear
            Clear()
        End Sub

        Private Function IList_IndexOf(value As Object) As Integer Implements IList.IndexOf
            Return IndexOf(value)
        End Function

        Private Sub IList_Insert(index As Integer, value As Object) Implements IList.Insert
            Insert(index, value)
        End Sub

        Private Sub IList_Remove(value As Object) Implements IList.Remove
            Remove(value)
        End Sub

        Private Sub IList_RemoveAt(index As Integer) Implements IList.RemoveAt
            Remove(index)
        End Sub

        Private Sub ICollection_CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            CopyTo(array, index)
        End Sub

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function




        Private Property Item(index As Integer) As Object Implements IList.Item
            Get
                Return Items(index)
            End Get
            Set(value As Object)
                Items(index) = value
            End Set
        End Property

        Private ReadOnly Property IList_IsReadOnly As Boolean Implements IList.IsReadOnly
            Get
                Return IsReadOnly
            End Get
        End Property

        Private ReadOnly Property IList_IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return IsFixedSize
            End Get
        End Property

        Private ReadOnly Property ICollection_Count As Integer Implements ICollection.Count
            Get
                Return Count
            End Get
        End Property

        Private ReadOnly Property ICollection_SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Return SyncRoot
            End Get
        End Property

        Private ReadOnly Property ICollection_IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return IsSynchronized
            End Get
        End Property


    End Class


    <Description("选项")>
    <DefaultProperty("Data")>
    <DefaultValue("Data")>
    Public Class Item
        Protected Friend owner As ListBoxExt = Nothing
        Private menabled As Boolean = True


        <DefaultValue(True)>
        Public Property Enabled As Boolean
            Get
                Return menabled
            End Get
            Set(ByVal value As Boolean)
                If menabled = value Then Return
                menabled = value
                Me.owner?.Invalidate()
            End Set
        End Property

        Private mmouseStatus As ItemMouseStatuss = ItemMouseStatuss.Normal

        <Description("选项鼠标状态")>
        <DefaultValue(ItemMouseStatuss.Normal)>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property MouseStatus As ItemMouseStatuss
            Get
                Return mmouseStatus
            End Get
            Set(ByVal value As ItemMouseStatuss)
                If mmouseStatus = value Then Return
                mmouseStatus = value
            End Set
        End Property

        Private mselected As Boolean = False

        <Description("选项是否选中")>
        <DefaultValue(False)>
        Public Property Selected As Boolean
            Get
                Return mselected
            End Get
            Set(ByVal value As Boolean)
                If mselected = value Then Return
                If Me.owner IsNot Nothing Then
                    Dim point As New Point(CInt((Me.Rect.X + 1)), CInt((Me.Rect.Y + 1)))
                    Me.owner.UpdateItemSelectedStatus(Me, value)
                End If
            End Set
        End Property

        Private mtext As String = ""
        <Description("选项文本")>
        <DefaultValue("")>
        Public Property Text As String
            Get
                If Data IsNot Nothing Then
                    Return Data.ToString
                End If
                Return mtext
            End Get
            Set(ByVal value As String)
                If Me.Text = value Then Return
                mtext = value
                Me.owner?.Invalidate()
            End Set
        End Property

        Private mdata As Object = Nothing

        <Description("选项自定义数据")>
        <Browsable(False)>
        Public Property Data As Object
            Get
                Return mdata
            End Get
            Set(ByVal value As Object)
                mdata = value
                If TypeOf (value) Is String Then
                    mtext = value
                End If
                Me.owner?.Invalidate()
            End Set
        End Property

        Public Overrides Function ToString() As String
            If Data IsNot Nothing Then
                Return Data.ToString
            Else
                Return Text
            End If
        End Function
        Private mimage As Image = Nothing

        <Description("列表图片")>
        Public Property Image As Image
            Get
                Return mimage
            End Get
            Set(ByVal value As Image)
                If value Is Nothing OrElse mimage.Equals(value) Then Return
                mimage = value
                Me.owner?.Invalidate()
            End Set
        End Property

        Private mrect As RectangleF = RectangleF.Empty

        <Description("选项Rect")>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property Rect As RectangleF
            Get
                Return mrect
            End Get
            Set(ByVal value As RectangleF)
                If mrect = value Then Return
                mrect = value
            End Set
        End Property

        Private mnormalBackColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项背景颜色（正常）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property NormalBackColor As Color
            Get
                Return mnormalBackColor
            End Get
            Set(ByVal value As Color)
                If mnormalBackColor = value Then Return
                mnormalBackColor = value
                Me.owner?.Invalidate()

            End Set
        End Property

        Private mnormalTextColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项文本颜色（正常）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property NormalTextColor As Color
            Get
                Return mnormalTextColor
            End Get
            Set(ByVal value As Color)
                If mnormalTextColor = value Then Return
                mnormalTextColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Private menterBackColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项背景颜色（鼠标进入）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property EnterBackColor As Color
            Get
                Return menterBackColor
            End Get
            Set(ByVal value As Color)
                If menterBackColor = value Then Return
                menterBackColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Private menterTextColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项文本颜色（鼠标进入）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property EnterTextColor As Color
            Get
                Return menterTextColor
            End Get
            Set(ByVal value As Color)
                If menterTextColor = value Then Return
                menterTextColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Private mselectedBackColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项背景颜色（选中）(限于MainTab类型)")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property SelectedBackColor As Color
            Get
                Return mselectedBackColor
            End Get
            Set(ByVal value As Color)
                If mselectedBackColor = value Then Return
                mselectedBackColor = value
                Me.owner?.Invalidate()
            End Set
        End Property

        Private mselectedTextColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项文本颜色（选中）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property SelectedTextColor As Color
            Get
                Return mselectedTextColor
            End Get
            Set(ByVal value As Color)
                If Me.SelectedTextColor = value Then Return
                mselectedTextColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Private mdisableBackColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项背景颜色（禁止）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property DisableBackColor As Color
            Get
                Return mdisableBackColor
            End Get
            Set(ByVal value As Color)
                If mdisableBackColor = value Then Return
                mdisableBackColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Private mdisableTextColor As Color = Color.Empty

        <DefaultValue(GetType(Color), "")>
        <Description(" 选项文本颜色（禁止）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property DisableTextColor As Color
            Get
                Return mdisableTextColor
            End Get
            Set(ByVal value As Color)
                If mdisableTextColor = value Then Return
                mdisableTextColor = value


                Me.owner?.Invalidate()

            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As ListBoxExt)
            Me.owner = owner
        End Sub

        Public Sub UpdateItemStatus(ByVal isselected As Boolean)
            If mselected = isselected Then Return
            mselected = isselected
            owner?.Invalidate()
        End Sub


    End Class

    <Description("滚动条")>
    Public Class ScrollClass
        Private ReadOnly owner As ListBoxExt = Nothing
        Private mrect As RectangleF = Rectangle.Empty

        <Description("Rect")>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property Rect As RectangleF
            Get
                Return mrect
            End Get
            Set(ByVal value As RectangleF)
                If Me.Rect = value Then Return
                mrect = value
            End Set
        End Property

        Private mthickness As Integer = 10

        <DefaultValue(10)>
        <Description("滑条厚度")>
        <NotifyParentProperty(True)>
        Public Property Thickness As Integer
            Get
                Return mthickness
            End Get
            Set(ByVal value As Integer)
                If mthickness = value OrElse value < 0 Then Return
                mthickness = value
                Me.owner.InitializeScrollRectangle()
                Me.owner.Invalidate()
            End Set
        End Property

        Private mbarNormalBackColor As Color = Color.FromArgb(68, 128, 128, 128)

        <DefaultValue(GetType(Color), "68, 128, 128, 128")>
        <Description("滑条背景颜色（正常）")>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        <NotifyParentProperty(True)>
        Public Property BarNormalBackColor As Color
            Get
                Return mbarNormalBackColor
            End Get
            Set(ByVal value As Color)
                If mbarNormalBackColor = value Then Return
                mbarNormalBackColor = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mbarDisableBackColor As Color = Color.FromArgb(224, 224, 224)

        <Browsable(True)>
        <DefaultValue(GetType(Color), "224, 224, 224")>
        <Description("滑条背景颜色（禁止）")>
        <NotifyParentProperty(True)>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property BarDisableBackColor As Color
            Get
                Return mbarDisableBackColor
            End Get
            Set(ByVal value As Color)
                If mbarDisableBackColor = value Then Return
                mbarDisableBackColor = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mslideMinHeight As Integer = 26

        <DefaultValue(26)>
        <Description("滑块最小高度")>
        <NotifyParentProperty(True)>
        Public Property SlideMinHeight As Integer
            Get
                Return mslideMinHeight
            End Get
            Set(ByVal value As Integer)
                If mslideMinHeight = value OrElse value < 1 Then Return
                mslideMinHeight = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mslideNormalBackColor As Color = Color.FromArgb(120, 64, 64, 64)

        <DefaultValue(GetType(Color), "120, 64, 64, 64")>
        <Description("滑块背景颜色（正常）")>
        <NotifyParentProperty(True)>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property SlideNormalBackColor As Color
            Get
                Return mslideNormalBackColor
            End Get
            Set(ByVal value As Color)
                If mslideNormalBackColor = value Then Return
                mslideNormalBackColor = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mslideEnterBackColor As Color = Color.FromArgb(160, 64, 64, 64)

        <DefaultValue(GetType(Color), "160,64, 64, 64")>
        <Description("滑块背景颜色（鼠标进入）")>
        <NotifyParentProperty(True)>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property SlideEnterBackColor As Color
            Get
                Return mslideEnterBackColor
            End Get
            Set(ByVal value As Color)
                If mslideEnterBackColor = value Then Return
                mslideEnterBackColor = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mslideDisableBackColor As Color = Color.FromArgb(192, 192, 192)

        <DefaultValue(GetType(Color), "192, 192, 192")>
        <Description("滑块背景颜色（禁止）")>
        <NotifyParentProperty(True)>
        <Editor(GetType(ColorEditorExt), GetType(UITypeEditor))>
        Public Property SlideDisableBackColor As Color
            Get
                Return mslideDisableBackColor
            End Get
            Set(ByVal value As Color)
                If mslideDisableBackColor = value Then Return
                mslideDisableBackColor = value
                Me.owner.Invalidate()
            End Set
        End Property

        Private mslideRect As RectangleF = RectangleF.Empty

        <Description("滑块rect")>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SlideRect As RectangleF
            Get
                Return mslideRect
            End Get
            Set(ByVal value As RectangleF)
                mslideRect = value
            End Set
        End Property

        Private mslideStatus As ScrollSlideMoveStatus = ScrollSlideMoveStatus.Normal

        <Browsable(False)>
        <Description("滑块鼠标状态")>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SlideStatus As ScrollSlideMoveStatus
            Get
                Return mslideStatus
            End Get
            Set(ByVal value As ScrollSlideMoveStatus)
                mslideStatus = value
            End Set
        End Property

        Public Sub New(ByVal owner As ListBoxExt)
            Me.owner = owner
        End Sub


    End Class

    <Description("鼠标按下功能类型")>
    Protected Friend Class MouseDownClass
        <Description("鼠标按下功能类型")>
        Public Property Type As MouseDownTypes
        <Description("鼠标按下功能对象")>
        Public Property Sender As Object


    End Class

    <Description("选项单击事件参数")>
    Public Class ItemClickEventArgs
        Inherits EventArgs

        <Description("选项")>
        Public Property Item As Item


    End Class

    <Description("选项选中状态更改事件参数")>
    Public Class ItemSelectedChangedEventArgs
        Inherits EventArgs

        <Description("选项")>
        Public Property Item As Item
        Public Property Index As Integer

    End Class

    <Description("选中选项更改事件参数(限制于单选)")>
    Public Class SelectedIndexChangedEventArgs
        Inherits EventArgs

        <Description("选中选项")>
        Public Property Item As Item
        Public Property Index As Integer

    End Class

    <Description("选项自定义绘制事件参数")>
    Public Class DrawItemEventArgs
        Inherits EventArgs

        <Description("Graphics")>
        Public Property Graphics As Graphics
        <Description("当前选项")>
        Public Property Item As Item
        <Description("当前选项图片")>
        Public Property Image As Image
        <Description("当前选项背景画笔")>
        Public Property BackBrush As LinearGradientBrush
        <Description("当前选项文本画笔")>
        Public Property TextBrush As SolidBrush
        Property XoffSet As Integer
        Property BorderPen As Pen

    End Class

    <Description("ImageList自定义索引管理")>
    Public Class Indexer
        Private mkey As String = String.Empty
        Private mindex As Integer = -1
        Private useIntegerIndex As Boolean = True
        Private mimageList As ImageList

        Public Overridable Property ImageList As ImageList
            Get
                Return mimageList
            End Get
            Set(ByVal value As ImageList)
                mimageList = value
            End Set
        End Property

        Public Overridable Property Key As String
            Get
                Return mkey
            End Get
            Set(ByVal value As String)
                mindex = -1
                mkey = If(value, String.Empty)
                Me.useIntegerIndex = False
            End Set
        End Property

        Public Overridable Property Index As Integer
            Get
                Return mindex
            End Get
            Set(ByVal value As Integer)
                mkey = String.Empty
                mindex = value
                Me.useIntegerIndex = True
            End Set
        End Property

        Public Overridable ReadOnly Property ActualIndex As Integer
            Get
                If Me.useIntegerIndex Then Return Me.Index
                If Me.ImageList IsNot Nothing Then Return Me.ImageList.Images.IndexOfKey(Me.Key)
                Return -1
            End Get
        End Property

    End Class

    <Description("选项绘制方式")>
    Public Enum DrawTypes
        [Default]
        Custom
    End Enum

    <Description("选项边框风格")>
    Public Enum ItemBorderStyles
        None
        Line
        GradualLine
    End Enum

    <Description("鼠标单击类型")>
    Protected Friend Enum MouseDownTypes
        None
        MainItem
        Scroll
    End Enum

    <Description("选项鼠标状态")>
    Public Enum ItemMouseStatuss
        Normal
        Enter
    End Enum

    <Description("滚动条滑块鼠标状态")>
    Public Enum ScrollSlideMoveStatus
        Normal
        Enter
    End Enum


End Class

