Public Class frmAbout

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtAbout.Text = ReadFromAssembly("About.txt")
        txtHistory.Text = ReadFromAssembly("History.txt")
    End Sub

    Private Function ReadFromAssembly(ByVal filename As String) As String
        Dim sRet As String = ""
        Dim executing_assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim my_namespace As String = GetType(frmAbout).Namespace
        Dim text_stream As IO.Stream = executing_assembly.GetManifestResourceStream(my_namespace & "." & filename)
        If text_stream IsNot Nothing Then
            Dim stream_reader As New IO.StreamReader(text_stream)
            sRet = stream_reader.ReadToEnd()
        End If
        Return sRet
    End Function

End Class