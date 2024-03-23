Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO.Ports


Public Class ButtonExt
    Inherits Control
    Implements Windows.Forms.IButtonControl

    Dim mImageList As ImageList
    Property ImageList As ImageList
        Get
            Return mImageList
        End Get
        Set(value As ImageList)
            mImageList = value
            If ImageList IsNot Nothing AndAlso ImageList.Images.Count > mImageIndex Then
                Image = ImageList.Images.Item(mImageIndex)
            End If
            Invalidate()
        End Set
    End Property


    Dim mImageIndex As Integer
    Property ImageIndex As Integer
        Get
            Return mImageIndex
        End Get
        Set(value As Integer)
            mImageIndex = value
            If ImageList IsNot Nothing AndAlso ImageList.Images.Count > mImageIndex Then
                Image = ImageList.Images.Item(mImageIndex)
            End If
            Invalidate()
        End Set
    End Property





    Public Overrides Property Site As ISite
        Get
            Return MyBase.Site
        End Get
        Set(ByVal value As ISite)

            If MyBase.Site Is Nothing OrElse MyBase.Site.Equals(value) Then
                MyBase.Site = value
                Dim referenceService As IReferenceService = CType(Me.GetService(GetType(IReferenceService)), IReferenceService)

                If referenceService IsNot Nothing Then
                    Dim parent As Object() = referenceService.GetReferences(GetType(MicsonControlExt.FormExt))
                    If parent.Length > 0 Then
                        Dim container As Form = TryCast(parent(0), MicsonControlExt.FormExt)
                        FatherForm = container
                    Else
                        'FatherForm = nothing
                    End If

                End If
            End If
        End Set
    End Property

    Private mstyleForm As Form = Nothing


    Public Property FatherForm As Form
        Get
            Return mstyleForm
        End Get
        Set(ByVal value As Form)
            If mstyleForm Is Nothing OrElse mstyleForm.Equals(value) Then
                mstyleForm = value
            End If
        End Set
    End Property

    Overloads Property Name As String
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            MyBase.Name = value

        End Set
    End Property

    Protected Overrides Sub OnClick(e As EventArgs)
        If Enabled = False Then Return
        MyBase.OnClick(e)
    End Sub


    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        If Enabled = False Then Return
        MyBase.OnMouseClick(e)
    End Sub








    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        UpdateStyles()
        FillColor = SystemColors.ButtonFace
        If DesignMode Then Return
    End Sub



    Enum RoundStyle
        None = 0
        All = 1
        Left = 2
        Right = 3
        Top = 4
        Bottom = 5
        TopLeft = 6
        TopRight = 7
        BottomLeft = 8
        BottomRight = 9
    End Enum


    Dim _Ellipse As Boolean
    Property Ellipse As Boolean
        Get
            Return _Ellipse
        End Get
        Set(value As Boolean)
            _Ellipse = value
            Invalidate()

        End Set
    End Property
    Dim mFillAngle As Single = 90
    Property FillAngle As Single
        Get
            Return mFillAngle
        End Get
        Set(value As Single)
            mFillAngle = value
            Invalidate()
        End Set
    End Property

    Dim mRadius As Integer = 8
    Property Radius As Integer
        Get
            Return mRadius
        End Get
        Set(value As Integer)
            mRadius = value
            Invalidate()
        End Set
    End Property

    Dim mFill2D As Boolean = True
    Property Fill2D As Boolean
        Get
            Return mFill2D
        End Get
        Set(value As Boolean)
            mFill2D = value
            Invalidate()
        End Set
    End Property
    Dim mTextAlign As ContentAlignment = ContentAlignment.MiddleCenter
    Property TextAlign As ContentAlignment
        Get
            Return mTextAlign
        End Get
        Set(value As ContentAlignment)
            mTextAlign = value
            Invalidate()
        End Set
    End Property


    Dim Path As System.Drawing.Drawing2D.GraphicsPath
    Dim rect As System.Drawing.Rectangle
    Dim PathBrush As Brush
    Dim linGrBrush As LinearGradientBrush
    Dim mStyle As RoundStyle = RoundStyle.All
    Property Style As RoundStyle
        Get
            Return mStyle
        End Get
        Set(value As RoundStyle)
            mStyle = value
            Invalidate()
        End Set
    End Property
    Dim _MouseDown As Boolean
    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)

        MyBase.OnMouseDown(mevent)
        _MouseDown = True
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        MyBase.OnMouseUp(mevent)
        _MouseDown = False
        Invalidate()
    End Sub
    Dim isEnter As Boolean
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        isEnter = True
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _MouseDown = False
        isEnter = False
        Invalidate()
    End Sub

    Property Auto_Size As Boolean
        Get
            Return AutoSize
        End Get
        Set(value As Boolean)
            AutoSize = value
            Invalidate()
        End Set
    End Property



    Dim mImage As Image
    Overloads Property Image As Image
        Get
            Return mImage
        End Get
        Set(value As Image)
            mImage = value
            Invalidate()
        End Set
    End Property
    Dim mImageAlign As ContentAlignment = ContentAlignment.MiddleLeft
    Property ImageAlign As ContentAlignment
        Get
            Return mImageAlign
        End Get
        Set(value As ContentAlignment)
            mImageAlign = value
            Invalidate()
        End Set
    End Property
    Dim mDrwauBorder As Boolean = True
    Property DrawBorder As Boolean
        Get
            Return mDrwauBorder
        End Get
        Set(value As Boolean)
            mDrwauBorder = value

            Invalidate()
        End Set
    End Property

    Dim mFillColor As Color = SystemColors.ButtonFace 'Color.FromArgb(176, 197, 175)
    Property FillColor As Color
        Get
            Return mFillColor
        End Get
        Set(value As Color)
            mFillColor = value
            Invalidate()
        End Set
    End Property




    Overloads Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Dim _Mouse_Down As Integer = 0
    Property Mouse_Down As Integer
        Get
            Return _Mouse_Down
        End Get
        Set(value As Integer)
            If _Mouse_Down <> value Then
                _Mouse_Down = value
                Invalidate()
            End If

        End Set
    End Property
    'Overloads Property BackColor As Color
    '    Get
    '        If Parent IsNot Nothing Then
    '            MyBase.BackColor = Parent.BackColor
    '        End If
    '        Return MyBase.BackColor
    '    End Get
    '    Set(value As Color)
    '        MyBase.BackColor = value
    '    End Set
    'End Property


    Dim mBorderColor As Color = Color.Black
    Property BorderColor As Color
        Get
            Return mBorderColor
        End Get
        Set(value As Color)
            mBorderColor = value
            Invalidate()
        End Set
    End Property

    Dim ImageRec As Rectangle
    'Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
    '    Try
    '        MyBase.OnPaint(e)
    '        
    '        Dim pen As New System.Drawing.Pen(If(DesignMode, ForeColor, System.Drawing.SystemColors.ButtonShadow), 1)
    '        ' Paint_bmp = New Bitmap(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
    '        Paint_g = e.Graphics ' Graphics.FromImage(Paint_bmp)
    '        Dim Opacity As Integer = 255 * If(isEnter, 0.5, 1)
    '        rect = New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
    '        If Ellipse = True Then
    '            '画外框
    '            Path = New System.Drawing.Drawing2D.GraphicsPath()
    '            Path.AddEllipse(rect)
    '            Me.Region = New Region(Path)
    '            If _MouseDown = True Then
    '                rect.Inflate(-1, -1)
    '            End If
    '            If DrawBorder Then
    '                Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
    '                Path.AddEllipse(rect)
    '                pen = New System.Drawing.Pen(BorderColor, 1)
    '                Paint_g.DrawPath(pen, Path)
    '            End If
    '            '填充区域
    '            rect.Inflate(-1, -1)
    '            Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
    '            Path.AddEllipse(rect)
    '            If Fill2D Then
    '                linGrBrush = New LinearGradientBrush(rect, Color.FromArgb(Opacity, FillColor), Color.FromArgb(Opacity, Color.Black), FillAngle)
    '                Paint_g.FillPath(linGrBrush, Path)
    '            Else
    '                PathBrush = New SolidBrush(Color.FromArgb(Opacity, Color.FromArgb(Opacity, FillColor)))
    '                Paint_g.FillPath(PathBrush, Path)
    '            End If
    '        Else
    '            Path = New System.Drawing.Drawing2D.GraphicsPath()
    '            Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)
    '            Me.Region = New Region(Path)
    '            If _MouseDown = True Then
    '                rect.Inflate(-1, -1)
    '            End If
    '            If DrawBorder Then
    '                Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
    '                Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)
    '                pen = New System.Drawing.Pen(BorderColor, 1)
    '                Paint_g.DrawPath(pen, Path)
    '            End If

    '            rect.Inflate(-1, -1)
    '            Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
    '            Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)
    '            If Fill2D Then
    '                linGrBrush = New LinearGradientBrush(rect, Color.FromArgb(Opacity, FillColor), Color.FromArgb(Opacity, Color.Black), FillAngle)
    '                Paint_g.FillPath(linGrBrush, Path)
    '            Else
    '                PathBrush = New SolidBrush(Color.FromArgb(Opacity, FillColor))
    '                Paint_g.FillPath(PathBrush, Path)
    '            End If

    '        End If
    '        Dim drawFormat As New StringFormat With {.Alignment = StringAlignment.Center,
    '        .LineAlignment = StringAlignment.Center}
    '        Dim textBrush As SolidBrush
    '        If Me.Enabled = False AndAlso DesignMode = False Then
    '            textBrush = New SolidBrush(SystemColors.ControlDarkDark)
    '        Else
    '            textBrush = New SolidBrush(Me.ForeColor)
    '        End If
    '        Paint_g.DrawString(Text, Me.Font, textBrush, rect, drawFormat)
    '        Dim imageRect As Rectangle

    '        If Image IsNot Nothing Then

    '            Dim ImageH As Integer
    '            Dim ImageW As Integer
    '            If Me.Height < Image.Height Then
    '                ImageH = Me.Height
    '            Else
    '                ImageH = Image.Height
    '            End If
    '            Dim mImageW As Integer = Image.Width * ImageH / Image.Height
    '            If ImageW > Me.Width Then
    '                ImageW = Me.Width
    '            Else
    '                ImageW = mImageW
    '            End If
    '            ImageH = ImageH * ImageW / mImageW
    '            Dim colorMatrixElements As Single()() = {
    '           New Single() {1, 0, 0, 0, 0},
    '           New Single() {0, 1, 0, 0, 0},
    '           New Single() {0, 0, 1, 0, 0},
    '           New Single() {0, 0, 0, 1, 0},
    '           New Single() {0, 0, 0, 0, 1}}

    '            Dim colorMatrix As New ColorMatrix(colorMatrixElements)
    '            Dim imageAttributes As New ImageAttributes()
    '            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)
    '            ImageRec = Me.ClientRectangle
    '            Select Case ImageAlign

    '                Case ContentAlignment.MiddleCenter
    '                    imageRect = New Rectangle(CSng(ImageRec.Width / 2 - ImageW / 2) - 1, CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.MiddleLeft
    '                    imageRect = New Rectangle(1, CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.MiddleRight
    '                    imageRect = New Rectangle(CSng(ImageRec.Width - ImageW) - 1, CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.TopCenter
    '                    imageRect = New Rectangle(CSng(ImageRec.Width / 2 - ImageW / 2), 0, ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.TopLeft
    '                    imageRect = New Rectangle(1, 0, ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.TopRight
    '                    imageRect = New Rectangle(CSng(ImageRec.Width - ImageW) - 1, 0, ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.BottomCenter
    '                    imageRect = New Rectangle(CSng(ImageRec.Width / 2 - ImageW / 2), CSng(ImageRec.Height - ImageH), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.BottomLeft
    '                    imageRect = New Rectangle(1, CSng(ImageRec.Height - ImageH), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '                Case ContentAlignment.BottomRight
    '                    imageRect = New Rectangle(CSng(ImageRec.Width - ImageW) - 1, CSng(ImageRec.Height - ImageH), ImageW, ImageH)
    '                    Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
    '            End Select
    '        End If
    '        ' e.Graphics.DrawImage(Paint_bmp, 0, 0)
    '       Catch ex As Exception

    '    End Try

    'End Sub
    Dim mLinearGradientColor As Color = Color.Black
    Property LinearGradientColor As Color
        Get
            Return mLinearGradientColor
        End Get
        Set(value As Color)
            mLinearGradientColor = value
            Invalidate()
        End Set
    End Property

    <Description("选项绘制方式")>
    Public Enum DrawTypes
        [Default]
        Custom
    End Enum

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

    <Description("绘制文本内容")>
    Protected Overridable Sub OnDrawText(ByVal e As DrawItemEventArgs)

    End Sub
    Class DrawItemEventArgs
        Inherits EventArgs
        Property G As Graphics
        Property Font As Font
        Property Brush As SolidBrush
        Property Rect As Rectangle
        Property DrawFormat As StringFormat
    End Class
    Public Function Graphics_SetSmoothHighQuality(g As Graphics) As Graphics
        With g
            .SmoothingMode = SmoothingMode.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .CompositingQuality = CompositingQuality.HighQuality
        End With
        Return g
    End Function
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        MainRect = New Rectangle(0, 0, Me.Width, Me.Height)

        Me.Invalidate()
    End Sub

    Dim MainRect As Rectangle

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Try
            'MyBase.OnPaintBackground(e)
            MyBase.OnPaint(e)

            If MainRect.Equals(Rectangle.Empty) Then Return


            Dim Paint_g As Graphics = Graphics_SetSmoothHighQuality(e.Graphics)

            Paint_g.Clear(If(Parent?.BackColor, Me.BackColor))
            Dim Opacity As Integer = 255 * If(isEnter, 0.5, 1)
            Dim PenW As Integer = If(isEnter, 2, 1)
            Dim BorderColor As Color = If(isEnter, Color.FromArgb(100, Color.White), Me.BorderColor)
            rect = New Rectangle(0, 0, Me.Width, Me.Height)
            If _MouseDown = True Then
                rect.Inflate(-1, -1)
            End If
            Dim pen As New System.Drawing.Pen(If(DesignMode, ForeColor, System.Drawing.SystemColors.ButtonShadow), PenW)
            If Ellipse = True Then
                Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
                Path.AddEllipse(rect)

                Paint_g.SetClip(Path)
                If DrawBorder OrElse isEnter Then
                    rect.Inflate(-PenW, -PenW)
                    pen = New System.Drawing.Pen(BorderColor, PenW)
                    Paint_g.DrawEllipse(pen, rect)
                    pen.Dispose()
                    rect.Inflate(-PenW, -PenW)
                End If
                '填充区域
                Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
                Path.AddEllipse(rect)
                If Fill2D Then
                    linGrBrush = New LinearGradientBrush(rect, Color.FromArgb(Opacity, FillColor), LinearGradientColor, 90)
                    Paint_g.FillPath(linGrBrush, Path)
                Else
                    PathBrush = New SolidBrush(Color.FromArgb(Opacity, FillColor))
                    Paint_g.FillPath(PathBrush, Path)
                End If

            Else
                Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)

                Paint_g.SetClip(Path)
                If DrawBorder OrElse isEnter Then
                    Path = New System.Drawing.Drawing2D.GraphicsPath() With {.FillMode = System.Drawing.Drawing2D.FillMode.Winding}
                    rect.Inflate(-PenW, -PenW)
                    Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)
                    pen = New System.Drawing.Pen(BorderColor, PenW)
                    Paint_g.DrawPath(pen, Path)
                    pen.Dispose()
                    rect.Inflate(-PenW, -PenW)
                End If
                Path = ControlExt.Common.DrawRoundRect(rect, Radius, Style)
                If Fill2D Then
                    linGrBrush = New LinearGradientBrush(rect, Color.FromArgb(Opacity, FillColor), LinearGradientColor, 90)
                    Paint_g.FillPath(linGrBrush, Path)
                Else
                    PathBrush = New SolidBrush(Color.FromArgb(Opacity, FillColor))
                    Paint_g.FillPath(PathBrush, Path)
                End If
            End If
            Dim drawFormat As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}

            Dim textBrush As SolidBrush
            If Me.Enabled = False AndAlso DesignMode = False Then
                textBrush = New SolidBrush(SystemColors.ControlDarkDark)
            Else
                textBrush = New SolidBrush(Me.ForeColor)
            End If
            If Me.DrawType = DrawTypes.[Default] Then
                Paint_g.DrawString(Text, Me.Font, textBrush, rect, drawFormat)
            Else
                Me.OnDrawText(New DrawItemEventArgs With {.G = Paint_g, .Font = Me.Font, .Rect = rect, .Brush = textBrush, .DrawFormat = drawFormat})
            End If


            Dim imageRect As RectangleF

            If Image IsNot Nothing Then

                Dim ImageH As Integer
                Dim ImageW As Integer
                If Me.Height < Image.Height Then
                    ImageH = Me.Height
                Else
                    ImageH = Image.Height
                End If
                Dim mImageW As Integer = Image.Width * ImageH / Image.Height
                If ImageW > Me.Width Then
                    ImageW = Me.Width
                Else
                    ImageW = mImageW
                End If
                ImageH = ImageH * ImageW / mImageW




                Dim colorMatrixElements As Single()() = {
               New Single() {1, 0, 0, 0, 0},
               New Single() {0, 1, 0, 0, 0},
               New Single() {0, 0, 1, 0, 0},
               New Single() {0, 0, 0, 1, 0},
               New Single() {0, 0, 0, 0, 1}}

                Dim colorMatrix As New ColorMatrix(colorMatrixElements)
                Dim imageAttributes As New ImageAttributes()
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)
                ImageRec = New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
                ImageRec.Inflate(-1, -1)
                ImageH = Math.Min(ImageRec.Height, ImageH)
                ImageW = Math.Min(ImageRec.Width, ImageW)
                Select Case ImageAlign
                    Case ContentAlignment.MiddleCenter
                        imageRect = New RectangleF(ImageRec.Left + (ImageRec.Width / 2 - ImageW / 2), ImageRec.Top + CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect)', 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.MiddleLeft
                        imageRect = New Rectangle(ImageRec.Left, ImageRec.Top + CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.MiddleRight
                        imageRect = New Rectangle(ImageRec.Left + CSng(ImageRec.Width - ImageW), ImageRec.Top + CSng(ImageRec.Height / 2 - ImageH / 2), ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.TopCenter
                        imageRect = New Rectangle(ImageRec.Left + CSng(ImageRec.Width / 2 - ImageW / 2), ImageRec.Top, ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.TopLeft
                        imageRect = New Rectangle(ImageRec.Left, ImageRec.Top, ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.TopRight
                        imageRect = New Rectangle(ImageRec.Left + CSng(ImageRec.Width - ImageW), ImageRec.Top, ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.BottomCenter
                        imageRect = New Rectangle(ImageRec.Left + CSng(ImageRec.Width / 2 - ImageW / 2), ImageRec.Top + CSng(ImageRec.Height - ImageH), ImageW, ImageH)
                                                    'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.BottomLeft
                        imageRect = New Rectangle(ImageRec.Left, ImageRec.Top + CSng(ImageRec.Height - ImageH), ImageW, ImageH)
                                                   ' Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                    Case ContentAlignment.BottomRight
                        imageRect = New Rectangle(ImageRec.Left + CSng(ImageRec.Width - ImageW), ImageRec.Top + CSng(ImageRec.Height - ImageH), ImageW, ImageH)
                        'Paint_g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, imageAttributes)
                End Select
                Paint_g.DrawImage(Image, imageRect)


            End If

        Catch ex As Exception
            MicsonControlExt.MessageBoxExt.Show(Nothing, ex.Message & vbCrLf & ex.StackTrace)
        End Try

    End Sub


    Private myDialogResult As DialogResult
    Public Property DialogResult() As DialogResult Implements IButtonControl.DialogResult
        Get
            Return Me.myDialogResult
        End Get

        Set
            If [Enum].IsDefined(GetType(DialogResult), Value) Then
                Me.myDialogResult = Value
            End If
        End Set
    End Property
    Property IsDefault As Boolean
    ' Add implementation to the IButtonControl.NotifyDefault method.
    Public Sub NotifyDefault(value As Boolean) Implements IButtonControl.NotifyDefault
        If Me.IsDefault <> value Then
            Me.IsDefault = value
        End If
    End Sub

    ' Add implementation to the IButtonControl.PerformClick method.
    Public Sub PerformClick() Implements IButtonControl.PerformClick
        If Me.CanSelect Then
            Me.OnClick(EventArgs.Empty)
        End If
    End Sub
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
End Class
