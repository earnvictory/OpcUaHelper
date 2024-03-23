Imports System.ComponentModel

<ToolboxItem(False)>
Public Class InputTextBox
    Inherits System.Windows.Forms.TextBox
    Public Sub New()
        MyBase.New
        InitializeComponent()
    End Sub

    Public Delegate Sub AcceptEventHandler(ByVal sender As InputTextBox, ByVal e As AcceptEventArgs)
    Event Accept As AcceptEventHandler
    Class AcceptEventArgs
        Inherits EventArgs
        Property Value As String
        Property Accept As Boolean
    End Class
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
        MyBase.OnPaint(e)
        Dim Rect As New Rectangle(Me.ClientRectangle.X, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1)
        Dim BorderPen As New Pen(Color.Gold)
        e.Graphics.DrawRectangle(BorderPen, Rect)
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Enter Then
            RaiseEvent Accept(Me, New AcceptEventArgs() With {.Value = Me.Text, .Accept = True})
        ElseIf e.KeyCode = Keys.Cancel Then
            RaiseEvent Accept(Me, New AcceptEventArgs() With {.Value = Me.Text, .Accept = False})
        End If
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'InputTextBox
        '
        Me.Font = New System.Drawing.Font("Consolas", 11.0!)
        Me.Multiline = True
        Me.Size = New System.Drawing.Size(100, 21)
        Me.TabStop = False
        Me.ResumeLayout(False)

    End Sub
End Class
