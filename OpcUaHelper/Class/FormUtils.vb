Imports Opc.Ua
Imports Opc.Ua.Client
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks


    Public Class FormUtils
        Private Shared Function GetAccessLevelDisplayText(ByVal accessLevel As Byte) As String
            Dim buffer As  New StringBuilder()

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

        Private Shared Function GetEventNotifierDisplayText(ByVal eventNotifier As Byte) As String
            Dim buffer As  New StringBuilder()

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

        Private Shared Function GetValueRankDisplayText(ByVal valueRank As Integer) As String
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

                    If Not NodeId.IsNull(field) Then
                        Return field.ToString()
                    End If

                    Return "Null"
            End Select

            If TypeOf value.Value Is Byte() Then
                Return Utils.ToHexString(TryCast(value.Value, Byte()))
            End If

            Return value.ToString()
        End Function

        Public Shared Function DiscoverServers(ByVal configuration As ApplicationConfiguration) As IList(Of String)
            Dim serverUrls As New List(Of String)()
            Dim endpointConfiguration As EndpointConfiguration = EndpointConfiguration.Create(configuration)
            endpointConfiguration.OperationTimeout = 5000

            Using client As DiscoveryClient = DiscoveryClient.Create(New Uri("opc.tcp://localhost:4840"), endpointConfiguration)
                Dim servers As ApplicationDescriptionCollection = client.FindServers(Nothing)

                For ii As Integer = 0 To servers.Count - 1

                    If servers(ii).ApplicationType = ApplicationType.DiscoveryServer Then
                        Continue For
                    End If

                    For jj As Integer = 0 To servers(ii).DiscoveryUrls.Count - 1
                        Dim discoveryUrl As String = servers(ii).DiscoveryUrls(jj)

                        If discoveryUrl.EndsWith("/discovery") Then
                            discoveryUrl = discoveryUrl.Substring(0, discoveryUrl.Length - "/discovery".Length)
                        End If

                        If Not serverUrls.Contains(discoveryUrl) Then
                            serverUrls.Add(discoveryUrl)
                        End If
                    Next
                Next
            End Using

            Return serverUrls
        End Function

        Public Shared Function SelectEndpoint(ByVal discoveryUrl As String, ByVal useSecurity As Boolean) As EndpointDescription
            If Not discoveryUrl.StartsWith(Utils.UriSchemeOpcTcp) Then

                If Not discoveryUrl.EndsWith("/discovery") Then
                    discoveryUrl += "/discovery"
                End If
            End If

            Dim uri As Uri = New Uri(discoveryUrl)
            Dim configuration As EndpointConfiguration = EndpointConfiguration.Create()
            configuration.OperationTimeout = 5000
            Dim selectedEndpoint As EndpointDescription = Nothing

            Using client As DiscoveryClient = DiscoveryClient.Create(uri, configuration)
                Dim endpoints As EndpointDescriptionCollection = client.GetEndpoints(Nothing)

                For ii As Integer = 0 To endpoints.Count - 1
                    Dim endpoint As EndpointDescription = endpoints(ii)

                    If endpoint.EndpointUrl.StartsWith(uri.Scheme) Then

                        If useSecurity Then

                            If endpoint.SecurityMode = MessageSecurityMode.None Then
                                Continue For
                            End If
                        Else

                            If endpoint.SecurityMode <> MessageSecurityMode.None Then
                                Continue For
                            End If
                        End If

                        If selectedEndpoint Is Nothing Then
                            selectedEndpoint = endpoint
                        End If

                        If endpoint.SecurityLevel > selectedEndpoint.SecurityLevel Then
                            selectedEndpoint = endpoint
                        End If
                    End If
                Next

                If selectedEndpoint Is Nothing AndAlso endpoints.Count > 0 Then
                    selectedEndpoint = endpoints(0)
                End If
            End Using

            Dim endpointUrl As Uri = Utils.ParseUri(selectedEndpoint.EndpointUrl)

            If endpointUrl IsNot Nothing AndAlso endpointUrl.Scheme = uri.Scheme Then
                Dim builder As UriBuilder = New UriBuilder(endpointUrl)
                builder.Host = uri.DnsSafeHost
                builder.Port = uri.Port
                selectedEndpoint.EndpointUrl = builder.ToString()
            End If

            Return selectedEndpoint
        End Function

        Public Shared Function Browse(ByVal session As Session, ByVal nodesToBrowse As BrowseDescriptionCollection, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
            Try
                Dim references As ReferenceDescriptionCollection = New ReferenceDescriptionCollection()
                Dim unprocessedOperations As BrowseDescriptionCollection = New BrowseDescriptionCollection()

                While nodesToBrowse.Count > 0
                    Dim results As BrowseResultCollection = Nothing
                    Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
                    session.Browse(Nothing, Nothing, 0, nodesToBrowse, results, diagnosticInfos)
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
                        session.BrowseNext(Nothing, True, continuationPoints, results, diagnosticInfos)
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

        Public Shared Function Browse(ByVal session As Session, ByVal nodeToBrowse As BrowseDescription, ByVal throwOnError As Boolean) As ReferenceDescriptionCollection
            Try
                Dim references As ReferenceDescriptionCollection = New ReferenceDescriptionCollection()
                Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()
                nodesToBrowse.Add(nodeToBrowse)
                Dim results As BrowseResultCollection = Nothing
                Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
                session.Browse(Nothing, Nothing, 0, nodesToBrowse, results, diagnosticInfos)
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
                Dim supertypes As ReferenceDescriptionCollection = FormUtils.BrowseSuperTypes(session, eventTypeId, False)

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

        Public Shared Sub CollectFieldsForType(ByVal session As Session, ByVal typeId As NodeId, ByVal fields As SimpleAttributeOperandCollection, ByVal fieldNodeIds As List(Of NodeId))
            Dim supertypes As ReferenceDescriptionCollection = FormUtils.BrowseSuperTypes(session, typeId, False)

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

        Public Shared Sub CollectFieldsForInstance(ByVal session As Session, ByVal instanceId As NodeId, ByVal fields As SimpleAttributeOperandCollection, ByVal fieldNodeIds As List(Of NodeId))
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
            Dim children As ReferenceDescriptionCollection = FormUtils.Browse(session, nodeToBrowse, False)

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

