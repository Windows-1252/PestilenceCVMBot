Imports System.Net

Public Class CvmBot
    Private Const Name = "PestilenceBot"
    Private Const Prefix As Char = ":"c

    Private Guac As Guacamole
    Private CmdHandler As CommandHandler
    Private _stopwatch As Stopwatch

    Sub New(url As String)
        Guac = New Guacamole(url)
        CmdHandler = New CommandHandler(Prefix)

        AddHandler Guac.OnOpen, AddressOf OnGuacOpen
        AddHandler Guac.OnMessage, AddressOf OnGuacMessage

        CmdHandler.AddCmds(New HelpCmd(CmdHandler),
                           New EchoCmd,
                           New MessageBoxCmd,
                           New StartCmd,
                           New ClipboardCmd)
    End Sub

    Sub Start()
        _stopwatch = Stopwatch.StartNew()
        Guac.Connect()
    End Sub

    Private Sub OnGuacOpen()
        Dim message = $"{Name} connected."
        If _stopwatch.IsRunning Then
            _stopwatch.Stop()
            message &= $" ({_stopwatch.Elapsed.TotalSeconds:F3}s)"
        End If
        Guac.SetName(Name)
        Guac.Chat(message)
    End Sub

    Private Sub OnGuacMessage(message As String())
        If message(0) = "chat" Then
            Dim sender = message(1)
            Dim text = WebUtility.HtmlDecode(message(2))
            CmdHandler.HandleChatCmd(text, sender, Guac)
        End If
    End Sub
End Class