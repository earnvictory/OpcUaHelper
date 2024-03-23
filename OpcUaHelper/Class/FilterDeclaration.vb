Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Opc.Ua
Imports Opc.Ua.Client


    Public Class TypeDeclaration
        Public NodeId As NodeId
        Public Declarations As List(Of InstanceDeclaration)
    End Class

    Public Class InstanceDeclaration
        Public RootTypeId As NodeId
        Public BrowsePath As QualifiedNameCollection
        Public BrowsePathDisplayText As String
        Public DisplayPath As String
        Public NodeId As NodeId
        Public NodeClass As NodeClass
        Public BrowseName As QualifiedName
        Public DisplayName As String
        Public Description As String
        Public ModellingRule As NodeId
        Public DataType As NodeId
        Public ValueRank As Integer
        Public BuiltInType As BuiltInType
        Public DataTypeDisplayText As String
        Public OverriddenDeclaration As InstanceDeclaration
    End Class

    Public Class FilterDeclarationField
        Public Sub New()
            Selected = True
            DisplayInList = False
            FilterEnabled = False
            FilterOperator = FilterOperator.Equals
            FilterValue = [Variant].Null
            InstanceDeclaration = Nothing
        End Sub

        Public Sub New(ByVal instanceDeclaration As InstanceDeclaration)
            Selected = True
            DisplayInList = False
            FilterEnabled = False
            FilterOperator = FilterOperator.Equals
            FilterValue = [Variant].Null
            instanceDeclaration = instanceDeclaration
        End Sub

        Public Sub New(ByVal field As FilterDeclarationField)
            Selected = field.Selected
            DisplayInList = field.DisplayInList
            FilterEnabled = field.FilterEnabled
            FilterOperator = field.FilterOperator
            FilterValue = field.FilterValue
            InstanceDeclaration = field.InstanceDeclaration
        End Sub

        Public Selected As Boolean
        Public DisplayInList As Boolean
        Public FilterEnabled As Boolean
        Public FilterOperator As FilterOperator
        Public FilterValue As [Variant]
        Public InstanceDeclaration As InstanceDeclaration
    End Class

    Public Class FilterDeclaration
        Public Sub New()
            EventTypeId = Opc.Ua.ObjectTypeIds.BaseEventType
            Fields = New List(Of FilterDeclarationField)()
        End Sub

        Public Sub New(ByVal eventType As TypeDeclaration, ByVal template As FilterDeclaration)
            EventTypeId = eventType.NodeId
            Fields = New List(Of FilterDeclarationField)()

            For Each instanceDeclaration As InstanceDeclaration In eventType.Declarations

                If instanceDeclaration.NodeClass = NodeClass.Method Then
                    Continue For
                End If

                If NodeId.IsNull(instanceDeclaration.ModellingRule) Then
                    Continue For
                End If

                Dim element As FilterDeclarationField = New FilterDeclarationField(instanceDeclaration)
                Fields.Add(element)

                If template Is Nothing Then

                    If instanceDeclaration.RootTypeId = Opc.Ua.ObjectTypeIds.BaseEventType AndAlso instanceDeclaration.BrowseName <> Opc.Ua.BrowseNames.EventId Then
                        element.DisplayInList = True
                    End If
                Else

                    For Each field As FilterDeclarationField In template.Fields

                        If field.InstanceDeclaration.BrowsePathDisplayText = element.InstanceDeclaration.BrowsePathDisplayText Then
                            element.DisplayInList = field.DisplayInList
                            element.FilterEnabled = field.FilterEnabled
                            element.FilterOperator = field.FilterOperator
                            element.FilterValue = field.FilterValue
                            Exit For
                        End If
                    Next
                End If
            Next
        End Sub

        Public Sub New(ByVal declaration As FilterDeclaration)
            EventTypeId = declaration.EventTypeId
            Fields = New List(Of FilterDeclarationField)(declaration.Fields.Count)

            For ii As Integer = 0 To declaration.Fields.Count - 1
                Fields.Add(New FilterDeclarationField(declaration.Fields(ii)))
            Next
        End Sub

        Public Function GetFilter() As EventFilter
            Dim filter As EventFilter = New EventFilter()
            filter.SelectClauses = GetSelectClause()
            filter.WhereClause = GetWhereClause()
            Return filter
        End Function

        Public Sub AddSimpleField(ByVal browseName As QualifiedName, ByVal dataType As BuiltInType, ByVal displayInList As Boolean)
            AddSimpleField(New QualifiedName() {browseName}, NodeClass.Variable, dataType, ValueRanks.Scalar, displayInList)
        End Sub

        Public Sub AddSimpleField(ByVal browseName As QualifiedName, ByVal dataType As BuiltInType, ByVal valueRank As Integer, ByVal displayInList As Boolean)
            AddSimpleField(New QualifiedName() {browseName}, NodeClass.Variable, dataType, valueRank, displayInList)
        End Sub

        Public Sub AddSimpleField(ByVal browseNames As QualifiedName(), ByVal dataType As BuiltInType, ByVal valueRank As Integer, ByVal displayInList As Boolean)
            AddSimpleField(browseNames, NodeClass.Variable, dataType, valueRank, displayInList)
        End Sub

        Public Sub AddSimpleField(ByVal browseNames As QualifiedName(), ByVal nodeClass As NodeClass, ByVal dataType As BuiltInType, ByVal valueRank As Integer, ByVal displayInList As Boolean)
            Dim field As FilterDeclarationField = New FilterDeclarationField()
            field.DisplayInList = displayInList
            field.InstanceDeclaration = New InstanceDeclaration()
            field.InstanceDeclaration.NodeClass = nodeClass

            If browseNames IsNot Nothing Then
                field.InstanceDeclaration.BrowseName = browseNames(browseNames.Length - 1)
                field.InstanceDeclaration.BrowsePath = New QualifiedNameCollection()
                Dim path As  New StringBuilder()

                For ii As Integer = 0 To browseNames.Length - 1

                    If path.Length > 0 Then
                        path.Append("/"c)
                    End If

                    path.Append(browseNames(ii))
                    field.InstanceDeclaration.BrowsePath.Add(browseNames(ii))
                Next

                field.InstanceDeclaration.BrowsePathDisplayText = path.ToString()
            End If

            field.InstanceDeclaration.BuiltInType = dataType
            field.InstanceDeclaration.DataType = CUInt(dataType)
            field.InstanceDeclaration.ValueRank = valueRank
            field.InstanceDeclaration.DataTypeDisplayText = dataType.ToString()

            If valueRank >= 0 Then
                field.InstanceDeclaration.DataTypeDisplayText += "[]"
            End If

            field.InstanceDeclaration.DisplayName = field.InstanceDeclaration.BrowseName.Name
            field.InstanceDeclaration.DisplayPath = field.InstanceDeclaration.BrowsePathDisplayText
            field.InstanceDeclaration.RootTypeId = ObjectTypeIds.BaseEventType
            Fields.Add(field)
        End Sub

        Public Function GetSelectClause() As SimpleAttributeOperandCollection
            Dim selectClause As SimpleAttributeOperandCollection = New SimpleAttributeOperandCollection()
            Dim operand As SimpleAttributeOperand = New SimpleAttributeOperand()
            operand.TypeDefinitionId = Opc.Ua.ObjectTypeIds.BaseEventType
            operand.AttributeId = Opc.Ua.Attributes.NodeId
            selectClause.Add(operand)

            For Each field As FilterDeclarationField In Fields

                If field.Selected Then
                    operand = New SimpleAttributeOperand()
                    operand.TypeDefinitionId = field.InstanceDeclaration.RootTypeId
                    operand.AttributeId = If((field.InstanceDeclaration.NodeClass = NodeClass.Object), Opc.Ua.Attributes.NodeId, Opc.Ua.Attributes.Value)
                    operand.BrowsePath = field.InstanceDeclaration.BrowsePath
                    selectClause.Add(operand)
                End If
            Next

            Return selectClause
        End Function

        Public Function GetWhereClause() As ContentFilter
            Dim whereClause As ContentFilter = New ContentFilter()
            Dim element1 As ContentFilterElement = whereClause.Push(FilterOperator.OfType, EventTypeId)
            Dim filter As EventFilter = New EventFilter()

            For Each field As FilterDeclarationField In Fields

                If field.FilterEnabled Then
                    Dim operand1 As SimpleAttributeOperand = New SimpleAttributeOperand()
                    operand1.TypeDefinitionId = field.InstanceDeclaration.RootTypeId
                    operand1.AttributeId = If((field.InstanceDeclaration.NodeClass = NodeClass.Object), Opc.Ua.Attributes.NodeId, Opc.Ua.Attributes.Value)
                    operand1.BrowsePath = field.InstanceDeclaration.BrowsePath
                    Dim operand2 As LiteralOperand = New LiteralOperand()
                    operand2.Value = field.FilterValue
                    Dim element2 As ContentFilterElement = whereClause.Push(field.FilterOperator, operand1, operand2)
                    element1 = whereClause.Push(FilterOperator.[And], element1, element2)
                End If
            Next

            Return whereClause
        End Function

        Public Function GetValue(Of T)(ByVal browseName As QualifiedName, ByVal fields As VariantCollection, ByVal defaultValue As T) As T
            If fields Is Nothing OrElse fields.Count = 0 Then
                Return defaultValue
            End If

            If browseName Is Nothing Then
                browseName = QualifiedName.Null
            End If

            For ii As Integer = 0 To Me.Fields.Count - 1

                If Me.Fields(ii).InstanceDeclaration.BrowseName = browseName Then

                    If ii >= fields.Count + 1 Then
                        Return defaultValue
                    End If

                    Dim value As Object = fields(ii + 1).Value

                    If GetType(T).IsInstanceOfType(value) Then
                        Return CType(value, T)
                    End If

                    Exit For
                End If
            Next

            Return defaultValue
        End Function

        Public EventTypeId As NodeId
        Public Fields As List(Of FilterDeclarationField)
    End Class

