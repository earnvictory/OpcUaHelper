Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Net.Security
Imports System.Runtime.InteropServices.WindowsRuntime

Namespace ControlExt
    <ToolboxItem(False)>
    Public Class Control
        Inherits Windows.Forms.Control
        Dim mOpacity As Byte
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

        Public Sub New()
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            UpdateStyles()

        End Sub

        Public Function CalculatePos() As Point
            Dim ShowPoint As Point = MousePosition
            If (ShowPoint.Y + Me.Height) > Screen.PrimaryScreen.WorkingArea.Height Then
                ShowPoint.Y = -((Me.PointToScreen(ShowPoint).Y + Me.Height) - Screen.PrimaryScreen.WorkingArea.Height) - 7
            End If
            If (Me.PointToScreen(ShowPoint).X + Me.Width) > Screen.PrimaryScreen.WorkingArea.Width Then
                ShowPoint.X = -Me.Width - 7 + (Screen.PrimaryScreen.WorkingArea.Width - Me.PointToScreen(New Point(0, 0)).X)
            End If
            ShowPoint.Y += 24
            Return ShowPoint
        End Function
        Private mtextAlign As ContentAlignment = ContentAlignment.TopLeft

        <Description("文本对齐方式")>
        Public Property TextAlign As ContentAlignment
            Get
                Return Me.mtextAlign
            End Get
            Set(ByVal value As ContentAlignment)
                If Me.mtextAlign = value Then Return
                Me.mtextAlign = value
                Me.Invalidate()
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
        Dim mBorderColor As Color = SystemColors.ButtonShadow
        Property BorderColor As Color
            Get
                Return mBorderColor
            End Get
            Set(value As Color)
                mBorderColor = value
                Invalidate()
            End Set
        End Property
        Dim mBackColor As Color = Color.White
        Overloads Property BackColor As Color
            Get
                Return mBackColor
            End Get
            Set(value As Color)
                If mBackColor.Equals(value) Then Return
                mBackColor = value
                Invalidate()
            End Set
        End Property
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



    End Class

End Namespace
