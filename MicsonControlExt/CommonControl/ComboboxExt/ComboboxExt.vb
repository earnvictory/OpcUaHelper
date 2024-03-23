Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Windows
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Header
Imports MicsonControlExt.ListBoxExt
<ToolboxItem(True)>
<DefaultEvent("Accept")>
Public Class ComboboxExt
    Inherits ControlExt.Control

    Dim WithEvents InputBox As New InputTextBox
    Private ControlHost As ToolStripControlHost
    Private Shadows dropDown As ToolStripDropDown
    Dim WithEvents DownListBox As New ListBoxExt


    Public Sub New()
        BackColor = Color.White
        DrawBorder = True
        ControlHost = New ToolStripControlHost(InputBox) With {.AutoSize = False, .Margin = New Padding(0, 0, 0, 0)}
        dropDown = New ToolStripDropDown() With {.AutoSize = False, .Margin = New Padding(0, 0, 0, 0)}
        dropDown.Items.Add(ControlHost)
        Me.Text = ""
        TextAlign = ContentAlignment.MiddleLeft
        If DesignMode Then Return

    End Sub

    Public Function FindStringExact(ByVal s As String) As Integer

        Return Array.FindIndex(Of ListBoxExt.Item)(Items.GetArray, Function(x) x.ToString = s)

    End Function



    Dim mText As String = ""
    <Editor(GetType(TextEditorExt), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overloads Property Text As String
        Get

            Return mText
        End Get
        Set(ByVal value As String)
            If mText = value Then Return
            mText = value

            Me.Invalidate()

        End Set
    End Property

    Private Sub ShowDropDown()
        DownListBox.Font = MyBase.Font
        dropDown?.Items?.Clear()
        ControlHost = New ToolStripControlHost(DownListBox) With {.AutoSize = False}
        dropDown = New ToolStripDropDown() With {.AutoSize = False}
        dropDown.Items.Add(ControlHost)
        ControlHost.Width = Me.Width
        dropDown.Width = Me.Width
        ControlHost.Height = Math.Max(DownListBox.ItemHeight, Me.Font.Height) * (If(DownListBox.Items.Count > 7, 7, DownListBox.Items.Count)) + 6
        dropDown.Height = ControlHost.Height
        dropDown.Show(Me, 0, Me.Height)
        DownListBox.BringToFront()
        DownListBox.Focus()

    End Sub


    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If e.Button <> MouseButtons.Left Then Return
        If DesignMode Then Return
        If isEnter AndAlso DialogButtonRect.Contains(e.Location) Then
            ShowDropDown()
            Dim ItemList As New List(Of String)
            For Each item As Object In Items
                ItemList.Add(item.ToString)
            Next

            Dim i As Integer = ItemList.FindIndex(Function(x) x.ToString.ToUpper.StartsWith(Me.Text.ToUpper))
            If i >= 0 Then
                DownListBox.SelectedIndex = i
                DownListBox.VerticalScrollSetFirstShowItem(i)
            Else
                Me.Text = ""
            End If
        Else
            ShowDropDownInput()
        End If
    End Sub
    Enum ComboBoxStyle
        DropDown
        DropDownList
    End Enum

    Property DropDownStyle As ComboBoxStyle
    Private Sub ShowDropDownInput()
        If DropDownStyle = ComboBoxStyle.DropDownList Then Return
        dropDown?.Items?.Clear()
        ControlHost = New ToolStripControlHost(InputBox) With {.AutoSize = False}
        dropDown.Items.Add(ControlHost)
        InputBox.Text = Me.Text
        InputBox.Width = Me.Width
        InputBox.Height = Me.Height
        ControlHost.Width = InputBox.Width + 1
        dropDown.Width = ControlHost.Width + 2
        ControlHost.Height = InputBox.Height + 1
        dropDown.Height = ControlHost.Height + 2
        dropDown.Show(Me, -2, -2)
        InputBox.BringToFront()
        InputBox.Focus()
        InputBox.SelectAll()

    End Sub



    Event Accept(sender As Object, e As InputTextBox.AcceptEventArgs)
    Private Sub InputBox_Accept(sender As InputTextBox, e As InputTextBox.AcceptEventArgs) Handles InputBox.Accept
        If e.Accept Then
            Me.Text = e.Value

            RaiseEvent Accept(Me, e)
        End If
        dropDown?.Close()
        Invalidate()
    End Sub
    <Description("选项集合")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Overloads ReadOnly Property Items As ListBoxExt.ItemCollection
        Get
            Return DownListBox.Items
        End Get
    End Property

    Overloads Property SelectedItem As Object
        Get
            Return DownListBox.SelectedItem
        End Get
        Set(value As Object)
            DownListBox.SelectedItem = value
        End Set
    End Property
    Property SelectedIndex As Integer
        Get
            Return DownListBox.SelectedIndex
        End Get
        Set(value As Integer)
            DownListBox.SelectedIndex = value
        End Set
    End Property

    Event SelectedItemChanged(sender As ComboboxExt, e As ListBoxExt.ItemClickEventArgs)
    Private Sub ItemClick(sender As Object, e As ListBoxExt.ItemClickEventArgs) Handles DownListBox.ItemClick
        If e.Item Is Nothing Then Return
        Me.Text = e.Item.ToString
        Me.Tag = e.Item.Data
        If dropDown IsNot Nothing Then dropDown.Close()
        OnSelectedItemChanged(Me, e)

    End Sub
    Protected Overridable Sub OnSelectedItemChanged(sender As ComboboxExt, e As ListBoxExt.ItemClickEventArgs)
        RaiseEvent SelectedItemChanged(Me, e)
        RaiseEvent Accept(Me, New InputTextBox.AcceptEventArgs() With {.Value = Text, .Accept = True})
    End Sub
    Dim mNoWrap As Boolean = True
    ''' <summary>
    ''' 禁止自动换行
    ''' </summary>
    ''' <returns></returns>
    Property NoWrap As Boolean
        Get
            Return mNoWrap
        End Get
        Set(value As Boolean)
            mNoWrap = value
            Invalidate()
        End Set
    End Property
    Protected DialogButtonRect As Rectangle = Rectangle.Empty
    Dim isEnter As Boolean
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        isEnter = True
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        isEnter = False
        Invalidate()
    End Sub
    Enum ImageColorType
        Black
        Blue
    End Enum
    Dim mImageType As ImageColorType = ImageColorType.Blue
    Property ImageType As ImageColorType
        Get
            Return mImageType
        End Get
        Set(value As ImageColorType)
            mImageType = value
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
        MyBase.OnPaint(e)

        Dim RegionRect As New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)

        Dim g As Graphics = ControlExt.Common.Graphics_SetSmoothHighQuality(e.Graphics)
        g.Clear(BackColor)

        If Me.Height < Font.Height + 2 Then
            Me.Height = Font.Height + 2
        End If
        If DrawBorder Then
            Dim BorderPen As New Pen(BorderColor)
            Dim BorderRec As New Rectangle(RegionRect.X, RegionRect.Y, RegionRect.Width - 1, RegionRect.Height - 1)
            Dim BorderPathInflate As GraphicsPath = ControlExt.Common.DrawRoundRect(BorderRec, 2, ControlExt.Common.RoundStyle.All)
            g.DrawPath(BorderPen, BorderPathInflate)
            BorderPen.Dispose()
        End If
        If isEnter Then
            DialogButtonRect = New Rectangle(RegionRect.Right - SystemInformation.VerticalScrollBarWidth, 0, SystemInformation.VerticalScrollBarWidth, RegionRect.Height)
            g.DrawImage(If(ImageType = ImageColorType.Blue, My.Resources.Combobox.ArrowDown_blue, My.Resources.Combobox.ArrowDown_black), Rectangle.Inflate(DialogButtonRect, (SystemInformation.VerticalScrollBarWidth - DialogButtonRect.Width) / 2, (SystemInformation.VerticalScrollBarWidth - DialogButtonRect.Height) / 2))
        End If
        Dim tsf As StringFormat = ControlExt.Common.GetTextFormat(TextAlign)
        If NoWrap Then
            tsf.FormatFlags = StringFormatFlags.NoWrap
        End If
        RegionRect.Width -= If(isEnter, SystemInformation.VerticalScrollBarWidth, -1)
        g.DrawString(Text, Me.Font, Brushes.Black, RegionRect, tsf)

    End Sub


End Class
