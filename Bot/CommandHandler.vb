Public Class CommandHandler
    Private Commands As Dictionary(Of String, ICommand)
    Public Prefix As Char

    Sub New(_prefix As Char)
        Commands = New Dictionary(Of String, ICommand)
        Prefix = _prefix
    End Sub

    Sub HandleChatCmd(text As String, sender As String, guac As Guacamole)
        If Not text.StartsWith(Prefix) Then Return

        Dim textSp = text.TrimStart(Prefix).Split(New Char() {" "c}, 2)
        Dim cmdName = textSp(0)
        Dim args = If(textSp.Length = 2, textSp(1), "")

        Dim command As ICommand = Nothing
        If TryGetCmd(cmdName, command) Then
            command.Execute(args, sender, guac)
        End If
    End Sub

    Function GetCmd(name As String) As ICommand
        Dim result As ICommand = Nothing
        TryGetCmd(name, result)
        Return result
    End Function

    Function TryGetCmd(name As String, ByRef cmd As ICommand) As Boolean
        Return Commands.TryGetValue(name, cmd)
    End Function

    Sub AddCmd(cmd As ICommand)
        Commands(cmd.Name) = cmd
    End Sub

    Sub AddCmds(ByVal ParamArray cmds As ICommand())
        For Each cmd As ICommand In cmds
            AddCmd(cmd)
        Next
    End Sub

    Function GetCmdNames() As IEnumerable(Of String)
        Return Commands.Keys
    End Function
End Class