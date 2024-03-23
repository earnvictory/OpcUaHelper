
Imports System.ComponentModel
Imports System.Windows.Forms.Design

<Description("RadioButton控件设计模式行为")>
Friend Class RadioButtonExtDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            SelectionRules = MyBase.SelectionRules
            Dim component As Object = CObj(Me.Component)
            Dim propertyDescriptor As PropertyDescriptor = TypeDescriptor.GetProperties(component)("AutoSize")
            If propertyDescriptor IsNot Nothing AndAlso CBool(propertyDescriptor.GetValue(component)) Then selectionRules = selectionRules And Not selectionRules.AllSizeable
            Return selectionRules
        End Get
    End Property

    Public Sub New()
        Me.AutoResizeHandles = True
    End Sub
End Class
