Imports System.ComponentModel
Imports System.ComponentModel.Design


Public Class ToolStripEx
    Inherits ToolStrip


    Private _themeColor As Color = SystemColors.Highlight

    Public Sub New()
        MyBase.New
        Me.Renderer = New ToolStripProfessionalRendererExt(_themeColor)

    End Sub




    Public Property ThemeColor As Color
        Get
            Return _themeColor
        End Get
        Set(ByVal value As Color)
            _themeColor = value
            Me.Renderer = New ToolStripProfessionalRendererExt(_themeColor)
        End Set
    End Property


End Class
