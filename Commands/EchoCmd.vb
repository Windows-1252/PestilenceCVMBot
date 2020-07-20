Public Class EchoCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "echo" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Usage: echo [message]" Implements ICommand.Help

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        If Not String.IsNullOrEmpty(args) Then
            guac.Chat($"@{sender} {args}")
        End If
    End Sub
End Class