Imports System.ComponentModel

Public Class ContextMenuStripExt
    Inherits ContextMenuStrip

    Overloads Property Renderer As ToolStripProfessionalRendererExt
        Get
            Return MyBase.Renderer
        End Get
        Set(value As ToolStripProfessionalRendererExt)
            MyBase.Renderer = value
        End Set
    End Property



    Public Sub New()
        Me.Font = New Font("Consolas", 10)
        Me.Renderer = New ToolStripProfessionalRendererExt(_themeColor)

    End Sub



    Private _themeColor As Color = SystemColors.Highlight
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
