<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ExceptionDlg
    Inherits MicsonControlExt.FormDialog

    'Form 重写 Dispose,以清理组件列表。

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.BottomPN = New System.Windows.Forms.Panel()
        Me.ShowStackTracesCK = New System.Windows.Forms.CheckBox()
        Me.CloseBTN = New OpcUaHelper.ButtonExt()
        Me.ExceptionBrowser = New System.Windows.Forms.RichTextBox()
        Me.BottomPN.SuspendLayout()
        Me.SuspendLayout()
        '
        'BottomPN
        '
        Me.BottomPN.Controls.Add(Me.ShowStackTracesCK)
        Me.BottomPN.Controls.Add(Me.CloseBTN)
        Me.BottomPN.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BottomPN.Location = New System.Drawing.Point(0, 198)
        Me.BottomPN.Margin = New System.Windows.Forms.Padding(4)
        Me.BottomPN.Name = "BottomPN"
        Me.BottomPN.Size = New System.Drawing.Size(810, 34)
        Me.BottomPN.TabIndex = 5
        '
        'ShowStackTracesCK
        '
        Me.ShowStackTracesCK.AutoSize = True
        Me.ShowStackTracesCK.Location = New System.Drawing.Point(4, 8)
        Me.ShowStackTracesCK.Margin = New System.Windows.Forms.Padding(4)
        Me.ShowStackTracesCK.Name = "ShowStackTracesCK"
        Me.ShowStackTracesCK.Size = New System.Drawing.Size(203, 21)
        Me.ShowStackTracesCK.TabIndex = 1
        Me.ShowStackTracesCK.Text = "Show Exception Details"
        '
        'CloseBTN
        '
        Me.CloseBTN.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.CloseBTN.Auto_Size = False
        Me.CloseBTN.BackColor = System.Drawing.SystemColors.Highlight
        Me.CloseBTN.BorderColor = System.Drawing.SystemColors.ButtonShadow
        Me.CloseBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseBTN.DrawBorder = True
        Me.CloseBTN.Ellipse = False
        Me.CloseBTN.FatherForm = Me
        Me.CloseBTN.Fill2D = True
        Me.CloseBTN.FillAngle = 0!
        Me.CloseBTN.FillColor = System.Drawing.SystemColors.Highlight
        Me.CloseBTN.Image = Nothing
        Me.CloseBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CloseBTN.ImageIndex = 0
        Me.CloseBTN.ImageList = Nothing
        Me.CloseBTN.IsDefault = False
        Me.CloseBTN.LinearGradientColor = System.Drawing.Color.Black
        Me.CloseBTN.Location = New System.Drawing.Point(362, 4)
        Me.CloseBTN.Margin = New System.Windows.Forms.Padding(4)
        Me.CloseBTN.Mouse_Down = 0
        Me.CloseBTN.Name = "CloseBTN"
        Me.CloseBTN.Radius = 8
        Me.CloseBTN.Size = New System.Drawing.Size(88, 26)
        Me.CloseBTN.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.CloseBTN.TabIndex = 0
        Me.CloseBTN.Text = "Close"
        Me.CloseBTN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ExceptionBrowser
        '
        Me.ExceptionBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExceptionBrowser.Location = New System.Drawing.Point(0, 0)
        Me.ExceptionBrowser.Name = "ExceptionBrowser"
        Me.ExceptionBrowser.Size = New System.Drawing.Size(810, 198)
        Me.ExceptionBrowser.TabIndex = 7
        Me.ExceptionBrowser.Text = ""
        '
        'ExceptionDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(810, 232)
        Me.Controls.Add(Me.ExceptionBrowser)
        Me.Controls.Add(Me.BottomPN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExceptionDlg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ExceptionDlg"
        Me.BottomPN.ResumeLayout(False)
        Me.BottomPN.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents BottomPN As Panel
    Private WithEvents ShowStackTracesCK As CheckBox
    Private WithEvents CloseBTN As ButtonExt
    Friend WithEvents ExceptionBrowser As RichTextBox
End Class
