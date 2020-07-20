Public Class HelpCmd
    Implements ICommand
    Sub New(cmdHandler As CommandHandler)
        Me.CmdHandler = cmdHandler
    End Sub

    Private CmdHandler As CommandHandler

    Public ReadOnly Property Name As String = "help" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Self-explanatory. Usage: help {command}" Implements ICommand.Help

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        If String.IsNullOrEmpty(args) Then
            Dim allCmds As String = String.Join(" ", CmdHandler.GetCmdNames())
            guac.Chat($"Commands: {allCmds}")
        Else
            Dim cmd As ICommand = Nothing
            If CmdHandler.TryGetCmd(args, cmd) Then
                guac.Chat(cmd.Help)
            End If
        End If
    End Sub
End Class