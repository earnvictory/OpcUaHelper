Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports Opc.Ua
Imports Opc.Ua.Client


Public Class ClientUtils
    Shared Event ShowException(ByVal e As Exception)
    Public Shared Sub HandleException(ByVal caption As String, ByVal e As Exception)
        RaiseEvent ShowException(e)
        ExceptionDlg.Show(caption, e)
    End Sub
    Shared Sub ShowException_Event(ByVal e As Exception)
        RaiseEvent ShowException(e)
    End Sub
    Public Shared Function GetAppIcon() As System.Drawing.Icon
        Try
            Return New Icon("App.ico")
        Catch __unusedException1__ As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetAccessLevelDisplayText(ByVal accessLevel As Byte) As String
        Dim buffer As New StringBuilder()

        If accessLevel = AccessLevels.None Then
            buffer.Append("None")
        End If

        If (accessLevel And AccessLevels.CurrentRead) = AccessLevels.CurrentRead Then
            buffer.Append("Read")
        End If

        If (accessLevel And AccessLevels.CurrentWrite) = AccessLevels.CurrentWrite Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("Write")
        End If

        If (accessLevel And AccessLevels.HistoryRead) = AccessLevels.HistoryRead Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("HistoryRead")
        End If

        If (accessLevel And AccessLevels.HistoryWrite) = AccessLevels.HistoryWrite Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("HistoryWrite")
        End If

        If (accessLevel And AccessLevels.SemanticChange) = AccessLevels.SemanticChange Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("SemanticChange")
        End If

        Return buffer.ToString()
    End Function

    Public Shared Function GetEventNotifierDisplayText(ByVal eventNotifier As Byte) As String
        Dim buffer As New StringBuilder()

        If eventNotifier = EventNotifiers.None Then
            buffer.Append("None")
        End If

        If (eventNotifier And EventNotifiers.SubscribeToEvents) = EventNotifiers.SubscribeToEvents Then
            buffer.Append("Subscribe")
        End If

        If (eventNotifier And EventNotifiers.HistoryRead) = EventNotifiers.HistoryRead Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("HistoryRead")
        End If

        If (eventNotifier And EventNotifiers.HistoryWrite) = EventNotifiers.HistoryWrite Then

            If buffer.Length > 0 Then
                buffer.Append(" | ")
            End If

            buffer.Append("HistoryWrite")
        End If

        Return buffer.ToString()
    End Function

    Public Shared Function GetValueRankDisplayText(ByVal valueRank As Integer) As String
        Select Case valueRank
            Case ValueRanks.Any
                Return "Any"
            Case ValueRanks.Scalar
                Return "Scalar"
            Case ValueRanks.ScalarOrOneDimension
                Return "ScalarOrOneDimension"
            Case ValueRanks.OneOrMoreDimensions
                Return "OneOrMoreDimensions"
            Case ValueRanks.OneDimension
                Return "OneDimension"
            Case ValueRanks.TwoDimensions
                Return "TwoDimensions"
        End Select

        Return valueRank.ToString()
    End Function

    Public Shared Function GetAttributeDisplayText(ByVal session As Session, ByVal attributeId As UInteger, ByVal value As [Variant]) As String
        If value = [Variant].Null Then
            Return String.Empty
        End If

        Select Case attributeId
            Case Opc.Ua.Attributes.AccessLevel, Opc.Ua.Attributes.UserAccessLevel
                Dim field As Byte? = New Byte?(value.Value)

                If field IsNot Nothing Then
                    Return GetAccessLevelDisplayText(field.Value)
                End If

                Exit Select
            Case Opc.Ua.Attributes.EventNotifier
                Dim field As Byte? = New Byte?(value.Value)

                If field IsNot Nothing Then
                    Return GetEventNotifierDisplayText(field.Value)
                End If

                Exit Select
            Case Opc.Ua.Attributes.DataType
                Return session.NodeCache.GetDisplayText(TryCast(value.Value, NodeId))
            Case Opc.Ua.Attributes.ValueRank
                Dim field As Integer? = New Integer?(value.Value)

                If field IsNot Nothing Then
                    Return GetValueRankDisplayText(field.Value)
                End If

                Exit Select
            Case Opc.Ua.Attributes.NodeClass
                Dim field As Integer? = New Integer?(value.Value)

                If field IsNot Nothing Then
                    Return (CType(field.Value, NodeClass)).ToString()
                End If

                Exit Select
            Case Opc.Ua.Attributes.NodeId
                Dim field As NodeId = TryCast(value.Value, NodeId)

                If Not Opc.Ua.NodeId.IsNull(field) Then
                    Return field.ToString()
                End If

                Return "Null"
        End Select

        If TypeOf value.Value Is Byte() Then
            Return Utils.ToHexString(TryCast(value.Value, Byte()))
        End If

        Return value.ToString()
    End Function

    Public Shared Function Browse(ByVal session As Session, ByVal nodesToBrowse As BrowseDescriptionCollection, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
        Return Browse(session, Nothing, nodesToBrowse, throwOnError)
    End Function

    Public Shared Function Browse(ByVal session As Session, ByVal view As ViewDescription, ByVal nodesToBrowse As BrowseDescriptionCollection, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
        Try
            Dim references As ReferenceDescriptionCollection = New ReferenceDescriptionCollection()
            Dim unprocessedOperations As BrowseDescriptionCollection = New BrowseDescriptionCollection()

            While nodesToBrowse.Count > 0
                Dim results As BrowseResultCollection = Nothing
                Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
                session.Browse(Nothing, view, 0, nodesToBrowse, results, diagnosticInfos)
                ClientBase.ValidateResponse(results, nodesToBrowse)
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse)
                Dim continuationPoints As ByteStringCollection = New ByteStringCollection()

                For ii As Integer = 0 To nodesToBrowse.Count - 1

                    If StatusCode.IsBad(results(ii).StatusCode) Then

                        If results(ii).StatusCode = StatusCodes.BadNoContinuationPoints Then
                            unprocessedOperations.Add(nodesToBrowse(ii))
                        End If

                        Continue For
                    End If

                    If results(ii).References.Count = 0 Then
                        Continue For
                    End If

                    references.AddRange(results(ii).References)

                    If results(ii).ContinuationPoint IsNot Nothing Then
                        continuationPoints.Add(results(ii).ContinuationPoint)
                    End If
                Next

                Dim revisedContiuationPoints As ByteStringCollection = New ByteStringCollection()

                While continuationPoints.Count > 0
                    session.BrowseNext(Nothing, False, continuationPoints, results, diagnosticInfos)
                    ClientBase.ValidateResponse(results, continuationPoints)
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints)

                    For ii As Integer = 0 To continuationPoints.Count - 1

                        If StatusCode.IsBad(results(ii).StatusCode) Then
                            Continue For
                        End If

                        If results(ii).References.Count = 0 Then
                            Continue For
                        End If

                        references.AddRange(results(ii).References)

                        If results(ii).ContinuationPoint IsNot Nothing Then
                            revisedContiuationPoints.Add(results(ii).ContinuationPoint)
                        End If
                    Next

                    revisedContiuationPoints = continuationPoints
                End While

                nodesToBrowse = unprocessedOperations
            End While

            Return references
        Catch exception As Exception

            If throwOnError Then
                Throw New ServiceResultException(exception, StatusCodes.BadUnexpectedError)
            End If

            Return Nothing
        End Try
    End Function

    Public Shared Function Browse(ByVal session As Session, ByVal nodeToBrowse As BrowseDescription, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
        Return Browse(session, Nothing, nodeToBrowse, throwOnError)
    End Function

    Public Shared Function Browse(ByVal session As Session, ByVal view As ViewDescription, ByVal nodeToBrowse As BrowseDescription, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
        Try
            Dim references As ReferenceDescriptionCollection = New ReferenceDescriptionCollection()
            Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()
            nodesToBrowse.Add(nodeToBrowse)
            Dim results As BrowseResultCollection = Nothing
            Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
            session.Browse(Nothing, view, 0, nodesToBrowse, results, diagnosticInfos)
            ClientBase.ValidateResponse(results, nodesToBrowse)
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse)

            Do

                If StatusCode.IsBad(results(0).StatusCode) Then
                    Throw New ServiceResultException(results(0).StatusCode)
                End If

                For ii As Integer = 0 To results(0).References.Count - 1
                    references.Add(results(0).References(ii))
                Next

                If results(0).References.Count = 0 OrElse results(0).ContinuationPoint Is Nothing Then
                    Exit Do
                End If

                Dim continuationPoints As ByteStringCollection = New ByteStringCollection()
                continuationPoints.Add(results(0).ContinuationPoint)
                session.BrowseNext(Nothing, False, continuationPoints, results, diagnosticInfos)
                ClientBase.ValidateResponse(results, continuationPoints)
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints)
            Loop While True

            Return references
        Catch exception As Exception

            If throwOnError Then
                Throw New ServiceResultException(exception, StatusCodes.BadUnexpectedError)
            End If

            Return Nothing
        End Try
    End Function

    Public Shared Function BrowseSuperTypes(ByVal session As Session, ByVal typeId As NodeId, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
        Dim supertypes As ReferenceDescriptionCollection = New ReferenceDescriptionCollection()

        Try
            Dim nodeToBrowse As BrowseDescription = New BrowseDescription()
            nodeToBrowse.NodeId = typeId
            nodeToBrowse.BrowseDirection = BrowseDirection.Inverse
            nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.HasSubtype
            nodeToBrowse.IncludeSubtypes = False
            nodeToBrowse.NodeClassMask = 0
            nodeToBrowse.ResultMask = CUInt(BrowseResultMask.All)
            Dim references As ReferenceDescriptionCollection = Browse(session, nodeToBrowse, throwOnError)

            While references IsNot Nothing AndAlso references.Count > 0
                supertypes.Add(references(0))

                If references(0).NodeId.IsAbsolute Then
                    Exit While
                End If

                nodeToBrowse.NodeId = CType(references(0).NodeId, NodeId)
                references = Browse(session, nodeToBrowse, throwOnError)
            End While

            Return supertypes
        Catch exception As Exception

            If throwOnError Then
                Throw New ServiceResultException(exception, StatusCodes.BadUnexpectedError)
            End If

            Return Nothing
        End Try
    End Function

    Public Shared Function TranslateBrowsePaths(ByVal session As Session, ByVal startNodeId As NodeId, ByVal namespacesUris As NamespaceTable, ParamArray relativePaths As String()) As List(Of NodeId)
        Dim browsePaths As BrowsePathCollection = New BrowsePathCollection()

        If relativePaths IsNot Nothing Then

            For ii As Integer = 0 To relativePaths.Length - 1
                Dim browsePath As BrowsePath = New BrowsePath()
                browsePath.RelativePath = RelativePath.Parse(relativePaths(ii), session.TypeTree, namespacesUris, session.NamespaceUris)
                browsePath.StartingNode = startNodeId
                browsePaths.Add(browsePath)
            Next
        End If

        Dim results As BrowsePathResultCollection = Nothing
        Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
        Dim responseHeader As ResponseHeader = session.TranslateBrowsePathsToNodeIds(Nothing, browsePaths, results, diagnosticInfos)
        Session.ValidateResponse(results, browsePaths)
        Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths)
        Dim nodes As List(Of NodeId) = New List(Of NodeId)()

        For ii As Integer = 0 To results.Count - 1

            If StatusCode.IsBad(results(ii).StatusCode) Then
                nodes.Add(Nothing)
                Continue For
            End If

            If results(ii).Targets.Count = 0 Then
                nodes.Add(Nothing)
                Continue For
            End If

            Dim target As BrowsePathTarget = results(ii).Targets(0)

            If target.RemainingPathIndex <> UInt32.MaxValue Then
                nodes.Add(Nothing)
                Continue For
            End If

            nodes.Add(ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris))
        Next

        Return nodes
    End Function

    Public Shared Function FindEventType(ByVal monitoredItem As MonitoredItem, ByVal notification As EventFieldList) As NodeId
        Dim filter As EventFilter = TryCast(monitoredItem.Status.Filter, EventFilter)

        If filter IsNot Nothing Then

            For ii As Integer = 0 To filter.SelectClauses.Count - 1
                Dim clause As SimpleAttributeOperand = filter.SelectClauses(ii)

                If clause.BrowsePath.Count = 1 AndAlso clause.BrowsePath(0) = BrowseNames.EventType Then
                    Return TryCast(notification.EventFields(ii).Value, NodeId)
                End If
            Next
        End If

        Return Nothing
    End Function

    Public Shared Function ConstructEvent(ByVal session As Session, ByVal monitoredItem As MonitoredItem, ByVal notification As EventFieldList, ByVal knownEventTypes As Dictionary(Of NodeId, Type), ByVal eventTypeMappings As Dictionary(Of NodeId, NodeId)) As BaseEventState
        Dim eventTypeId As NodeId = FindEventType(monitoredItem, notification)

        If eventTypeId Is Nothing Then
            Return Nothing
        End If

        Dim knownType As Type = Nothing
        Dim knownTypeId As NodeId = Nothing

        If eventTypeMappings.TryGetValue(eventTypeId, knownTypeId) Then
            knownType = knownEventTypes(knownTypeId)
        End If

        If knownType Is Nothing Then

            If knownEventTypes.TryGetValue(eventTypeId, knownType) Then
                knownTypeId = eventTypeId
                eventTypeMappings.Add(eventTypeId, eventTypeId)
            End If
        End If

        If knownType Is Nothing Then
            Dim supertypes As ReferenceDescriptionCollection = ClientUtils.BrowseSuperTypes(session, eventTypeId, False)

            If supertypes Is Nothing Then
                Return Nothing
            End If

            For ii As Integer = 0 To supertypes.Count - 1
                Dim superTypeId As NodeId = CType(supertypes(ii).NodeId, NodeId)

                If knownEventTypes.TryGetValue(superTypeId, knownType) Then
                    knownTypeId = superTypeId
                    eventTypeMappings.Add(eventTypeId, superTypeId)
                End If

                If knownTypeId IsNot Nothing Then
                    Exit For
                End If
            Next

            If knownTypeId Is Nothing Then
                Return Nothing
            End If
        End If

        Dim e As BaseEventState = CType(Activator.CreateInstance(knownType, New Object() {CType(Nothing, NodeState)}), BaseEventState)
        Dim filter As EventFilter = TryCast(monitoredItem.Status.Filter, EventFilter)
        e.Update(session.SystemContext, filter.SelectClauses, notification)
        e.Handle = notification
        Return e
    End Function

    Public Shared Function CollectInstanceDeclarationsForType(ByVal session As Session, ByVal typeId As NodeId) As List(Of InstanceDeclaration)
        Return CollectInstanceDeclarationsForType(session, typeId, True)
    End Function

    Public Shared Function CollectInstanceDeclarationsForType(ByVal session As Session, ByVal typeId As NodeId, ByVal includeSupertypes As Boolean) As List(Of InstanceDeclaration)
        Dim instances As List(Of InstanceDeclaration) = New List(Of InstanceDeclaration)()
        Dim map As Dictionary(Of String, InstanceDeclaration) = New Dictionary(Of String, InstanceDeclaration)()

        If includeSupertypes Then
            Dim supertypes As ReferenceDescriptionCollection = ClientUtils.BrowseSuperTypes(session, typeId, False)

            If supertypes IsNot Nothing Then

                For ii As Integer = supertypes.Count - 1 To 0
                    CollectInstanceDeclarations(session, CType(supertypes(ii).NodeId, NodeId), Nothing, instances, map)
                Next
            End If
        End If

        CollectInstanceDeclarations(session, typeId, Nothing, instances, map)
        Return instances
    End Function

    Private Shared Sub CollectInstanceDeclarations(ByVal session As Session, ByVal typeId As NodeId, ByVal parent As InstanceDeclaration, ByVal instances As List(Of InstanceDeclaration), ByVal map As IDictionary(Of String, InstanceDeclaration))
        Dim nodeToBrowse As BrowseDescription = New BrowseDescription()

        If parent Is Nothing Then
            nodeToBrowse.NodeId = typeId
        Else
            nodeToBrowse.NodeId = parent.NodeId
        End If

        nodeToBrowse.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.HasChild
        nodeToBrowse.IncludeSubtypes = True
        nodeToBrowse.NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable Or NodeClass.Method))
        nodeToBrowse.ResultMask = CUInt(BrowseResultMask.All)
        Dim references As ReferenceDescriptionCollection = ClientUtils.Browse(session, nodeToBrowse, False)

        If references Is Nothing Then
            Return
        End If

        Dim nodeIds As List(Of NodeId) = New List(Of NodeId)()
        Dim children As List(Of InstanceDeclaration) = New List(Of InstanceDeclaration)()

        For ii As Integer = 0 To references.Count - 1
            Dim reference As ReferenceDescription = references(ii)

            If reference.NodeId.IsAbsolute Then
                Continue For
            End If

            Dim child As InstanceDeclaration = New InstanceDeclaration()
            child.RootTypeId = typeId
            child.NodeId = CType(reference.NodeId, NodeId)
            child.BrowseName = reference.BrowseName
            child.NodeClass = reference.NodeClass

            If Not LocalizedText.IsNullOrEmpty(reference.DisplayName) Then
                child.DisplayName = reference.DisplayName.Text
            Else
                child.DisplayName = reference.BrowseName.Name
            End If

            If parent IsNot Nothing Then
                child.BrowsePath = New QualifiedNameCollection(parent.BrowsePath)
                child.BrowsePathDisplayText = Utils.Format("{0}/{1}", parent.BrowsePathDisplayText, reference.BrowseName)
                child.DisplayPath = Utils.Format("{0}/{1}", parent.DisplayPath, reference.DisplayName)
            Else
                child.BrowsePath = New QualifiedNameCollection()
                child.BrowsePathDisplayText = Utils.Format("{0}", reference.BrowseName)
                child.DisplayPath = Utils.Format("{0}", reference.DisplayName)
            End If

            child.BrowsePath.Add(reference.BrowseName)
            Dim overriden As InstanceDeclaration = Nothing

            If map.TryGetValue(child.BrowsePathDisplayText, overriden) Then
                child.OverriddenDeclaration = overriden
            End If

            map(child.BrowsePathDisplayText) = child
            children.Add(child)
            nodeIds.Add(child.NodeId)
        Next

        If children.Count = 0 Then
            Return
        End If

        Dim modellingRules As List(Of NodeId) = FindTargetOfReference(session, nodeIds, Opc.Ua.ReferenceTypeIds.HasModellingRule, False)

        If modellingRules IsNot Nothing Then

            For ii As Integer = 0 To nodeIds.Count - 1
                children(ii).ModellingRule = modellingRules(ii)

                If NodeId.IsNull(modellingRules(ii)) Then
                    map.Remove(children(ii).BrowsePathDisplayText)
                End If
            Next
        End If

        UpdateInstanceDescriptions(session, children, False)

        For ii As Integer = 0 To children.Count - 1

            If Not NodeId.IsNull(children(ii).ModellingRule) Then
                instances.Add(children(ii))
                CollectInstanceDeclarations(session, typeId, children(ii), instances, map)
            End If
        Next
    End Sub

    Private Shared Function FindTargetOfReference(ByVal session As Session, ByVal nodeIds As List(Of NodeId), ByVal referenceTypeId As NodeId, ByVal throwOnError As Boolean) As List(Of NodeId)
        Try
            Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()

            For ii As Integer = 0 To nodeIds.Count - 1
                Dim nodeToBrowse As BrowseDescription = New BrowseDescription()
                nodeToBrowse.NodeId = nodeIds(ii)
                nodeToBrowse.BrowseDirection = BrowseDirection.Forward
                nodeToBrowse.ReferenceTypeId = referenceTypeId
                nodeToBrowse.IncludeSubtypes = False
                nodeToBrowse.NodeClassMask = 0
                nodeToBrowse.ResultMask = CUInt(BrowseResultMask.None)
                nodesToBrowse.Add(nodeToBrowse)
            Next

            Dim results As BrowseResultCollection = Nothing
            Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
            session.Browse(Nothing, Nothing, 1, nodesToBrowse, results, diagnosticInfos)
            ClientBase.ValidateResponse(results, nodesToBrowse)
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse)
            Dim targetIds As List(Of NodeId) = New List(Of NodeId)()
            Dim continuationPoints As ByteStringCollection = New ByteStringCollection()

            For ii As Integer = 0 To nodeIds.Count - 1
                targetIds.Add(Nothing)

                If StatusCode.IsBad(results(ii).StatusCode) Then
                    Continue For
                End If

                If results(ii).ContinuationPoint IsNot Nothing AndAlso results(ii).ContinuationPoint.Length > 0 Then
                    continuationPoints.Add(results(ii).ContinuationPoint)
                End If

                If results(ii).References.Count > 0 Then

                    If NodeId.IsNull(results(ii).References(0).NodeId) OrElse results(ii).References(0).NodeId.IsAbsolute Then
                        Continue For
                    End If

                    targetIds(ii) = CType(results(ii).References(0).NodeId, NodeId)
                End If
            Next

            If continuationPoints.Count > 0 Then
                session.BrowseNext(Nothing, True, continuationPoints, results, diagnosticInfos)
                ClientBase.ValidateResponse(results, nodesToBrowse)
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse)
            End If

            Return targetIds
        Catch exception As Exception

            If throwOnError Then
                Throw New ServiceResultException(exception, StatusCodes.BadUnexpectedError)
            End If

            Return Nothing
        End Try
    End Function

    Private Shared Sub UpdateInstanceDescriptions(ByVal session As Session, ByVal instances As List(Of InstanceDeclaration), ByVal throwOnError As Boolean)
        Try
            Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

            For ii As Integer = 0 To instances.Count - 1
                Dim nodeToRead As ReadValueId = New ReadValueId()
                nodeToRead.NodeId = instances(ii).NodeId
                nodeToRead.AttributeId = Opc.Ua.Attributes.Description
                nodesToRead.Add(nodeToRead)
                nodeToRead = New ReadValueId()
                nodeToRead.NodeId = instances(ii).NodeId
                nodeToRead.AttributeId = Opc.Ua.Attributes.DataType
                nodesToRead.Add(nodeToRead)
                nodeToRead = New ReadValueId()
                nodeToRead.NodeId = instances(ii).NodeId
                nodeToRead.AttributeId = Opc.Ua.Attributes.ValueRank
                nodesToRead.Add(nodeToRead)
            Next

            Dim results As DataValueCollection = Nothing
            Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
            session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
            ClientBase.ValidateResponse(results, nodesToRead)
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)

            For ii As Integer = 0 To nodesToRead.Count - 1 Step 3
                Dim instance As InstanceDeclaration = instances(ii / 3)
                instance.Description = results(ii).GetValue(Of LocalizedText)(LocalizedText.Null).Text
                instance.DataType = results(ii + 1).GetValue(Of NodeId)(NodeId.Null)
                instance.ValueRank = results(ii + 2).GetValue(Of Integer)(ValueRanks.Any)

                If Not NodeId.IsNull(instance.DataType) Then
                    instance.BuiltInType = DataTypes.GetBuiltInType(instance.DataType, session.TypeTree)
                    instance.DataTypeDisplayText = session.NodeCache.GetDisplayText(instance.DataType)

                    If instance.ValueRank >= 0 Then
                        instance.DataTypeDisplayText += "[]"
                    End If
                End If
            Next

        Catch exception As Exception

            If throwOnError Then
                Throw New ServiceResultException(exception, StatusCodes.BadUnexpectedError)
            End If
        End Try
    End Sub

    Private Shared Sub CollectFieldsForType(ByVal session As Session, ByVal typeId As NodeId, ByVal fields As SimpleAttributeOperandCollection, ByVal fieldNodeIds As List(Of NodeId))
        Dim supertypes As ReferenceDescriptionCollection = ClientUtils.BrowseSuperTypes(session, typeId, False)

        If supertypes Is Nothing Then
            Return
        End If

        Dim foundNodes As Dictionary(Of NodeId, QualifiedNameCollection) = New Dictionary(Of NodeId, QualifiedNameCollection)()
        Dim parentPath As QualifiedNameCollection = New QualifiedNameCollection()

        For ii As Integer = supertypes.Count - 1 To 0
            CollectFields(session, CType(supertypes(ii).NodeId, NodeId), parentPath, fields, fieldNodeIds, foundNodes)
        Next

        CollectFields(session, typeId, parentPath, fields, fieldNodeIds, foundNodes)
    End Sub

    Private Shared Sub CollectFieldsForInstance(ByVal session As Session, ByVal instanceId As NodeId, ByVal fields As SimpleAttributeOperandCollection, ByVal fieldNodeIds As List(Of NodeId))
        Dim foundNodes As Dictionary(Of NodeId, QualifiedNameCollection) = New Dictionary(Of NodeId, QualifiedNameCollection)()
        Dim parentPath As QualifiedNameCollection = New QualifiedNameCollection()
        CollectFields(session, instanceId, parentPath, fields, fieldNodeIds, foundNodes)
    End Sub

    Private Shared Sub CollectFields(ByVal session As Session, ByVal nodeId As NodeId, ByVal parentPath As QualifiedNameCollection, ByVal fields As SimpleAttributeOperandCollection, ByVal fieldNodeIds As List(Of NodeId), ByVal foundNodes As Dictionary(Of NodeId, QualifiedNameCollection))
        Dim nodeToBrowse As BrowseDescription = New BrowseDescription()
        nodeToBrowse.NodeId = nodeId
        nodeToBrowse.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.Aggregates
        nodeToBrowse.IncludeSubtypes = True
        nodeToBrowse.NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable))
        nodeToBrowse.ResultMask = CUInt(BrowseResultMask.All)
        Dim children As ReferenceDescriptionCollection = ClientUtils.Browse(session, nodeToBrowse, False)

        If children Is Nothing Then
            Return
        End If

        For ii As Integer = 0 To children.Count - 1
            Dim child As ReferenceDescription = children(ii)

            If child.NodeId.IsAbsolute Then
                Continue For
            End If

            Dim browsePath As QualifiedNameCollection = New QualifiedNameCollection(parentPath)
            browsePath.Add(child.BrowseName)
            Dim index As Integer = ContainsPath(fields, browsePath)

            If index < 0 Then
                Dim field As SimpleAttributeOperand = New SimpleAttributeOperand()
                field.TypeDefinitionId = ObjectTypeIds.BaseEventType
                field.BrowsePath = browsePath
                field.AttributeId = If((child.NodeClass = NodeClass.Variable), Opc.Ua.Attributes.Value, Opc.Ua.Attributes.NodeId)
                fields.Add(field)
                fieldNodeIds.Add(CType(child.NodeId, NodeId))
            End If

            Dim targetId As NodeId = CType(child.NodeId, NodeId)

            If Not foundNodes.ContainsKey(targetId) Then
                foundNodes.Add(targetId, browsePath)
                CollectFields(session, CType(child.NodeId, NodeId), browsePath, fields, fieldNodeIds, foundNodes)
            End If
        Next
    End Sub

    Private Shared Function ContainsPath(ByVal selectClause As SimpleAttributeOperandCollection, ByVal browsePath As QualifiedNameCollection) As Integer
        For ii As Integer = 0 To selectClause.Count - 1
            Dim field As SimpleAttributeOperand = selectClause(ii)

            If field.BrowsePath.Count <> browsePath.Count Then
                Continue For
            End If

            Dim match As Boolean = True

            For jj As Integer = 0 To field.BrowsePath.Count - 1

                If field.BrowsePath(jj) <> browsePath(jj) Then
                    match = False
                    Exit For
                End If
            Next

            If match Then
                Return ii
            End If
        Next

        Return -1
    End Function
End Class

