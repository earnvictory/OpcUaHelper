Imports System.Text
Imports System.Windows.Forms
Imports Opc.Ua

Public Class ExceptionDlg


    Private m_exception As Exception





    Private Overloads Sub ShowStack(ByVal showStackTrace As Boolean)


        Dim e As Exception = m_exception

        ExceptionBrowser.Text = If(ShowStackTracesCK.Checked, e.Message & vbCrLf & If(e.InnerException?.ToString, "") & vbCrLf & If(e.StackTrace?.ToString, ""), e.Message)
    End Sub

    Public Overloads Shared Sub Show(ByVal caption As String, ByVal e As Exception)
        If Not Environment.UserInteractive Then
            Utils.Trace(e, "Unexpected error in '{0}'.", caption)
            Return
        End If

        Dim Frm As New ExceptionDlg()
        Frm.TopMost = True
        Frm.ShowDialog(caption, e)
    End Sub

    Public Overloads Sub ShowDialog(ByVal caption As String, ByVal e As Exception)
        If Not String.IsNullOrEmpty(caption) Then
            Text = caption
        End If

        m_exception = e
        ShowStackTracesCK.Checked = False
        ShowStack(ShowStackTracesCK.Checked)
        TopMost = True
        ShowDialog()

    End Sub
    Private Sub CloseBTN_Click(sender As Object, e As EventArgs) Handles CloseBTN.Click
        Close()
    End Sub


    Private Sub ShowStackTracesCK_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ShowStackTracesCK.CheckedChanged
        ShowStack(ShowStackTracesCK.Checked)
    End Sub
End Class
