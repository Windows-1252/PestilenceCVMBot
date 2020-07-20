Public Class StartCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "start" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Open program or file. Usage: start [path], {args}" Implements ICommand.Help

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        Dim splitArgs = args.Split(New Char() {","c}, 2)

        If splitArgs.Length > 0 Then
            Dim exec = splitArgs(0).Trim()
            Dim execArgs = If(splitArgs.Length = 2, splitArgs(1).Trim(), "")

            Try
                Process.Start(exec, execArgs)
                guac.Chat($"@{sender} {exec} started.")
            Catch ex As Exception
                guac.Chat($"@{sender} Cannot start {exec}.")
            End Try
        End If
    End Sub
End Class