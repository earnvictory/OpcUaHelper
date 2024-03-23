Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design

Public Class TabPageExt
    Public Sub New()
        MyBase.New()
        Me.SetStyle(
                      ControlStyles.UserPaint Or
                      ControlStyles.OptimizedDoubleBuffer Or
                      ControlStyles.AllPaintingInWmPaint Or
                      ControlStyles.ResizeRedraw Or
                      ControlStyles.SupportsTransparentBackColor,
                         True)
        Me.UpdateStyles()
        '组件设计器需要此调用。

        InitializeComponent()

    End Sub



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
    Dim mBorderWidth As Integer = 1

    Property BorderWidth As Integer
        Get
            Return mBorderWidth
        End Get
        Set(value As Integer)
            mBorderWidth = value
            If mBorderWidth < 1 Then
                mBorderWidth = 1
            End If
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Try
            'MyBase.OnPaintBackground(e)
            MyBase.OnPaint(e)

            Dim BorderPen As New Pen(BorderColor) With {.Width = If(BorderWidth >= 1, BorderWidth, 1)}
            e.Graphics.DrawRectangle(BorderPen, Me.ClientRectangle)
            BorderPen.Dispose()
        Catch ex As Exception
            MicsonControlExt.MessageBoxExt.Show(Nothing, ex.Message & vbCrLf & ex.StackTrace)
        End Try

    End Sub

    Private Sub InitializeComponent()
        Me.TabControlExt1 = New OpcUaHelper.TabControlExt()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabControlExt1.SuspendLayout
        Me.SuspendLayout()
        '
        'TabControlExt1
        '
        Me.TabControlExt1.BackroundColor = System.Drawing.SystemColors.Control
        Me.TabControlExt1.BorderColor = System.Drawing.SystemColors.ControlText
        Me.TabControlExt1.Controls.Add(Me.TabPage1)
        Me.TabControlExt1.Controls.Add(Me.TabPage2)
        Me.TabControlExt1.Location = New System.Drawing.Point(0, 0)
        Me.TabControlExt1.Name = "TabControlExt1"
        Me.TabControlExt1.NormalTileColor = System.Drawing.SystemColors.Highlight
        Me.TabControlExt1.OperateType = Nothing
        Me.TabControlExt1.Padding = New System.Drawing.Point(14, 7)
        Me.TabControlExt1.SelectedIndex = 0
        Me.TabControlExt1.SelectTitleColor = System.Drawing.Color.Blue
        Me.TabControlExt1.Size = New System.Drawing.Size(200, 100)
        Me.TabControlExt1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(0, 0)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(200, 100)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(0, 0)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(200, 100)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        Me.TabControlExt1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
End Class
