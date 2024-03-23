Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design


Public Class ToolStripMenuItemExt
    Inherits ToolStripMenuItem



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



    Protected Overridable Overloads Sub Invalidate()
        MyBase.Invalidate()
    End Sub




    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Protected Overloads ReadOnly Property DesignMode As Boolean
        Get

            If Me.GetService(GetType(IDesignerHost)) IsNot Nothing OrElse System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then
                Return True
            Else
                Return False

            End If

        End Get
    End Property


End Class
