<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormConnectSelect
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tabControl1 = New OpcUaHelper.TabControlExt()
        Me.tabPage1 = New OpcUaHelper.TabPageExt()
        Me.Button1 = New OpcUaHelper.ButtonExt()
        Me.tabPage2 = New OpcUaHelper.TabPageExt()
        Me.textBox2 = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.textBox1 = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.Button2 = New OpcUaHelper.ButtonExt()
        Me.tabPage3 = New System.Windows.Forms.TabPage()
        Me.Button4 = New OpcUaHelper.ButtonExt()
        Me.textBox3 = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.textBox4 = New System.Windows.Forms.TextBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.Button3 = New OpcUaHelper.ButtonExt()
        Me.tabControl1.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.tabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabControl1
        '
        Me.tabControl1.BackroundColor = System.Drawing.SystemColors.Highlight
        Me.tabControl1.BorderColor = System.Drawing.SystemColors.ControlText
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Controls.Add(Me.tabPage3)
        Me.tabControl1.Location = New System.Drawing.Point(14, 15)
        Me.tabControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.NormalTileColor = System.Drawing.SystemColors.Highlight
        Me.tabControl1.OperateType = Nothing
        Me.tabControl1.Padding = New System.Drawing.Point(14, 7)
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.SelectTitleColor = System.Drawing.Color.Blue
        Me.tabControl1.Size = New System.Drawing.Size(475, 268)
        Me.tabControl1.TabIndex = 1
        '
        'tabPage1
        '
        Me.tabPage1.BackColor = System.Drawing.SystemColors.Highlight
        Me.tabPage1.BorderColor = System.Drawing.Color.Black
        Me.tabPage1.BorderWidth = 1
        Me.tabPage1.Controls.Add(Me.Button1)
        Me.tabPage1.Location = New System.Drawing.Point(4, 34)
        Me.tabPage1.Margin = New System.Windows.Forms.Padding(4)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Padding = New System.Windows.Forms.Padding(4)
        Me.tabPage1.Size = New System.Drawing.Size(467, 230)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "匿名登录"
        '
        'Button1
        '
        Me.Button1.Auto_Size = False
        Me.Button1.BackColor = System.Drawing.SystemColors.Highlight
        Me.Button1.BorderColor = System.Drawing.SystemColors.ButtonShadow
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.None
        Me.Button1.DrawBorder = True
        Me.Button1.Ellipse = False
        Me.Button1.FatherForm = Me
        Me.Button1.Fill2D = True
        Me.Button1.FillAngle = 90.0!
        Me.Button1.FillColor = System.Drawing.SystemColors.Highlight
        Me.Button1.Image = Nothing
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.ImageIndex = 0
        Me.Button1.ImageList = Nothing
        Me.Button1.IsDefault = False
        Me.Button1.LinearGradientColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(166, 91)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Mouse_Down = 0
        Me.Button1.Name = "Button1"
        Me.Button1.Radius = 8
        Me.Button1.Size = New System.Drawing.Size(121, 40)
        Me.Button1.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "登录"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabPage2
        '
        Me.tabPage2.BackColor = System.Drawing.SystemColors.Highlight
        Me.tabPage2.BorderColor = System.Drawing.Color.Black
        Me.tabPage2.BorderWidth = 1
        Me.tabPage2.Controls.Add(Me.textBox2)
        Me.tabPage2.Controls.Add(Me.label2)
        Me.tabPage2.Controls.Add(Me.textBox1)
        Me.tabPage2.Controls.Add(Me.label1)
        Me.tabPage2.Controls.Add(Me.Button2)
        Me.tabPage2.Location = New System.Drawing.Point(4, 34)
        Me.tabPage2.Margin = New System.Windows.Forms.Padding(4)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Padding = New System.Windows.Forms.Padding(4)
        Me.tabPage2.Size = New System.Drawing.Size(467, 230)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "用户名登录"
        '
        'textBox2
        '
        Me.textBox2.Location = New System.Drawing.Point(152, 85)
        Me.textBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New System.Drawing.Size(235, 23)
        Me.textBox2.TabIndex = 5
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(79, 89)
        Me.label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(56, 17)
        Me.label2.TabIndex = 4
        Me.label2.Text = "密码："
        '
        'textBox1
        '
        Me.textBox1.Location = New System.Drawing.Point(152, 40)
        Me.textBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New System.Drawing.Size(235, 23)
        Me.textBox1.TabIndex = 3
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(79, 44)
        Me.label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(72, 17)
        Me.label1.TabIndex = 2
        Me.label1.Text = "用户名："
        '
        'Button2
        '
        Me.Button2.Auto_Size = False
        Me.Button2.BackColor = System.Drawing.SystemColors.Highlight
        Me.Button2.BorderColor = System.Drawing.SystemColors.ButtonShadow
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.None
        Me.Button2.DrawBorder = False
        Me.Button2.Ellipse = False
        Me.Button2.FatherForm = Me
        Me.Button2.Fill2D = True
        Me.Button2.FillAngle = 90.0!
        Me.Button2.FillColor = System.Drawing.SystemColors.ButtonFace
        Me.Button2.Image = Nothing
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.ImageIndex = 0
        Me.Button2.ImageList = Nothing
        Me.Button2.IsDefault = False
        Me.Button2.LinearGradientColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(164, 146)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Mouse_Down = 0
        Me.Button2.Name = "Button2"
        Me.Button2.Radius = 8
        Me.Button2.Size = New System.Drawing.Size(121, 40)
        Me.Button2.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "登录"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabPage3
        '
        Me.tabPage3.BackColor = System.Drawing.SystemColors.Highlight
        Me.tabPage3.Controls.Add(Me.Button4)
        Me.tabPage3.Controls.Add(Me.textBox3)
        Me.tabPage3.Controls.Add(Me.label3)
        Me.tabPage3.Controls.Add(Me.textBox4)
        Me.tabPage3.Controls.Add(Me.label4)
        Me.tabPage3.Controls.Add(Me.Button3)
        Me.tabPage3.Location = New System.Drawing.Point(4, 34)
        Me.tabPage3.Margin = New System.Windows.Forms.Padding(4)
        Me.tabPage3.Name = "tabPage3"
        Me.tabPage3.Padding = New System.Windows.Forms.Padding(4)
        Me.tabPage3.Size = New System.Drawing.Size(467, 230)
        Me.tabPage3.TabIndex = 2
        Me.tabPage3.Text = "证书登录"
        '
        'Button4
        '
        Me.Button4.Auto_Size = False
        Me.Button4.BackColor = System.Drawing.SystemColors.Highlight
        Me.Button4.BorderColor = System.Drawing.SystemColors.ButtonShadow
        Me.Button4.DialogResult = System.Windows.Forms.DialogResult.None
        Me.Button4.DrawBorder = True
        Me.Button4.Ellipse = False
        Me.Button4.FatherForm = Me
        Me.Button4.Fill2D = True
        Me.Button4.FillAngle = 90.0!
        Me.Button4.FillColor = System.Drawing.SystemColors.Highlight
        Me.Button4.Image = Nothing
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button4.ImageIndex = 0
        Me.Button4.ImageList = Nothing
        Me.Button4.IsDefault = False
        Me.Button4.LinearGradientColor = System.Drawing.Color.Black
        Me.Button4.Location = New System.Drawing.Point(394, 39)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4)
        Me.Button4.Mouse_Down = 0
        Me.Button4.Name = "Button4"
        Me.Button4.Radius = 8
        Me.Button4.Size = New System.Drawing.Size(37, 31)
        Me.Button4.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.Button4.TabIndex = 11
        Me.Button4.Text = "..."
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'textBox3
        '
        Me.textBox3.Location = New System.Drawing.Point(152, 85)
        Me.textBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New System.Drawing.Size(235, 23)
        Me.textBox3.TabIndex = 10
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(79, 89)
        Me.label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(56, 17)
        Me.label3.TabIndex = 9
        Me.label3.Text = "密钥："
        '
        'textBox4
        '
        Me.textBox4.Location = New System.Drawing.Point(152, 40)
        Me.textBox4.Margin = New System.Windows.Forms.Padding(4)
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New System.Drawing.Size(235, 23)
        Me.textBox4.TabIndex = 8
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(79, 44)
        Me.label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(56, 17)
        Me.label4.TabIndex = 7
        Me.label4.Text = "证书："
        '
        'Button3
        '
        Me.Button3.Auto_Size = False
        Me.Button3.BackColor = System.Drawing.SystemColors.Highlight
        Me.Button3.BorderColor = System.Drawing.SystemColors.ButtonShadow
        Me.Button3.DialogResult = System.Windows.Forms.DialogResult.None
        Me.Button3.DrawBorder = True
        Me.Button3.Ellipse = False
        Me.Button3.FatherForm = Me
        Me.Button3.Fill2D = True
        Me.Button3.FillAngle = 90.0!
        Me.Button3.FillColor = System.Drawing.SystemColors.Highlight
        Me.Button3.Image = Nothing
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.ImageIndex = 0
        Me.Button3.ImageList = Nothing
        Me.Button3.IsDefault = True
        Me.Button3.LinearGradientColor = System.Drawing.Color.Black
        Me.Button3.Location = New System.Drawing.Point(164, 146)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Mouse_Down = 0
        Me.Button3.Name = "Button3"
        Me.Button3.Radius = 8
        Me.Button3.Size = New System.Drawing.Size(121, 40)
        Me.Button3.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "登录"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormConnectSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 292)
        Me.Controls.Add(Me.tabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormConnectSelect"
        Me.Text = "登录方式选择"
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage2.PerformLayout()
        Me.tabPage3.ResumeLayout(False)
        Me.tabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents tabControl1 As TabControlExt
    Private WithEvents tabPage1 As TabPageExt
    Private WithEvents Button1 As ButtonExt
    Private WithEvents tabPage2 As TabPageExt
    Private WithEvents textBox2 As TextBox
    Private WithEvents label2 As Label
    Private WithEvents textBox1 As TextBox
    Private WithEvents label1 As Label
    Private WithEvents Button2 As ButtonExt
    Private WithEvents tabPage3 As TabPage
    Private WithEvents Button4 As ButtonExt
    Private WithEvents textBox3 As TextBox
    Private WithEvents label3 As Label
    Private WithEvents textBox4 As TextBox
    Private WithEvents label4 As Label
    Private WithEvents Button3 As ButtonExt
End Class
