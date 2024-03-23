Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Drawing2D
Imports System.Media
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar
Imports MicsonControlExt.FormExt

Public Class FormExt


    Structure NonClientSizeInfo
        Public CaptionButtonSize As Size
        Public BorderSize As Size
        Public CaptionHeight As Integer
        Public CaptionRect As Rectangle
        Public Rect As Rectangle
        Public ClientRect As Rectangle
        Public Width As Integer
        Public Height As Integer
    End Structure

    Const WM_NCACTIVATE As Integer = &H86
    Const WM_NCPAINT As Integer = &H85
    Const WM_NCLBUTTONDOWN As Integer = &HA1
    Const WM_NCRBUTTONDOWN As Integer = &HA4
    ' Const WM_NCRBUTTONUP As Integer = &HA5
    '  Const WM_NCMOUSEMOVE As Integer = &HA0
    Const WM_NCLBUTTONUP As Integer = &HA2
    'Const WM_NCCALCSIZE As Integer = &H83
    'Const WM_NCMOUSEHOVER As Integer = &H2A0
    'Const WM_NCMOUSELEAVE As Integer = &H2A2
    'Const WM_NCHITTEST As Integer = &H84
    'Const WM_NCCREATE As Integer = &H81
    'Const WM_LBUTTONDOWN As Integer = &H201
    'Const WM_CAPTURECHANGED As Integer = &H215
    'Const WM_LBUTTONUP As Integer = &H202
    Const WM_SETCURSOR As Integer = &H20
    ' Const WM_CLOSE As Integer = &H10
    Const WM_SYSCOMMAND As Integer = &H112
    'Const WM_MOUSEMOVE As Integer = &H200
    'Const WM_SIZE As Integer = &H5
    'Const WM_SIZING As Integer = &H214
    'Const WM_GETMINMAXINFO As Integer = &H24
    'Const WM_ENTERSIZEMOVE As Integer = &H231
    'Const WM_WINDOWPOSCHANGING As Integer = &H46
    'Const WMSZ_BOTTOM As Integer = 6
    'Const WMSZ_BOTTOMLEFT As Integer = 7
    'Const WMSZ_BOTTOMRIGHT As Integer = 8
    'Const WMSZ_LEFT As Integer = 1
    'Const WMSZ_RIGHT As Integer = 2
    'Const WMSZ_TOP As Integer = 3
    'Const WMSZ_TOPLEFT As Integer = 4
    'Const WMSZ_TOPRIGHT As Integer = 5
    'Const MK_LBUTTON As Integer = &H1
    Const SC_CLOSE As Integer = &HF060
    Const SC_MAXIMIZE As Integer = &HF030
    Const SC_MINIMIZE As Integer = &HF020
    Const SC_RESTORE As Integer = &HF120
    Const SC_CONTEXTHELP As Integer = &HF180
    Const HTCAPTION As Integer = 2
    Const HTCLOSE As Integer = 20
    Const HTHELP As Integer = 21
    Const HTMAXBUTTON As Integer = 9
    Const HTMINBUTTON As Integer = 8
    'Const HTTOP As Integer = 12
    'Const SM_CYBORDER As Integer = 6
    'Const SM_CXBORDER As Integer = 5
    'Const SM_CYCAPTION As Integer = 4
    ' Const CS_DropSHADOW As Integer = &H20000
    ' Const GCL_STYLE As Integer = (-26)
    '''<summary>返回窗口设备厂描述表</summary>
    '''<param name="hwnd">将获取其设备场景的窗口</param>
    Public Declare Auto Function GetWindowDC Lib "user32.dll" Alias "GetWindowDC" (ByVal hwnd As Integer) As Integer

    '''<summary>返回窗口坐标</summary>
    '''<param name="hwnd">想获得范围矩形的那个窗口的句柄</param>
    '''<param name="lpRect">屏幕坐标中随同窗口装载的矩形</param>
    Public Declare Auto Function GetWindowRect Lib "user32.dll" Alias "GetWindowRect" (ByVal hwnd As Integer, ByRef lpRect As RECT) As Integer
    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure
    '''<summary>从附加窗口内存中返回长型数值</summary>
    '''<param name="hwnd">欲为其获取信息的窗口的句柄</param>
    '''<param name="nIndex">
    '''欲取回的信息,可以是下述任何一个常数：
    '''     GWL_EXSTYLE    扩展窗口样式
    '''     GWL_STYLE      窗口样式
    '''     GWL_WNDPROC    该窗口的窗口函数的地址
    '''     GWL_HINSTANCE  拥有窗口的实例的句柄
    '''     GWL_HWNDPARENT 该窗口之父的句柄。不要用SetWindowWord来改变这个值
    '''     GWL_ID         对话框中一个子窗口的标识符
    '''     GWL_USERDATA   含义由应用程序规定
    '''     DWL_DLGPROC    这个窗口的对话框函数地址
    '''     DWL_MSGRESULT  在对话框函数中处理的一条消息返回的值
    '''     DWL_USER       含义由应用程序规定
    '''</param>
    Public Declare Auto Function GetWindowLong Lib "user32.dll" Alias "GetWindowLong" (
    ByVal hwnd As Integer,
    ByVal nIndex As Integer
) As Integer

    '''<summary>释放设备描述表</summary>
    '''<param name="hwnd">要释放的设备场景相关的窗口句柄</param>
    '''<param name="hdc">要释放的设备场景句柄</param>
    Public Declare Auto Function ReleaseDC Lib "user32.dll" Alias "ReleaseDC" (ByVal hwnd As Integer, ByVal hdc As Integer) As Integer

    '''<summary>
    '''设置附加类内存长数值
    '''     hwnd:Integer型,欲为其设置类信息的那个窗口的句柄
    '''     nIndex:Integer型,参考GetClassLong函数的nIndex参数说明
    '''     dwNewLong:Integer型,类信息的新值,具体取决于nIndex
    '''</summary>
    Public Declare Auto Function SetClassLong Lib "user32.dll" Alias "SetClassLong" (ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer

    '''<summary>返回窗口类数据</summary>
    '''<param name="hwnd">要为其获得类信息的窗口的句柄</param>
    '''<param name="nIndex">
    '''欲取得的信息,可能是下述任何一个常数：（正数表示一个字节偏移,用于取得在额外字节中为这个类分配的类信息）
    '''     GCL_CBCLSEXTRA      这个类结构中分配的额外字节数
    '''     GCL_CBWNDEXTRA      窗口结构里为这个类中每个窗口分配的额外字节数
    '''     GCL_HBRBACKGROUND   描绘这个类每个窗口的背景时,使用的默认刷子的句柄
    '''     GCL_HCURSOR         指向这个类窗口默认光标的句柄
    '''     GCL_HICON           这个类中窗口默认图标的句柄
    '''     GCL_HICONSM         这个类的小图标
    '''     GCL_HMODULE         这个类的模块的句柄
    '''     GCL_MENUNAME        为类菜单取得名称或资源ID
    '''     GCL_STYLE           这个类的样式
    '''     GCL_WNDPROC         取得类窗口函数（该类窗口的默认窗口函数）的地址
    '''</param>
    Public Declare Auto Function GetClassLong Lib "user32.dll" Alias "GetClassLong" (ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer

    '''<summary>
    '''设置窗口大小、位置、顺序
    '''     hwnd:Integer型,欲定位的窗口
    '''     hWndInsertAfter:Integer型,窗口句柄。在窗口列表中,窗口hwnd会置于这个窗口句柄的后面。也可能选用下述值之一：
    '''     HWND_BOTTOM
    '''     将窗口置于窗口列表底部
    '''     HWND_TOP
    '''     将窗口置于Z序列的顶部；Z序列代表在分级结构中,窗口针对一个给定级别的窗口显示的顺序
    '''     HWND_TOPMOST
    '''     将窗口置于列表顶部,并位于任何最顶部窗口的前面
    '''     HWND_NOTOPMOST
    '''     将窗口置于列表顶部,并位于任何最顶部窗口的后面
    '''     x:Integer型,窗口新的x坐标。如hwnd是一个子窗口,则x用父窗口的客户区坐标表示
    '''     y:Integer型,窗口新的y坐标。如hwnd是一个子窗口,则y用父窗口的客户区坐标表示
    '''     cx:Integer型,指定新的窗口宽度
    '''     cy:Integer型,指定新的窗口高度
    '''     wFlags:Integer型,包含了旗标的一个整数
    '''     SWP_DRAWFRAME
    '''     围绕窗口画一个框
    '''     SWP_HIDEWINDOW
    '''     隐藏窗口
    '''     SWP_NOACTIVATE
    '''     不激活窗口
    '''     SWP_NOMOVE
    '''     保持当前位置（x和y设定将被忽略）
    '''     SWP_NOREDRAW
    '''     窗口不自动重画
    '''     SWP_NOSIZE
    '''     保持当前大小（cx和cy会被忽略）
    '''     SWP_NOZORDER
    '''     保持窗口在列表的当前位置（hWndInsertAfter将被忽略）
    '''     SWP_SHOWWINDOW
    '''     显示窗口
    '''     SWP_FRAMECHANGED
    '''     强迫一条WM_NCCALCSIZE消息进入窗口,即使窗口的大小没有改变
    '''</summary>
    Public Declare Auto Function SetWindowPos Lib "user32.dll" Alias "SetWindowPos" (
    ByVal hwnd As Integer,
    ByVal hWndInsertAfter As Integer,
    ByVal x As Integer,
    ByVal y As Integer,
    ByVal cx As Integer,
    ByVal cy As Integer,
    ByVal wFlags As Integer
) As Integer
    <DefaultValue("")>
    <Browsable(True)>
    <Category("ControlBox")>
    Public Overridable Property CaptionContextMenu As ContextMenuStrip

    Protected Overridable Sub OnCaptionContextMenu(ByVal x As Integer, ByVal y As Integer)
        If Me.CaptionContextMenu IsNot Nothing Then Me.CaptionContextMenu.Show(x, y)
    End Sub

    <Category("ControlBox")>
    <Description("Close button image in control box.")>
    <DisplayName("CloseButtonImage")>
    <DesignOnly(True)>
    Public Property CloseButtonImage As Image
    <Category("ControlBox")>
    <Description("Close button image pressed down in control box.")>
    <DisplayName("CloseButtonPressDownImage")>
    <DesignOnly(True)>
    Public Property CloseButtonPressDownImage As Image
    <Category("ControlBox")>
    <Description("Close button image hover in control box.")>
    <DisplayName("CloseButtonHoverImage")>
    <DesignOnly(True)>
    Public Property CloseButtonHoverImage As Image
    <Category("ControlBox")>
    <Description("Maximum button image in control box.")>
    <DisplayName("MaximumButtonImage")>
    <DesignOnly(True)>
    Public Property MaximumButtonImage As Image
    <Category("ControlBox")>
    <Description("Maximum button hover image in control box.")>
    <DisplayName("MaximumButtonHoverImage")>
    <DesignOnly(True)>
    Public Property MaximumButtonHoverImage As Image
    <Category("ControlBox")>
    <Description("Maximum button pressed down image in control box.")>
    <DisplayName("MaximumButtonPressDownImage")>
    <DesignOnly(True)>
    Public Property MaximumButtonPressDownImage As Image
    <Category("ControlBox")>
    <Description("Maximum Normal button image in control box.")>
    <DisplayName("MaximumNormalButtonImage")>
    <DesignOnly(True)>
    Public Property MaximumNormalButtonImage As Image
    <Category("ControlBox")>
    <Description("Maximum Normal button hover image in control box.")>
    <DisplayName("MaximumNormalButtonHoverImage")>
    <DesignOnly(True)>
    Public Property MaximumNormalButtonHoverImage As Image
    <Category("ControlBox")>
    <Description("Maximum Normal button pressed down image in control box.")>
    <DisplayName("MaximumNormalButtonPressDownImage")>
    <DesignOnly(True)>
    Public Property MaximumNormalButtonPressDownImage As Image
    <Category("ControlBox")>
    <Description("Minimum button image in control box.")>
    <DisplayName("MinimumButtonImage")>
    <DesignOnly(True)>
    Public Property MinimumButtonImage As Image
    <Category("ControlBox")>
    <Description("Minimum button hover image in control box.")>
    <DisplayName("MinimumButtonHoverImage")>
    <DesignOnly(True)>
    Public Property MinimumButtonHoverImage As Image
    <Category("ControlBox")>
    <Description("Minimum button pressed down image in control box.")>
    <DisplayName("MinimumButtonPressDownImage")>
    <DesignOnly(True)>
    Public Property MinimumButtonPressDownImage As Image
    <Category("ControlBox")>
    <Description("Help button image in control box.")>
    <DisplayName("HelpButtonImage")>
    <DesignOnly(True)>
    Public Property HelpButtonImage As Image
    <Category("ControlBox")>
    <Description("Help button hover image in control box.")>
    <DisplayName("HelpButtonHoverImage")>
    <DesignOnly(True)>
    Public Property HelpButtonHoverImage As Image
    <Category("ControlBox")>
    <Description("Help button pressed down image in control box.")>
    <DisplayName("HelpButtonPressDownImage")>
    <DesignOnly(True)>
    Public Property HelpButtonPressDownImage As Image
    Dim mCaptionColor As Color = Color.White
    '  <DesignOnly(True)>
    ' <Category("CaptionColor")>
    <Description("The color of caption.")>
    <DisplayName("CaptionTextColor")>
    <DefaultValue(GetType(Color), "255,255,255")>
    Public Property CaptionTextColor As Color
        Get
            Return mCaptionColor
        End Get
        Set(value As Color)
            mCaptionColor = value
            Invalidate()
        End Set
    End Property

    Dim mCaptionBackgroundColor As Color = SystemColors.Highlight 'Color.FromArgb(0, 120, 215)
    ' <Category("CaptionColor")>
    <Description("The color of caption.")>
    <DisplayName("CaptionBackgroundColor")>
    <DefaultValue(GetType(Color), "0,120,215")>
    Public Property CaptionBackgroundColor As Color
        Get
            Return mCaptionBackgroundColor
        End Get
        Set(value As Color)
            mCaptionBackgroundColor = value
            Invalidate()
        End Set
    End Property

    Dim mBorderColor As Color = SystemColors.Highlight 'Color.FromArgb(0, 120, 215)
    ' <Category("CaptionColor")>
    <Description("The color of caption.")>
    <DisplayName("BorderColor")>
    <DefaultValue(GetType(Color), "0,120,215")>
    Public Property BorderColor As Color
        Get
            Return mBorderColor
        End Get
        Set(value As Color)
            mBorderColor = value
            DrawCaption(Me.Handle)
        End Set
    End Property

    Overloads Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            DrawCaption(Me.Handle)
        End Set
    End Property


    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Overloads Property Opacity As Double
        Get
            Return MyBase.Opacity
        End Get
        Set(value As Double)
            MyBase.Opacity = value
        End Set
    End Property

    Private Function GetNonClientInfo(ByVal hwnd As IntPtr) As NonClientSizeInfo

        Dim info As New NonClientSizeInfo() With {.CaptionButtonSize = SystemInformation.CaptionButtonSize, .CaptionHeight = SystemInformation.CaptionHeight}
        Select Case Me.FormBorderStyle
            Case System.Windows.Forms.FormBorderStyle.Fixed3D
                info.BorderSize = SystemInformation.FixedFrameBorderSize
            Case System.Windows.Forms.FormBorderStyle.FixedDialog
                info.BorderSize = SystemInformation.FixedFrameBorderSize
            Case System.Windows.Forms.FormBorderStyle.FixedSingle
                info.BorderSize = SystemInformation.FixedFrameBorderSize
            Case System.Windows.Forms.FormBorderStyle.FixedToolWindow
                info.BorderSize = SystemInformation.FixedFrameBorderSize
                info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize
                info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight
            Case System.Windows.Forms.FormBorderStyle.Sizable
                info.BorderSize = SystemInformation.FrameBorderSize
            Case System.Windows.Forms.FormBorderStyle.SizableToolWindow
                info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize
                info.BorderSize = SystemInformation.FrameBorderSize
                info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight
            Case Else
                info.BorderSize = SystemInformation.BorderSize
        End Select
        Dim areatRect As New RECT()
        GetWindowRect(hwnd, areatRect)
        Dim width As Integer = areatRect.Right - areatRect.Left
        Dim height As Integer = areatRect.Bottom - areatRect.Top
        info.Width = width
        info.Height = height
        Dim xy As New Point(areatRect.Left, areatRect.Top)
        xy.Offset(-areatRect.Left, -areatRect.Top)
        info.CaptionRect = New Rectangle(xy.X, xy.Y + info.BorderSize.Height, width + Padding.Left + Padding.Right, info.CaptionHeight + Padding.Top)
        info.Rect = New Rectangle(xy.X, xy.Y, width, height)
        info.ClientRect = New Rectangle(xy.X + info.BorderSize.Width, xy.Y + info.CaptionHeight + info.BorderSize.Height, width - info.BorderSize.Width * 2, height - info.CaptionHeight - info.BorderSize.Height * 2)
        Return info
    End Function
    Dim mDrawIco As Boolean = False
    Property DrawIco As Boolean
        Get
            Return mDrawIco
        End Get
        Set(value As Boolean)
            mDrawIco = value
            Invalidate()
        End Set
    End Property
    Private Sub DrawTitle(ByVal g As Graphics, ByVal ncInfo As NonClientSizeInfo) ', ByVal active As Boolean
        Dim titleX As Integer

        If Me.ShowIcon AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then
            Dim iconSize As Size = SystemInformation.SmallIconSize
            If DrawIco Then
                g.DrawIcon(Me.Icon, New Rectangle(New Point(ncInfo.BorderSize.Width, ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - iconSize.Height) / 2), iconSize))
            End If
            titleX = ncInfo.BorderSize.Width + If(DrawIco, iconSize.Width, 0) + ncInfo.BorderSize.Width
        Else
            titleX = ncInfo.BorderSize.Width
        End If
        Dim Caption As String = If(DesignMode OrElse ShowProductVersion = False, Me.Text, Application.ProductName & "-" & Me.Text & "-" & Application.ProductVersion)
        Dim captionTitleSize As SizeF = g.MeasureString(Caption, SystemFonts.CaptionFont)
        Dim cationBrush As New SolidBrush(CaptionTextColor)
        g.DrawString(Caption, Me.Font, cationBrush, New RectangleF(titleX, (ncInfo.BorderSize.Height + ncInfo.CaptionHeight - captionTitleSize.Height) / 2, ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width * 2 - SystemInformation.MinimumWindowSize.Width, ncInfo.CaptionRect.Height), StringFormat.GenericTypographic)
        cationBrush.Dispose()
    End Sub
    Property ShowProductVersion As Boolean = False
    Dim mClientGapColor As Color = Color.Empty
    Property ClientGapColor As Color
        Get
            Return mClientGapColor
        End Get
        Set(value As Color)
            mClientGapColor = value
            Me.Invalidate()
        End Set
    End Property
    Private Sub DrawBorder(ByVal g As Graphics, ByVal ncInfo As NonClientSizeInfo, ByVal background As Brush) ', ByVal active As Boolean
        Dim borderTop As New Rectangle(ncInfo.Rect.Left, ncInfo.Rect.Top, ncInfo.Rect.Left + ncInfo.Rect.Width, ncInfo.Rect.Top + ncInfo.BorderSize.Height)
        'Dim topimg As New Bitmap(borderTop.Width, borderTop.Height)
        'Dim Topg As Graphics = Graphics.FromImage(topimg)
        'Topg.FillRectangle(Brushes.Gold, borderTop)
        'Topg.DrawLine(Pens.White, New Point(borderTop.X, borderTop.Y + borderTop.Height / 2), New Point(borderTop.X + borderTop.Width, borderTop.Y + borderTop.Height / 2))
        'Topg.Dispose()
        'g.DrawImage(topimg, borderTop.X, borderTop.Y)

        Dim borderLeft As New Rectangle(New Point(ncInfo.Rect.Location.X, ncInfo.Rect.Location.Y + ncInfo.BorderSize.Height), New Size(ncInfo.BorderSize.Width, ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height))
        'Dim Leftimg As New Bitmap(borderLeft.Width, borderLeft.Height)
        'Dim Leftg As Graphics = Graphics.FromImage(Leftimg)
        'Leftg.FillRectangle(Brushes.Gold, borderLeft)
        'Leftg.DrawLine(Pens.White, New Point(borderLeft.X + borderLeft.Width / 2, borderTop.Y), New Point(borderTop.X + borderTop.Width / 2, borderTop.Y + borderTop.Height))
        'Leftg.Dispose()
        'g.DrawImage(Leftimg, borderLeft.X, borderLeft.Y)

        Dim borderRight As New Rectangle(ncInfo.Rect.Left + ncInfo.Rect.Width - ncInfo.BorderSize.Width, ncInfo.Rect.Top + ncInfo.BorderSize.Height, ncInfo.BorderSize.Width, ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height)
        'Dim Rightimg As New Bitmap(borderRight.Width, borderRight.Height)
        'Dim Rightg As Graphics = Graphics.FromImage(Rightimg)
        'Rightg.FillRectangle(Brushes.Gold, borderRight)
        'Rightg.DrawLine(Pens.White, New Point(borderRight.X + borderRight.Width / 2, borderTop.Y), New Point(borderRight.X + borderRight.Width / 2, borderRight.Y + borderRight.Height))
        'Rightg.Dispose()
        'g.DrawImage(Rightimg, borderRight.X, borderRight.Y)


        Dim borderBottom As New Rectangle(ncInfo.Rect.Left + ncInfo.BorderSize.Width, ncInfo.Rect.Top + ncInfo.Rect.Height - ncInfo.BorderSize.Height, ncInfo.Rect.Width - ncInfo.BorderSize.Width * 2, ncInfo.BorderSize.Height) 'ncInfo.Rect.Height
        'Dim Bottomimg As New Bitmap(borderBottom.Width, borderBottom.Height)
        'Dim Bottomg As Graphics = Graphics.FromImage(Bottomimg)
        'Bottomg.FillRectangle(Brushes.Gold, borderBottom)
        'Bottomg.DrawLine(Pens.White, New Point(borderBottom.X, borderBottom.Y + borderBottom.Height / 2), New Point(borderBottom.X + borderBottom.Width, borderBottom.Y + borderBottom.Height / 2))
        'Bottomg.Dispose()
        'g.DrawImage(Bottomimg, borderBottom.X - 2, borderBottom.Y - 2)

        Dim Topfx As New Rectangle(ncInfo.Rect.Left + ncInfo.BorderSize.Width, If(ControlBox = False AndAlso Me.Text.Length = 0, ncInfo.BorderSize.Height, ncInfo.CaptionRect.Y + ncInfo.CaptionHeight), ncInfo.Rect.Width - ncInfo.BorderSize.Width * 2, (ncInfo.Rect.Height - ClientRectangle.Height - If(ControlBox = False AndAlso Me.Text.Length = 0, ncInfo.BorderSize.Height, ncInfo.CaptionRect.Y + ncInfo.CaptionHeight) - ncInfo.BorderSize.Width) / 2)
        'If Topfx.Width > 0 AndAlso Topfx.Height > 0 Then
        '    Dim Fxtopimg As New Bitmap(Topfx.Width, Topfx.Height)
        '    Dim Topfxg As Graphics = Graphics.FromImage(Fxtopimg)
        '    Topfxg.FillRectangle(Brushes.Gold, Topfx)
        '    Topfxg.Dispose()
        '    g.DrawImage(Fxtopimg, Topfx.X, Topfx.Y)
        'End If

        Dim Leftfx As New Rectangle(ncInfo.Rect.Left + ncInfo.BorderSize.Width, Topfx.Y, (ncInfo.Rect.Width - ClientRectangle.Width - ncInfo.BorderSize.Width * 2) / 2, ncInfo.Rect.Height - If(ControlBox = False AndAlso Me.Text.Length = 0, 0, ncInfo.CaptionRect.Y + ncInfo.CaptionHeight) - ncInfo.BorderSize.Width * 2)
        'Dim FxLeftimg As New Bitmap(Leftfx.Width, Leftfx.Height)
        'Dim Leftfxg As Graphics = Graphics.FromImage(FxLeftimg)
        'Leftfxg.FillRectangle(background, Leftfx)
        'Leftfxg.Dispose()
        'g.DrawImage(FxLeftimg, Leftfx.X, Leftfx.Y)

        Dim Bottomfx As New Rectangle(Leftfx.X, Leftfx.Y + ClientRectangle.Height + Topfx.Height, Topfx.Width, Topfx.Height) 'ncInfo.Rect.Height - ncInfo.CaptionHeight - ncInfo.BorderSize.Width * 2 - Topfx.Height - ClientRectangle.Height
        'Dim FxBottomimg As New Bitmap(Bottomfx.Width, Bottomfx.Height)
        'Dim Bottomfxg As Graphics = Graphics.FromImage(FxBottomimg)
        'Bottomfxg.FillRectangle(background, Bottomfx)
        'Bottomfxg.Dispose()
        'g.DrawImage(FxBottomimg, Bottomfx.X, Bottomfx.Y)


        Dim Rightfx As New Rectangle(ncInfo.Rect.Right - ncInfo.BorderSize.Width - Leftfx.Width, Leftfx.Y, Leftfx.Width, Leftfx.Height)
        'Dim FxRightimg As New Bitmap(Rightfx.Width, Rightfx.Height)
        'Dim Rightfxg As Graphics = Graphics.FromImage(FxRightimg)
        'Rightfxg.FillRectangle(background, Rightfx)
        'Rightfxg.Dispose()
        'g.DrawImage(FxRightimg, Rightfx.X, Rightfx.Y)


        ' Dim FxBrush As New SolidBrush(If(ClientGapColor.Equals(Color.Empty), SystemColors.Highlight, ClientGapColor))
        'Dim FxPath As New GraphicsPath
        'FxPath.AddRectangles(New Rectangle() {borderTop, borderLeft, borderRight, borderBottom})
        'g.FillPath(FxBrush, FxPath)
        g.FillRectangles(background, New Rectangle() {Topfx, Leftfx, Bottomfx, Rightfx})
        Dim BorderPath As New GraphicsPath
        BorderPath.AddRectangles(New Rectangle() {borderTop, borderLeft, borderRight, borderBottom})
        Dim BorderBrush As New SolidBrush(Color.FromArgb(150, SystemColors.Highlight))
        g.SetClip(BorderPath)
        g.FillPath(Brushes.Gold, BorderPath) 'background







        '  FxBrush.Dispose()
    End Sub

    Dim ncInfo As NonClientSizeInfo
    'Dim PrencInfo As NonClientSizeInfo
    Private Sub DrawCaption(ByVal hwnd As IntPtr)
        Dim dc As IntPtr
        Dim g As Graphics
        Dim iconSize As Size
        ncInfo = GetNonClientInfo(hwnd)
        'If PrencInfo.Equals(ncInfo) Then Return
        'PrencInfo = ncInfo
        iconSize = SystemInformation.SmallIconSize
        dc = GetWindowDC(hwnd)
        Dim BorderBrush As New SolidBrush(BorderColor)
        g = Graphics.FromHdc(dc)

        DrawControlBox(g, ncInfo, Me.ControlBox, Me.MaximizeBox, Me.MinimizeBox, Me.HelpButton)
        DrawBorder(g, ncInfo, BorderBrush)
        g.Dispose()
        ReleaseDC(hwnd, dc)
        BorderBrush.Dispose()
    End Sub

    Private Sub DrawControlBox(ByVal g As Graphics, ByVal info As NonClientSizeInfo, ByVal closeBtn As Boolean, ByVal maxBtn As Boolean, ByVal minBtn As Boolean, HelpBtn As Boolean)
        Dim ControlBoxImage As New Bitmap（ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width * 2, ncInfo.CaptionRect.Height + ncInfo.CaptionRect.Y * 2）
        Dim ControlBoxG As Graphics = Graphics.FromImage(ControlBoxImage)
        If ControlBox OrElse Me.Text.Length > 0 Then
            Dim backgroundColor As New SolidBrush(CaptionBackgroundColor)
            ControlBoxG.FillRectangle(backgroundColor, ncInfo.CaptionRect)
            'g.FillRectangle(backgroundColor, ncInfo.CaptionRect)
            DrawTitle(ControlBoxG, ncInfo)
            backgroundColor.Dispose()
        End If

        If Me.ControlBox Then
            GetImg()
            Dim closeBtnPosX As Integer = info.CaptionRect.Left + info.CaptionRect.Width - info.BorderSize.Width - info.CaptionButtonSize.Width
            Dim maxBtnPosX As Integer = closeBtnPosX - info.CaptionButtonSize.Width
            Dim minBtnPosX As Integer = maxBtnPosX - info.CaptionButtonSize.Width
            Dim btnPosY As Integer = info.BorderSize.Height + (info.CaptionHeight - info.CaptionButtonSize.Height) / 2
            Dim btnRect As New Rectangle(New Point(closeBtnPosX, btnPosY), info.CaptionButtonSize)
            Dim maxRect As New Rectangle(New Point(maxBtnPosX, btnPosY), info.CaptionButtonSize)
            Dim minRect As New Rectangle(New Point(minBtnPosX, btnPosY), info.CaptionButtonSize)
            btnRect.Inflate((16 - btnRect.Width) / 2, (16 - btnRect.Height) / 2)
            maxRect.Inflate((16 - maxRect.Width) / 2, (16 - maxRect.Height) / 2)
            minRect.Inflate((16 - minRect.Width) / 2, (16 - minRect.Height) / 2)


            If closeBtn Then ControlBoxG.DrawImage(CloseButtonImage, btnRect)

            If maxBtn OrElse minBtn Then

                If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then

                    If Me.WindowState = FormWindowState.Maximized Then
                        ControlBoxG.DrawImage(MaximumNormalButtonImage, maxRect)
                    Else
                        ControlBoxG.DrawImage(MaximumButtonImage, maxRect)
                    End If

                    ControlBoxG.DrawImage(MinimumButtonImage, minRect)
                End If
            ElseIf HelpBtn Then

                If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then
                    ControlBoxG.DrawImage(HelpButtonImage, maxRect)
                End If
            End If

            '  g.DrawImage(ControlBoxImage, ncInfo.BorderSize.Width, 0)


        End If
        g.DrawImage(ControlBoxImage, ncInfo.BorderSize.Width, 0)
        ControlBoxG.Dispose()
    End Sub
    Enum ColorType
        White
        Black
        Blue
    End Enum
    Dim mImgColorType As ColorType = ColorType.White
    Property ImgColorType As ColorType
        Get
            Return mImgColorType
        End Get
        Set(value As ColorType)
            mImgColorType = value
            Invalidate()
            'Me.StartPosition = FormStartPosition.CenterScreen
        End Set
    End Property
    Private Sub GetImg()
        If ImgColorType = ColorType.White Then
            CloseButtonImage = My.Resources.SysControl_Default.Form_close
            CloseButtonHoverImage = My.Resources.SysControl_Other.Form_close_blue
            CloseButtonPressDownImage = My.Resources.SysControl_Other.Form_close_black
            MaximumButtonImage = My.Resources.SysControl_Default.Form_maximize
            MaximumButtonHoverImage = My.Resources.SysControl_Other.Form_maximize_blue
            MaximumButtonPressDownImage = My.Resources.SysControl_Other.Form_maximize_black
            MaximumNormalButtonImage = My.Resources.SysControl_Default.Form_normal
            MaximumNormalButtonHoverImage = My.Resources.SysControl_Other.Form_normal_blue
            MaximumNormalButtonPressDownImage = My.Resources.SysControl_Other.Form_normal_black
            MinimumButtonImage = My.Resources.SysControl_Default.Form_minimizing
            MinimumButtonHoverImage = My.Resources.SysControl_Other.Form_minimizing_blue
            MinimumButtonPressDownImage = My.Resources.SysControl_Other.Form_minimizing_black
            HelpButtonImage = My.Resources.SysControl_Default.Help
            HelpButtonHoverImage = My.Resources.SysControl_Other.Help_blue
            HelpButtonPressDownImage = My.Resources.SysControl_Other.Help_black
        ElseIf ImgColorType = ColorType.Blue Then
            CloseButtonImage = My.Resources.SysControl_Other.Form_close_blue
            CloseButtonHoverImage = My.Resources.SysControl_Default.Form_close
            CloseButtonPressDownImage = My.Resources.SysControl_Other.Form_close_black
            MaximumButtonImage = My.Resources.SysControl_Other.Form_maximize_blue
            MaximumButtonHoverImage = My.Resources.SysControl_Default.Form_maximize
            MaximumButtonPressDownImage = My.Resources.SysControl_Other.Form_maximize_black
            MaximumNormalButtonImage = My.Resources.SysControl_Other.Form_normal_blue
            MaximumNormalButtonHoverImage = My.Resources.SysControl_Default.Form_normal
            MaximumNormalButtonPressDownImage = My.Resources.SysControl_Other.Form_normal_black
            MinimumButtonImage = My.Resources.SysControl_Other.Form_minimizing_blue
            MinimumButtonHoverImage = My.Resources.SysControl_Default.Form_minimizing
            MinimumButtonPressDownImage = My.Resources.SysControl_Other.Form_minimizing_black
            HelpButtonImage = My.Resources.SysControl_Other.Help_blue
            HelpButtonHoverImage = My.Resources.SysControl_Default.Help
            HelpButtonPressDownImage = My.Resources.SysControl_Other.Help_black
        ElseIf ImgColorType = ColorType.Black Then
            CloseButtonImage = My.Resources.SysControl_Other.Form_close_black
            CloseButtonHoverImage = My.Resources.SysControl_Default.Form_close
            CloseButtonPressDownImage = My.Resources.SysControl_Other.Form_close_blue
            MaximumButtonImage = My.Resources.SysControl_Other.Form_maximize_black
            MaximumButtonHoverImage = My.Resources.SysControl_Default.Form_maximize
            MaximumButtonPressDownImage = My.Resources.SysControl_Other.Form_maximize_blue
            MaximumNormalButtonImage = My.Resources.SysControl_Other.Form_normal_black
            MaximumNormalButtonHoverImage = My.Resources.SysControl_Default.Form_normal
            MaximumNormalButtonPressDownImage = My.Resources.SysControl_Other.Form_normal_blue
            MinimumButtonImage = My.Resources.SysControl_Other.Form_minimizing_black
            MinimumButtonHoverImage = My.Resources.SysControl_Default.Form_minimizing
            MinimumButtonPressDownImage = My.Resources.SysControl_Other.Form_minimizing_blue
            HelpButtonImage = My.Resources.SysControl_Other.Help_blue
            HelpButtonHoverImage = My.Resources.SysControl_Default.Help
            HelpButtonPressDownImage = My.Resources.SysControl_Other.Help_black
        End If

    End Sub
    Private Function LOBYTE(ByVal p As Long) As Integer
        Return CInt((p And &HFFFF))

    End Function

    Private Function HIBYTE(ByVal p As Long) As Integer

        Return CInt(p >> 16)

    End Function

    '  Dim IsSuspendLayout As Boolean
    Protected Overrides Sub OnResizeBegin(e As EventArgs)
        MyBase.OnResizeBegin(e)
        ' IsSuspendLayout = True
    End Sub
    Protected Overrides Sub OnResizeEnd(e As EventArgs)
        MyBase.OnResizeEnd(e)
        'IsSuspendLayout = False
        MyBase.OnResize(e)
        Dim m As New Message With {.Msg = WM_NCPAINT,
       .HWnd = Me.Handle}
        WndProc(m)
    End Sub
    'Protected Overrides Sub OnPaint(e As PaintEventArgs)
    '    Dim m As New Message With {.Msg = WM_NCPAINT,
    '    .HWnd = Me.Handle}
    '    WndProc(m)
    '    MyBase.OnPaint(e)
    'End Sub
    Public Const GWL_EXSTYLE As Integer = &HFFFFFFEC
    Public Const WS_EX_TOPMOST As Integer = &H8
    Function IsWndTopMost(hwnd As IntPtr) As Boolean
        Return GetWindowLong(hwnd, GWL_EXSTYLE) & WS_EX_TOPMOST
    End Function
    '''<summary>
    '''确定窗口句柄是否有效
    '''     hwnd:Integer型,待检查窗口的句柄
    '''</summary>
    Public Declare Auto Function IsWindow Lib "user32.dll" Alias "IsWindow" (ByVal hwnd As Integer) As Integer
    '''<summary>将窗口置于前台</summary>
    '''<param name="hwnd">带到前台的窗口</param>
    Public Declare Auto Function SetForegroundWindow Lib "user32.dll" Alias "SetForegroundWindow" (ByVal hwnd As Integer) As Integer


    'Protected Overrides Sub OnPaint(e As PaintEventArgs)
    '    MyBase.OnPaint(e)
    '    Dim m As New Message With {.Msg = WM_NCPAINT, .HWnd = Me.Handle}
    '    WndProc(m)
    'End Sub

    Public Const WM_ACTIVATEAPP As Integer = &H1C
    Public Const WM_ACTIVATE As Integer = &H6




    Protected Overrides Sub WndProc(ByRef m As Message)

        If m.Msg = &H210 OrElse m.Msg = 132 Then
            Me.BringToFront()
            Me.Activate()
        End If
        If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.None Then
            Select Case m.Msg
                Case WM_NCPAINT
                    DrawCaption(m.HWnd)
                    If m.WParam.ToInt32() = 0 Then m.Result = CType(1, IntPtr)
                    Return
                Case WM_NCACTIVATE
                    DrawCaption(m.HWnd)
                    If m.WParam.ToInt32() = 0 Then m.Result = CType(1, IntPtr)
                    Return
                Case WM_NCRBUTTONDOWN
                    Dim posX, posY As Integer
                    Dim wp As Integer = m.WParam.ToInt32()
                    Dim lp As Long = m.LParam.ToInt64()
                    posX = LOBYTE(lp)
                    posY = HIBYTE(lp)

                    If wp = HTCAPTION Then
                        Dim pt As Point = Me.PointToClient(New Point(posX, posY))

                        If Me.CaptionContextMenu IsNot Nothing Then
                            Me.CaptionContextMenu.Show(posX, posY)
                            Return
                        End If
                    End If

                  '  Exit Select
                Case WM_SETCURSOR
                    If Me.ControlBox Then
                        GetImg()
                        Dim posX, posY As Integer
                        Dim wp As Integer = m.WParam.ToInt32()
                        Dim lp As Long = m.LParam.ToInt64()
                        posX = LOBYTE(lp)
                        posY = HIBYTE(lp)
                        Dim backgroundColor As Brush = New SolidBrush(CaptionBackgroundColor)
                        Dim ncInfo As NonClientSizeInfo = GetNonClientInfo(m.HWnd)
                        Dim dc As IntPtr = GetWindowDC(m.HWnd)
                        Dim g As Graphics = Graphics.FromHdc(dc)
                        ' g.Clear(Color.Transparent)
                        'g.DrawImage(CaptionImg, New PointF(0, 0))
                        Dim closeBtnPosX As Integer = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width + ncInfo.BorderSize.Width

                        Dim maxBtnPosX, minBtnPosX As Integer
                        maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width
                        minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width
                        Dim btnPosY As Integer = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2
                        Dim btnRect As New Rectangle(New Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        Dim maxRect As New Rectangle(New Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        Dim minRect As New Rectangle(New Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        btnRect.Inflate((16 - btnRect.Width) / 2, (16 - btnRect.Height) / 2)
                        maxRect.Inflate((16 - maxRect.Width) / 2, (16 - maxRect.Height) / 2)
                        minRect.Inflate((16 - minRect.Width) / 2, (16 - minRect.Height) / 2)
                        If posX <> HTCLOSE Then
                            'g.DrawImage(CloseButtonImage, btnRect)
                            Drawclose(2, g, CloseButtonImage, btnRect)
                        ElseIf MouseButtons <> System.Windows.Forms.MouseButtons.Left Then
                            ' g.DrawImage(CloseButtonHoverImage, btnRect)
                            Drawclose(3, g, CloseButtonHoverImage, btnRect)
                        Else
                            'g.DrawImage(CloseButtonPressDownImage, btnRect)
                            Drawclose(1, g, CloseButtonPressDownImage, btnRect)
                        End If

                        If Me.MaximizeBox OrElse Me.MinimizeBox Then

                            If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then

                                If Me.WindowState = FormWindowState.Maximized Then

                                    If Me.MaximizeBox Then

                                        If posX <> HTMAXBUTTON Then
                                            ' g.DrawImage(MaximumNormalButtonImage, maxRect)
                                            Drawmaxnormal(2, g, MaximumNormalButtonImage, maxRect)
                                        ElseIf MouseButtons <> System.Windows.Forms.MouseButtons.Left Then
                                            'g.DrawImage(MaximumNormalButtonHoverImage, maxRect)
                                            Drawmaxnormal(5, g, MaximumNormalButtonHoverImage, maxRect)
                                        Else
                                            'g.DrawImage(MaximumNormalButtonPressDownImage, maxRect)
                                            Drawmaxnormal(1, g, MaximumNormalButtonPressDownImage, maxRect)
                                        End If
                                    Else
                                        'g.DrawImage(MaximumNormalButtonImage, maxRect)
                                        Drawmaxnormal(2, g, MaximumNormalButtonImage, maxRect)
                                    End If
                                Else

                                    If Me.MaximizeBox Then

                                        If posX <> HTMAXBUTTON Then
                                            ' g.DrawImage(MaximumButtonImage, maxRect)
                                            Drawmaxnormal(4, g, MaximumButtonImage, maxRect)
                                        ElseIf MouseButtons <> System.Windows.Forms.MouseButtons.Left Then
                                            'g.DrawImage(MaximumButtonHoverImage, maxRect)
                                            Drawmaxnormal(6, g, MaximumButtonHoverImage, maxRect)
                                        Else
                                            ' g.DrawImage(MaximumButtonPressDownImage, maxRect)
                                            Drawmaxnormal(3, g, MaximumButtonPressDownImage, maxRect)
                                        End If
                                    Else
                                        'g.DrawImage(MaximumButtonImage, maxRect)
                                        Drawmaxnormal(4, g, MaximumButtonImage, maxRect)
                                    End If
                                End If

                                If Me.MinimizeBox Then

                                    If posX <> HTMINBUTTON Then
                                        'g.DrawImage(MinimumButtonImage, minRect)
                                        Drawmin(2, g, MinimumButtonImage, minRect)
                                    ElseIf MouseButtons <> System.Windows.Forms.MouseButtons.Left Then
                                        'g.DrawImage(MinimumButtonHoverImage, minRect)
                                        Drawmin(3, g, MinimumButtonHoverImage, minRect)
                                    Else
                                        'g.DrawImage(MinimumButtonPressDownImage, minRect)
                                        Drawmin(1, g, MinimumButtonPressDownImage, minRect)
                                    End If
                                Else
                                    g.DrawImage(MinimumButtonImage, minRect)
                                End If
                            End If
                        ElseIf Me.HelpButton Then

                            If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then
                                If posX <> HTHELP Then
                                    'g.DrawImage(HelpButtonImage, maxRect)
                                    Drawhelp(2, g, HelpButtonImage, maxRect)
                                ElseIf MouseButtons <> System.Windows.Forms.MouseButtons.Left Then
                                    'g.DrawImage(HelpButtonHoverImage, maxRect)
                                    Drawhelp(3, g, HelpButtonHoverImage, maxRect)
                                Else
                                    'g.DrawImage(HelpButtonPressDownImage, maxRect)
                                    Drawhelp(1, g, HelpButtonPressDownImage, maxRect)
                                End If
                            End If
                        End If

                        g.Dispose()
                        ReleaseDC(m.HWnd, dc)
                    End If

                Case WM_NCLBUTTONUP
                    Dim wp As Integer = m.WParam.ToInt32()

                    Select Case wp
                        Case HTCLOSE
                            m.Msg = WM_SYSCOMMAND
                            m.WParam = New IntPtr(SC_CLOSE)
                        Case HTMAXBUTTON

                            If Me.MaximizeBox Then
                                m.Msg = WM_SYSCOMMAND

                                If Me.WindowState = FormWindowState.Maximized Then
                                    m.WParam = New IntPtr(SC_RESTORE)
                                Else
                                    m.WParam = New IntPtr(SC_MAXIMIZE)
                                End If
                            End If

                        Case HTMINBUTTON

                            If Me.MinimizeBox Then
                                m.Msg = WM_SYSCOMMAND
                                m.WParam = New IntPtr(SC_MINIMIZE)
                            End If

                        Case HTHELP
                            m.Msg = WM_SYSCOMMAND
                            m.WParam = New IntPtr(SC_CONTEXTHELP)
                        Case Else
                    End Select

                Case WM_NCLBUTTONDOWN

                    If Me.ControlBox Then
                        GetImg()
                        Dim ret As Boolean = False
                        Dim posX, posY As Integer
                        Dim wp As Integer = m.WParam.ToInt32()
                        Dim lp As Long = m.LParam.ToInt64()
                        posX = LOBYTE(lp)
                        posY = HIBYTE(lp)
                        Dim ncInfo As NonClientSizeInfo = GetNonClientInfo(m.HWnd)
                        Dim dc As IntPtr = GetWindowDC(m.HWnd)
                        Dim backgroundColor As Brush = New SolidBrush(CaptionBackgroundColor)
                        Dim g As Graphics = Graphics.FromHdc(dc)
                        ' g.Clear(Color.Transparent)
                        ' g.DrawImage(CaptionImg, New PointF(0, 0))
                        Dim closeBtnPosX As Integer = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width + ncInfo.BorderSize.Width
                        Dim maxBtnPosX, minBtnPosX As Integer
                        Dim btnPosY As Integer = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2
                        maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width
                        minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width
                        Dim btnRect As New Rectangle(New Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        Dim maxRect As New Rectangle(New Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        Dim minRect As New Rectangle(New Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize)
                        btnRect.Inflate((16 - btnRect.Width) / 2, (16 - btnRect.Height) / 2)
                        maxRect.Inflate((16 - maxRect.Width) / 2, (16 - maxRect.Height) / 2)
                        minRect.Inflate((16 - minRect.Width) / 2, (16 - minRect.Height) / 2)
                        If wp = HTCLOSE Then
                            '  g.DrawImage(CloseButtonPressDownImage, btnRect)
                            Drawclose(1, g, CloseButtonPressDownImage, btnRect)
                            ret = True
                        Else
                            'g.DrawImage(CloseButtonImage, btnRect)
                            Drawclose(2, g, CloseButtonImage, btnRect)
                        End If
                        If Me.MaximizeBox OrElse Me.MinimizeBox Then
                            If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow Then
                                If Me.WindowState = FormWindowState.Maximized Then
                                    If wp = HTMAXBUTTON AndAlso Me.MaximizeBox Then
                                        ' g.DrawImage(MaximumNormalButtonPressDownImage, maxRect)
                                        Drawmaxnormal(1, g, MaximumNormalButtonPressDownImage, maxRect)
                                        ret = True
                                    Else
                                        'g.DrawImage(MaximumNormalButtonImage, maxRect)
                                        Drawmaxnormal(2, g, MaximumNormalButtonImage, maxRect)
                                    End If
                                Else
                                    If wp = HTMAXBUTTON AndAlso Me.MaximizeBox Then
                                        'g.DrawImage(MaximumButtonPressDownImage, maxRect)
                                        Drawmaxnormal(3, g, MaximumButtonPressDownImage, maxRect)
                                        ret = True
                                    Else
                                        'g.DrawImage(MaximumButtonImage, maxRect)
                                        Drawmaxnormal(4, g, MaximumButtonImage, maxRect)
                                    End If
                                End If

                                If wp = HTMINBUTTON AndAlso Me.MinimizeBox Then
                                    ' g.DrawImage(MinimumButtonPressDownImage, minRect)
                                    Drawmin(1, g, MinimumButtonPressDownImage, minRect)
                                    ret = True
                                Else
                                    'g.DrawImage(MinimumButtonImage, minRect)
                                    Drawmin(2, g, MinimumButtonImage, minRect)
                                End If
                            End If
                        ElseIf Me.HelpButton Then

                            If Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.FixedToolWindow AndAlso Me.FormBorderStyle <> System.Windows.Forms.FormBorderStyle.SizableToolWindow Then

                                If wp = HTHELP Then
                                    '   g.DrawImage(HelpButtonPressDownImage, maxRect)
                                    Drawhelp(1, g, HelpButtonPressDownImage, maxRect)
                                    ret = True
                                Else
                                    'g.DrawImage(HelpButtonImage, maxRect)
                                    Drawhelp(2, g, HelpButtonImage, maxRect)
                                End If
                            End If
                        End If
                        g.Dispose()
                        ReleaseDC(m.HWnd, dc)
                        If ret Then Return
                    End If
            End Select
        End If
        MyBase.WndProc(m)
    End Sub






















    Dim isonclose As Integer
    Dim isonmaxnormal As Integer
    Dim isonmin As Integer
    Dim isonhelp As Integer

    Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.UpdateStyles()
        ' 此调用是设计器所必需的。
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.WindowState = FormWindowState.Normal
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        HelpButton = False

        Opacity = 0

    End Sub
    Shadows Property Name As String
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            If MyBase.Name <> value Then
                MyBase.Name = value
                OnNameSet(New NameSetEventArgs With {.Name = value})
            End If

        End Set

    End Property
    Overloads Property FormBorderStyle As FormBorderStyle
        Get
            Return MyBase.FormBorderStyle
        End Get
        Set(value As FormBorderStyle)
            MyBase.FormBorderStyle = value
            Invalidate()

        End Set
    End Property

    Overloads Property StartPosition As FormStartPosition
        Get
            Return MyBase.StartPosition
        End Get
        Set(value As FormStartPosition)
            MyBase.StartPosition = value
            If DesignMode Then
                MyBase.Location = New Point(0, 0)
                Return
            End If
            If value = FormStartPosition.CenterScreen Then
                MyBase.Left = Screen.PrimaryScreen.Bounds.X + (Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2
                MyBase.Top = Screen.PrimaryScreen.Bounds.Y + (Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2
            ElseIf Value = FormStartPosition.CenterParent AndAlso Parent IsNot Nothing Then
                MyBase.Left = Parent.Bounds.X + (Parent.Bounds.Width - Me.Width) / 2
                MyBase.Top = Parent.Bounds.Y + (Parent.Bounds.Height - Me.Height) / 2
            End If
        End Set
    End Property

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        If DesignMode Then
            MyBase.Location = New Point(0, 0)
            Return
        End If
        If StartPosition = FormStartPosition.CenterScreen Then
            MyBase.Left = Screen.PrimaryScreen.Bounds.X + (Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2
            MyBase.Top = Screen.PrimaryScreen.Bounds.Y + (Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2
        ElseIf StartPosition = FormStartPosition.CenterParent AndAlso Parent IsNot Nothing Then
            MyBase.Left = Parent.Bounds.X + (Parent.Bounds.Width - Me.Width) / 2
            MyBase.Top = Parent.Bounds.Y + (Parent.Bounds.Height - Me.Height) / 2
        End If
    End Sub
    Protected Overridable Sub OnNameSet(e As NameSetEventArgs)
        RaiseEvent NameSet(Me, e)
    End Sub
    Public Event NameSet As EventHandler(Of NameSetEventArgs)




    Class NameSetEventArgs
        Inherits EventArgs
        Public Property Name As String

    End Class
    Private Sub Drawclose(onbtn As Integer, g As Graphics, img As Image, rec As Rectangle)
        If isonclose = onbtn Then Return
        isonclose = onbtn
        g.SetClip(rec)
        g.Clear(CaptionBackgroundColor)
        g.DrawImage(img, rec)

    End Sub
    Private Sub Drawmaxnormal(onbtn As Integer, g As Graphics, img As Image, rec As Rectangle)
        If isonmaxnormal = onbtn Then Return
        isonmaxnormal = onbtn
        g.SetClip(rec)
        g.Clear(CaptionBackgroundColor)
        g.DrawImage(img, rec)
    End Sub
    Private Sub Drawmin(onbtn As Integer, g As Graphics, img As Image, rec As Rectangle)
        If isonmin = onbtn Then Return
        isonmin = onbtn
        g.SetClip(rec)
        g.Clear(CaptionBackgroundColor)
        g.DrawImage(img, rec)
    End Sub
    Private Sub Drawhelp(onbtn As Integer, g As Graphics, img As Image, rec As Rectangle)
        If isonhelp = onbtn Then Return
        isonhelp = onbtn
        g.SetClip(rec)
        g.Clear(CaptionBackgroundColor)
        g.DrawImage(img, rec)
    End Sub
    Shared Property TopActiveFrom As Form

    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
        TopActiveFrom = Me
        Dim m As New Message With {.Msg = WM_NCPAINT, .HWnd = Me.Handle}
        WndProc(m)
    End Sub

    Protected Overrides Async Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        If DesignMode Then
            Me.Opacity = 1
            Return
        End If
        Dim i As Integer = 0
        Do
            If Me.Opacity = 1 Then Exit Do
            Await Task.Delay(100)
            i += 1
            If Me.IsDisposed OrElse Me.Disposing Then Exit Do
            Me.Opacity = i * 10 / 100
        Loop
    End Sub
    Public Const HWND_TOPMOST As Integer = &HFFFFFFFF
    Public Const SWP_SHOWWINDOW As Integer = &H40
    Public Const HWND_NOTOPMOST As Integer = &HFFFFFFFE
    Overloads Property Parent As Control
        Get
            Return MyBase.Parent
        End Get
        Set(value As Control)
            If DesignMode Then
                MyBase.Parent = value
                Return
            End If
            If MyBase.Parent IsNot Nothing Then
                RemoveHandler MyBase.Parent.Resize, AddressOf ParentResize
            End If
            MyBase.Parent = value
            If MyBase.Parent IsNot Nothing Then
                Me.Width = MyBase.Parent.Width
                Me.Height = MyBase.Parent.Height
                AddHandler MyBase.Parent.Resize, AddressOf ParentResize
            End If
        End Set
    End Property
    Private Sub ParentResize(sender As Object, e As EventArgs)
        Dim ParentCtr As Control = sender
        If ParentCtr IsNot Nothing Then
            Dim x As Integer, y As Integer
            x = Me.Location.X
            y = Me.Location.Y
            If ParentCtr.Width < Me.Width AndAlso ParentCtr.Width > 0 Then
                Me.Width = ParentCtr.Width
                x = 0
            End If
            If ParentCtr.Height < Me.Height AndAlso ParentCtr.Height > 0 Then
                Me.Height = ParentCtr.Height
                y = 0
            End If
            Me.Location = New Point(x, y)
        End If


    End Sub









    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            If MyBase.Parent IsNot Nothing Then
                RemoveHandler MyBase.Parent.Resize, AddressOf ParentResize
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
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


    Public Overloads Sub BringToFront()
        MyBase.BringToFront()
        MyBase.Invalidate()
    End Sub

    Private Sub FormExt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class