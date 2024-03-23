Imports System.ComponentModel
Imports System.Windows.Forms.Design

<Description("ColorPickerExt控件设计模式行为")>
Public Class ColorPickerExtDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            Return SelectionRules.Moveable
        End Get
    End Property
End Class
