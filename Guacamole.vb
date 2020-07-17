Imports WebSocketSharp

Public Class Guacamole
    Private WithEvents Sock As WebSocket

    Event OnMessage(message As String())
    Event OnOpen()

    Private Queue As Queue(Of String)
    Private Processing As Boolean

    Public DisplayName As String

    Sub New(url As String)
        Sock = New WebSocket(url, "guacamole")
        Sock.Compression = CompressionMethod.Deflate
        Queue = New Queue(Of String)
        Processing = False
    End Sub

    Sub Connect()
        Sock.Connect()
    End Sub

    Sub Send(ByVal ParamArray message As String())
        Dim encoded = EncodeGuac(message)
        Sock.Send(encoded)
    End Sub

    Sub Chat(message As String)
        Dim i = 0
        Dim length = 100

        While i < message.Length
            Dim nextIdx = i + length
            If nextIdx > message.Length Then length = message.Length

            Dim result = message.Substring(i, length)
            EnqueueMsg("chat", result)
            i = nextIdx
        End While
    End Sub

    Sub SetName(newName As String)
        DisplayName = newName
        Send("rename", newName)
    End Sub

    Private Async Sub ProcessQueue()
        If Processing Then Return
        While Queue.Count > 0
            Processing = True
            Dim msg = Queue.Dequeue()
            Sock.Send(msg)
            Await Task.Delay(1000)
        End While
        Processing = False
    End Sub

    Private Sub EnqueueMsg(ByVal ParamArray message As String())
        Dim encoded = EncodeGuac(message)
        Queue.Enqueue(encoded)
        If Not Processing Then Task.Run(AddressOf ProcessQueue)
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

    Private Sub OnOpenHandler(sender As Object, e As EventArgs) Handles Sock.OnOpen
        RaiseEvent OnOpen()
    End Sub

    Private Sub OnMessageHandler(sender As Object, e As MessageEventArgs) Handles Sock.OnMessage
        Dim message = DecodeGuac(e.Data)
        If message(0) = "nop" Then
            Send("nop")
        Else
            RaiseEvent OnMessage(message)
        End If
    End Sub
End Class