

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormBrowseServer
    Inherits MicsonControlExt.FormExt



    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBrowseServer))
        Me.label1 = New System.Windows.Forms.Label()
        Me.txtServerUrl = New System.Windows.Forms.TextBox()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.checkBox1 = New System.Windows.Forms.CheckBox()
        Me.BrowseNodesTV = New System.Windows.Forms.TreeView()
        Me.pictureBox2 = New System.Windows.Forms.PictureBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.NodeDetailsGridView = New System.Windows.Forms.DataGridView()
        Me.Image = New System.Windows.Forms.DataGridViewImageColumn()
        Me.DisplayName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AccessLevel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.label_time_spend = New System.Windows.Forms.Label()
        Me.pictureBox3 = New System.Windows.Forms.PictureBox()
        Me.BtnSubscript = New OpcUaHelper.ButtonExt()
        Me.textBox_nodeId = New System.Windows.Forms.TextBox()
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        Me.SelectNodeID_Label = New System.Windows.Forms.Label()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.toolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.toolStripStatusLabel_opc = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ContextMenuStrip_SelectNode = New MicsonControlExt.ContextMenuStripExt
        Me.SelectToolStripMenuItem = New MicsonControlExt.ToolStripMenuItemExt
        Me.CancelToolStripMenuItem = New MicsonControlExt.ToolStripMenuItemExt
        Me.BtnConnect = New OpcUaHelper.ButtonExt()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NodeDetailsGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panel1.SuspendLayout()
        CType(Me.pictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.statusStrip1.SuspendLayout()
        Me.ContextMenuStrip_SelectNode.SuspendLayout()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.ForeColor = System.Drawing.Color.White
        Me.label1.Location = New System.Drawing.Point(7, 16)
        Me.label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(63, 15)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Address:"
        '
        'txtServerUrl
        '
        Me.txtServerUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServerUrl.Location = New System.Drawing.Point(83, 15)
        Me.txtServerUrl.Margin = New System.Windows.Forms.Padding(4)
        Me.txtServerUrl.Name = "txtServerUrl"
        Me.txtServerUrl.Size = New System.Drawing.Size(1057, 23)
        Me.txtServerUrl.TabIndex = 3
        '
        'splitContainer1
        '
        Me.splitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContainer1.Location = New System.Drawing.Point(7, 49)
        Me.splitContainer1.Margin = New System.Windows.Forms.Padding(4)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.checkBox1)
        Me.splitContainer1.Panel1.Controls.Add(Me.BrowseNodesTV)
        Me.splitContainer1.Panel1.Controls.Add(Me.pictureBox2)
        Me.splitContainer1.Panel1.Controls.Add(Me.label3)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.NodeDetailsGridView)
        Me.splitContainer1.Panel2.Controls.Add(Me.panel1)
        Me.splitContainer1.Size = New System.Drawing.Size(1218, 632)
        Me.splitContainer1.SplitterDistance = 350
        Me.splitContainer1.SplitterWidth = 5
        Me.splitContainer1.TabIndex = 6
        '
        'checkBox1
        '
        Me.checkBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBox1.AutoSize = True
        Me.checkBox1.ForeColor = System.Drawing.Color.White
        Me.checkBox1.Location = New System.Drawing.Point(229, 12)
        Me.checkBox1.Name = "checkBox1"
        Me.checkBox1.Size = New System.Drawing.Size(103, 19)
        Me.checkBox1.TabIndex = 9
        Me.checkBox1.Text = "Fast Access"
        Me.checkBox1.UseVisualStyleBackColor = True
        '
        'BrowseNodesTV
        '
        Me.BrowseNodesTV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BrowseNodesTV.Location = New System.Drawing.Point(4, 41)
        Me.BrowseNodesTV.Margin = New System.Windows.Forms.Padding(4)
        Me.BrowseNodesTV.Name = "BrowseNodesTV"
        Me.BrowseNodesTV.Size = New System.Drawing.Size(338, 585)
        Me.BrowseNodesTV.TabIndex = 8
        '
        'pictureBox2
        '
        Me.pictureBox2.Image = Global.OpcUaHelper.My.Resources.Resources.glasses_16xLG
        Me.pictureBox2.Location = New System.Drawing.Point(8, 6)
        Me.pictureBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.pictureBox2.Name = "pictureBox2"
        Me.pictureBox2.Size = New System.Drawing.Size(20, 22)
        Me.pictureBox2.TabIndex = 6
        Me.pictureBox2.TabStop = False
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.ForeColor = System.Drawing.Color.Tomato
        Me.label3.Location = New System.Drawing.Point(35, 9)
        Me.label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(119, 15)
        Me.label3.TabIndex = 3
        Me.label3.Text = "Server Browser :"
        '
        'NodeDetailsGridView
        '
        Me.NodeDetailsGridView.AllowUserToAddRows = False
        Me.NodeDetailsGridView.AllowUserToDeleteRows = False
        Me.NodeDetailsGridView.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue
        Me.NodeDetailsGridView.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.NodeDetailsGridView.BackgroundColor = System.Drawing.Color.White
        Me.NodeDetailsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NodeDetailsGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Image, Me.DisplayName, Me.Value, Me.Type, Me.AccessLevel, Me.Description})
        Me.NodeDetailsGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NodeDetailsGridView.Location = New System.Drawing.Point(0, 34)
        Me.NodeDetailsGridView.MultiSelect = False
        Me.NodeDetailsGridView.Name = "NodeDetailsGridView"
        Me.NodeDetailsGridView.RowHeadersVisible = False
        Me.NodeDetailsGridView.RowTemplate.Height = 23
        Me.NodeDetailsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.NodeDetailsGridView.Size = New System.Drawing.Size(863, 598)
        Me.NodeDetailsGridView.TabIndex = 2
        '
        'Image
        '
        Me.Image.HeaderText = ""
        Me.Image.Name = "Image"
        Me.Image.Width = 20
        '
        'DisplayName
        '
        Me.DisplayName.HeaderText = "Name"
        Me.DisplayName.Name = "DisplayName"
        Me.DisplayName.ReadOnly = True
        Me.DisplayName.ToolTipText = "参数的显示名称"
        Me.DisplayName.Width = 145
        '
        'Value
        '
        Me.Value.HeaderText = "Value"
        Me.Value.Name = "Value"
        Me.Value.ToolTipText = "参数的实际数据值"
        Me.Value.Width = 200
        '
        'Type
        '
        Me.Type.HeaderText = "Type"
        Me.Type.Name = "Type"
        Me.Type.ReadOnly = True
        Me.Type.ToolTipText = "参数的类型"
        Me.Type.Width = 80
        '
        'AccessLevel
        '
        Me.AccessLevel.HeaderText = "AccessLevel"
        Me.AccessLevel.Name = "AccessLevel"
        Me.AccessLevel.ReadOnly = True
        Me.AccessLevel.ToolTipText = "数据的访问等级"
        Me.AccessLevel.Width = 130
        '
        'Description
        '
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.ToolTipText = "数据的文本描述"
        Me.Description.Width = 140
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.label_time_spend)
        Me.panel1.Controls.Add(Me.pictureBox3)
        Me.panel1.Controls.Add(Me.BtnSubscript)
        Me.panel1.Controls.Add(Me.textBox_nodeId)
        Me.panel1.Controls.Add(Me.pictureBox1)
        Me.panel1.Controls.Add(Me.SelectNodeID_Label)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel1.Location = New System.Drawing.Point(0, 0)
        Me.panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(863, 34)
        Me.panel1.TabIndex = 0
        '
        'label_time_spend
        '
        Me.label_time_spend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label_time_spend.AutoSize = True
        Me.label_time_spend.ForeColor = System.Drawing.Color.White
        Me.label_time_spend.Location = New System.Drawing.Point(788, 10)
        Me.label_time_spend.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.label_time_spend.Name = "label_time_spend"
        Me.label_time_spend.Size = New System.Drawing.Size(28, 15)
        Me.label_time_spend.TabIndex = 9
        Me.label_time_spend.Text = "0ms"
        '
        'pictureBox3
        '
        Me.pictureBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox3.Image = Global.OpcUaHelper.My.Resources.Resources.usbcontroller
        Me.pictureBox3.Location = New System.Drawing.Point(761, 6)
        Me.pictureBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.pictureBox3.Name = "pictureBox3"
        Me.pictureBox3.Size = New System.Drawing.Size(20, 22)
        Me.pictureBox3.TabIndex = 8
        Me.pictureBox3.TabStop = False
        '
        'BtnSubscript
        '
        Me.BtnSubscript.Auto_Size = False
        Me.BtnSubscript.BackColor = System.Drawing.SystemColors.Highlight
        Me.BtnSubscript.BorderColor = System.Drawing.Color.Black
        Me.BtnSubscript.DialogResult = System.Windows.Forms.DialogResult.None
        Me.BtnSubscript.DrawBorder = True
        Me.BtnSubscript.Ellipse = False
        Me.BtnSubscript.FatherForm = Me
        Me.BtnSubscript.Fill2D = True
        Me.BtnSubscript.FillAngle = 90.0!
        Me.BtnSubscript.FillColor = System.Drawing.SystemColors.Highlight
        Me.BtnSubscript.ForeColor = System.Drawing.Color.White
        Me.BtnSubscript.Image = Global.OpcUaHelper.My.Resources.Resources.Activity_16xLG
        Me.BtnSubscript.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnSubscript.ImageIndex = 0
        Me.BtnSubscript.ImageList = Nothing
        Me.BtnSubscript.IsDefault = False
        Me.BtnSubscript.LinearGradientColor = System.Drawing.Color.Black
        Me.BtnSubscript.Location = New System.Drawing.Point(0, 1)
        Me.BtnSubscript.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSubscript.Mouse_Down = 0
        Me.BtnSubscript.Name = "BtnSubscript"
        Me.BtnSubscript.Radius = 8
        Me.BtnSubscript.Size = New System.Drawing.Size(124, 31)
        Me.BtnSubscript.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.BtnSubscript.TabIndex = 7
        Me.BtnSubscript.Text = " Subscript"
        Me.BtnSubscript.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'textBox_nodeId
        '
        Me.textBox_nodeId.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.textBox_nodeId.Location = New System.Drawing.Point(344, 2)
        Me.textBox_nodeId.Margin = New System.Windows.Forms.Padding(5)
        Me.textBox_nodeId.Name = "textBox_nodeId"
        Me.textBox_nodeId.ReadOnly = True
        Me.textBox_nodeId.Size = New System.Drawing.Size(409, 23)
        Me.textBox_nodeId.TabIndex = 6
        '
        'pictureBox1
        '
        Me.pictureBox1.Image = Global.OpcUaHelper.My.Resources.Resources.Copy_6524
        Me.pictureBox1.Location = New System.Drawing.Point(184, 5)
        Me.pictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(20, 22)
        Me.pictureBox1.TabIndex = 5
        Me.pictureBox1.TabStop = False
        '
        'SelectNodeID_Label
        '
        Me.SelectNodeID_Label.AutoSize = True
        Me.SelectNodeID_Label.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SelectNodeID_Label.ForeColor = System.Drawing.Color.White
        Me.SelectNodeID_Label.Location = New System.Drawing.Point(217, 10)
        Me.SelectNodeID_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.SelectNodeID_Label.Name = "SelectNodeID_Label"
        Me.SelectNodeID_Label.Size = New System.Drawing.Size(119, 15)
        Me.SelectNodeID_Label.TabIndex = 3
        Me.SelectNodeID_Label.Text = "Selected NodeId:"
        '
        'statusStrip1
        '
        Me.statusStrip1.BackColor = System.Drawing.SystemColors.Highlight
        Me.statusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripStatusLabel1, Me.toolStripStatusLabel_opc})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 689)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me.statusStrip1.Size = New System.Drawing.Size(1237, 22)
        Me.statusStrip1.TabIndex = 7
        Me.statusStrip1.Text = "statusStrip1"
        '
        'toolStripStatusLabel1
        '
        Me.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Cyan
        Me.toolStripStatusLabel1.Name = "toolStripStatusLabel1"
        Me.toolStripStatusLabel1.Size = New System.Drawing.Size(78, 17)
        Me.toolStripStatusLabel1.Text = "Opc Status: "
        '
        'toolStripStatusLabel_opc
        '
        Me.toolStripStatusLabel_opc.ForeColor = System.Drawing.Color.Cyan
        Me.toolStripStatusLabel_opc.Name = "toolStripStatusLabel_opc"
        Me.toolStripStatusLabel_opc.Size = New System.Drawing.Size(0, 17)
        '
        'ContextMenuStrip_SelectNode
        '
        Me.ContextMenuStrip_SelectNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectToolStripMenuItem, Me.CancelToolStripMenuItem})
        Me.ContextMenuStrip_SelectNode.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_SelectNode.Size = New System.Drawing.Size(115, 48)
        '
        'SelectToolStripMenuItem
        '
        Me.SelectToolStripMenuItem.Name = "SelectToolStripMenuItem"
        Me.SelectToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.SelectToolStripMenuItem.Text = "Select"
        '
        'CancelToolStripMenuItem
        '
        Me.CancelToolStripMenuItem.Name = "CancelToolStripMenuItem"
        Me.CancelToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.CancelToolStripMenuItem.Text = "Cancel"
        '
        'BtnConnect
        '
        Me.BtnConnect.Auto_Size = False
        Me.BtnConnect.BackColor = System.Drawing.SystemColors.Highlight
        Me.BtnConnect.BorderColor = System.Drawing.Color.Black
        Me.BtnConnect.DialogResult = System.Windows.Forms.DialogResult.None
        Me.BtnConnect.DrawBorder = True
        Me.BtnConnect.Ellipse = False
        Me.BtnConnect.FatherForm = Me
        Me.BtnConnect.Fill2D = True
        Me.BtnConnect.FillAngle = 90.0!
        Me.BtnConnect.FillColor = System.Drawing.SystemColors.Highlight
        Me.BtnConnect.ForeColor = System.Drawing.Color.White
        Me.BtnConnect.Image = Nothing
        Me.BtnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnConnect.ImageIndex = 0
        Me.BtnConnect.ImageList = Nothing
        Me.BtnConnect.IsDefault = False
        Me.BtnConnect.LinearGradientColor = System.Drawing.Color.Black
        Me.BtnConnect.Location = New System.Drawing.Point(1148, 10)
        Me.BtnConnect.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnConnect.Mouse_Down = 0
        Me.BtnConnect.Name = "BtnConnect"
        Me.BtnConnect.Radius = 8
        Me.BtnConnect.Size = New System.Drawing.Size(77, 31)
        Me.BtnConnect.Style = OpcUaHelper.ButtonExt.RoundStyle.All
        Me.BtnConnect.TabIndex = 10
        Me.BtnConnect.Text = "Connect"
        Me.BtnConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormBrowseServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1237, 711)
        Me.Controls.Add(Me.BtnConnect)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.txtServerUrl)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.Name = "FormBrowseServer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OPC Monitor [ OPC UA Helper ]"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel1.PerformLayout()
        Me.splitContainer1.Panel2.ResumeLayout(False)
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NodeDetailsGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        CType(Me.pictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        Me.ContextMenuStrip_SelectNode.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents label1 As Label
    Private WithEvents txtServerUrl As TextBox
    Private WithEvents splitContainer1 As SplitContainer
    Friend WithEvents BrowseNodesTV As TreeView
    Private WithEvents pictureBox2 As PictureBox
    Private WithEvents label3 As Label
    Private WithEvents panel1 As Panel
    Private WithEvents label_time_spend As Label
    Private WithEvents pictureBox3 As PictureBox
    Private WithEvents BtnSubscript As ButtonExt
    Private WithEvents textBox_nodeId As TextBox
    Private WithEvents pictureBox1 As PictureBox
    Private WithEvents SelectNodeID_Label As Label
    Private WithEvents statusStrip1 As StatusStrip
    Private WithEvents toolStripStatusLabel1 As ToolStripStatusLabel
    Private WithEvents toolStripStatusLabel_opc As ToolStripStatusLabel
    Friend WithEvents ContextMenuStrip_SelectNode As MicsonControlExt.ContextMenuStripExt
    Friend WithEvents SelectToolStripMenuItem As MicsonControlExt.ToolStripMenuItemExt
    Friend WithEvents CancelToolStripMenuItem As MicsonControlExt.ToolStripMenuItemExt
    Private WithEvents checkBox1 As CheckBox
    Private WithEvents NodeDetailsGridView As DataGridView
    Private WithEvents Image As DataGridViewImageColumn
    Private WithEvents DisplayName As DataGridViewTextBoxColumn
    Private WithEvents Value As DataGridViewTextBoxColumn
    Private WithEvents Type As DataGridViewTextBoxColumn
    Private WithEvents AccessLevel As DataGridViewTextBoxColumn
    Private WithEvents Description As DataGridViewTextBoxColumn
    Private WithEvents BtnConnect As ButtonExt
End Class

