Public Class CvmBot
    Private Const Name = "PestilenceBot"
    Private Const Prefix As Char = ","c

    Private Guac As Guacamole
    Private CmdHandler As CommandHandler

    Sub New(url As String)
        Guac = New Guacamole(url)
        CmdHandler = New CommandHandler(":"c)

        AddHandler Guac.OnOpen, AddressOf OnGuacOpen
        AddHandler Guac.OnMessage, AddressOf OnGuacMessage

        CmdHandler.AddCmds(New MessageCmd, New StartCmd)
    End Sub

    Sub Start()
        Guac.Connect()
    End Sub

    Private Sub OnGuacOpen()
        Guac.SetName(Name)
        Guac.Chat($"{Name} connected.")
    End Sub

    Private Sub OnGuacMessage(message As String())
        If message(0) = "chat" Then
            Dim sender = message(1)
            Dim text = message(2)
            CmdHandler.HandleChatCmd(text, sender, Guac)
        End If
    End Sub
End Class