Imports Microsoft.VisualBasic.ApplicationServices
Imports Opc.Ua
Imports Opc.Ua.Client

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Threading.Tasks
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel


Public Class OpcUaClient
    Implements IDisposable

    Private m_configuration As ApplicationConfiguration
    Private m_session As Session
    Private m_IsConnected As Boolean
    Private m_reconnectPeriod As Integer = 10
    Private m_useSecurity As Boolean
    Private m_reConnectHandler As SessionReconnectHandler
    Private dic_subscriptions As Dictionary(Of String, Subscription)
    Private disposedValue As Boolean



    Event KeepAliveComplete As EventHandler
    Event ReconnectStarting As EventHandler
    Event ReconnectComplete As EventHandler
    Event ConnectComplete As EventHandler
    Event OpcStatusChange As EventHandler(Of OpcUaStatusEventArgs)

    Public Sub New()




        dic_subscriptions = New Dictionary(Of String, Subscription)()
        Dim certificateValidator = New CertificateValidator()
        AddHandler certificateValidator.CertificateValidation, Sub(sender, eventArgs)

                                                                   If ServiceResult.IsGood(eventArgs.[Error]) Then
                                                                       eventArgs.Accept = True
                                                                   ElseIf eventArgs.[Error].StatusCode.Code = StatusCodes.BadCertificateUntrusted Then
                                                                       eventArgs.Accept = True
                                                                   Else
                                                                       Throw New Exception(String.Format("Failed to validate certificate with error code {0}: {1}", eventArgs.[Error].Code, eventArgs.[Error].AdditionalInfo))
                                                                   End If
                                                               End Sub

        Dim securityConfigurationcv As SecurityConfiguration = New SecurityConfiguration With {
                                    .AutoAcceptUntrustedCertificates = True,
                                    .RejectSHA1SignedCertificates = False,
                                    .MinimumCertificateKeySize = 1024}
        certificateValidator.Update(securityConfigurationcv)
        Dim configuration = New ApplicationConfiguration With {
                                    .ApplicationName = OpcUaName,
                                    .ApplicationType = ApplicationType.Client,
                                    .CertificateValidator = certificateValidator,
                                    .ApplicationUri = "urn:MicsonClient",
                                    .ProductUri = "MicsonClient",
                                    .ServerConfiguration = New ServerConfiguration With {
                                        .MaxSubscriptionCount = 100000,
                                        .MaxMessageQueueSize = 1000000,
                                        .MaxNotificationQueueSize = 1000000,
                                        .MaxPublishRequestCount = 10000000
                                    },
                                    .SecurityConfiguration = New SecurityConfiguration With {
                                        .AutoAcceptUntrustedCertificates = True,
                                        .RejectSHA1SignedCertificates = False,
                                        .MinimumCertificateKeySize = 1024,
                                        .SuppressNonceValidationErrors = True,
                                        .ApplicationCertificate = New CertificateIdentifier With {
                                            .StoreType = CertificateStoreType.X509Store,
                                            .StorePath = "CurrentUser\My",
                                            .SubjectName = OpcUaName
                                        },
                                        .TrustedIssuerCertificates = New CertificateTrustList With {
                                            .StoreType = CertificateStoreType.X509Store,
                                            .StorePath = "CurrentUser\Root"
                                        },
                                        .TrustedPeerCertificates = New CertificateTrustList With {
                                            .StoreType = CertificateStoreType.X509Store,
                                            .StorePath = "CurrentUser\Root"
                                        }
                                    },
                                    .TransportQuotas = New TransportQuotas With {
                                        .OperationTimeout = 6000000,
                                        .MaxStringLength = Integer.MaxValue,
                                        .MaxByteStringLength = Integer.MaxValue,
                                        .MaxArrayLength = 65535,
                                        .MaxMessageSize = 419430400,
                                        .MaxBufferSize = 65535,
                                        .ChannelLifetime = -1,
                                        .SecurityTokenLifetime = -1
                                    },
                                    .ClientConfiguration = New ClientConfiguration With {
                                        .DefaultSessionTimeout = -1,
                                        .MinSubscriptionLifetime = -1
                                    },
                                    .DisableHiResClock = True
                                }
        configuration.Validate(ApplicationType.Client).GetAwaiter.GetResult()
        m_configuration = configuration
    End Sub
    Property Url As String
    Public Async Function ConnectServer(ByVal serverUrl As String, Optional user As String = Nothing, Optional password As String = "") As Task(Of Boolean)
        Url = serverUrl
        If user?.Length = 0 Then
            UserIdentity = New Opc.Ua.UserIdentity(New Opc.Ua.AnonymousIdentityToken())
        Else
            UserIdentity = New UserIdentity(user, password)
        End If
        m_session = Await Connect(serverUrl)
        If m_session IsNot Nothing Then
            If DefaultSubscription IsNot Nothing Then
                m_session.RemoveSubscription(DefaultSubscription)
            End If
            DefaultSubscription = New Subscription(m_session.DefaultSubscription)
            m_session.AddSubscription(DefaultSubscription)
            DefaultSubscription.Create()
            DefaultSubscription.PublishingEnabled = True
            DefaultSubscription.PublishingInterval = 0
            DefaultSubscription.KeepAliveCount = UInteger.MaxValue
            DefaultSubscription.LifetimeCount = UInteger.MaxValue
            DefaultSubscription.MaxNotificationsPerPublish = UInteger.MaxValue
            DefaultSubscription.Priority = 100
            DefaultSubscription.DisplayName = "DefaultSubscription"
            Return True
        End If
        Return False
    End Function

    Private Async Function Connect(ByVal serverUrl As String) As Task(Of Session)
        Disconnect()
        Try
            If m_configuration Is Nothing Then
                Throw New ArgumentNullException("_configuration")
            End If
            If UserIdentity.DisplayName = "Anonymous" Then
                UseSecurity = False
            Else
                UseSecurity = True
            End If
            Dim endpoint As ConfiguredEndpoint = Nothing
            Dim t As Task(Of Exception) = Task.Run(Function()
                                                       Try
                                                           Dim endpointDescription As EndpointDescription = CoreClientUtils.SelectEndpoint(serverUrl, UseSecurity)
                                                           Dim endpointConfiguration As EndpointConfiguration = EndpointConfiguration.Create(m_configuration)
                                                           endpoint = New ConfiguredEndpoint(Nothing, endpointDescription, endpointConfiguration)
                                                           Return Nothing
                                                       Catch ex1 As Exception
                                                           Return ex1
                                                       End Try

                                                   End Function)
            Dim ex As Exception = Await t
            If ex Is Nothing Then
                m_session = Await Session.Create(m_configuration, endpoint, False, False, If((String.IsNullOrEmpty(OpcUaName)), m_configuration.ApplicationName, OpcUaName), 60000, UserIdentity, New String() {})
                AddHandler m_session.KeepAlive, AddressOf Session_KeepAlive
                m_IsConnected = True
                DoConnectComplete(Nothing)
                Return m_session
            Else
                ClientUtils.ShowException_Event(ex)
            End If
        Catch ex As Exception
            ClientUtils.ShowException_Event(ex)
        End Try
        Return Nothing
    End Function

    Public Sub Disconnect()
        UpdateStatus(False, DateTime.UtcNow, "Disconnected")

        If m_reConnectHandler IsNot Nothing Then
            m_reConnectHandler.Dispose()
            m_reConnectHandler = Nothing
        End If

        If m_session IsNot Nothing Then
            m_session.Close(10000)
            m_session = Nothing
        End If

        m_IsConnected = False
        DoConnectComplete(Nothing)
    End Sub
    Property ErrStatus As Boolean
    Property StatusString As String
    Private Sub UpdateStatus(ByVal [error] As Boolean, ByVal time As DateTime, ByVal status As String, ParamArray args As Object())

        ErrStatus = [error]
        Dim e As New OpcUaStatusEventArgs() With {
            .[Error] = [error],
            .Time = time.ToLocalTime(),
            .Text = String.Format(status, args)
        }
        RaiseEvent OpcStatusChange(Me, e)

        StatusString = e.ToString

    End Sub

    Private Sub Session_KeepAlive(ByVal session As Session, ByVal e As KeepAliveEventArgs)
        Try

            If Not Object.ReferenceEquals(session, m_session) Then
                Return
            End If

            If ServiceResult.IsBad(e.Status) Then

                If m_reconnectPeriod <= 0 Then
                    UpdateStatus(True, e.CurrentTime, "Communication Error ({0})", e.Status)
                    Return
                End If

                UpdateStatus(True, e.CurrentTime, "Reconnecting in {0}s", m_reconnectPeriod)

                If m_reConnectHandler Is Nothing Then
                    RaiseEvent ReconnectStarting(Me, e)
                    m_reConnectHandler = New SessionReconnectHandler()
                    m_reConnectHandler.BeginReconnect(m_session, m_reconnectPeriod * 1000, AddressOf Server_ReconnectComplete)
                End If

                Return
            End If

            UpdateStatus(False, e.CurrentTime, "Connected [{0}]", session.Endpoint.EndpointUrl)
            RaiseEvent KeepAliveComplete(Me, e)
        Catch exception As Exception
            ClientUtils.ShowException_Event(exception)
        End Try
    End Sub

    Private Sub Server_ReconnectComplete(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not Object.ReferenceEquals(sender, m_reConnectHandler) Then
                Return
            End If

            m_session = m_reConnectHandler.Session
            m_reConnectHandler.Dispose()
            m_reConnectHandler = Nothing
            RaiseEvent ReconnectComplete(Me, e)
        Catch exception As Exception
            ClientUtils.ShowException_Event(exception)
        End Try
    End Sub

    Public Sub SetLogPathName(ByVal filePath As String, ByVal deleteExisting As Boolean)
        Utils.SetTraceLog(filePath, deleteExisting)
        Utils.SetTraceMask(515)
    End Sub

    Public Property OpcUaName As String = "Opc Ua Helper"

    Public Property UseSecurity As Boolean
        Get
            Return m_useSecurity
        End Get
        Set(ByVal value As Boolean)
            m_useSecurity = value
        End Set
    End Property
    Dim mUserIdentity As IUserIdentity
    Property UserIdentity As IUserIdentity
        Get
            If mUserIdentity Is Nothing Then
                Return New UserIdentity(New AnonymousIdentityToken)
            End If
            Return mUserIdentity
        End Get
        Set(value As IUserIdentity)
            mUserIdentity = value
        End Set
    End Property
    Public ReadOnly Property Session As Session
        Get
            Return m_session
        End Get
    End Property

    Public ReadOnly Property Connected As Boolean
        Get
            Return m_IsConnected
        End Get
    End Property

    Public Property ReconnectPeriod As Integer
        Get
            Return m_reconnectPeriod
        End Get
        Set(ByVal value As Integer)
            m_reconnectPeriod = value
        End Set
    End Property




    Public ReadOnly Property AppConfig As ApplicationConfiguration
        Get
            Return m_configuration
        End Get
    End Property

    Public Function ReadNode(ByVal nodeId As NodeId) As DataValue
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection From {
            New ReadValueId() With {
                .NodeId = nodeId,
                .AttributeId = Attributes.Value
            }
        }
        Dim results As DataValueCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Return results(0)
    End Function

    Public Function ReadNode(Of T)(ByVal tag As String) As T
        Dim dataValue As DataValue = ReadNode(New NodeId(tag))
        Return CType(dataValue.Value, T)
    End Function

    Public Function ReadNodeAsync(Of T)(ByVal tag As String) As Task(Of T)
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection From {
            New ReadValueId() With {
                .NodeId = New NodeId(tag),
                .AttributeId = Attributes.Value
            }
        }
        Dim taskCompletionSource = New TaskCompletionSource(Of T)()
        m_session.BeginRead(requestHeader:=Nothing, maxAge:=0, timestampsToReturn:=TimestampsToReturn.Neither, nodesToRead:=nodesToRead, callback:=Sub(ar)
                                                                                                                                                       Dim results As DataValueCollection = Nothing
                                                                                                                                                       Dim diag As DiagnosticInfoCollection = Nothing
                                                                                                                                                       Dim response = m_session.EndRead(result:=ar, results:=results, diagnosticInfos:=diag)

                                                                                                                                                       Try
                                                                                                                                                           CheckReturnValue(response.ServiceResult)
                                                                                                                                                           CheckReturnValue(results(0).StatusCode)
                                                                                                                                                           Dim val = results(0)
                                                                                                                                                           taskCompletionSource.TrySetResult(CType(val.Value, T))
                                                                                                                                                       Catch ex As Exception
                                                                                                                                                           taskCompletionSource.TrySetException(ex)
                                                                                                                                                       End Try
                                                                                                                                                   End Sub, asyncState:=Nothing)
        Return taskCompletionSource.Task
    End Function

    Public Function ReadNodes(ByVal nodeIds As NodeId()) As List(Of DataValue)
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For i As Integer = 0 To nodeIds.Length - 1
            nodesToRead.Add(New ReadValueId() With {
                .NodeId = nodeIds(i),
                .AttributeId = Attributes.Value
            })
        Next

        Dim results As DataValueCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Return results.ToList()
    End Function

    Public Function ReadNodesAsync(ByVal nodeIds As NodeId()) As Task(Of List(Of DataValue))
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For i As Integer = 0 To nodeIds.Length - 1
            nodesToRead.Add(New ReadValueId() With {
                .NodeId = nodeIds(i),
                .AttributeId = Attributes.Value
            })
        Next

        Dim taskCompletionSource = New TaskCompletionSource(Of List(Of DataValue))()
        m_session.BeginRead(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, callback:=Sub(ar)
                                                                                               Dim results As DataValueCollection = Nothing
                                                                                               Dim diag As DiagnosticInfoCollection = Nothing
                                                                                               Dim response = m_session.EndRead(result:=ar, results:=results, diagnosticInfos:=diag)

                                                                                               Try
                                                                                                   CheckReturnValue(response.ServiceResult)
                                                                                                   taskCompletionSource.TrySetResult(results.ToList())
                                                                                               Catch ex As Exception
                                                                                                   taskCompletionSource.TrySetException(ex)
                                                                                               End Try
                                                                                           End Sub, asyncState:=Nothing)
        Return taskCompletionSource.Task
    End Function

    Public Function ReadNodes(Of T)(ByVal tags As String()) As List(Of T)
        Dim result As List(Of T) = New List(Of T)()
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For i As Integer = 0 To tags.Length - 1
            nodesToRead.Add(New ReadValueId() With {
                .NodeId = New NodeId(tags(i)),
                .AttributeId = Attributes.Value
            })
        Next

        Dim results As DataValueCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)

        For Each item In results
            result.Add(CType(item.Value, T))
        Next

        Return result
    End Function

    Public Function ReadNodesAsync(Of T)(ByVal tags As String()) As Task(Of List(Of T))
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For i As Integer = 0 To tags.Length - 1
            nodesToRead.Add(New ReadValueId() With {
                .NodeId = New NodeId(tags(i)),
                .AttributeId = Attributes.Value
            })
        Next

        Dim taskCompletionSource = New TaskCompletionSource(Of List(Of T))()
        m_session.BeginRead(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, callback:=Sub(ar)
                                                                                               Dim results As DataValueCollection = Nothing
                                                                                               Dim diag As DiagnosticInfoCollection = Nothing
                                                                                               Dim response = m_session.EndRead(result:=ar, results:=results, diagnosticInfos:=diag)

                                                                                               Try
                                                                                                   CheckReturnValue(response.ServiceResult)
                                                                                                   Dim result As List(Of T) = New List(Of T)()

                                                                                                   For Each item In results
                                                                                                       result.Add(CType(item.Value, T))
                                                                                                   Next

                                                                                                   taskCompletionSource.TrySetResult(result)
                                                                                               Catch ex As Exception
                                                                                                   taskCompletionSource.TrySetException(ex)
                                                                                               End Try
                                                                                           End Sub, asyncState:=Nothing)
        Return taskCompletionSource.Task
    End Function

    Public Function WriteNode(Of T)(ByVal tag As String, ByVal value As T) As Boolean
        Dim valueToWrite As WriteValue = New WriteValue() With {
            .NodeId = New NodeId(tag),
            .AttributeId = Attributes.Value
        }
        valueToWrite.Value.Value = value
        valueToWrite.Value.StatusCode = StatusCodes.Uncertain ' StatusCodes.Good
        valueToWrite.Value.ServerTimestamp = DateTime.MinValue
        valueToWrite.Value.SourceTimestamp = DateTime.MinValue
        Dim valuesToWrite As WriteValueCollection = New WriteValueCollection From {
            valueToWrite
        }
        Dim results As StatusCodeCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Write(Nothing, valuesToWrite, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, valuesToWrite)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite)

        If StatusCode.IsBad(results(0)) Then
            Throw New ServiceResultException(results(0))
        End If

        Return Not StatusCode.IsBad(results(0))
    End Function

    Public Function WriteNodeAsync(Of T)(ByVal tag As String, ByVal value As T) As Task(Of Boolean)
        Dim valueToWrite As WriteValue = New WriteValue() With {.NodeId = New NodeId(tag), .AttributeId = Attributes.Value}
        valueToWrite.Value.Value = value
        valueToWrite.Value.StatusCode = StatusCodes.Uncertain ' StatusCodes.Good
        valueToWrite.Value.ServerTimestamp = DateTime.MinValue
        valueToWrite.Value.SourceTimestamp = DateTime.MinValue
        Dim valuesToWrite As WriteValueCollection = New WriteValueCollection From {valueToWrite}
        Dim taskCompletionSource = New TaskCompletionSource(Of Boolean)()
        Dim results As StatusCodeCollection = Nothing, diag As DiagnosticInfoCollection = Nothing
        m_session.BeginWrite(requestHeader:=Nothing, nodesToWrite:=valuesToWrite, callback:=Sub(ar)

                                                                                                Dim response = m_session.EndWrite(result:=ar, results:=results, diagnosticInfos:=diag)
                                                                                                Try
                                                                                                    ClientBase.ValidateResponse(results, valuesToWrite)
                                                                                                    ClientBase.ValidateDiagnosticInfos(diag, valuesToWrite)
                                                                                                    taskCompletionSource.SetResult(StatusCode.IsGood(results(0)))
                                                                                                Catch ex As Exception
                                                                                                    taskCompletionSource.TrySetException(ex)
                                                                                                End Try
                                                                                            End Sub, asyncState:=Nothing)
        Return taskCompletionSource.Task
    End Function
    Public Function WriteNodes(ByVal tags As String(), ByVal values As Object()) As Boolean
        Dim valuesToWrite As WriteValueCollection = New WriteValueCollection()

        For i As Integer = 0 To tags.Length - 1

            If i < values.Length Then
                Dim valueToWrite As WriteValue = New WriteValue() With {
                    .NodeId = New NodeId(tags(i)),
                    .AttributeId = Attributes.Value
                }
                valueToWrite.Value.Value = values(i)
                valueToWrite.Value.StatusCode = StatusCodes.Good
                valueToWrite.Value.ServerTimestamp = DateTime.MinValue
                valueToWrite.Value.SourceTimestamp = DateTime.MinValue
                valuesToWrite.Add(valueToWrite)
            End If
        Next

        Dim results As StatusCodeCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Write(Nothing, valuesToWrite, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, valuesToWrite)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite)
        Dim result As Boolean = True

        For Each r In results

            If StatusCode.IsBad(r) Then
                result = False
                Exit For
            End If
        Next

        Return result
    End Function

    Public Function DeleteExsistNode(ByVal tag As String) As Boolean
        Dim waitDelete As DeleteNodesItemCollection = New DeleteNodesItemCollection()
        Dim nodesItem As DeleteNodesItem = New DeleteNodesItem() With {
            .NodeId = New NodeId(tag)
        }
        Dim results As StatusCodeCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.DeleteNodes(Nothing, waitDelete, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, waitDelete)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, waitDelete)
        Return Not StatusCode.IsBad(results(0))
    End Function

    <Obsolete("还未经过测试,无法使用")>
    Public Sub AddNewNode(ByVal parent As NodeId)
        Dim node2 As AddNodesItem = New AddNodesItem()
        node2.ParentNodeId = New NodeId(parent)
        node2.ReferenceTypeId = ReferenceTypes.HasComponent
        node2.RequestedNewNodeId = Nothing
        node2.BrowseName = New QualifiedName("DataVariable1")
        node2.NodeClass = NodeClass.Variable
        node2.NodeAttributes = Nothing
        node2.TypeDefinition = VariableTypeIds.BaseDataVariableType
        Dim node2Attribtues As VariableAttributes = New VariableAttributes()
        node2Attribtues.DisplayName = "DataVariable1"
        node2Attribtues.Description = "DataVariable1 Description"
        node2Attribtues.Value = New [Variant](123)
        node2Attribtues.DataType = CUInt(BuiltInType.Int32)
        node2Attribtues.ValueRank = ValueRanks.Scalar
        node2Attribtues.ArrayDimensions = New UInt32Collection()
        node2Attribtues.AccessLevel = AccessLevels.CurrentReadOrWrite
        node2Attribtues.UserAccessLevel = AccessLevels.CurrentReadOrWrite
        node2Attribtues.MinimumSamplingInterval = 0
        node2Attribtues.Historizing = False
        node2Attribtues.WriteMask = CUInt(AttributeWriteMask.None)
        node2Attribtues.UserWriteMask = CUInt(AttributeWriteMask.None)
        node2Attribtues.SpecifiedAttributes = CUInt(NodeAttributesMask.All)
        node2.NodeAttributes = New ExtensionObject(node2Attribtues)
        Dim nodesToAdd As AddNodesItemCollection = New AddNodesItemCollection From {
            node2
        }
        Dim results As AddNodesResultCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.AddNodes(Nothing, nodesToAdd, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToAdd)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToAdd)
    End Sub
    Dim DefaultSubscription As Subscription
    Public Function GetMonitoredItem(ByVal tag As String, ByVal callback As Action(Of String, MonitoredItem, MonitoredItemNotificationEventArgs)) As MonitoredItem

        Dim item = New MonitoredItem With {
                .StartNodeId = New NodeId(tag),
                .AttributeId = Attributes.Value,
                .DisplayName = tag,
                .SamplingInterval = 100
            }
        AddHandler item.Notification, Sub(ByVal monitoredItem As MonitoredItem, ByVal args As MonitoredItemNotificationEventArgs)
                                          callback?.Invoke(item.DisplayName, monitoredItem, args)
                                      End Sub
        DefaultSubscription.AddItem(item)
        If DefaultSubscription.Created Then
            DefaultSubscription.ApplyChanges()
        Else
            DefaultSubscription.Create()
        End If
        Return item
    End Function

    Public Sub RemoveMonitoredItem(ByVal item As MonitoredItem)


        If DefaultSubscription IsNot Nothing Then
            DefaultSubscription.RemoveItem(item)
            If DefaultSubscription.MonitoredItems.Count = 0 Then
                DefaultSubscription.Delete(True)
                m_session.RemoveSubscription(DefaultSubscription)
                DefaultSubscription.Dispose()
            End If

        End If

    End Sub
    Public Sub AddSubscription(ByVal key As String, ByVal tag As String, ByVal callback As Action(Of String, MonitoredItem, MonitoredItemNotificationEventArgs))
        AddSubscription(key, New String() {tag}, callback)
    End Sub
    Public Sub AddSubscription(ByVal key As String, ByVal tags As String(), ByVal callback As Action(Of String, MonitoredItem, MonitoredItemNotificationEventArgs))
        Dim m_subscription As Subscription
        SyncLock dic_subscriptions

            If dic_subscriptions.ContainsKey(key) Then
                m_subscription = dic_subscriptions(key)
            Else
                m_subscription = New Subscription(m_session.DefaultSubscription)
                dic_subscriptions.Add(key, m_subscription)
                m_session.AddSubscription(m_subscription)
                m_subscription.Create()
            End If
        End SyncLock
        m_subscription.PublishingEnabled = True
        m_subscription.PublishingInterval = 0
        m_subscription.KeepAliveCount = UInteger.MaxValue
        m_subscription.LifetimeCount = UInteger.MaxValue
        m_subscription.MaxNotificationsPerPublish = UInteger.MaxValue
        m_subscription.Priority = 100
        m_subscription.DisplayName = key

        For i As Integer = 0 To tags.Length - 1
            Dim item = New MonitoredItem With {
                .StartNodeId = New NodeId(tags(i)),
                .AttributeId = Attributes.Value,
                .DisplayName = tags(i),
                .SamplingInterval = 100
            }
            AddHandler item.Notification, Sub(ByVal monitoredItem As MonitoredItem, ByVal args As MonitoredItemNotificationEventArgs)
                                              callback?.Invoke(item.DisplayName, monitoredItem, args)
                                          End Sub
            m_subscription.AddItem(item)
        Next
    End Sub

    Public Sub RemoveSubscription(ByVal key As String)
        SyncLock dic_subscriptions

            If dic_subscriptions.ContainsKey(key) Then
                dic_subscriptions(key).Delete(True)
                m_session.RemoveSubscription(dic_subscriptions(key))
                dic_subscriptions(key).Dispose()
                dic_subscriptions.Remove(key)
            End If
        End SyncLock
    End Sub

    Public Sub RemoveAllSubscription()
        SyncLock dic_subscriptions

            For Each item In dic_subscriptions
                m_session.RemoveSubscription(item.Value)
                item.Value.Delete(True)
                item.Value.Dispose()
            Next

            dic_subscriptions.Clear()
        End SyncLock
    End Sub

    Public Iterator Function ReadHistoryRawDataValues(ByVal tag As String, ByVal start As DateTime, ByVal [end] As DateTime, ByVal Optional count As UInteger = 1, ByVal Optional containBound As Boolean = False) As IEnumerable(Of DataValue)
        Dim m_nodeToContinue As HistoryReadValueId = New HistoryReadValueId() With {
            .NodeId = New NodeId(tag)
        }
        Dim m_details As ReadRawModifiedDetails = New ReadRawModifiedDetails With {
            .StartTime = start,
            .EndTime = [end],
            .NumValuesPerNode = count,
            .IsReadModified = False,
            .ReturnBounds = containBound
        }
        Dim nodesToRead As HistoryReadValueIdCollection = New HistoryReadValueIdCollection()
        nodesToRead.Add(m_nodeToContinue)
        Dim results As HistoryReadResultCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.HistoryRead(Nothing, New ExtensionObject(m_details), TimestampsToReturn.Both, False, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)

        If StatusCode.IsBad(results(0).StatusCode) Then
            Throw New ServiceResultException(results(0).StatusCode)
        End If

        Dim values As HistoryData = TryCast(ExtensionObject.ToEncodeable(results(0).HistoryData), HistoryData)

        For Each value In values.DataValues
            Yield value
        Next
    End Function

    Public Iterator Function ReadHistoryRawDataValues(Of T)(ByVal tag As String, ByVal start As DateTime, ByVal [end] As DateTime, ByVal Optional count As UInteger = 1, ByVal Optional containBound As Boolean = False) As IEnumerable(Of T)
        Dim m_nodeToContinue As HistoryReadValueId = New HistoryReadValueId() With {
            .NodeId = New NodeId(tag)
        }
        Dim m_details As ReadRawModifiedDetails = New ReadRawModifiedDetails With {
            .StartTime = start.ToUniversalTime(),
            .EndTime = [end].ToUniversalTime(),
            .NumValuesPerNode = count,
            .IsReadModified = False,
            .ReturnBounds = containBound
        }
        Dim nodesToRead As HistoryReadValueIdCollection = New HistoryReadValueIdCollection()
        nodesToRead.Add(m_nodeToContinue)
        Dim results As HistoryReadResultCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.HistoryRead(Nothing, New ExtensionObject(m_details), TimestampsToReturn.Both, False, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)

        If StatusCode.IsBad(results(0).StatusCode) Then
            Throw New ServiceResultException(results(0).StatusCode)
        End If

        Dim values As HistoryData = TryCast(ExtensionObject.ToEncodeable(results(0).HistoryData), HistoryData)

        For Each value In values.DataValues
            Yield CType(value.Value, T)
        Next
    End Function

    Public Function BrowseNodeReference(ByVal tag As String) As ReferenceDescription()
        Dim sourceId As NodeId = New NodeId(tag)
        Dim nodeToBrowse1 As New BrowseDescription()
        nodeToBrowse1.NodeId = sourceId
        nodeToBrowse1.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.Aggregates
        nodeToBrowse1.IncludeSubtypes = True
        nodeToBrowse1.NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable Or NodeClass.Method))
        nodeToBrowse1.ResultMask = CUInt(BrowseResultMask.All)
        Dim nodeToBrowse2 As New BrowseDescription()
        nodeToBrowse2.NodeId = sourceId
        nodeToBrowse2.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse2.ReferenceTypeId = ReferenceTypeIds.Organizes
        nodeToBrowse2.IncludeSubtypes = True
        nodeToBrowse2.NodeClassMask = CUInt((NodeClass.Object Or NodeClass.Variable))
        nodeToBrowse2.ResultMask = CUInt(BrowseResultMask.All)
        Dim nodesToBrowse As New BrowseDescriptionCollection()
        nodesToBrowse.Add(nodeToBrowse1)
        nodesToBrowse.Add(nodeToBrowse2)
        Dim references As ReferenceDescriptionCollection = FormUtils.Browse(m_session, nodesToBrowse, False)
        Return references.ToArray()
    End Function

    Public Function ReadNoteAttributes(ByVal tag As String) As OpcNodeAttribute()
        Dim sourceId As NodeId = New NodeId(tag)
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For ii As UInteger = Attributes.NodeClass To Attributes.UserExecutable
            Dim nodeToRead As ReadValueId = New ReadValueId()
            nodeToRead.NodeId = sourceId
            nodeToRead.AttributeId = ii
            nodesToRead.Add(nodeToRead)
        Next

        Dim startOfProperties As Integer = nodesToRead.Count
        Dim nodeToBrowse1 As BrowseDescription = New BrowseDescription()
        nodeToBrowse1.NodeId = sourceId
        nodeToBrowse1.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.HasProperty
        nodeToBrowse1.IncludeSubtypes = True
        nodeToBrowse1.NodeClassMask = 0
        nodeToBrowse1.ResultMask = CUInt(BrowseResultMask.All)
        Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()
        nodesToBrowse.Add(nodeToBrowse1)
        Dim references As ReferenceDescriptionCollection = FormUtils.Browse(m_session, nodesToBrowse, False)

        If references Is Nothing Then
            Return New OpcNodeAttribute(-1) {}
        End If

        For ii As Integer = 0 To references.Count - 1

            If references(ii).NodeId.IsAbsolute Then
                Continue For
            End If

            Dim nodeToRead As ReadValueId = New ReadValueId()
            nodeToRead.NodeId = CType(references(ii).NodeId, NodeId)
            nodeToRead.AttributeId = Attributes.Value
            nodesToRead.Add(nodeToRead)
        Next

        Dim results As DataValueCollection = Nothing
        Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Dim nodeAttribute As List(Of OpcNodeAttribute) = New List(Of OpcNodeAttribute)()

        For ii As Integer = 0 To results.Count - 1
            Dim item As OpcNodeAttribute = New OpcNodeAttribute()

            If ii < startOfProperties Then

                If results(ii).StatusCode = StatusCodes.BadAttributeIdInvalid Then
                    Continue For
                End If

                item.Name = Attributes.GetBrowseName(nodesToRead(ii).AttributeId)

                If StatusCode.IsBad(results(ii).StatusCode) Then
                    item.Type = Utils.Format("{0}", Attributes.GetDataTypeId(nodesToRead(ii).AttributeId))
                    item.Value = Utils.Format("{0}", results(ii).StatusCode)
                Else
                    Dim typeInfo As TypeInfo = TypeInfo.Construct(results(ii).Value)
                    item.Type = typeInfo.BuiltInType.ToString()

                    If typeInfo.ValueRank >= ValueRanks.OneOrMoreDimensions Then
                        item.Type += "[]"
                    End If

                    item.Value = results(ii).Value
                End If
            Else

                If results(ii).StatusCode = StatusCodes.BadNodeIdUnknown Then
                    Continue For
                End If

                item.Name = Utils.Format("{0}", references(ii - startOfProperties))

                If StatusCode.IsBad(results(ii).StatusCode) Then
                    item.Type = String.Empty
                    item.Value = Utils.Format("{0}", results(ii).StatusCode)
                Else
                    Dim typeInfo As TypeInfo = TypeInfo.Construct(results(ii).Value)
                    item.Type = typeInfo.BuiltInType.ToString()

                    If typeInfo.ValueRank >= ValueRanks.OneOrMoreDimensions Then
                        item.Type += "[]"
                    End If

                    item.Value = results(ii).Value
                End If
            End If

            nodeAttribute.Add(item)
        Next

        Return nodeAttribute.ToArray()
    End Function
    'Public Function ReadNoteAttributeValue(ByVal tag As String, AttributeID As UInteger) As DataValue()
    '    Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()


    '    Dim sourceId As NodeId = New NodeId(tag)

    '    nodesToRead.Add(New ReadValueId() With {
    '                .NodeId = sourceId,
    '                .AttributeId = AttributeID
    '            })
    '    Dim results As DataValueCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
    '    Session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
    '    ClientBase.ValidateResponse(results, nodesToRead)
    '    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
    '    Return results.ToArray()
    'End Function
    Public Function ReadOneNodeFiveAttributes(ByVal nodeIds As List(Of NodeId)) As DataValue()
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For Each nodeId In nodeIds
            Dim sourceId As NodeId = nodeId
            nodesToRead.Add(New ReadValueId() With {
                    .NodeId = sourceId,
                    .AttributeId = Attributes.NodeClass
                })
            nodesToRead.Add(New ReadValueId() With {
                    .NodeId = sourceId,
                    .AttributeId = Attributes.Value
                })
            nodesToRead.Add(New ReadValueId() With {
                    .NodeId = sourceId,
                    .AttributeId = Attributes.AccessLevel
                })
            nodesToRead.Add(New ReadValueId() With {
                    .NodeId = sourceId,
                    .AttributeId = Attributes.DisplayName
                })
            nodesToRead.Add(New ReadValueId() With {
                    .NodeId = sourceId,
                    .AttributeId = Attributes.Description
                })

        Next

        Dim results As DataValueCollection = Nothing, diagnosticInfos As DiagnosticInfoCollection = Nothing
        Session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Return results.ToArray()
    End Function
    Public Function GetDescription(NodeID As String) As String
        Return ReadNoteDataValueAttributes(NodeID, Attributes.Description).WrappedValue.Value
    End Function
    Public Function ReadValue(NodeID As String) As String
        Return If(ReadNoteDataValueAttributes(NodeID, Attributes.Value)?.WrappedValue.Value, "")
    End Function
    Public Function ReadNoteDataValueAttributes(ByVal nodeId As NodeId, ByVal attribute As UInteger) As DataValue
        Dim sourceId As NodeId = nodeId
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()
        Dim nodeToRead As ReadValueId = New ReadValueId() With {.NodeId = sourceId, .AttributeId = attribute}

        nodesToRead.Add(nodeToRead)
        Dim startOfProperties As Integer = nodesToRead.Count
        Dim nodeToBrowse1 As BrowseDescription = New BrowseDescription()
        nodeToBrowse1.NodeId = sourceId
        nodeToBrowse1.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.HasProperty
        nodeToBrowse1.IncludeSubtypes = True
        nodeToBrowse1.NodeClassMask = 0
        nodeToBrowse1.ResultMask = CUInt(BrowseResultMask.All)
        Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()
        nodesToBrowse.Add(nodeToBrowse1)
        Dim references As ReferenceDescriptionCollection = FormUtils.Browse(Session, nodesToBrowse, False)

        If references Is Nothing Then
            Return Nothing
        End If

        For ii As Integer = 0 To references.Count - 1

            If references(ii).NodeId.IsAbsolute Then
                Continue For
            End If

            Dim nodeToRead2 As ReadValueId = New ReadValueId()
            nodeToRead2.NodeId = CType(references(ii).NodeId, NodeId)
            nodeToRead2.AttributeId = Attributes.Value
            nodesToRead.Add(nodeToRead2)
        Next

        Dim results As DataValueCollection = Nothing
        Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
        Session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Return results(0)
    End Function
    Public Function ReadNoteDataValueAttributes(ByVal tag As String) As DataValue()
        Dim sourceId As NodeId = New NodeId(tag)
        Dim nodesToRead As ReadValueIdCollection = New ReadValueIdCollection()

        For ii As UInteger = Attributes.NodeId To Attributes.UserExecutable
            Dim nodeToRead As ReadValueId = New ReadValueId()
            nodeToRead.NodeId = sourceId
            nodeToRead.AttributeId = ii
            nodesToRead.Add(nodeToRead)
        Next

        Dim startOfProperties As Integer = nodesToRead.Count
        Dim nodeToBrowse1 As BrowseDescription = New BrowseDescription()
        nodeToBrowse1.NodeId = sourceId
        nodeToBrowse1.BrowseDirection = BrowseDirection.Forward
        nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.HasProperty
        nodeToBrowse1.IncludeSubtypes = True
        nodeToBrowse1.NodeClassMask = 0
        nodeToBrowse1.ResultMask = CUInt(BrowseResultMask.All)
        Dim nodesToBrowse As BrowseDescriptionCollection = New BrowseDescriptionCollection()
        nodesToBrowse.Add(nodeToBrowse1)
        Dim references As ReferenceDescriptionCollection = FormUtils.Browse(m_session, nodesToBrowse, False)

        If references Is Nothing Then
            Return New DataValue(-1) {}
        End If

        For ii As Integer = 0 To references.Count - 1

            If references(ii).NodeId.IsAbsolute Then
                Continue For
            End If

            Dim nodeToRead As ReadValueId = New ReadValueId()
            nodeToRead.NodeId = CType(references(ii).NodeId, NodeId)
            nodeToRead.AttributeId = Attributes.Value
            nodesToRead.Add(nodeToRead)
        Next

        Dim results As DataValueCollection = Nothing
        Dim diagnosticInfos As DiagnosticInfoCollection = Nothing
        m_session.Read(Nothing, 0, TimestampsToReturn.Neither, nodesToRead, results, diagnosticInfos)
        ClientBase.ValidateResponse(results, nodesToRead)
        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead)
        Return results.ToArray()
    End Function

    Public Function CallMethodByNodeId(ByVal tagParent As String, ByVal tag As String, ParamArray args As Object()) As Object()
        If m_session Is Nothing Then
            Return Nothing
        End If

        Dim outputArguments As IList(Of Object) = m_session.[Call](New NodeId(tagParent), New NodeId(tag), args)
        Return outputArguments.ToArray()
    End Function

    Private Sub DoConnectComplete(ByVal state As Object)
        RaiseEvent ConnectComplete(Me, Nothing)
    End Sub

    Private Sub CheckReturnValue(ByVal status As StatusCode)
        If Not StatusCode.IsGood(status) Then Throw New Exception(String.Format("Invalid response from the server. (Response Status: {0})", status))
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
            End If
            RemoveAllSubscription()
            If m_session IsNot Nothing AndAlso DefaultSubscription IsNot Nothing Then m_session.RemoveSubscription(DefaultSubscription)
            Disconnect()
            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

