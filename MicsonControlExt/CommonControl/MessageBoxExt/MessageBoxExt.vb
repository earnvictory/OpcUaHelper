
Imports System.ComponentModel

<Description("提示信息窗口")>
Public NotInheritable Class MessageBoxExt
    Private Shared _styles As New MessageBoxExtStyles()

    <Description("提示信息窗口样式配置")>
    Public Shared Property Styles As MessageBoxExtStyles
        Get
            If _styles Is Nothing Then _styles = New MessageBoxExtStyles()
            Return _styles
        End Get
        Set(ByVal value As MessageBoxExtStyles)
            _styles = value
        End Set
    End Property

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String) As DialogResult
        Return Show(owner, text, String.Empty, MessageBoxExtButtons.OK, MessageBoxExtIcon.None, MessageBoxExtDefaultButton.Button1, Styles, Nothing)
    End Function

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String) As DialogResult
        Return Show(owner, text, caption, MessageBoxExtButtons.OK, MessageBoxExtIcon.None, MessageBoxExtDefaultButton.Button1, Styles, Nothing)
    End Function

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxExtButtons) As DialogResult
        Return Show(owner, text, caption, buttons, MessageBoxExtIcon.None, MessageBoxExtDefaultButton.Button1, Styles, Nothing)
    End Function

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxExtButtons, ByVal icon As MessageBoxExtIcon) As DialogResult
        Return Show(owner, text, caption, buttons, icon, MessageBoxExtDefaultButton.Button1, Styles, Nothing)
    End Function

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxExtButtons, ByVal icon As MessageBoxExtIcon, ByVal defaultButton As MessageBoxExtDefaultButton) As DialogResult
        Return Show(owner, text, caption, buttons, icon, defaultButton, Styles, Nothing)
    End Function

    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxExtButtons, ByVal icon As MessageBoxExtIcon, ByVal defaultButton As MessageBoxExtDefaultButton, ByVal styles As MessageBoxExtStyles) As DialogResult
        Return Show(owner, text, caption, buttons, icon, defaultButton, styles, Nothing)
    End Function



    Public Shared Function Show(ByVal owner As IWin32Window, ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxExtButtons, ByVal icon As MessageBoxExtIcon, ByVal defaultButton As MessageBoxExtDefaultButton, ByVal styles As MessageBoxExtStyles, ByVal customImage As Image) As DialogResult
        If styles Is Nothing Then
            styles = styles
        End If

        Dim sfe As New FormExt()
        Dim label As New MessageExt
        Dim text_captionBoxHeight As Integer = SystemInformation.CaptionHeight
        Dim scale_ico_size As New Size(CInt((32)), CInt((32)))
        Dim scale_margin As Integer = CInt((6))
        Dim scale_btn_width As Integer = CInt((75))
        Dim scale_btn_height As Integer = CInt((24))
        Dim scale_btn_interval As Integer = CInt((10))
        Dim scale_win_maxsize As New Size(CInt((900)), CInt((600)))
        Dim scale_win_minsize As New Size(CInt((200)), CInt((100)))

        If buttons = MessageBoxExtButtons.YesNoCancel OrElse buttons = MessageBoxExtButtons.AbortRetryIgnore Then
            scale_win_minsize = New Size(CInt((285)), CInt((100)))
        End If

        Dim ico_rect As Rectangle = Rectangle.Empty
        Dim text_rect As Rectangle = Rectangle.Empty
        Dim btn_rect As Rectangle = Rectangle.Empty
        Dim bmp As New Bitmap(1000, 1000)
        Dim g As Graphics = Graphics.FromImage(bmp)
        Dim text_size As Size = Size.Ceiling(g.MeasureString(text, styles.TextFont, Integer.MaxValue))
        g.Dispose()
        bmp.Dispose()
        Dim win_size As Size = Size.Empty

        If icon <> MessageBoxExtIcon.None Then
            ico_rect = New Rectangle(scale_margin, text_captionBoxHeight + scale_margin, scale_ico_size.Width, scale_ico_size.Height)
            text_rect = New Rectangle(scale_margin + scale_ico_size.Width + scale_margin, text_captionBoxHeight + scale_margin, text_size.Width, text_size.Height)

            If text_rect.Width > scale_win_maxsize.Width - scale_margin - scale_ico_size.Width - scale_margin - scale_margin Then
                text_rect.Width = scale_win_maxsize.Width - scale_margin - scale_ico_size.Width - scale_margin - scale_margin
            End If

            If text_rect.Width < scale_win_minsize.Width - scale_margin - scale_ico_size.Width - scale_margin - scale_margin Then
                text_rect.Width = scale_win_minsize.Width - scale_margin - scale_ico_size.Width - scale_margin - scale_margin
            End If

            If text_rect.Height > scale_win_maxsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight Then
                text_rect.Height = scale_win_maxsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight
            End If

            If text_rect.Height < scale_win_minsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight Then
                text_rect.Height = scale_win_minsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight
            End If

            btn_rect = New Rectangle(scale_margin, text_rect.Bottom + scale_margin, scale_ico_size.Width + scale_margin + text_rect.Width, scale_btn_height)
            win_size = New Size(text_rect.Right + scale_margin, btn_rect.Bottom + scale_margin * 2 + text_captionBoxHeight * 2)
        Else
            text_rect = New Rectangle(scale_margin, text_captionBoxHeight + scale_margin, text_size.Width, text_size.Height)

            If text_rect.Width > scale_win_maxsize.Width - scale_margin - scale_margin Then
                text_rect.Width = scale_win_maxsize.Width - scale_margin - scale_margin
            End If

            If text_rect.Width < scale_win_minsize.Width - scale_margin - scale_margin Then
                text_rect.Width = scale_win_minsize.Width - scale_margin - scale_margin
            End If

            If text_rect.Height > scale_win_maxsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight Then
                text_rect.Height = scale_win_maxsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight
            End If

            If text_rect.Height < scale_win_minsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight Then
                text_rect.Height = scale_win_minsize.Height - scale_margin - scale_btn_height - scale_margin - scale_margin - text_captionBoxHeight
            End If

            btn_rect = New Rectangle(scale_margin, text_rect.Bottom + scale_margin, scale_margin + text_rect.Width + scale_margin, scale_btn_height)
            win_size = New Size(text_rect.Right + scale_margin, btn_rect.Bottom + scale_margin * 2 + text_captionBoxHeight * 2)
        End If

        sfe.BorderColor = styles.BorderColor
        sfe.ShowInTaskbar = False
        sfe.ShowIcon = False
        sfe.FormBorderStyle = FormBorderStyle.Sizable 'FormBorderStyle.FixedDialog
        sfe.StartPosition = FormStartPosition.CenterParent
        sfe.Size = win_size
        sfe.Text = caption
        sfe.BackColor = styles.BackColor
        sfe.ControlBox = styles.ControlBox
        sfe.MaximizeBox = False
        sfe.MinimizeBox = False
        sfe.CaptionBackgroundColor = styles.CaptionBackColor
        sfe.CaptionTextColor = styles.CaptionTextColor
        sfe.SizeGripStyle = SizeGripStyle.Hide
        label.Text = text
        label.ForeColor = styles.TextColor
        label.Font = styles.TextFont

        If icon <> MessageBoxExtIcon.None Then
            Dim ico_pb As New PictureBox() With {
            .Size = scale_ico_size,
            .BackgroundImageLayout = ImageLayout.Zoom,
            .BackgroundImage = GetIco(icon, customImage)
        }
            sfe.Controls.Add(ico_pb)
            ico_pb.SetBounds(ico_rect.X, ico_rect.Y, ico_rect.Width, ico_rect.Height)
        End If

        sfe.Controls.Add(label)
        label.SetBounds(text_rect.X, text_rect.Y, text_rect.Width, text_rect.Height)
        Dim btnList As New List(Of MessageBoxExtButton)()

        If buttons = MessageBoxExtButtons.OK Then
            Dim ok_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "OK", styles.Button1Text), New EventHandler(AddressOf OK_Click), scale_btn_width, scale_btn_height, 0, styles)
            ok_btn.Location = New Point(CInt(((btn_rect.Width - ok_btn.Width) / 2)), btn_rect.Y)
            sfe.Controls.Add(ok_btn)
            btnList.Add(ok_btn)
        ElseIf buttons = MessageBoxExtButtons.OKCancel Then
            Dim ok_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "OK", styles.Button1Text), New EventHandler(AddressOf OK_Click), scale_btn_width, scale_btn_height, 0, styles)
            ok_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)), btn_rect.Y)
            sfe.Controls.Add(ok_btn)
            btnList.Add(ok_btn)
            Dim cancel_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button2Text = String.Empty, "Cancel", styles.Button2Text), New EventHandler(AddressOf Cancel_Click), scale_btn_width, scale_btn_height, 1, styles)
            cancel_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)) + cancel_btn.Width + scale_btn_interval, btn_rect.Y)
            sfe.Controls.Add(cancel_btn)
            btnList.Add(cancel_btn)
        ElseIf buttons = MessageBoxExtButtons.YesNo Then
            Dim ok_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "Yes", styles.Button1Text), New EventHandler(AddressOf OK_Click), scale_btn_width, scale_btn_height, 0, styles)
            ok_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)), btn_rect.Y)
            sfe.Controls.Add(ok_btn)
            btnList.Add(ok_btn)
            Dim cancel_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button2Text = String.Empty, "No", styles.Button2Text), New EventHandler(AddressOf No_Click), scale_btn_width, scale_btn_height, 1, styles)
            cancel_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)) + cancel_btn.Width + scale_btn_interval, btn_rect.Y)
            sfe.Controls.Add(cancel_btn)
            btnList.Add(cancel_btn)
        ElseIf buttons = MessageBoxExtButtons.YesNoCancel Then
            Dim ok_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "Yes", styles.Button1Text), New EventHandler(AddressOf OK_Click), scale_btn_width, scale_btn_height, 0, styles)
            ok_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)), btn_rect.Y)
            sfe.Controls.Add(ok_btn)
            btnList.Add(ok_btn)
            Dim no_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button2Text = String.Empty, "No", styles.Button2Text), New EventHandler(AddressOf No_Click), scale_btn_width, scale_btn_height, 1, styles)
            no_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)) + no_btn.Width + scale_btn_interval, btn_rect.Y)
            sfe.Controls.Add(no_btn)
            btnList.Add(no_btn)
            Dim cancel_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button3Text = String.Empty, "Cancel", styles.Button3Text), New EventHandler(AddressOf Cancel_Click), scale_btn_width, scale_btn_height, 2, styles)
            cancel_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)) + cancel_btn.Width * 2 + scale_btn_interval * 2, btn_rect.Y)
            sfe.Controls.Add(cancel_btn)
            btnList.Add(cancel_btn)
        ElseIf buttons = MessageBoxExtButtons.RetryCancel Then
            Dim retry_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "Retry", styles.Button1Text), New EventHandler(AddressOf Retry_Click), scale_btn_width, scale_btn_height, 0, styles)
            retry_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)), btn_rect.Y)
            sfe.Controls.Add(retry_btn)
            btnList.Add(retry_btn)
            Dim cancel_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button2Text = String.Empty, "Cancel", styles.Button2Text), New EventHandler(AddressOf Cancel_Click), scale_btn_width, scale_btn_height, 1, styles)
            cancel_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 2 - scale_btn_interval) / 2)) + cancel_btn.Width + scale_btn_interval, btn_rect.Y)
            sfe.Controls.Add(cancel_btn)
            btnList.Add(cancel_btn)
        ElseIf buttons = MessageBoxExtButtons.AbortRetryIgnore Then
            Dim abort_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button1Text = String.Empty, "Abort", styles.Button1Text), New EventHandler(AddressOf Abort_Click), scale_btn_width, scale_btn_height, 0, styles)
            abort_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)), btn_rect.Y)
            sfe.Controls.Add(abort_btn)
            btnList.Add(abort_btn)
            Dim retry_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button2Text = String.Empty, "Retry", styles.Button2Text), New EventHandler(AddressOf Retry_Click), scale_btn_width, scale_btn_height, 1, styles)
            retry_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)) + retry_btn.Width + scale_btn_interval, btn_rect.Y)
            sfe.Controls.Add(retry_btn)
            btnList.Add(retry_btn)
            Dim ignore_btn As MessageBoxExtButton = CreateButton(sfe, If(styles.Button3Text = String.Empty, "Ignore", styles.Button3Text), New EventHandler(AddressOf Ignore_Click), scale_btn_width, scale_btn_height, 2, styles)
            ignore_btn.Location = New Point(btn_rect.X + CInt(((btn_rect.Width - scale_btn_width * 3 - scale_btn_interval * 2) / 2)) + ignore_btn.Width * 2 + scale_btn_interval * 2, btn_rect.Y)
            sfe.Controls.Add(ignore_btn)
            btnList.Add(ignore_btn)
        End If

        If defaultButton = MessageBoxExtDefaultButton.Button1 Then

            If btnList(0) IsNot Nothing Then
                btnList(0).Focus()
                btnList(0).[Select]()
            End If
        ElseIf defaultButton = MessageBoxExtDefaultButton.Button2 Then

            If btnList(1) IsNot Nothing Then
                btnList(0).Focus()
                btnList(1).[Select]()
            End If
        ElseIf defaultButton = MessageBoxExtDefaultButton.Button3 Then

            If btnList(2) IsNot Nothing Then
                btnList(0).Focus()
                btnList(2).[Select]()
            End If
        End If
        If btnList.Last.Right + btnList.First.Left + SystemInformation.BorderSize.Width * 2 + 6 + scale_margin * (btnList.Count - 1) > sfe.Width Then
            sfe.Width = btnList.Last.Right + btnList.First.Left + SystemInformation.BorderSize.Width * 2 + 6 + scale_margin * (btnList.Count - 1)
        End If
        sfe.TopMost = True
        If owner IsNot Nothing Then
            sfe.StartPosition = FormStartPosition.CenterParent
        Else
            sfe.StartPosition = FormStartPosition.CenterScreen
        End If
        sfe.TopMost = True
        sfe.ShowDialog(owner)
        If sfe.Tag Is Nothing Then
            Return DialogResult.None
        End If

        Return CType(sfe.Tag, DialogResult)
    End Function
    Shared Function CalculatePos(Frm As Form) As Point
        Dim ShowPoint As Point = Control.MousePosition
        If (ShowPoint.Y + Frm.Height) > Screen.PrimaryScreen.WorkingArea.Height Then
            ShowPoint.Y = -((Frm.PointToScreen(ShowPoint).Y + Frm.Height) - Screen.PrimaryScreen.WorkingArea.Height) - 7
        End If
        If (Frm.PointToScreen(ShowPoint).X + Frm.Width) > Screen.PrimaryScreen.WorkingArea.Width Then
            ShowPoint.X = -Frm.Width - 7 + (Screen.PrimaryScreen.WorkingArea.Width - Frm.PointToScreen(New Point(0, 0)).X)
        End If
        ShowPoint.Y += 24
        Return ShowPoint
    End Function
    Private Shared Sub OK_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
        fe.Tag = DialogResult.OK
        fe.Hide()
        'fe.Dispose()
    End Sub

    Private Shared Sub Yes_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
        fe.Tag = DialogResult.Yes
        fe.Hide()
        'fe.Dispose()
    End Sub

    Private Shared Sub No_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mea As MouseEventArgs = CType(e, MouseEventArgs)

        If mea.Button = MouseButtons.Left Then
            Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
            fe.Tag = DialogResult.No
            fe.Hide()
            ' fe.Dispose()
        End If
    End Sub

    Private Shared Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mea As MouseEventArgs = CType(e, MouseEventArgs)

        If mea.Button = MouseButtons.Left Then
            Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
            fe.Tag = DialogResult.Cancel
            fe.Hide()
            'fe.Dispose()
        End If
    End Sub

    Private Shared Sub Abort_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mea As MouseEventArgs = CType(e, MouseEventArgs)

        If mea.Button = MouseButtons.Left Then
            Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
            fe.Tag = DialogResult.Abort
            fe.Hide()
            ' fe.Dispose()
        End If
    End Sub

    Private Shared Sub Retry_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mea As MouseEventArgs = CType(e, MouseEventArgs)

        If mea.Button = MouseButtons.Left Then
            Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
            fe.Tag = DialogResult.Retry
            fe.Hide()
            ' fe.Dispose()
        End If
    End Sub

    Private Shared Sub Ignore_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mea As MouseEventArgs = CType(e, MouseEventArgs)

        If mea.Button = MouseButtons.Left Then
            Dim fe As FormExt = CType(((CType(sender, MessageBoxExtButton)).Tag), FormExt)
            fe.Tag = DialogResult.Ignore
            fe.Hide()
            ' fe.Dispose()
        End If
    End Sub

    Private Shared Function CreateButton(ByVal fe As FormExt, ByVal text As String, ByVal handler As EventHandler, ByVal btn_w As Integer, ByVal btn_h As Integer, ByVal tabIndex As Integer, ByVal style As MessageBoxExtStyles) As MessageBoxExtButton
        Dim btn As New MessageBoxExtButton() With {.Text = text,
        .Size = New Size(btn_w, btn_h),
        .TabIndex = tabIndex,
        .TabStop = True,
        .Tag = fe,
        .BackColor = style.ButtonBackColor,
        .BackEnterColor = style.ButtonBackEnterColor,
        .ForeColor = style.ButtonTextColor}

        AddHandler btn.Click, handler
        Return btn
    End Function

    Private Shared Function GetIco(ByVal ico As MessageBoxExtIcon, ByVal image As Image) As Image
        If ico = MessageBoxExtIcon.Warning Then
            Return My.Resources.MessageBox_Default.Warning
        ElseIf ico = MessageBoxExtIcon.Question Then
            Return My.Resources.MessageBox_Default.Question
        ElseIf ico = MessageBoxExtIcon.[Error] Then
            Return My.Resources.MessageBox_Default._Error
        ElseIf ico = MessageBoxExtIcon.Custom Then
            Return image
        Else
            Return Nothing
        End If
    End Function

    <Description("按钮")>
    Public Class MessageBoxExtButton
        Inherits Control

        <Description("边框颜色")>
        Private mborderColor As Color = Color.White

        Friend Overloads Property BorderColor As Color
            Get
                Return Me.mborderColor
            End Get
            Set(ByVal value As Color)
                If Me.mborderColor = value Then Return
                Me.mborderColor = value
                Me.Invalidate()
            End Set
        End Property

        <Description("背景颜色（鼠标进入）")>
        Private mbackEnterColor As Color = Color.White

        Friend Property BackEnterColor As Color
            Get
                Return Me.mbackEnterColor
            End Get
            Set(ByVal value As Color)
                If Me.mbackEnterColor = value Then Return
                Me.mbackEnterColor = value
                Me.Invalidate()
            End Set
        End Property

        Private tabStatus As Boolean = False
        Private isEnter As Boolean = False

        Protected Overrides Sub OnEnter(ByVal e As EventArgs)
            MyBase.OnEnter(e)
            Me.tabStatus = True
            Me.Invalidate()
        End Sub

        Protected Overrides Sub OnLeave(ByVal e As EventArgs)
            MyBase.OnLeave(e)
            Me.tabStatus = False
            Me.Invalidate()
        End Sub

        Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
            MyBase.OnMouseEnter(e)
            Me.isEnter = True
            Me.Invalidate()
        End Sub

        Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            MyBase.OnMouseLeave(e)
            Me.isEnter = False
            Me.Invalidate()
        End Sub

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            'MyBase.OnPaintBackground(e)
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics
            Dim back_sb As New SolidBrush(If(Me.isEnter, Me.mbackEnterColor, Me.BackColor))
            g.FillRectangle(back_sb, Me.ClientRectangle)
            back_sb.Dispose()
            Dim padding As Integer = 2
            Dim text_sb As New SolidBrush(Me.ForeColor)
            Dim text_sf As New StringFormat() With {
                .FormatFlags = StringFormatFlags.NoWrap,
                .Trimming = StringTrimming.None,
                .LineAlignment = StringAlignment.Center
            }
            Dim text_size As Size = Size.Ceiling(g.MeasureString(Me.Text, Me.Font, Me.Width - padding * 2, text_sf))
            Dim text_rect As New Rectangle(Me.ClientRectangle.X + padding + (Me.ClientRectangle.Width - padding * 2 - text_size.Width) / 2, Me.ClientRectangle.Y + padding + (Me.ClientRectangle.Height - padding * 2 - text_size.Height) / 2, text_size.Width, text_size.Height)
            g.DrawString(Me.Text, Me.Font, text_sb, text_rect, text_sf)
            text_sb.Dispose()
            text_sf.Dispose()

            If Me.tabStatus Then
                Dim tabborder_pen As New Pen(Color.FromArgb(150, Me.mborderColor), 1) With {.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot}

                g.DrawRectangle(tabborder_pen, New Rectangle(Me.ClientRectangle.X + padding, Me.ClientRectangle.Y + padding, Me.ClientRectangle.Width - padding * 2 - 1, Me.ClientRectangle.Height - padding * 2 - 1))
                tabborder_pen.Dispose()
            End If
        End Sub

        Protected Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
            If Me.DesignMode Then
                Return MyBase.ProcessDialogKey(keyData)
            End If

            If Me.tabStatus Then

                If keyData = Keys.Enter Then
                    Me.InvokeOnClick(Me, CType((New MouseEventArgs(MouseButtons.Left, 1, Me.ClientRectangle.X + Me.ClientRectangle.Width / 2, Me.ClientRectangle.Y + Me.ClientRectangle.Height / 2, 0)), EventArgs))
                    Return False
                End If
            End If

            Return MyBase.ProcessDialogKey(keyData)
        End Function
    End Class
End Class

<Description("提示框样式")>
Public Class MessageBoxExtStyles
    Private mControlBox As Boolean = False

    <Description("弹窗YesNo显示关闭按钮")>
    Public Property ControlBox As Boolean
        Get
            Return Me.mControlBox
        End Get
        Set(ByVal value As Boolean)
            If Me.mControlBox = value Then Return
            Me.mControlBox = value
        End Set
    End Property

    Private mborderColor As Color = Color.FromArgb(137, 158, 136)

    <Description("边框颜色")>
    Public Property BorderColor As Color
        Get
            Return Me.mborderColor
        End Get
        Set(ByVal value As Color)
            If Me.mborderColor = value Then Return
            Me.mborderColor = value
        End Set
    End Property

    Private mbackColor As Color = Color.Black

    <Description("背景颜色")>
    Public Property BackColor As Color
        Get
            Return Me.mbackColor
        End Get
        Set(ByVal value As Color)
            If Me.mbackColor = value Then Return
            Me.mbackColor = value
        End Set
    End Property

    Private mcaptionBackColor As Color = SystemColors.Highlight 'Color.FromArgb(137, 165, 136)

    <Description("标题栏背景颜色")>
    Public Property CaptionBackColor As Color
        Get
            Return Me.mcaptionBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mcaptionBackColor = value Then Return
            Me.mcaptionBackColor = value
        End Set
    End Property

    Private mcaptionTextColor As Color = Color.White 'Color.FromArgb(255, 255, 255)

    <Description("标题文本颜色")>
    Public Property CaptionTextColor As Color
        Get
            Return Me.mcaptionTextColor
        End Get
        Set(ByVal value As Color)
            If Me.mcaptionTextColor = value Then Return
            Me.mcaptionTextColor = value
        End Set
    End Property

    Private mtextColor As Color = Color.LimeGreen

    <Description("信息文本颜色")>
    Public Property TextColor As Color
        Get
            Return Me.mtextColor
        End Get
        Set(ByVal value As Color)
            If Me.mtextColor = value Then Return
            Me.mtextColor = value
        End Set
    End Property

    Private mtextFont As New Font("Consolas", 10)

    <Description("信息文本颜色")>
    Public Property TextFont As Font
        Get
            Return Me.mtextFont
        End Get
        Set(ByVal value As Font)
            If Me.mtextFont.Equals(value) Then Return
            Me.mtextFont = value
        End Set
    End Property
    Private mbuttonBackColor As Color = Color.FromArgb(137, 165, 136)

    <Description("按钮背景颜色")>
    Public Property ButtonBackColor As Color
        Get
            Return Me.mbuttonBackColor
        End Get
        Set(ByVal value As Color)
            If Me.mbuttonBackColor = value Then Return
            Me.mbuttonBackColor = value
        End Set
    End Property

    Private mbuttonBackEnterColor As Color = Color.FromArgb(123, 148, 122)

    <Description("按钮背景颜色(鼠标进入)")>
    Public Property ButtonBackEnterColor As Color
        Get
            Return Me.mbuttonBackEnterColor
        End Get
        Set(ByVal value As Color)
            If Me.mbuttonBackEnterColor = value Then Return
            Me.mbuttonBackEnterColor = value
        End Set
    End Property

    Private mbuttonTextColor As Color = Color.FromArgb(255, 255, 255)

    <Description("按钮文本颜色")>
    Public Property ButtonTextColor As Color
        Get
            Return Me.mbuttonTextColor
        End Get
        Set(ByVal value As Color)
            If Me.mbuttonTextColor = value Then Return
            Me.mbuttonTextColor = value
        End Set
    End Property

    Private mbutton1Text As String = String.Empty

    <Description("Button1自定义文本")>
    Public Property Button1Text As String
        Get
            Return Me.mbutton1Text
        End Get
        Set(ByVal value As String)
            If Me.mbutton1Text = value Then Return
            Me.mbutton1Text = value.Trim()
        End Set
    End Property

    Private mbutton2Text As String = String.Empty

    <Description("Button2自定义文本")>
    Public Property Button2Text As String
        Get
            Return Me.mbutton2Text
        End Get
        Set(ByVal value As String)
            If Me.mbutton2Text = value Then Return
            Me.mbutton2Text = value.Trim()
        End Set
    End Property

    Private mbutton3Text As String = String.Empty

    Public Sub New()

    End Sub

    <Description("Button3自定义文本")>
    Public Property Button3Text As String
        Get
            Return Me.mbutton3Text
        End Get
        Set(ByVal value As String)
            If Me.mbutton3Text = value Then Return
            Me.mbutton3Text = value.Trim()
        End Set
    End Property
End Class

<Description("提示按钮类型")>
Public Enum MessageBoxExtButtons
    OK = 0
    OKCancel = 1
    AbortRetryIgnore = 2
    YesNoCancel = 3
    YesNo = 4
    RetryCancel = 5
End Enum

<Description("提示框图标（32x32）")>
Public Enum MessageBoxExtIcon
    None = 0
    Question
    [Error]
    Warning
    Custom
End Enum

<Description("默认激活按钮")>
Public Enum MessageBoxExtDefaultButton
    Button1
    Button2
    Button3
End Enum
