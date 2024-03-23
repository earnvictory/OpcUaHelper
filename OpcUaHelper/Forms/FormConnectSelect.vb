Imports System.Windows.Forms

Imports Opc.Ua
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading.Tasks

Imports System.Windows.Forms.VisualStyles.VisualStyleElement


Public Class FormConnectSelect


    Public Sub New(ByVal opcUaClient As OpcUaClient)
        InitializeComponent()
        Me.m_OpcUaClient = opcUaClient
    End Sub



    Private ReadOnly m_OpcUaClient As OpcUaClient

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        m_OpcUaClient.UserIdentity = New UserIdentity(New AnonymousIdentityToken())
        DialogResult = DialogResult.OK
        Return
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        m_OpcUaClient.UserIdentity = New UserIdentity(textBox1.Text, textBox2.Text)
        DialogResult = DialogResult.OK
        Return
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Try
            Dim certificate As New X509Certificate2(textBox4.Text, textBox3.Text, X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.Exportable)
            m_OpcUaClient.UserIdentity = New UserIdentity(certificate)
            DialogResult = DialogResult.OK
            Return
            Catch ex As Exception
            ClientUtils.HandleException(Me.Text, ex)
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.Multiselect = False

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                textBox4.Text = openFileDialog.FileName
            End If
        End Using
    End Sub


End Class
