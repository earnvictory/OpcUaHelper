Imports System.ComponentModel
Imports System.ComponentModel.Design

Public Class ToolStripDropDownButtonExt
    Inherits ToolStripDropDownButton



    Public Sub New()
        ForeColor = Color.White

    End Sub
    <DefaultValue(GetType(Color), "255, 255, 255")>
    Overrides Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
        End Set
    End Property


End Class
