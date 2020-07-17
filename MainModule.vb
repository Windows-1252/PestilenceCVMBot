Imports System.IO

Public Module MainModule
    Private WithEvents Tray As NotifyIcon
    Private Bot As CvmBot
    Private ClickCounter = 0

    Private Const ConfigFile = "vmip.txt"

    Sub Main(ByVal args() As String)
        Dim info = My.Application.Info
        Tray = New NotifyIcon()
        Tray.Icon = My.Resources.elError
        Tray.Text = $"{My.Application.Info.ProductName} v{info.Version.Major}.{info.Version.Minor}" &
                    $"{vbCrLf}by {My.Application.Info.CompanyName}"
        Tray.Visible = True

        If File.Exists(ConfigFile) Then
            Dim config = File.ReadAllLines(ConfigFile)

            If config.Length = 0 Then Return
            If String.IsNullOrEmpty(config(0)) Then Return

            Bot = New CvmBot(config(0))
            Bot.Start()

            KernelUtils.Protect()
            Application.Run()
        End If
    End Sub

    Private Sub Tray_MouseClick(sender As Object, e As MouseEventArgs) Handles Tray.MouseClick
        If e.Button = MouseButtons.Right Then
            ClickCounter += 1
            If ClickCounter = 4 Then
                KernelUtils.Unprotect()
                Application.Exit()
            End If
        End If
    End Sub
End Module