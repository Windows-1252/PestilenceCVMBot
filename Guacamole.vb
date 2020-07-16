Imports WebSocketSharp

Public Class Guacamole
    Private WithEvents Sock As WebSocket
    Event OnMessage(message As String())

    Sub New(url As String)
        Sock = New WebSocket(url, "guacamole")
        Sock.Compression = CompressionMethod.Deflate
    End Sub

    Sub Connect()
        Sock.Connect()
    End Sub

    Sub Send(ByVal ParamArray message As String())
        Dim encoded = EncodeGuac(message)
        Sock.Send(encoded)
    End Sub

    Sub SendChat(message As String)
        'TODO: split into 100 char substrings and send
    End Sub

    Private Sub OnOpenHandler(sender As Object, e As EventArgs) Handles Sock.OnOpen
        Send("rename", "PestilenceBot")
        Send("chat", "Pestilence Bot Active.")
    End Sub

    Private Sub OnMessageHandler(sender As Object, e As MessageEventArgs) Handles Sock.OnMessage
        Dim message = DecodeGuac(e.Data)
        'TODO: commands
        RaiseEvent OnMessage(message)
    End Sub

    Private Function DecodeGuac(msg As String) As String()
        Dim result = New List(Of String)
        Dim i = 0

        While i < msg.Length
            Dim toIdx = msg.IndexOf("."c)
            If toIdx = -1 Then Exit While
            Dim length = Integer.Parse(msg.Substring(i, toIdx))
            i += toIdx + 1

            Dim text = msg.Substring(i, length)
            result.Add(text)
            i += length

            If msg(i) = ","c Then
                i += 1
            ElseIf msg(i) = ";"c Then
                Exit While
            Else
                Throw New FormatException($"Expected ',' or ';', but got {msg(i)}")
            End If

            msg = msg.Substring(i)
            i = 0
        End While

        Return result.ToArray()
    End Function

    Private Function EncodeGuac(ByVal ParamArray msg As String()) As String
        Dim result = ""
        Dim lastIdx = msg.Length - 1

        For i = 0 To lastIdx
            result += $"{msg(i).Length}.{msg(i)}"
            If i < lastIdx Then result += ","
        Next

        result += ";"
        Return result
    End Function
End Class