Partial Class TreeViewEx
    Inherits System.Windows.Forms.TreeView


    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Windows.Forms 类撰写设计器支持所必需的
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub


    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.EnableNotifyMessage, True)
        Me.UpdateStyles()
        '组件设计器需要此调用。
        InitializeComponent()

    End Sub
    Protected Overrides Sub OnNotifyMessage(ByVal m As Message)
        If m.Msg <> &H14 Then
            MyBase.OnNotifyMessage(m)
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '组件设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是组件设计器所必需的
    '可使用组件设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

End Class
