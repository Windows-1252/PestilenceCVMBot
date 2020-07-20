Public Class ClipboardCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "clip" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Clipboard. Usage: clip [get|set] {value}" Implements ICommand.Help

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        Dim splitArgs = args.Split(New Char() {" "c}, 2)

        If splitArgs.Length > 0 Then
            Dim cmd = splitArgs(0).ToLower()
            If cmd = "get" Then
                If Clipboard.ContainsText() Then
                    Dim clipText = Clipboard.GetText()
                    guac.Chat($"@{sender} {clipText}")
                Else
                    guac.Chat($"@{sender} Clipboard empty or non-text.")
                End If
            ElseIf cmd = "set" Then
                If splitArgs.Length = 2 Then
                    Dim newText = splitArgs(1)
                    Clipboard.SetText(newText)
                    guac.Chat($"@{sender} Clipboard set.")
                End If
            End If
        End If
    End Sub
End Class