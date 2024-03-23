Imports Opc.Ua
Imports Opc.Ua.Client
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button



Public Class FormBrowseServer

    Public Sub New()
        InitializeComponent()
        Icon = ClientUtils.GetAppIcon()
    End Sub

    Public Sub New(Client As OpcUaClient)

        ' 此调用是设计器所必需的。
        InitializeComponent()
        Icon = ClientUtils.GetAppIcon()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        '  
        m_OpcUaClient = Client
        txtServerUrl.Text = Client.Url

        If Client.ErrStatus Then
            toolStripStatusLabel1.BackColor = Color.Red
        Else
            toolStripStatusLabel1.BackColor = SystemColors.MenuHighlight
        End If

        toolStripStatusLabel_opc.Text = Client.StatusString

        AddHandler m_OpcUaClient.OpcStatusChange, AddressOf OpcUaClient_OpcStatusChange
        AddHandler m_OpcUaClient.ConnectComplete, AddressOf OpcUaClient_ConnectComplete

        BrowseNodesTV.ContextMenuStrip = ContextMenuStrip_SelectNode
        If m_OpcUaClient.Connected Then
            BtnConnect.Enabled = False
            PopulateBranch(m_OpcUaClient)
        End If
    End Sub
    Private Sub OpcUaClient_ConnectComplete(ByVal sender As Object, ByVal e As EventArgs)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Of Object, EventArgs)(AddressOf ConnectComplete), sender, e)
        End If
    End Sub
    Private Sub ConnectComplete(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim client As OpcUaClient = CType(sender, OpcUaClient)

            If client.Connected Then
                BtnConnect.Enabled = False
                PopulateBranch(ObjectIds.ObjectsFolder, BrowseNodesTV.Nodes)
                BrowseNodesTV.Enabled = True
            End If

        Catch exception As Exception
            ClientUtils.HandleException(Text, exception)
        End Try
    End Sub
    Private Sub PopulateBranch(ByVal sender As OpcUaClient)
        Try
            Dim client As OpcUaClient = sender

            If client.Connected Then
                PopulateBranch(ObjectIds.ObjectsFolder, BrowseNodesTV.Nodes)
                BrowseNodesTV.Enabled = True
            End If

        Catch exception As Exception
            ClientUtils.HandleException(Text, exception)
        End Try
    End Sub
    'Public Sub New(ByVal server As String)
    '    InitializeComponent()
    '    Icon = ClientUtils.GetAppIcon()
    '    txtServerUrl.Text = server
    'End Sub

    Private Sub FormBrowseServer_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        BrowseNodesTV.ImageList = New ImageList()
        BrowseNodesTV.ImageList.Images.Add("Class_489", My.Resources.Class_489)
        BrowseNodesTV.ImageList.Images.Add("ClassIcon", My.Resources.ClassIcon)
        BrowseNodesTV.ImageList.Images.Add("brackets", My.Resources.brackets_Square_16xMD)
        BrowseNodesTV.ImageList.Images.Add("VirtualMachine", My.Resources.VirtualMachine)
        BrowseNodesTV.ImageList.Images.Add("Enum_582", My.Resources.Enum_582)
        BrowseNodesTV.ImageList.Images.Add("Method_636", My.Resources.Method_636)
        BrowseNodesTV.ImageList.Images.Add("Module_648", My.Resources.Module_648)
        BrowseNodesTV.ImageList.Images.Add("Loading", My.Resources.loading)
        If Not String.IsNullOrEmpty(txtServerUrl.Text) Then txtServerUrl.[ReadOnly] = True
        ' OpcUaClientInitialization()
    End Sub

    Private Function GetImageKeyFromDescription(ByVal target As ReferenceDescription, ByVal sourceId As NodeId) As String
        If target.NodeClass = NodeClass.Variable Then
            Dim dataValue As DataValue = m_OpcUaClient.ReadNode(CType(target.NodeId, NodeId))

            If dataValue.WrappedValue.TypeInfo IsNot Nothing Then

                If dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.Scalar Then
                    Return "Enum_582"
                ElseIf dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.OneDimension Then
                    Return "brackets"
                ElseIf dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.TwoDimensions Then
                    Return "Module_648"
                Else
                    Return "ClassIcon"
                End If
            Else
                Return "ClassIcon"
            End If
        ElseIf target.NodeClass = NodeClass.Object Then

            If sourceId = ObjectIds.ObjectsFolder Then
                Return "VirtualMachine"
            Else
                Return "ClassIcon"
            End If
        ElseIf target.NodeClass = NodeClass.Method Then
            Return "Method_636"
        Else
            Return "ClassIcon"
        End If
    End Function

    'Private Sub FormBrowseServer_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
    '    m_OpcUaClient.Disconnect()
    '    If m_OpcUaClient IsNot Nothing Then
    '        RemoveHandler m_OpcUaClient.OpcStatusChange, AddressOf OpcUaClient_OpcStatusChange
    '        RemoveHandler m_OpcUaClient.ConnectComplete, AddressOf OpcUaClient_ConnectComplete
    '    End If

    'End Sub
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If m_OpcUaClient IsNot Nothing Then
                RemoveHandler m_OpcUaClient.OpcStatusChange, AddressOf OpcUaClient_OpcStatusChange
                RemoveHandler m_OpcUaClient.ConnectComplete, AddressOf OpcUaClient_ConnectComplete
            End If

            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private m_OpcUaClient As OpcUaClient = Nothing
#Region "做成Dll这里不需要"
    'Private Sub OpcUaClientInitialization()
    '    m_OpcUaClient = New OpcUaClient()
    '    AddHandler m_OpcUaClient.OpcStatusChange, AddressOf OpcUaClient_OpcStatusChange
    '    AddHandler m_OpcUaClient.ConnectComplete, AddressOf OpcUaClient_ConnectComplete
    'End Sub
#End Region




    Private Sub OpcUaClient_OpcStatusChange(ByVal sender As Object, ByVal e As OpcUaStatusEventArgs)
        If InvokeRequired Then
            BeginInvoke(New Action(Sub()
                                       OpcUaClient_OpcStatusChange(sender, e)
                                   End Sub))
            Return
        End If

        If e.[Error] Then
            toolStripStatusLabel1.BackColor = Color.Red
        Else
            toolStripStatusLabel1.BackColor = SystemColors.MenuHighlight
        End If

        toolStripStatusLabel_opc.Text = e.ToString()
    End Sub

    Private Function GetReferenceDescriptionCollection(ByVal sourceId As NodeId) As ReferenceDescriptionCollection
        Dim task As New TaskCompletionSource(Of ReferenceDescriptionCollection)()
        Dim nodeToBrowse1 As New BrowseDescription() With {.NodeId = sourceId,
        .BrowseDirection = BrowseDirection.Forward,
        .ReferenceTypeId = ReferenceTypeIds.Aggregates,
        .IncludeSubtypes = True,
        .NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable Or NodeClass.Method Or NodeClass.ReferenceType Or NodeClass.ObjectType Or NodeClass.View Or NodeClass.VariableType Or NodeClass.DataType)),
        .ResultMask = CUInt(BrowseResultMask.All)}

        Dim nodeToBrowse2 As New BrowseDescription() With {.NodeId = sourceId,
        .BrowseDirection = BrowseDirection.Forward,
        .ReferenceTypeId = ReferenceTypeIds.Organizes,
        .IncludeSubtypes = True,
        .NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable Or NodeClass.Method Or NodeClass.View Or NodeClass.ReferenceType Or NodeClass.ObjectType Or NodeClass.VariableType Or NodeClass.DataType)),
        .ResultMask = CUInt(BrowseResultMask.All)}

        Dim nodesToBrowse As New BrowseDescriptionCollection()
        With nodesToBrowse
            .Add(nodeToBrowse1)
            .Add(nodeToBrowse2)
        End With

        Dim references As ReferenceDescriptionCollection = FormUtils.Browse(m_OpcUaClient.Session, nodesToBrowse, False)
        Return references
    End Function


    Private Async Sub BtnConnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnConnect.Click
        Using formConnectSelect As FormConnectSelect = New FormConnectSelect(m_OpcUaClient)

            If formConnectSelect.ShowDialog() = DialogResult.OK Then

                Try
                    Await m_OpcUaClient.ConnectServer(txtServerUrl.Text)
                    BtnConnect.BackColor = Color.LimeGreen
                Catch ex As Exception
                    ClientUtils.HandleException(Text, ex)
                End Try
            End If
        End Using
    End Sub

    Private Async Sub PopulateBranch(ByVal sourceId As NodeId, ByVal nodes As TreeNodeCollection)

        nodes.Clear()
        nodes.Add(New TreeNode("Browsering...", 7, 7))
        Dim listNode As TreeNode() = Await Task.Run(Function()
                                                        Dim references As ReferenceDescriptionCollection = GetReferenceDescriptionCollection(sourceId)
                                                        Dim list As New List(Of TreeNode)()

                                                        If references IsNot Nothing Then

                                                            For ii As Integer = 0 To references.Count - 1
                                                                Dim target As ReferenceDescription = references(ii)
                                                                Dim key As String = GetImageKeyFromDescription(target, sourceId)
                                                                Dim child As New TreeNode(Utils.Format("{0}", target)) With {.Tag = target,
                                                        .ImageKey = key,
                                                        .SelectedImageKey = key}
                                                                If Not checkBox1.Checked Then

                                                                    If GetReferenceDescriptionCollection(CType(target.NodeId, NodeId)).Count > 0 Then
                                                                        child.Nodes.Add(New TreeNode())
                                                                    End If
                                                                Else
                                                                    child.Nodes.Add(New TreeNode())
                                                                End If

                                                                list.Add(child)
                                                            Next
                                                        End If

                                                        Return list.ToArray()
                                                    End Function)
        nodes.Clear()
        nodes.AddRange(listNode.ToArray())

    End Sub

    Private Sub BrowseNodesTV_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles BrowseNodesTV.MouseClick
        If e.Button = MouseButtons.Right Then
            Dim tn As TreeNode = BrowseNodesTV.GetNodeAt(e.X, e.Y)

            If tn IsNot Nothing Then
                BrowseNodesTV.SelectedNode = tn
            End If
        End If
    End Sub

    Private Sub BrowseNodesTV_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles BrowseNodesTV.BeforeExpand
        Try

            If e.Node.Nodes.Count <> 1 Then
                Return
            End If

            If e.Node.Nodes.Count > 0 Then

                If e.Node.Nodes(0).Text <> String.Empty Then
                    Return
                End If
            End If

            Dim reference As ReferenceDescription = TryCast(e.Node.Tag, ReferenceDescription)

            If reference Is Nothing OrElse reference.NodeId.IsAbsolute Then
                e.Cancel = True
                Return
            End If

            PopulateBranch(CType(reference.NodeId, NodeId), e.Node.Nodes)
        Catch exception As Exception
            ClientUtils.HandleException(Me.Text, exception)
        End Try
    End Sub

    Private Sub BrowseNodesTV_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles BrowseNodesTV.AfterSelect
        Try
            RemoveAllSubscript()
            BtnSubscript.BackColor = SystemColors.Control
            Dim reference As ReferenceDescription = TryCast(e.Node.Tag, ReferenceDescription)

            If reference Is Nothing OrElse reference.NodeId.IsAbsolute Then
                Return
            End If

            ShowMember(CType(reference.NodeId, NodeId))
        Catch exception As Exception
            ClientUtils.HandleException(Text, exception)
        End Try
    End Sub

    Private Sub ClearDataGridViewRows(index As Integer)
        Try
            For i As Integer = Me.NodeDetailsGridView.Rows.Count - 1 To index Step -1
                Dim flag As Boolean = i >= 0
                If flag Then
                    Me.NodeDetailsGridView.Rows.RemoveAt(i)
                End If
            Next
        Catch ex As Exception
            MicsonControlExt.MessageBoxExt.Show(Nothing, ex.Message)
        End Try


    End Sub

    Private Sub SelectNodeID_Label_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectNodeID_Label.Click
        If Not String.IsNullOrEmpty(textBox_nodeId.Text) Then
            Clipboard.SetText(textBox_nodeId.Text)
        End If
    End Sub

    Private Async Sub ShowMember(ByVal sourceId As NodeId)
        SelectNodeID = sourceId.ToString()
        textBox_nodeId.Text = sourceId.ToString()
        Dim index As Integer = 0
        Dim references As ReferenceDescriptionCollection

        Try
            references = Await Task.Run(Function() GetReferenceDescriptionCollection(sourceId))
        Catch exception As Exception
            ClientUtils.HandleException(Text, exception)
            Return
        End Try

        If references?.Count > 0 Then
            Dim nodeIds As New List(Of NodeId)()

            For ii As Integer = 0 To references.Count - 1
                Dim target As ReferenceDescription = references(ii)
                nodeIds.Add(CType(target.NodeId, NodeId))
            Next

            Dim dateTimeStart As DateTime = DateTime.Now
            Dim dataValues As DataValue() = Await Task.Run(Function() m_OpcUaClient.ReadOneNodeFiveAttributes(nodeIds))
            label_time_spend.Text = CInt((DateTime.Now - dateTimeStart).TotalMilliseconds) & " ms"

            For jj As Integer = 0 To dataValues.Length - 1 Step 5
                AddDataGridViewNewRow(dataValues, jj, index, nodeIds(jj / 5))
                index += 1
            Next
        Else

            Try
                Dim dateTimeStart As DateTime = DateTime.Now
                Dim dataValue As DataValue = m_OpcUaClient.ReadNode(sourceId)

                If dataValue.WrappedValue.TypeInfo?.ValueRank = ValueRanks.OneDimension Then
                    AddDataGridViewArrayRow(sourceId, index)
                Else
                    label_time_spend.Text = CInt((DateTime.Now - dateTimeStart).TotalMilliseconds) & " ms"
                    AddDataGridViewNewRow(m_OpcUaClient.ReadOneNodeFiveAttributes(New List(Of NodeId)() From {
                        sourceId
                    }), 0, index, sourceId)
                    index += 1
                End If

            Catch exception As Exception
                ClientUtils.HandleException(Text, exception)
                Return
            End Try
        End If

        ClearDataGridViewRows(index)
    End Sub

    Private Sub AddDataGridViewNewRow(ByVal dataValues As DataValue(), ByVal startIndex As Integer, ByVal index As Integer, ByVal nodeId As NodeId)
        While index >= NodeDetailsGridView.Rows.Count
            NodeDetailsGridView.Rows.Add()
        End While

        Dim dgvr As DataGridViewRow = NodeDetailsGridView.Rows(index)
        dgvr.Tag = nodeId
        If dataValues(startIndex).WrappedValue.Value Is Nothing Then Return
        Dim nodeclass As NodeClass = CType(dataValues(startIndex).WrappedValue.Value, NodeClass)
        dgvr.Cells(1).Value = dataValues(3 + startIndex).WrappedValue.Value
        dgvr.Cells(5).Value = dataValues(4 + startIndex).WrappedValue.Value
        dgvr.Cells(4).Value = GetDiscriptionFromAccessLevel(dataValues(2 + startIndex))

        If nodeclass = NodeClass.Object Then
            dgvr.Cells(0).Value = My.Resources.ClassIcon
            dgvr.Cells(2).Value = ""
            dgvr.Cells(3).Value = nodeclass.ToString()
        ElseIf nodeclass = NodeClass.Method Then
            dgvr.Cells(0).Value = My.Resources.Method_636
            dgvr.Cells(2).Value = ""
            dgvr.Cells(3).Value = nodeclass.ToString()
        ElseIf nodeclass = NodeClass.Variable Then
            Dim dataValue As DataValue = dataValues(1 + startIndex)

            If dataValue.WrappedValue.TypeInfo IsNot Nothing Then
                dgvr.Cells(3).Value = dataValue.WrappedValue.TypeInfo.BuiltInType

                If dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.Scalar Then
                    dgvr.Cells(2).Value = dataValue.WrappedValue.Value
                    dgvr.Cells(0).Value = My.Resources.Enum_582
                ElseIf dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.OneDimension Then
                    dgvr.Cells(2).Value = dataValue.Value.[GetType]().ToString()
                    dgvr.Cells(0).Value = My.Resources.brackets_Square_16xMD
                ElseIf dataValue.WrappedValue.TypeInfo.ValueRank = ValueRanks.TwoDimensions Then
                    dgvr.Cells(2).Value = dataValue.Value.[GetType]().ToString()
                    dgvr.Cells(0).Value = My.Resources.Module_648
                Else
                    dgvr.Cells(2).Value = dataValue.Value.[GetType]().ToString()
                    dgvr.Cells(0).Value = My.Resources.ClassIcon
                End If
            Else
                dgvr.Cells(0).Value = My.Resources.ClassIcon
                dgvr.Cells(2).Value = dataValue.Value
                dgvr.Cells(3).Value = "null"
            End If
        Else
            dgvr.Cells(2).Value = ""
            dgvr.Cells(0).Value = My.Resources.ClassIcon
            dgvr.Cells(3).Value = nodeclass.ToString()
        End If
    End Sub

    Private Sub AddDataGridViewArrayRow(ByVal nodeId As NodeId, <Out> ByRef index As Integer)
        Dim dateTimeStart As DateTime = DateTime.Now
        Dim dataValues As DataValue() = m_OpcUaClient.ReadOneNodeFiveAttributes(New List(Of NodeId)() From {
            nodeId
        })
        label_time_spend.Text = CInt((DateTime.Now - dateTimeStart).TotalMilliseconds) & " ms"
        Dim dataValue As DataValue = dataValues(1)

        If dataValue.WrappedValue.TypeInfo?.ValueRank = ValueRanks.OneDimension Then
            Dim access As String = GetDiscriptionFromAccessLevel(dataValues(2))
            Dim type As BuiltInType = dataValue.WrappedValue.TypeInfo.BuiltInType
            Dim des As Object = If(dataValues(4).Value, "")
            Dim dis As Object = If(dataValues(3).Value, type)
            Dim array As Array = TryCast(dataValue.Value, Array)
            Dim i As Integer = 0

            For Each obj As Object In array

                While i >= NodeDetailsGridView.Rows.Count
                    NodeDetailsGridView.Rows.Add()
                End While

                Dim dgvr As DataGridViewRow = NodeDetailsGridView.Rows(i)
                dgvr.Tag = Nothing
                dgvr.Cells(0).Value = My.Resources.Enum_582
                dgvr.Cells(1).Value = $"{dis} [{Math.Min(System.Threading.Interlocked.Increment(i), i - 1)}]"
                dgvr.Cells(2).Value = obj
                dgvr.Cells(3).Value = type
                dgvr.Cells(4).Value = access
                dgvr.Cells(5).Value = des
            Next

            index = i
        Else
            index = 0
        End If
    End Sub

    Private Function GetDiscriptionFromAccessLevel(ByVal value As DataValue) As String
        If value.WrappedValue.Value IsNot Nothing Then

            Select Case CByte(value.WrappedValue.Value)
                Case 0
                    Return "None"
                Case 1
                    Return "CurrentRead"
                Case 2
                    Return "CurrentWrite"
                Case 3
                    Return "CurrentReadOrWrite"
                Case 4
                    Return "HistoryRead"
                Case 8
                    Return "HistoryWrite"
                Case 12
                    Return "HistoryReadOrWrite"
                Case 16
                    Return "SemanticChange"
                Case 32
                    Return "StatusWrite"
                Case 64
                    Return "TimestampWrite"
                Case Else
                    Return "None"
            End Select
        Else
            Return "null"
        End If
    End Function

    Private subNodeIds As New List(Of String)()
    Private isSingleValueSub As Boolean = False

    Private Sub RemoveAllSubscript()
        m_OpcUaClient?.RemoveAllSubscription()
    End Sub

    Private Sub SubCallBack(ByVal key As String, ByVal monitoredItem As MonitoredItem, ByVal eventArgs As MonitoredItemNotificationEventArgs)
        If InvokeRequired Then
            Invoke(New Action(Of String, MonitoredItem, MonitoredItemNotificationEventArgs)(AddressOf SubCallBack), key, monitoredItem, eventArgs)
            Return
        End If

        Dim notification As MonitoredItemNotification = TryCast(eventArgs.NotificationValue, MonitoredItemNotification)
        Dim nodeId As String = monitoredItem.StartNodeId.ToString()
        Dim index As Integer = subNodeIds.IndexOf(nodeId)

        If index >= 0 Then

            If isSingleValueSub Then

                If notification.Value.WrappedValue.TypeInfo?.ValueRank = ValueRanks.OneDimension Then
                    Dim array As Array = TryCast(notification.Value.WrappedValue.Value, Array)
                    Dim i As Integer = 0

                    For Each obj As Object In array
                        Dim dgvr As DataGridViewRow = NodeDetailsGridView.Rows(i)
                        dgvr.Cells(2).Value = obj
                        i += 1
                    Next
                Else
                    NodeDetailsGridView.Rows(index).Cells(2).Value = notification.Value.WrappedValue.Value
                End If
            Else
                NodeDetailsGridView.Rows(index).Cells(2).Value = notification.Value.WrappedValue.Value
            End If
        End If
    End Sub

    Private Async Sub BtnSubscript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSubscript.Click
        If m_OpcUaClient IsNot Nothing Then
            RemoveAllSubscript()

            If BtnSubscript.BackColor <> Color.LimeGreen Then
                BtnSubscript.BackColor = Color.LimeGreen
                If String.IsNullOrEmpty(textBox_nodeId.Text) Then Return
                Dim references As ReferenceDescriptionCollection

                Try
                    references = Await Task.Run(Function() GetReferenceDescriptionCollection(New NodeId(textBox_nodeId.Text)))
                Catch exception As Exception
                    ClientUtils.HandleException(Text, exception)
                    Return
                End Try

                subNodeIds = New List(Of String)()

                If references?.Count > 0 Then
                    isSingleValueSub = False
                    For ii As Integer = 0 To references.Count - 1
                        Dim target As ReferenceDescription = references(ii)
                        subNodeIds.Add((CType(target.NodeId, NodeId)).ToString())
                    Next
                Else
                    isSingleValueSub = True
                    subNodeIds.Add(textBox_nodeId.Text)
                End If
                m_OpcUaClient.AddSubscription("Subscription", subNodeIds.ToArray(), AddressOf SubCallBack)
            Else
                BtnSubscript.BackColor = SystemColors.Control
            End If
        End If
    End Sub


    Private Sub NodeDetailsGridView_CellEndEdit(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles NodeDetailsGridView.CellEndEdit
        Dim builtInType As BuiltInType = Nothing, nodeId As NodeId

        If [Enum].TryParse(Of BuiltInType)(Me.NodeDetailsGridView.Rows(e.RowIndex).Cells(e.ColumnIndex + 1).Value, builtInType) Then
            Dim value As Object = Nothing
            nodeId = (TryCast(Me.NodeDetailsGridView.Rows(e.RowIndex).Tag, NodeId))
            If nodeId IsNot Nothing Then

                Try
                    Value = GetValueFromString(NodeDetailsGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString(), builtInType)
                Catch
                    MicsonControlExt.MessageBoxExt.Show(Nothing, "Invalid Input Value")
                    Return
                End Try

                If Not m_OpcUaClient.WriteNode(nodeId.ToString(), Value) Then
                    MicsonControlExt.MessageBoxExt.Show(Nothing, "Failed to write value")
                End If
            Else
                Dim list As New List(Of String)()

                For jj As Integer = 0 To NodeDetailsGridView.RowCount - 1
                    list.Add(NodeDetailsGridView.Rows(jj).Cells(e.ColumnIndex).Value.ToString())
                Next

                Try
                    Value = GetArrayValueFromString(list, builtInType)
                Catch ex As Exception
                    MicsonControlExt.MessageBoxExt.Show(Nothing, "Invalid Input Value: " & ex.Message & vbCrLf & ex.StackTrace)
                    Return
                End Try

                If Not m_OpcUaClient.WriteNode(textBox_nodeId.Text, Value) Then
                    MicsonControlExt.MessageBoxExt.Show(Nothing, "Failed to write value")
                End If
            End If
        Else
            MicsonControlExt.MessageBoxExt.Show(Nothing, "Invalid data type")
        End If
    End Sub

    Private Function GetValueFromString(ByVal value As String, ByVal builtInType As BuiltInType) As Object
        Select Case builtInType
            Case BuiltInType.Boolean
                Return Boolean.Parse(value)
            Case BuiltInType.Byte
                Return Byte.Parse(value)
            Case BuiltInType.DateTime
                Return DateTime.Parse(value)
            Case BuiltInType.Double
                Return Double.Parse(value)
            Case BuiltInType.Float
                Return Single.Parse(value)
            Case BuiltInType.Guid
                Return Guid.Parse(value)
            Case BuiltInType.Int16
                Return Short.Parse(value)
            Case BuiltInType.Int32
                Return Integer.Parse(value)
            Case BuiltInType.Int64
                Return Long.Parse(value)
            Case BuiltInType.Integer
                Return Integer.Parse(value)
            Case BuiltInType.LocalizedText
                Return value
            Case BuiltInType.SByte
                Return SByte.Parse(value)
            Case BuiltInType.String
                Return value
            Case BuiltInType.UInt16
                Return UShort.Parse(value)
            Case BuiltInType.UInt32
                Return UInteger.Parse(value)
            Case BuiltInType.UInt64
                Return ULong.Parse(value)
            Case BuiltInType.UInteger
                Return UInteger.Parse(value)
            Case Else
                Throw New Exception("Not supported data type")
        End Select
    End Function

    Private Function GetArrayValueFromString(ByVal values As IList(Of String), ByVal builtInType As BuiltInType) As Object
        Select Case builtInType
            Case BuiltInType.Boolean
                Dim result As Boolean() = New Boolean(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Boolean.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Byte
                Dim result As Byte() = New Byte(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Byte.Parse(values(i))
                Next

                Return result
            Case BuiltInType.DateTime
                Dim result As DateTime() = New DateTime(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = DateTime.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Double
                Dim result As Double() = New Double(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Double.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Float
                Dim result As Single() = New Single(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Single.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Guid
                Dim result As Guid() = New Guid(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Guid.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Int16
                Dim result As Short() = New Short(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Short.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Int32
                Dim result As Integer() = New Integer(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Integer.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Int64
                Dim result As Long() = New Long(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Long.Parse(values(i))
                Next

                Return result
            Case BuiltInType.Integer
                Dim result As Integer() = New Integer(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = Integer.Parse(values(i))
                Next

                Return result
            Case BuiltInType.LocalizedText
                Return values.ToArray()
            Case BuiltInType.SByte
                Dim result As SByte() = New SByte(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = SByte.Parse(values(i))
                Next

                Return result
            Case BuiltInType.String
                Return values.ToArray()
            Case BuiltInType.UInt16
                Dim result As UShort() = New UShort(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = UShort.Parse(values(i))
                Next

                Return result
            Case BuiltInType.UInt32
                Dim result As UInteger() = New UInteger(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = UInteger.Parse(values(i))
                Next

                Return result
            Case BuiltInType.UInt64
                Dim result As ULong() = New ULong(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = ULong.Parse(values(i))
                Next

                Return result
            Case BuiltInType.UInteger
                Dim result As UInteger() = New UInteger(values.Count - 1) {}

                For i As Integer = 0 To values.Count - 1
                    result(i) = UInteger.Parse(values(i))
                Next

                Return result
            Case Else
                Throw New Exception("Not supported data type")
        End Select
    End Function

    Private Sub NodeDetailsGridView_CellBeginEdit(ByVal sender As Object, ByVal e As DataGridViewCellCancelEventArgs) Handles NodeDetailsGridView.CellBeginEdit
        Dim builtInType As BuiltInType = Nothing

        If [Enum].TryParse(Of BuiltInType)(Me.NodeDetailsGridView.Rows(e.RowIndex).Cells(e.ColumnIndex + 1).Value, builtInType) Then

            If Not (builtInType = BuiltInType.Boolean OrElse builtInType = BuiltInType.Byte OrElse builtInType = BuiltInType.DateTime OrElse builtInType = BuiltInType.Double OrElse builtInType = BuiltInType.Float OrElse builtInType = BuiltInType.Guid OrElse builtInType = BuiltInType.Int16 OrElse builtInType = BuiltInType.Int32 OrElse builtInType = BuiltInType.Int64 OrElse builtInType = BuiltInType.Integer OrElse builtInType = BuiltInType.LocalizedText OrElse builtInType = BuiltInType.SByte OrElse builtInType = BuiltInType.String OrElse builtInType = BuiltInType.UInt16 OrElse builtInType = BuiltInType.UInt32 OrElse builtInType = BuiltInType.UInt64 OrElse builtInType = BuiltInType.UInteger) Then
                e.Cancel = True
                MicsonControlExt.MessageBoxExt.Show(Nothing, "Not support the Type of modify value!")
                Return
            End If
        Else
            e.Cancel = True
            MicsonControlExt.MessageBoxExt.Show(Nothing, "Not support the Type of modify value!")
            Return
        End If

        If Not NodeDetailsGridView.Rows(e.RowIndex).Cells(e.ColumnIndex + 2).Value.ToString().Contains("Write") Then
            e.Cancel = True
            MicsonControlExt.MessageBoxExt.Show(Nothing, "Not support the access of modify value!")
        End If
    End Sub
    Property SelectNodeID As String
    Private Sub SelectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectToolStripMenuItem.Click

        SelectNodeID = textBox_nodeId.Text
        Close()
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelToolStripMenuItem.Click
        Close()
        Me.DialogResult = DialogResult.Cancel
    End Sub

    'Private Async Sub FormBrowseServer_Shown(sender As Object, e As EventArgs) Handles Me.Shown
    '    If AutoConnect Then
    '        m_OpcUaClient.UserIdentity = UserIdentity
    '        Await m_OpcUaClient.ConnectServer(txtServerUrl.Text)
    '        'BtnConnect.BackColor = Color.LimeGreen
    '    End If

    'End Sub
End Class


