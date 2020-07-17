Public Class CvmBot
    Private Guac As Guacamole
    Private Const Name = "PestilenceBot"
    Private Const Prefix As Char = ","c

    Sub New(url As String)
        Guac = New Guacamole(url)
        AddHandler Guac.OnOpen, AddressOf OnGuacOpen
        AddHandler Guac.OnMessage, AddressOf OnGuacMessage
    End Sub

    Sub Start()
        Guac.Connect()
    End Sub

    Private Sub OnGuacOpen()
        Guac.SetName(Name)
        'Guac.Chat("PestilenceBot Connected")
    End Sub

    Private Sub OnGuacMessage(message As String())
        If message(0) = "chat" Then
            Dim sender = message(1)
            Dim text = message(2)

            If text.StartsWith(Prefix) Then
                Dim textSp = text.TrimStart(Prefix).Split(New Char() {" "c}, 2)
                Dim cmd = textSp(0)
                Dim args = If(textSp.Length = 2, textSp(1), "")

                If cmd = "message" Then
                    MessageBox.Show(args, $"{sender} says...", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

                ElseIf cmd = "start" Then
                    Dim splitArgs = args.Split(New Char() {","c}, 2)
                    If splitArgs.Length > 0 Then
                        Dim exec = splitArgs(0).Trim()
                        Dim execArgs = If(splitArgs.Length = 2, splitArgs(1).Trim(), "")
                        Try
                            Process.Start(exec, execArgs)
                            Guac.Chat($"@{sender} {exec} started.")
                        Catch ex As Exception
                            Guac.Chat($"@{sender} Cannot start {exec}.")
                        End Try
                    End If
                End If

            End If
        End If
    End Sub
End Class